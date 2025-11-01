using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Bogus;

public class MustashMethod
{
   public string Name { get; set; }
   public MethodInfo Method { get; set; }
   public object[] OptionalArgs { get; set; }
   // precompiled getter for nested properties
   public Func<object, object> Getter { get; set; }
}

public static class Tokenizer
{
   public static ILookup<string, MustashMethod> MustashMethods;
   public static ILookup<string, MustashMethod> PersonProperties;

   static Tokenizer()
   {
      RegisterMustashMethods(typeof(Faker));
      RegisterPersonPropertiesAsMustashMethod(typeof(Person));
      MustashMethods = MustashMethods.Concat(PersonProperties)
         .SelectMany(g => g)
         .ToLookup(mm => mm.Name);
   }

   /// <summary>
   /// Method to determine if to traverse into the property or not.
   /// </summary>
   /// <param name="property"></param>
   /// <returns></returns>
   public static bool IsComplex(PropertyInfo property)
   {
      var t = property.PropertyType;
      t = Nullable.GetUnderlyingType(t) ?? t; // unwrap Nullable<T>

      // There isn't a efficient way to determine if a property cant be traversed into across all the supported .NET versions.
#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3
        var ti = t.GetTypeInfo();
        bool isPrimitiveLike =
            ti.IsPrimitive ||
            ti.IsEnum ||
            t == typeof(string) ||
            t == typeof(DateTime) ||
            t == typeof(DateTimeOffset);
#else
      bool isPrimitiveLike =
          t.IsPrimitive ||
          t.IsEnum ||
          t == typeof(string) ||
          t == typeof(DateTime) ||
          t == typeof(DateTimeOffset);
#endif

      // drill into everything else
      var test = isPrimitiveLike;
      return !isPrimitiveLike;
   }

   /// <summary>
   /// Recursively get all properties from a class, including nested properties.
   /// </summary>
   /// <param name="parentPropertyInfo"></param>
   /// <param name="mi"></param>
   /// <param name="nestedName"></param>
   /// <returns></returns>
   static IEnumerable<MustashMethod> GetClassProperties(Type parentPropertyInfo, PropertyInfo mi, string nestedName = null)
   {
      // if the property is complex, recurse into it
      if (IsComplex(mi))
      {
         nestedName += mi.Name + ".";
         return mi.PropertyType.GetProperties()
            .SelectMany(nmi =>
            {
               return GetClassProperties(parentPropertyInfo, nmi, nestedName);
            });
      }
      else
      {
         // if the property has a getter, create a MustashMethod for it
         var category = DataSet.ResolveCategory(parentPropertyInfo);
         var methodName = nestedName != null ? $"{nestedName}{mi.Name}" : mi.Name;
         var mm = new MustashMethod
         {
            Name = $"{category}.{methodName}".ToUpperInvariant(),
            Method = mi.GetGetMethod(),
            Getter = BuildGetter($"{category}.{methodName}".ToUpperInvariant(), parentPropertyInfo),
            OptionalArgs = []
         };
         return new[] { mm }.Where(x => x.Method != null);
      }
   }

   /// <summary>
   /// Allows for nested property getting via compiled expression trees.
   /// </summary>
   /// <param name="mustashName"></param>
   /// <param name="rootType"></param>
   /// <returns></returns>
   private static Func<object, object> BuildGetter(string mustashName, Type rootType)
   {
      var parts = mustashName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length <= 1) return null; // nothing to walk

      var path = parts.Skip(1).ToArray(); // drop category

      var param = Expression.Parameter(typeof(object), "root");
      Expression current = Expression.Convert(param, rootType);

      foreach (var segment in path)
      {
         var prop = current.Type.GetProperty(segment, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
         if (prop == null) return null; // can't build
         current = Expression.Property(current, prop);
      }

      var body = Expression.Convert(current, typeof(object));
      var lambda = Expression.Lambda<Func<object, object>>(body, param);
      return lambda.Compile();
   }

   /// <summary>
   /// Finds the properties with the RegisterPersonPropertyAttribute to avoid getting properties that wont return a relevant value.
   /// </summary>
   /// <param name="type"></param>
   public static void RegisterPersonPropertiesAsMustashMethod(Type type)
   {
      PersonProperties = type.GetProperties()
         .Where(p => p.IsDefined(typeof(RegisterPersonPropertyAttribute), true))
         .SelectMany(p =>
         {
            return GetClassProperties(type, p);
         })
         .ToLookup(mm => mm.Name);
   }

   public static void RegisterMustashMethods(Type type)
   {
      MustashMethods = type.GetProperties()
         .Where(p => p.IsDefined(typeof(RegisterMustasheMethodsAttribute), true))
         .SelectMany(p =>
            {
               return p.PropertyType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                  .Where(mi => mi.GetGenericArguments().Length == 0)
                  .Select(mi =>
                     {
                        var category = DataSet.ResolveCategory(p.PropertyType);
                        var methodName = mi.Name;
                        var mm = new MustashMethod
                           {
                              Name = $"{category}.{methodName}".ToUpperInvariant(),
                              Method = mi,
                              OptionalArgs = mi.GetParameters().Where(pi => pi.IsOptional).Select(_ => Type.Missing).ToArray()
                           };
                        return mm;
                     });
            })
         .ToLookup(mm => mm.Name);
   }

   public static string Parse(string str, params object[] dataSets)
   {
      //Recursive base case. If there are no more {{ }} handle bars,
      //return.
      var start = str.IndexOf("{{", StringComparison.Ordinal);
      var end = str.IndexOf("}}", StringComparison.Ordinal);
      if( start == -1 && end == -1 )
      {
         return str;
      }

      //We have some handlebars to process. Get the method name and arguments.
      ParseMustashText(str, start, end, out var methodName, out var arguments);

      if( !MustashMethods.Contains(methodName) )
      {
         throw new ArgumentException($"Unknown method {methodName} can't be found.");
      }

      //At this point, we have a methodName like: RANDOMIZER.NUMBER
      //and if the dataset was registered with RegisterMustasheMethodsAttribute
      //we should be able to extract the dataset given it's methodName.
      var dataSet = FindDataSetWithMethod(dataSets, methodName);

      //Considering arguments, lets get the best method overload
      //that maps to a registered MustashMethod.
      var mm = FindMustashMethod(methodName, arguments);
      var providedArgumentList = ConvertStringArgumentsToObjects(arguments, mm);
      var optionalArgs = mm.OptionalArgs.Take(mm.Method.GetParameters().Length - providedArgumentList.Length);
      var argumentList = providedArgumentList.Concat(optionalArgs).ToArray();

      //make the actual invocation.
      //check whether to invoke getter for registered properties or invoke method
      object resultObj;
      if (mm.Getter != null && (argumentList == null || argumentList.Length == 0))
      {
         // fast path: precompiled nested-property getter
         resultObj = mm.Getter(dataSet);
      }
      else
      {
         resultObj = mm.Method.Invoke(dataSet, argumentList);
      }
      var fakeVal = resultObj?.ToString() ?? string.Empty;

      var sb = new StringBuilder();
      sb.Append(str, 0, start);
      sb.Append(fakeVal);
      sb.Append(str.Substring(end + 2));

      return Parse(sb.ToString(), dataSets);
   }

   private static object FindDataSetWithMethod(object[] dataSets, string methodName)
   {
      var method = MustashMethods[methodName].First().Method;
      var dataSetType = method.DeclaringType;

      // if the method is declared on a nested type, walk up to the top-level declaring type (e.g. Person)
      while (dataSetType?.DeclaringType != null)
      {
         dataSetType = dataSetType.DeclaringType;
      }

      var ds = dataSets
         .Select(o => o is Delegate del &&
                      del.GetType().GetGenericTypeDefinition() == typeof(Func<>) &&
                      del.GetType().GetGenericArguments()[0] == dataSetType
               ? del.DynamicInvoke()
               : o)
         .FirstOrDefault(o => o?.GetType() == dataSetType);

      if ( ds == null )
      {
         throw new ArgumentException($"Can't parse {methodName} because the dataset was not provided in the {nameof(dataSets)} parameter.", nameof(dataSets));
      }
      return ds;
   }

   private static void ParseMustashText(string str, int start, int end, out string methodName, out string[] arguments)
   {
      var methodCall = str.Substring(start + 2, end - start - 2)
         .Replace("}}", "")
         .Replace("{{", "");

      var argumentsStart = methodCall.IndexOf("(", StringComparison.Ordinal);
      if (argumentsStart != -1)
      {
         var argumentsString = GetArgumentsString(methodCall, argumentsStart);
         methodName = methodCall.Substring(0, argumentsStart).Trim();
         arguments = argumentsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
      }
      else
      {
         methodName = methodCall;
         arguments = new string[0];
      }

      methodName = methodName.ToUpperInvariant();
   }

   private static MustashMethod FindMustashMethod(string methodName, string[] arguments)
   {
      var selection =
         from mm in MustashMethods[methodName]
         orderby mm.Method.GetParameters().Count(pi => pi.IsOptional) - arguments.Length
         where mm.Method.GetParameters().Length >= arguments.Length
         where mm.OptionalArgs.Length + arguments.Length >= mm.Method.GetParameters().Length
         select mm;

      var found = selection.FirstOrDefault();
      return found ?? throw new ArgumentException($"Cannot find a method '{methodName}' that could accept {arguments.Length} arguments");
   }

   private static object[] ConvertStringArgumentsToObjects(string[] parameters, MustashMethod mm)
   {
      try
      {
         return mm.Method.GetParameters()
                         .Zip(parameters, GetValueForParameter)
                         .ToArray();
      }
      catch (OverflowException ex)
      {
         throw new ArgumentOutOfRangeException($"One of the arguments for {mm.Name} is out of supported range. Argument list: {string.Join(",", parameters)}", ex);
      }
      catch (Exception ex) when (ex is InvalidCastException or FormatException)
      {
         throw new ArgumentException($"One of the arguments for {mm.Name} cannot be converted to target type. Argument list: {string.Join(",", parameters)}", ex);
      }
      catch (Exception ex)
      {
         throw new ArgumentException($"Cannot parse arguments for {mm.Name}. Argument list: {string.Join(",", parameters)}", ex);
      }
   }

   private static object GetValueForParameter(ParameterInfo parameterInfo, string parameterValue)
   {
      var type = Nullable.GetUnderlyingType(parameterInfo.ParameterType) ?? parameterInfo.ParameterType;

      if( typeof(Enum).IsAssignableFrom(type)) return Enum.Parse(type, parameterValue);

      if( typeof(TimeSpan) == type ) return TimeSpan.Parse(parameterValue);

      return Convert.ChangeType(parameterValue, type);
   }

   private static string GetArgumentsString(string methodCall, int parametersStart)
   {
      var parametersEnd = methodCall.IndexOf(')');
      if( parametersEnd == -1 )
      {
         throw new ArgumentException($"The method call '{methodCall}' is missing a terminating ')' character.");
      }

      return methodCall.Substring(parametersStart + 1, parametersEnd - parametersStart - 1);
   }
}

[AttributeUsage(AttributeTargets.Property)]
internal class RegisterMustasheMethodsAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Property)]
internal class RegisterPersonPropertyAttribute : Attribute
{
}
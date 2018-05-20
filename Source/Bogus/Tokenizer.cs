using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bogus
{
   public class MustashMethod
   {
      public string Name { get; set; }
      public MethodInfo Method { get; set; }
      public object[] OptionalArgs { get; set; }
   }

   public class Tokenizer
   {
      public static ILookup<string, MustashMethod> MustashMethods;

      static Tokenizer()
      {
         RegisterMustashMethods();
      }

      private static void RegisterMustashMethods()
      {
         MustashMethods = typeof(Faker).GetProperties()
            .Where(p => p.IsDefined(typeof(RegisterMustasheMethodsAttribute), true))
            .SelectMany(p =>
               {
                  return p.PropertyType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                   .Where(mi => mi.GetGenericArguments().Length == 0)
                   .Select(mi =>
                   {
                      var mm = new MustashMethod
                      {
                         Name = string.Format("{0}.{1}", DataSet.ResolveCategory(p.PropertyType), mi.Name).ToUpperInvariant(),
                         Method = mi,
                         OptionalArgs = mi.GetParameters().Where(pi => pi.IsOptional).Select(_ => Type.Missing).ToArray()
                      };
                      return mm;
                   });
               })
            .ToLookup(mm => mm.Name);
      }

      public static string Parse(string str, params IDataSet[] dataSets)
      {
         var start = str.IndexOf("{{", StringComparison.Ordinal);
         var end = str.IndexOf("}}", StringComparison.Ordinal);
         if( start == -1 && end == -1 )
         {
            return str;
         }

         var methodCall = str.Substring(start + 2, end - start - 2)
            .Replace("}}", "")
            .Replace("{{", "");
         string methodName = methodCall;

         string argumentsString = String.Empty;
         var argumentsStart = methodCall.IndexOf("(", StringComparison.Ordinal);
         if (argumentsStart != -1)
         {
            argumentsString = GetArgumentsString(methodCall, argumentsStart);
            methodName = methodCall.Substring(0, argumentsStart).Trim();
         }

         methodName = methodName.ToUpperInvariant();
         if (!MustashMethods.Contains(methodName))
         {
            throw new ArgumentException($"Unknown method {methodName} can't be found.");
         }

         MustashMethod mm = MustashMethods[methodName].FirstOrDefault();

         var module = dataSets.FirstOrDefault(o => o.GetType() == mm.Method.DeclaringType);

         if( module == null )
         {
            throw new ArgumentException($"Can't parse {methodName} because the dataset was not provided in the {nameof(dataSets)} parameter.", nameof(dataSets));
         }
         
         var arguments = argumentsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
         mm = FindCorrectMustashMethod(methodName, arguments);
         var providedArgumentList = GetArgumentListForCall(arguments, mm);
         var optionalArgs = mm.OptionalArgs.Take(mm.Method.GetParameters().Length - providedArgumentList.Length);
         var argumentList = providedArgumentList.Concat(optionalArgs).ToArray();

         var fakeVal = mm.Method.Invoke(module, argumentList).ToString();

         var sb = new StringBuilder();
         sb.Append(str.Substring(0, start));
         sb.Append(fakeVal);
         sb.Append(str.Substring(end + 2));

         return Parse(sb.ToString(), dataSets);
      }

      private static MustashMethod FindCorrectMustashMethod(string methodName, string[] arguments)
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

      private static object[] GetArgumentListForCall(string[] parameters, MustashMethod mm)
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
         catch (Exception ex) when (ex is InvalidCastException || ex is FormatException)
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
         if (parametersEnd == -1) throw new ArgumentException($"The method call '{methodCall}' is missing a terminating ')' character.");

         return methodCall.Substring(parametersStart + 1, parametersEnd - parametersStart - 1);
      }
   }

   [AttributeUsage(AttributeTargets.Property)]
   internal class RegisterMustasheMethodsAttribute : Attribute
   {
   }
}
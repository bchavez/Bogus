using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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
      // For backwards compatibility
      [Obsolete]
      public static Dictionary<string, MustashMethod> MustashMethods;
      internal static ILookup<string, MustashMethod> ExtendedMustashMethods;

      static Tokenizer()
      {
         RegisterMustashMethods();
      }

      private static void RegisterMustashMethods()
      {
         ExtendedMustashMethods = typeof(Faker).GetProperties()
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

         MustashMethods = ExtendedMustashMethods.Select(mmCollection => mmCollection.FirstOrDefault(
                                                                                 mm => mm.Method.GetParameters().Length == 0
                                                                                       || mm.Method.GetParameters().All(pi => pi.IsOptional)))
                                                .Where(mm => mm != null)
                                                .ToDictionary(mm => mm.Name);
      }

      internal static Regex ParseMatcher = new Regex("(?<={{).+?(?=}})");

      public static string Parse(string str, params IDataset[] dataSets)
      {
         var start = str.IndexOf("{{", StringComparison.Ordinal);
         var end = str.IndexOf("}}", StringComparison.Ordinal);
         if (start == -1 && end == -1)
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
         if (!ExtendedMustashMethods.Contains(methodName))
         {
            throw new ArgumentException($"Unknown method {methodName} can't be found.");
         }

         MustashMethod mm = ExtendedMustashMethods[methodName].FirstOrDefault();

         var module = dataSets.FirstOrDefault(o => o.GetType() == mm.Method.DeclaringType);

         if (module == null)
         {
            throw new ArgumentException($"Can't parse {methodName} because the dataset was not provided in the dataSets parameter.");
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
         var method = ExtendedMustashMethods[methodName].OrderBy(mm => mm.Method.GetParameters().Count(pi => pi.IsOptional) - arguments.Length)
                                                        .FirstOrDefault(mm => mm.Method.GetParameters().Length >= arguments.Length
                                                                           && mm.OptionalArgs.Length + arguments.Length >= mm.Method.GetParameters().Length)
                      ?? throw new ArgumentException($"Cannot find a method '{methodName}' that could accept {arguments.Length} arguments");

         return method;
      }

      private static object[] GetArgumentListForCall(string []parameters, MustashMethod mm)
      {
         try
         {
            return mm.Method.GetParameters()
                            .Zip(parameters,
                                 (pi, pn) => Convert.ChangeType(pn, pi.ParameterType))
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

      private static string GetArgumentsString(string methodCall, int parametersStart)
      {
         var parametersEnd = methodCall.IndexOf(')');
         if (parametersEnd == -1)
            return String.Empty;

         return methodCall.Substring(parametersStart + 1, parametersEnd - parametersStart - 1);
      }

      public static string ParseOld(string expr, params DataSet[] dataSets)
      {
         if (dataSets.Length == 0)
         {
            throw new ArgumentOutOfRangeException("dataSets", "One or more data sets is required in order to evaluate a handlebar expression.");
         }

         var funcCalls = ParseMatcher.Matches(expr)
            .OfType<Match>().Select(d => d.Value.ToUpperInvariant())
            .Distinct();

         foreach (var func in funcCalls)
         {
            var handle = string.Format("{{{{{0}}}}}", func);

            MustashMethod mustashMethod;

            MustashMethods.TryGetValue(func, out mustashMethod);

            if (mustashMethod == null) continue;

            var module = dataSets.FirstOrDefault(o => o.GetType() == mustashMethod.Method.DeclaringType);

            if (module == null)
               throw new ArgumentException($"Can't parse {handle} because the dataset was not provided in the dataSets parameter.");

            var val = mustashMethod.Method.Invoke(module, null) as string;

            expr = Regex.Replace(expr, handle, val, RegexOptions.IgnoreCase);
         }

         return expr;
      }
   }

   [AttributeUsage((AttributeTargets.Property))]
   internal class RegisterMustasheMethodsAttribute : Attribute
   {
   }
}
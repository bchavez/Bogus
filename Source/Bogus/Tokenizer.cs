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
      public static Dictionary<string, MustashMethod> MustashMethods;

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
                     .Where(mi =>
                        {
                           if( mi.GetParameters().Length == 0 || mi.GetParameters().All(pi => pi.IsOptional) )
                           {
                              if( mi.GetGenericArguments().Length == 0 )
                              {
                                 return true;
                              }
                           }
                           return false;
                        })
                     .Select(mi =>
                        {
                           var mm = new MustashMethod
                              {
                                 Name = string.Format("{0}.{1}", DataSet.ResolveCategory(p.PropertyType), mi.Name).ToUpperInvariant(),
                                 Method = mi,
                                 OptionalArgs = Enumerable.Repeat(Type.Missing, mi.GetParameters().Length).ToArray()
                              };
                           return mm;
                        });
               }).ToDictionary(mm => mm.Name);
      }

      internal static Regex ParseMatcher = new Regex("(?<={{).+?(?=}})");

      public static string Parse(string str, params DataSet[] dataSets)
      {
         var start = str.IndexOf("{{", StringComparison.Ordinal);
         var end = str.IndexOf("}}", StringComparison.Ordinal);
         if( start == -1 && end == -1 )
         {
            return str;
         }

         var methodName = str.Substring(start + 2, end - start - 2)
            .Replace("}}", "")
            .Replace("{{", "")
            .ToUpperInvariant();

         MustashMethod mm;
         if( !MustashMethods.TryGetValue(methodName, out mm) )
         {
            throw new ArgumentException($"Unknown method {methodName} can't be found.");
         }

         var module = dataSets.FirstOrDefault(o => o.GetType() == mm.Method.DeclaringType);

         if( module == null )
         {
            throw new ArgumentException($"Can't parse {methodName} because the dataset was not provided in the dataSets parameter.");
         }

         var fakeVal = mm.Method.Invoke(module, mm.OptionalArgs) as string;

         var sb = new StringBuilder();
         sb.Append(str.Substring(0, start));
         sb.Append(fakeVal);
         sb.Append(str.Substring(end + 2));

         return Parse(sb.ToString(), dataSets);
      }

      public static string ParseOld(string expr, params DataSet[] dataSets)
      {
         if( dataSets.Length == 0 )
         {
            throw new ArgumentOutOfRangeException("dataSets", "One or more data sets is required in order to evaluate a handlebar expression.");
         }

         var funcCalls = ParseMatcher.Matches(expr)
            .OfType<Match>().Select(d => d.Value.ToUpperInvariant())
            .Distinct();

         foreach( var func in funcCalls )
         {
            var handle = string.Format("{{{{{0}}}}}", func);

            MustashMethod mustashMethod;

            MustashMethods.TryGetValue(func, out mustashMethod);

            if( mustashMethod == null ) continue;

            var module = dataSets.FirstOrDefault(o => o.GetType() == mustashMethod.Method.DeclaringType);

            if( module == null )
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
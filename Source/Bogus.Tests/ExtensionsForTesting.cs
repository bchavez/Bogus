using System;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace Bogus.Tests
{
   public static class ExtensionsForTesting
   {
      public static void Dump(this object obj)
      {
         Console.WriteLine(obj.DumpString());
      }

      public static string DumpString(this object obj)
      {
         return JsonConvert.SerializeObject(obj, Formatting.Indented);
      }

      public static void Dump(this ITestOutputHelper console, object obj)
      {
         console.WriteLine(obj.DumpString());
      }
   }
}
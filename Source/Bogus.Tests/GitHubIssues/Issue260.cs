using System;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue260 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue260(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void generate_new_ssn()
      {
         var r = new Randomizer();

         var x = r.Int();

         var a = (x << 10) & 0b1111_1111_10;

         var b = (x << 10 + 7);

         var c = (x << 10 + 7 + 10);

         console.Dump(Convert.ToString(x, 2));
         console.Dump(Convert.ToString(a, 2));
         console.Dump(Convert.ToString(b, 2));
         console.Dump(Convert.ToString(c, 2));

         var result = $"{a:000}-{b:00}-{c:0000}";

         console.Dump(result);
      }
   }
}
using FluentAssertions;
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
      public void fast_algo3_test()
      {
         var r = new Randomizer();

         var x = r.Int();

         // the remainder of this test depends on x having a predictable value
         x.Should().Be(1077349347);

         // right shift all bits except fir the first 10 bits = 2^10 = 1024.
         var a = (x >> (32 - 10)) % 898;
         if( a == 0 || a == 666 ) a++;

         // use the first 7 bits = 2^7 = 128 
         var b = (x & 0x7F);
         if( b == 0 ) b++;

         // last 2^14 = 16384, for last 4 digits of SSN
         var c = (x >> 7) & 0x3FFF;
         if( c >= 10000 ) c -= 10000;
         if( c == 0 ) c++;

         var result = $"{a:000}-{b:00}-{c:0000}";

         result.Should().Be("256-99-1799");
      }
   }
}
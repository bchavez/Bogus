using Bogus.Extensions;
using FluentAssertions;

namespace Bogus.Tests.GitHubIssues;

public class Issue319 : SeededTest
{
   [Fact]
   public void can_generate_decimal_edge_case()
   {
      var r = new Randomizer();

      Action a = () =>
         {
            r.Decimal(0m, decimal.MaxValue);
            r.Decimal(0m, decimal.MaxValue);
            r.Decimal(0m, decimal.MaxValue);
            r.Decimal(0m, decimal.MaxValue);
         };
      a.Should().NotThrow();
   }

   [Fact]
   public void decimal2_should_throw_on_edge_case()
   {
      var r = new Randomizer();
      Action a = () =>
         {
            r.Decimal2(0, decimal.MaxValue);
         };

      a.Should().Throw<OverflowException>();
   }
}
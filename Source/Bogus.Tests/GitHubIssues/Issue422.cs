using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;
using Z.ExtensionMethods;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue422
   {
      private Randomizer sut;

      public Issue422()
      {
         sut = new Randomizer();
      }

      /// <summary>
      /// Verifies that generating a random <see cref="double"/>
      /// between <see cref="double.MinValue"/> 
      /// and <see cref="double.MaxValue"/>
      /// yields a non-symbolic, valid value in that range.
      /// See issue #422.
      /// </summary>
      [Fact]
      public void generate_any_valid_double()
      {
         var result = sut.Double(double.MinValue, double.MaxValue);

         using (new AssertionScope())
         {
            result.Should().BeInRange(double.MinValue, double.MaxValue);
            result.IsNaN().Should().BeFalse();
            result.IsInfinity().Should().BeFalse();
         }
      }

      /// <summary>
      /// Verifies that generating a random <see cref="decimal"/>
      /// between <see cref="decimal.MinValue"/> 
      /// and <see cref="decimal.MaxValue"/>
      /// yields a non-symbolic, valid value in that range.
      /// See issue #422.
      /// </summary>
      [Fact]
      public void generate_any_valid_decimal()
      {
         sut.Decimal(decimal.MinValue, decimal.MaxValue).Should().BeInRange(decimal.MinValue, decimal.MaxValue);
      }

      /// <summary>
      /// Verifies that generating a random <see cref="float"/>
      /// between <see cref="float.MinValue"/> 
      /// and <see cref="float.MaxValue"/>
      /// yields a non-symbolic, valid value in that range.
      /// See issue #422.
      /// </summary>
      [Fact]
      public void generate_any_valid_float()
      {
         var result = sut.Float(float.MinValue, float.MaxValue);

         using (new AssertionScope())
         {
            result.Should().BeInRange(float.MinValue, float.MaxValue);
            result.IsNaN().Should().BeFalse();
            result.IsInfinity().Should().BeFalse();
         }
      }
   }
}

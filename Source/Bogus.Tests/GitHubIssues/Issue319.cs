using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue319 : SeededTest
   {
      class TestDataProvider : DataAttribute
      {
         public override IEnumerable<object[]> GetData(MethodInfo testMethod)
         {
            yield return new object[] { 0m, decimal.MaxValue };
            yield return new object[] { decimal.MinValue, decimal.MaxValue };
            yield return new object[] { decimal.MinValue, 0m };
            // A range whose size exceeds decimal.MaxValue but which doesn't have decimal.MinValue or decimal.MaxValue as a bound.
            yield return new object[] { decimal.MinValue * 0.6m, decimal.MaxValue * 0.6m };
         }
      }

      ITestOutputHelper _output;

      public Issue319(ITestOutputHelper output)
         => _output = output;

      [Theory, TestDataProvider]
      public void decimal_with_very_large_range_succeeds(decimal min, decimal max)
      {
         var randomizer = new Randomizer();

         for (int iteration = 0; iteration < 300; iteration++)
         {
            try
            {
               randomizer.Decimal(min, max).Should().BeInRange(min, max);
            }
            catch
            {
               _output.WriteLine("Test failed on iteration {0}", iteration);
               throw;
            }
         }
      }
   }
}

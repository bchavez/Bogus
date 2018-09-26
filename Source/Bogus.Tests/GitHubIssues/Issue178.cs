using System.Linq;
using Bogus.Extensions;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue178 : SeededTest
   {
      [Fact]
      public void weighted_null_check()
      {
         var f = new Faker();
         var mostlyNull = Enumerable.Range(1, 100)
            .Select(n => (int?)n.OrNull(f, 0.9))
            .Count( n => !n.HasValue);

         mostlyNull.Should().BeGreaterThan(80);


         var mostlyNotNull = Enumerable.Range(1, 100)
            .Select(n => (int?)n.OrNull(f, 0.1))
            .Count(n => !n.HasValue);

         mostlyNotNull.Should().BeLessThan(20);

      }
   }
}
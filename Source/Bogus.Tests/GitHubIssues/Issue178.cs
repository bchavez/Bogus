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
            .Select(n => (int?)n.OrNull(f, 0.9f))
            .Count( n => !n.HasValue);

         mostlyNull.Should().BeGreaterThan(80);

         var mostlyNotNull = Enumerable.Range(1, 100)
            .Select(n => (int?)n.OrNull(f, 0.1f))
            .Count(n => !n.HasValue);

         mostlyNotNull.Should().BeLessThan(20);
      }

      [Fact]
      public void weighted_default_check()
      {
         var f = new Faker();
         var mostlyDefault = Enumerable.Range(1, 100)
            .Select(n => n.OrDefault(f, 0.9f))
            .Count(n => n == default);

         mostlyDefault.Should().BeGreaterThan(80);

         var mostlyNotDefault = Enumerable.Range(1, 100)
            .Select(n => n.OrDefault(f, 0.1f))
            .Count(n => n == default);

         mostlyNotDefault.Should().BeLessThan(20);

         var mostlyNotDefaultObject = Enumerable.Range(1, 100)
            .Select( n => new object())
            .Select(s => s.OrDefault(f, 0.1f))
            .Count(s => s == null);

         mostlyNotDefaultObject.Should().BeLessThan(20);
      }
   }
}
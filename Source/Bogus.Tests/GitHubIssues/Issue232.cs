using System;
using System.Linq;
using Bogus.Extensions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue232 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue232(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void default_value_checks()
      {
         var f = new Faker();
         var mostlyDefault = Enumerable.Range(1, 100)
            .Select(n => n.OrDefault(f, 0.9f, 777))
            .Count(n => n == 777);

         mostlyDefault.Should().BeGreaterThan(80);

         var mostlyNotDefault = Enumerable.Range(1, 100)
            .Select(n => n.OrDefault(f, 0.1f, 7777))
            .Count(n => n == 7777);

         mostlyNotDefault.Should().BeLessThan(20);

         var mark = new Guid("669248FB-6FFA-4912-B93E-12611266E18F");

         var mostlyMarkObjects = Enumerable.Range(1, 100)
            .Select(n => new object())
            .Select(s => s.OrDefault(f, 0.9f, mark))
            .Count(s => s is Guid g && g == mark);

         mostlyMarkObjects.Should().BeGreaterThan(80);
      }
   }
}
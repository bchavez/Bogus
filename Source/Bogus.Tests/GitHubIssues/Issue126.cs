using System.Collections.Generic;
using Bogus.Extensions;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue126 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue126(ITestOutputHelper console)
      {
         this.console = console;
      }

      public class CustomFaker<T> : Faker<T> where T : class
      {
         public List<T> Generate(int min, int max, string ruleSets = null)
         {
            var n = this.FakerHub.Random.Number(min, max);
            return this.Generate(n, ruleSets);
         }
      }

      [Fact]
      public void can_generate_random_amount_by_derived_faker()
      {
         var faker = new CustomFaker<Order>()
            .RuleFor(x => x.Item, f => f.Commerce.Product())
            .RuleFor(x => x.Quantity, f => f.Random.Number(1, 10))
            .RuleFor(x => x.OrderId, f => f.UniqueIndex)
            as CustomFaker<Order>;

         var fakes = faker.Generate(1, 10);
         fakes.Count.Should().BeGreaterOrEqualTo(1).And.BeLessOrEqualTo(10);
         console.Dump(fakes.Count);
      }

      [Fact]
      public void can_generate_random_amount_with_extension_methods()
      {
         var faker = new Faker<Order>()
            .RuleFor(x => x.Item, f => f.Commerce.Product())
            .RuleFor(x => x.Quantity, f => f.Random.Number(1, 10))
            .RuleFor(x => x.OrderId, f => f.UniqueIndex);

         var fakes = faker.Generate(1, 10);
         fakes.Count.Should().BeGreaterOrEqualTo(1).And.BeLessOrEqualTo(10);
         console.Dump(fakes.Count);
      }

      [Fact]
      public void can_generate_random_amount_with_builtin_generate_between_extension_method()
      {
         var faker = new Faker<Order>()
            .RuleFor(x => x.Item, f => f.Commerce.Product())
            .RuleFor(x => x.Quantity, f => f.Random.Number(1, 10))
            .RuleFor(x => x.OrderId, f => f.UniqueIndex);

         var fakes = faker.GenerateBetween(2, 10);
         fakes.Count.Should().BeGreaterOrEqualTo(2).And.BeLessOrEqualTo(10);
         console.Dump(fakes.Count);
      }
   }

   public static class ExtensionsForFakerT
   {
      public static List<T> Generate<T>(this Faker<T> faker, int min, int max, string ruleSets = null) where T : class
      {
         var r = new Randomizer();
         var n = r.Number(min, max);
         return faker.Generate(n, ruleSets);
      }
   }
}
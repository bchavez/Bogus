using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue143 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue143(ITestOutputHelper console)
      {
         this.console = console;
      }

      private void RunAnotherPrecedingFaker()
      {
         var fakerRule = new Faker<Models.Order>()
            .RuleFor(o => o.OrderId, f => f.IndexGlobal)
            .RuleFor(o => o.Item, f => f.Commerce.Product())
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3));

         var ordersRules = fakerRule.Generate(5);
         console.Dump(ordersRules);
      }

      [Fact]
      public void IndexGlobal_should_be_incremented_when_CustomInstantiator_is_only_used()
      {
         //Uncomment to test to see what happens if a previous
         //faker is called that influences IndexGlobal.
         //RunAnotherPrecedingFaker();
         
         var faker = new Faker<Models.Order>()
            .CustomInstantiator(f => new Models.Order
               {
                  OrderId = f.IndexGlobal,
                  Item = f.Commerce.Product(),
                  Quantity = f.Random.Number(1, 3)
               });

         var orders = faker.Generate(5);
         //console.Dump(orders);

         var oids = orders.Select(o => o.OrderId).ToList();

         var start = oids[0];

         oids.Should().Equal(start, start + 1, start + 2, start + 3, start + 4);
      }
   }
}
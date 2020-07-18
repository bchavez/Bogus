using System;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue115 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue115(ITestOutputHelper console)
      {
         this.console = console;
      }

      public class Customer
      {
         public string Name { get; set; }
         public int OrderId { get; set; }
         public Order Order { get; set; }
      }
      
      [Fact]
      public void should_throw_with_nested_expression()
      {
         Action fakerMaker = () => new Faker<Customer>()
            .RuleFor(o => o.Order.Item, f => f.Random.Int(1, 1000).ToString());

         fakerMaker.Should().Throw<ArgumentException>();

      }

      [Fact]
      public void calling_finish_with_twice_is_okay()
      {
         var productCalled = false;
         var colorCalled = false;

         var faker = new Faker<Order>()
            .RuleFor(o => o.OrderId, f => f.Random.Number(1,50))
            .FinishWith((f, o) =>
               {
                  productCalled = true;
                  o.Item = f.Commerce.Product();
               })
            .FinishWith((f, o) =>
               {
                  colorCalled = true;
                  o.Item = f.Commerce.Color();
               });

         var order = faker.Generate();

         order.Item.Should().Be("yellow");
         
         //sanity check
         productCalled.Should().BeFalse();
         colorCalled.Should().BeTrue();

         console.Dump(order);
      }
   }
}

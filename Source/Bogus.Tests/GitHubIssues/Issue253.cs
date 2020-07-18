using System;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue253 : SeededTest
   {
      [Fact]
      public void can_use_advanced_namespace_rulefor_string()
      {
         var orderFaker = new Faker<Order>()
            .RuleFor(nameof(Order.OrderId), f => f.IndexVariable++)
            .RuleFor(nameof(Order.Quantity), f => f.Random.Number(1, 3))
            .RuleFor(nameof(Order.Item), f => f.Commerce.Product());

         var order = orderFaker.Generate();

         order.OrderId.Should().Be(0);
         order.Quantity.Should().Be(2);
         order.Item.Should().Be("Computer");
      }

      [Fact]
      public void rulefor_a_field_that_doesnt_exist_throws()
      {
         var orderFaker = new Faker<Order>()
            .StrictMode(true)
            .RuleFor(nameof(Order.OrderId), f => f.IndexVariable++)
            .RuleFor(nameof(Order.Quantity), f => f.Random.Number(1, 3))
            .RuleFor(nameof(Order.Item), f => f.Commerce.Product());
         

         Action act  = () => orderFaker.RuleFor("fffff", f => f.Random.Number());

         act.Should().Throw<ArgumentException>();
      }

      [Fact]
      public void ignoring_a_field_that_doesnt_exist_throws()
      {
         var orderFaker = new Faker<Order>()
            .StrictMode(true)
            .RuleFor(nameof(Order.OrderId), f => f.IndexVariable++)
            .RuleFor(nameof(Order.Quantity), f => f.Random.Number(1, 3))
            .RuleFor(nameof(Order.Item), f => f.Commerce.Product());

         orderFaker.Ignore(nameof(Order.Item));

         var o = orderFaker.Generate();

         o.Item.Should().BeNull();

         Action act = () => orderFaker.RuleFor("hhhhh", f => f.Random.Number());

         act.Should().Throw<ArgumentException>();
      }

      [Fact]
      public void should_be_able_to_use_rulefor_with_typeT()
      {
         var orderFaker = new Faker<Order>()
            .RuleFor(nameof(Order.OrderId), f => f.IndexVariable++)
            .RuleFor(nameof(Order.Quantity), f => f.Random.Number(1, 3))
            .RuleFor(nameof(Order.Item), (f, o) => o.OrderId + f.Commerce.Product());

         var order = orderFaker.Generate();

         order.OrderId.Should().Be(0);
         order.Quantity.Should().Be(2);
         order.Item.Should().Be("0Computer");
      }
   }
}

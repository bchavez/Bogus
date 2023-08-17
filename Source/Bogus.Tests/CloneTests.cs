using FluentAssertions;
using System;
using Xunit;

namespace Bogus.Tests
{
   public class CloneTests : SeededTest
   {
      public class Order
      {
         public int OrderId { get; set; }
         public string Item { get; set; }
         public int Quantity { get; set; }
         public int? LotNumber { get; set; }
         public DateTime Created { get; set; }
      }

      [Fact]
      public void can_create_a_simple_clone()
      {
         var orderFaker = new Faker<Order>()
            .UseSeed(88)
            .UseDateTimeReference(new DateTime(2022, 2, 2))
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product())
            .RuleFor(o => o.Created, f => f.Date.Recent());

         var clone = orderFaker.Clone();

         var clonedOrder = clone.Generate();
         var rootOrder = orderFaker.Generate();

         clonedOrder.Should().BeEquivalentTo(rootOrder);
         clonedOrder.Created.Should().BeAtLeast(TimeSpan.FromDays(1));
      }

      [Fact]
      public void clone_has_different_rules()
      {
         var rootFaker = new Faker<Order>()
            .UseSeed(88)
            .UseDateTimeReference(new DateTime(2022, 2, 2))
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product())
            .RuleFor(o => o.Created, f => f.Date.Recent());

         var cloneFaker = rootFaker.Clone()
            .RuleFor(o => o.Quantity, f => f.Random.Number(4, 6));

         var rootOrder = rootFaker.Generate();
         var clonedOrder = cloneFaker.Generate();

         rootOrder.Quantity.Should()
            .BeGreaterOrEqualTo(1).And
            .BeLessOrEqualTo(3);

         clonedOrder.Quantity.Should()
            .BeGreaterOrEqualTo(4).And
            .BeLessOrEqualTo(6);
      }
   }
}
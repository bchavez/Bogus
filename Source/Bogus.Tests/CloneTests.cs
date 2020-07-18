using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class CloneTests : SeededTest
   {
      [Fact]
      public void can_create_a_simple_clone()
      {
         var orderFaker = new Faker<Examples.Order>()
            .UseSeed(88)
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product());

         var clone = orderFaker.Clone();

         var clonedOrder = clone.Generate();

         var rootOrder = orderFaker.Generate();

         clonedOrder.Should().BeEquivalentTo(rootOrder);
      }

      [Fact]
      public void clone_has_different_rules()
      {
         var rootFaker = new Faker<Examples.Order>()
            .UseSeed(88)
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product());

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
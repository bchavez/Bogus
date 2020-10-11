using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class ValidationDeltaTests : SeededTest
   {
      [Fact]
      public void should_be_valid_and_not_throw_exception_on_complete_rules_with_ignore()
      {
         var testOrders = new Faker<Examples.Order>()
            .StrictMode(true)
            .Ignore(o => o.Item)
            .Ignore(o => o.LotNumber)
            .RuleFor(o => o.OrderId, f => 3343)
            .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));
         var result = testOrders.Validate();
         testOrders.AssertConfigurationIsValid();
         result.Should().BeTrue();
      }

      [Fact]
      public void should_not_be_valid_and_throw_exception_on_incomplete_rules()
      {
         var testOrders = new Faker<Examples.Order>()
            .StrictMode(true)
            .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));
         var result = testOrders.Validate();
         Assert.Throws<ValidationException>(() => testOrders.AssertConfigurationIsValid());
         result.Should().BeFalse();
      }

      [Fact]
      public void should_throw_exception_on_incomplete_rules()
      {
         var testOrders = new Faker<Examples.Order>()
            .StrictMode(true)
            .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));
         Assert.Throws<ValidationException>(() => testOrders.AssertConfigurationIsValid());
      }

      [Fact]
      public void should_be_valid_and_no_exceptions_on_complete_rules()
      {
         var testOrders = new Faker<Examples.Order>()
               .StrictMode(true)
               .RuleFor(o => o.LotNumber, f => 28)
               .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5))
               .RuleFor(o => o.OrderId, f => f.Random.Number(2, 5))
               .RuleFor(o => o.Item, f => f.Lorem.Sentence())
            ;
         var result = testOrders.Validate();
         testOrders.AssertConfigurationIsValid();
         result.Should().BeTrue();
      }

      [Fact]
      public void should_be_valid_no_exceptions_on_incomplete_rules_when_strict_false()
      {
         var testOrders = new Faker<Examples.Order>()
            .StrictMode(false)
            .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));
         var result = testOrders.Validate();
         testOrders.AssertConfigurationIsValid();
         result.Should().BeTrue();
      }
   }
}
using System;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.FakerTests
{
    public class StrictModeTests
    {
        [Fact]
        public void should_throw_exception_on_incomplete_rules()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));

            testOrders.Invoking(faker => faker.Generate())
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void should_not_throw_exception_on_complete_rule_set()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5))
                .RuleFor(o => o.Item, f => f.Lorem.Sentence())
                .RuleFor(o => o.OrderId, f => f.Random.Number());

            testOrders.Invoking(faker => faker.Generate())
                .ShouldNotThrow<InvalidOperationException>();
        }

        [Fact]
        public void cannot_use_rules_with_strictmode()
        {
            var faker = new Faker<Examples.Order>()
                .Rules((f, o) =>
                    {
                        o.Quantity = f.Random.Number(1, 4);
                        o.Item = f.Commerce.Product();
                        o.OrderId = 25;
                    })
                .StrictMode(true);

            Action act = () => faker.AssertConfigurationIsValid();
            act.ShouldThrow<ValidationException>();

            var faker2 = new Faker<Examples.Order>()
                .StrictMode(true)
                .Rules((f, o) =>
                    {
                        o.Quantity = f.Random.Number(1, 4);
                        o.Item = f.Commerce.Product();
                        o.OrderId = 25;
                    });

            Action act2 = () => faker2.AssertConfigurationIsValid();
            act2.ShouldThrow<ValidationException>();
        }
    }
}
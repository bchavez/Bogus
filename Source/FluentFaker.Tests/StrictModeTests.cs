using System;
using FluentAssertions;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    [TestFixture]
    public class StrictModeTests
    {
        [Test]
        public void should_throw_exception_on_incomplete_rules()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));

            testOrders.Invoking(faker => faker.Generate())
                .ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void should_not_throw_exception_on_complete_rule_set()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5))
                .RuleFor(o => o.Item, f => f.Lorem.Sentance())
                .RuleFor(o => o.OrderId, f => f.Random.Number());

            testOrders.Invoking(faker => faker.Generate())
                .ShouldNotThrow<InvalidOperationException>();
        }
    }
}
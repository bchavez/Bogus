using System;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class ValidationDeltaTests
    {
        [Test]
        public void should_return_validate_false_on_incomplete_rules()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));

            var result = testOrders.Validate();

            result.ShouldBeEquivalentTo(false);
        }

        [Test]
        public void should_return_validate_false_on_complete_rules()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5))
                .RuleFor(o => o.OrderId, f => f.Random.Number(2, 5))
                .RuleFor(o => o.Item, f => f.Lorem.Sentence())
                ;

            var result = testOrders.Validate();

            result.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void should_return_validate_false_and_delta_on_incomplete_rules()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));

            string [] missingPropsOrFields;
            var result = testOrders.Validate(out missingPropsOrFields);

            missingPropsOrFields.Length.Should().BeGreaterThan(0);

            result.ShouldBeEquivalentTo(false);
        }

        [Test]
        public void should_return_validate_true_and_delta_on_incomplete_rules_when_strict_true()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(false)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5));

            string[] missingPropsOrFields;
            var result = testOrders.Validate(out missingPropsOrFields);

            // missing props are proposed even if strict mode is false
            missingPropsOrFields.Length.Should().BeGreaterThan(0);

            result.ShouldBeEquivalentTo(true);
        }

        [Test]
        public void should_return_validate_false_and_delta_on_complete_rules()
        {
            var testOrders = new Faker<Examples.Order>()
                .StrictMode(true)
                .RuleFor(o => o.Quantity, f => f.Random.Number(2, 5))
                .RuleFor(o => o.OrderId, f => f.Random.Number(2, 5))
                .RuleFor(o => o.Item, f => f.Lorem.Sentence())
                ;

            string[] missingPropsOrFields;
            var result = testOrders.Validate(out missingPropsOrFields);
            missingPropsOrFields.Length.Should().Equals(0);

            result.ShouldBeEquivalentTo(true);
        }
    }
}

using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class RuleSetTests : SeededTest
    {
        [SetUp]
        public void BeforeEachTest()
        {
            Faker.DefaultStrictMode = false;
        }


        public class Customer
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public bool GoodCustomer { get; set; }
        }

        [Test]
        public void should_be_able_to_create_a_rule_set()
        {
            var orderIds = 0;
            var testCustomers = new Faker<Customer>()
                .RuleSet("Good",
                    (set) =>
                        {
                            set.StrictMode(true);
                            set.RuleFor(c => c.Id, f => orderIds++);
                            set.RuleFor(c => c.Description, f => f.Lorem.Sentence());
                            set.RuleFor(c => c.GoodCustomer, f => true);
                        })
                .StrictMode(true)
                .RuleFor(c => c.Id, f => orderIds++)
                //.RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.GoodCustomer, f => false);

            var results = testCustomers.Generate(5, "Good");

            results.All(s => s.GoodCustomer).Should().BeTrue();

            results.Dump();
        }

        [Test]
        public void should_be_able_to_run_two_rules_with_last_one_taking_presidence()
        {
            var orderIds = 0;
            var testCustomers = new Faker<Customer>()
                .RuleSet("Good",
                    (set) =>
                    {
                        set.StrictMode(true);
                        set.RuleFor(c => c.Id, f => orderIds++);
                        set.RuleFor(c => c.Description, f => f.Lorem.Sentence());
                        set.RuleFor(c => c.GoodCustomer, f => true);
                    })
                .StrictMode(true)
                .RuleFor(c => c.Id, f => orderIds++)
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.GoodCustomer, f => false);

            var results = testCustomers.Generate(5, "default,Good");

            results.All(s => s.GoodCustomer).Should().BeTrue();

            results.Dump();
        }

        [Test]
        public void should_be_able_to_run_default_ruleset()
        {
            var orderIds = 0;
            var testCustomers = new Faker<Customer>()
                .RuleSet("Good",
                    (set) =>
                    {
                        set.StrictMode(true);
                        set.RuleFor(c => c.Id, f => orderIds++);
                        set.RuleFor(c => c.Description, f => f.Lorem.Sentence());
                        set.RuleFor(c => c.GoodCustomer, f => true);
                    })
                .StrictMode(true)
                .RuleFor(c => c.Id, f => orderIds++)
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.GoodCustomer, f => false);

            var results = testCustomers.Generate(5);

            results.All(s => s.GoodCustomer).Should().BeFalse();

            results.Dump();
        }

        [Test]
        public void should_throw_error_when_strict_mode_is_set()
        {
            var orderIds = 0;
            var testCustomers = new Faker<Customer>()
                .RuleSet("Good",
                    (set) =>
                        {
                            set.StrictMode(true);
                            set.RuleFor(c => c.Id, f => orderIds++);
                            set.RuleFor(c => c.GoodCustomer, f => true);
                        })
                .StrictMode(true)
                .RuleFor(c => c.Id, f => orderIds++)
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.GoodCustomer, f => false);

            Action act = () =>
                {
                    var goodCustomers = testCustomers.Generate(5, "Good");
                    goodCustomers.Dump();
                };
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void should_be_able_to_override_existing_rules()
        {
            var testCustomers = new Faker<Customer>()
                .RuleSet("Good",
                    (set) =>
                        {
                            set.RuleFor(c => c.Description, f => f.Lorem.Sentence());
                            set.RuleFor(c => c.Description, f => "overridden");
                        });

            var results = testCustomers.Generate(5, "Good");

            results.Should().OnlyContain(c => c.Description == "overridden");
        }


    }

}
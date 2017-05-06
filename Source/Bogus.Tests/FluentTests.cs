using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using Bogus.DataSets;
using Bogus.Extensions;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class Examples : SeededTest
    {
        [Test]
        public void TestAPIDesign()
        {
            //Set the randomzier seed if you wish to generate repeatable data sets.
            Randomizer.Seed = new Random(3897234);

            var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

            var orderIds = 0;
            var testOrders = new Faker<Order>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode if you prefer.
                .StrictMode(true)
                //OrderId is deterministic
                .RuleFor(o => o.OrderId, f => orderIds++)
                //Pick some fruit from a basket
                .RuleFor(o => o.Item, f => f.PickRandom(fruit))
                //A random quantity from 1 to 10
                .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));



            var userIds = 0;
            var testUsers = new Faker<User>()
                //Optional: Call for objects that have complex initialization
                .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))

                //Basic rules using built-in generators
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")
                .RuleFor(u => u.SomeGuid, Guid.NewGuid)

                //Use an enum outside scope.
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                //Use a method outside scope.
                .RuleFor(u => u.CartId, f => Guid.NewGuid())
                //Compound property with context, use the first/last name properties
                .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
                //And composability of a complex collection.
                .RuleFor(u => u.Orders, f => testOrders.Generate(3).ToList())
                //After all rules are applied finish with the following action
                .FinishWith((f, u) =>
                    {
                        Console.WriteLine("User Created! Name={0}", u.FullName);
                    });

            //RunAfter
            //InvokeAfterRules
            //FinalizeWith
            //AfterRulesInvoke
            //PostProcess
            //InvokeAfterAll
            //BeforeReturn
            //FinallyInvoke
            //FinishWith
            
            var user = testUsers.Generate(3);

            user.Dump();
        }

        [Test]
        public void Without_Fluent_Syntax()
        {
            var random = new Randomizer();
            var lorem = new Lorem();
            var o = new Order()
                {
                    OrderId = random.Number(1, 100),
                    Item = lorem.Sentence(),
                    Quantity = random.Number(1, 10)
                };
            o.OrderId.Should().Be(61);
            o.Quantity.Should().Be(7);
            o.Dump();
        }

        [Test]
        public void With_Faker_Facade()
        {
            var faker = new Faker("en");
            var o = new Order()
                {
                    OrderId = faker.Random.Number(1, 100),
                    Item = faker.Lorem.Sentence(),
                    Quantity = faker.Random.Number(1, 10)
                };
            o.OrderId.Should().Be(61);
            o.Quantity.Should().Be(7);
            o.Dump();
        }

        [Test]
        public void With_Korean_Locale()
        {
            var lorem = new Lorem(locale: "ko");
            Console.WriteLine(lorem.Sentence(5));
        }

        [Test]
        public void Create_Context_Related_Person()
        {
            var person = new Person();

            person.Dump();
        }

        [Test]
        public void Create_an_SSN()
        {
            var ssn = new Randomizer().Replace("###-##-####");
            ssn.Dump();

            var code = new Randomizer().Replace("##? ??? ####");
            code.Dump();
        }




        [Test]
        public void Handlebar()
        {
            var faker = new Faker();
            var randomName = faker.Parse("{{name.lastName}}, {{name.firstName}} {{name.suffix}}");
            randomName.Dump();
        }

        [Test]
        public void TestIgnore()
        {
            var faker = new Faker<Order>()
                .StrictMode(true)
                .Ignore(o => o.Item)
                .RuleFor(o => o.OrderId, f => 3343)
                .RuleFor(o => o.Quantity, f => f.Random.Number(3));

            var fake = faker.Generate();

            fake.Dump();

            fake.Item.Should().BeNull();
        }

        [Test]
        public void Can_Define_Rule_By_Type()
        {
            var faker = new Faker<Issue47>()
                .RuleForType(typeof(string), f => f.Random.Word());

            var fake = faker.Generate();

            fake.Dump();

            fake.Bake.Should().NotBeNullOrEmpty();
            fake.Make.Should().NotBeNullOrEmpty();
            fake.Fake.Should().NotBeNullOrEmpty();
            fake.Bar.Should().NotBeNullOrEmpty();
            fake.Foo.Should().NotBeNullOrEmpty();
            fake.Baz.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Should_Throw_Exception_If_RuleForType_Types_Dont_Match()
        {
            Action action = () =>
                {
                    var faker = new Faker<Issue47>()
                        .RuleForType(typeof(int), f => f.Random.Word());
                };

            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void implicit_operator_test()
        {
            var orderFaker = new Faker<Order>()
                .RuleFor(o => o.OrderId, f => f.IndexVariable++)
                .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
                .RuleFor(o => o.Item, f => f.Commerce.Product());

            Order testOrder1 = orderFaker;
            Order testOrder2 = orderFaker;
            Order testOrder3 = orderFaker;

            testOrder1.Dump();
            testOrder2.Dump();
            testOrder3.Dump();

            var threeOrders = new[] {testOrder1, testOrder2, testOrder3};
            threeOrders.Select(o => o.Item).Should().ContainInOrder("Computer", "Tuna", "Soap");
            threeOrders.Select(o => o.Quantity).Should().ContainInOrder(2, 3, 1);

            var testOrders = Enumerable.Range(1, 3)
                .Select(x => (Order)orderFaker)
                .ToArray();

            testOrders.Dump();

            testOrders.Select(o => o.Item).Should().ContainInOrder("Chicken", "Gloves", "Mouse");
            testOrders.Select(o => o.Quantity).Should().ContainInOrder(1, 2, 3);
        }

        [Test]
        public void can_clamp_string_length()
        {
            var c = new Company();

            var cnames = Enumerable.Range(1, 200).Select(i => c.CompanyName(0).ClampLength(12, 15));

            cnames.Any(name => name.EndsWith(" ")).Should().BeFalse();
            cnames.Dump();
        }

        public class Issue47
        {
            public string Foo { get; set; }
            public string Bar { get; set; }
            public string Baz { get; set; }
            public string Bake;
            public string Make;
            public string Fake;
        }

        public class Order
        {
            public int OrderId { get; set; }
            public string Item { get; set; }
            public int Quantity { get; set; }

        }

        public enum Gender
        {
            Male,
            Female
        }

        public class User
        {
            public User(int userId, string ssn)
            {
                this.Id = userId;
                this.SSN = ssn;
            }

            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string SomethingUnique { get; set; }
            public Guid SomeGuid { get; set; }

            public string Avatar { get; set; }
            public Guid CartId { get; set; }
            public string SSN { get; set; }
            public Gender Gender { get; set; }

            public List<Order> Orders { get; set; }
        }

        [Test]
        public void just_want_to_set_a_value()
        {
            var faker = new Faker<Order>()
                .RuleFor(o => o.OrderId, 25)
                .RuleFor(o => o.Item, "foo");

            faker.Generate().OrderId.Should().Be(25);
            faker.Generate().Item.Should().Be("foo");

        }

        [Test]
        public void create_rule_for_an_object()
        {
            var faker = new Faker<Order>()
                .Rules((f, o) =>
                    {
                        o.Quantity = f.Random.Number(1, 4);
                        o.Item = f.Commerce.Product();
                        o.OrderId = 25;
                    });

            Order result = faker;
            result.Item.Should().Be("Computer");
            result.OrderId.Should().Be(25);
            result.Quantity.Should().BeInRange(1, 4);

        }

        [Test]
        public void can_create_rule_for_object_multiple_times()
        {
            var faker = new Faker<Order>()
                .Rules((f, o) =>
                    {
                        o.Quantity = f.Random.Number(1, 4);
                        o.Item = f.Commerce.Product();
                        o.OrderId = 25;
                    });
            
            var faker2 = faker.Rules((f, o) =>
                {
                    o.OrderId = 26;
                });

            Order result = faker2;
            result.OrderId.Should().Be(26);
            result.Item.Should().Be("Computer");
        }

        [Test]
        public void cannot_use_rules_with_strictmode()
        {
            var faker = new Faker<Order>()
                .Rules((f, o) =>
                    {
                        o.Quantity = f.Random.Number(1, 4);
                        o.Item = f.Commerce.Product();
                        o.OrderId = 25;
                    })
                .StrictMode(true);

            Action act =()=> faker.AssertConfigurationIsValid();
            act.ShouldThrow<ValidationException>();
        }
    }

}

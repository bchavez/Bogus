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



    }





}

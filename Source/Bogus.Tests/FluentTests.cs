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
        public void get_all_locales()
        {
            var data = Database.Data.Value;

            var locales = new List<string>();
            
            foreach( var prop in data.Properties().OrderBy( p => p.Name) )
            {
                var code = prop.Name;
                var title = prop.First["title"];

                var str = string.Format("|{0,-14}|{1}", "`"+code+"`", title);
                locales.Add(str);
            }

            Console.WriteLine(string.Join("\n", locales));
        }

        [Test]
        public void get_available_methods()
        {
            var x = XElement.Load(@"Bogus.XML");
            var json = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeXNode(x));

            var all = json.SelectTokens("doc.members.member").SelectMany(jt => jt)
                .Select(m =>
                    {
                        var member = m["@name"];
                        var summary = m["summary"];
                        if( member == null || summary == null ) return null;

                        var declare = member.ToString();
                        var argPos = declare.IndexOf('(');
                        if( argPos > 0 )
                        {
                            declare = declare.Substring(0, argPos);
                        }
                        if( !declare.StartsWith("M:Bogus.DataSets.") ) return null;

                        var method = declare.TrimStart('M', ':');
                        method = method.Replace("Bogus.DataSets.", "");

                        var methodSplit = method.Split('.');

                        var dataset = methodSplit[0];
                        var call = methodSplit[1];

                        if( call == "#ctor" ) return null;

                        return new {dataset = dataset, method = call, summary = summary.ToString().Trim()};
                    })
                .Where(a => a != null)
                .GroupBy(k => k.dataset)
                .OrderBy(k => k.Key);

            foreach( var g in all )
            {
                Console.WriteLine("* **`" + g.Key+"`**");
                foreach( var m in g )
                {
                    Console.WriteLine("\t* `" + m.method+"` - " + m.summary);
                }
            }
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
            public string Avatar { get; set; }
            public Guid CartId { get; set; }
            public string SSN { get; set; }
            public Gender Gender { get; set; }

            public List<Order> Orders { get; set; }
        }



    }





}

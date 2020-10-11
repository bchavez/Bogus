using System;
using System.Collections.Generic;
using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class Examples : SeededTest
   {
      [Fact]
      public void TestAPIDesign()
      {
         //Set the randomzier seed if you wish to generate repeatable data sets.
         Randomizer.Seed = new Random(3897234);

         var fruit = new[] {"apple", "banana", "orange", "strawberry", "kiwi"};

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
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
            //A nullable int? with 80% probability of being null.
            //The .OrNull extension is in the Bogus.Extensions namespace.
            .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));

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
            .RuleFor(u => u.Orders, f => testOrders.Generate(3))
            //After all rules are applied finish with the following action
            .FinishWith((f, u) => { Console.WriteLine("User Created! Name={0}", u.FullName); });

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

      [Fact]
      public void Without_Fluent_Syntax()
      {
         var random = new Randomizer();
         var lorem = new Lorem("en");
         var o = new Order
            {
               OrderId = random.Number(1, 100),
               Item = lorem.Sentence(),
               Quantity = random.Number(1, 10)
            };
         o.OrderId.Should().Be(61);
         o.Quantity.Should().Be(7);
         o.Dump();
      }

      [Fact]
      public void With_Faker_Facade()
      {
         var faker = new Faker("en");
         var o = new Order
            {
               OrderId = faker.Random.Number(1, 100),
               Item = faker.Lorem.Sentence(),
               Quantity = faker.Random.Number(1, 10)
            };
         o.OrderId.Should().Be(61);
         o.Quantity.Should().Be(7);
         o.Dump();
      }

      public class OrderFaker : Faker<Order>
      {
         public OrderFaker() : base("en")
         {
            RuleFor(o => o.OrderId, f => f.Random.Number(1, 100));
            RuleFor(o => o.Item, f => f.Lorem.Sentence());
            RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));
         }
      }

      [Fact]
      public void Using_FakerT_Inheritance()
      {
         var orderFaker = new OrderFaker();
         var o = orderFaker.Generate();
         o.Dump();
      }

      [Fact]
      public void With_Korean_Locale()
      {
         var lorem = new Lorem(locale: "ko");
         Console.WriteLine(lorem.Sentence(5));
      }

      [Fact]
      public void Create_Context_Related_Person()
      {
         var person = new Person();

         person.Dump();
      }

      [Fact]
      public void Create_an_SSN()
      {
         var ssn = new Randomizer().Replace("###-##-####");
         ssn.Dump();

         var code = new Randomizer().Replace("##? ??? ####");
         code.Dump();
      }


      [Fact]
      public void Handlebar()
      {
         var faker = new Faker();
         var randomName = faker.Parse("{{name.lastName}}, {{name.firstName}} {{name.suffix}}");
         randomName.Dump();
      }

      [Fact]
      public void TestIgnore()
      {
         var faker = new Faker<Order>()
            .StrictMode(true)
            .Ignore(o => o.Item)
            .Ignore(o => o.LotNumber)
            .RuleFor(o => o.OrderId, f => 3343)
            .RuleFor(o => o.Quantity, f => f.Random.Number(3));

         var fake = faker.Generate();

         fake.Dump();

         fake.Item.Should().BeNull();
         fake.LotNumber.Should().BeNull();
      }

      [Fact]
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

      [Fact]
      public void Should_Throw_Exception_If_RuleForType_Types_Dont_Match()
      {
         Action action = () =>
            {
               var faker = new Faker<Issue47>()
                  .RuleForType(typeof(int), f => f.Random.Word());
            };

         action.Should().Throw<ArgumentException>();
      }

      [Fact]
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

      [Fact]
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
         public int? LotNumber { get; set; }
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

      [Fact]
      public void just_want_to_set_a_value()
      {
         var faker = new Faker<Order>()
            .RuleFor(o => o.OrderId, 25)
            .RuleFor(o => o.Item, "foo");

         faker.Generate().OrderId.Should().Be(25);
         faker.Generate().Item.Should().Be("foo");
      }

      [Fact]
      public void create_rules_for_an_object_the_easy_way()
      {
         var faker = new Faker<Order>()
            .Rules((f, o) =>
               {
                  o.Quantity = f.Random.Number(1, 4);
                  o.Item = f.Commerce.Product();
                  o.OrderId = 25;
               });

         Order result = faker;
         result.Dump();
         result.Item.Should().Be("Computer");
         result.OrderId.Should().Be(25);
         result.Quantity.Should().BeInRange(1, 4);
      }

      [Fact]
      public void can_create_rule_for_object_multiple_times()
      {
         var faker = new Faker<Order>()
            .Rules((f, o) =>
               {
                  o.Quantity = f.Random.Number(1, 4);
                  o.Item = f.Commerce.Product();
                  o.OrderId = 25;
               });

         var faker2 = faker.Rules((f, o) => { o.OrderId = 26; });

         Order result = faker2;
         result.OrderId.Should().Be(26);
         result.Item.Should().Be("Computer");
      }

      public enum Colors
      {
         Red,
         Blue,
         Green
      }

      [Fact]
      public void pick_random_exclude()
      {
         var faker = new Faker();
         var m = faker.PickRandomWithout(Colors.Red, Colors.Green);
         m.Should().Be(Colors.Blue);
      }

      [Fact]
      public void pick_a_random_enum_only_from_the_param_list()
      {
         var faker = new Faker();
         var f = faker.PickRandom(Colors.Red, Colors.Green);
         f.Should().Be(Colors.Green);
      }

      [Fact]
      public void can_pick_a_random_item_from_parameter_list()
      {
         var faker = new Faker();
         var pet = faker.PickRandom("cat", "dog", "fish");
         pet.Should().Be("dog");
      }

      [Fact]
      public void can_pick_a_random_item_from_parameter_list2()
      {
         var faker = new Faker();
         var n = faker.PickRandomParam(1, 2, 3);
         n.Should().Be(2);
      }

      [Fact]
      public void can_pick_a_random_number_int32array()
      {
         var numbers = new[] {1, 2, 3};
         var faker = new Faker();
         var n = faker.PickRandom(numbers);
         n.Should().BeOneOf(1, 2, 3);
      }

      [Fact]
      public void can_pick_random_item_of_linq_of_things()
      {
         var numbers = Enumerable.Range(1, 3).Select(i => i);
         var faker = new Faker();
         var n = faker.PickRandom(numbers);
         n.Should().BeOneOf(1, 2, 3);
      }

      [Fact]
      public void can_generate_forever()
      {
         var orderFaker = new Faker<Order>()
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product());

         var source = orderFaker.GenerateForever();
         var count = 0;
         foreach( var order in source )
         {
            order.Item.Should().NotBeNullOrWhiteSpace();
            order.Quantity.Should().BeInRange(1, 3);
            order.OrderId.Should().BeGreaterOrEqualTo(0);
            count++;
            if( count > 99 ) break;
         }
      }
   }
}

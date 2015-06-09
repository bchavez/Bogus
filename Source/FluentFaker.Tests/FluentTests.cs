//API DESIGN
//new Faker<User>()
//    .RuleFor(u => u.FirstName, (Name n) => n.FirstName())
//    .RuleFor(u => u.UserName, (Internet i) => i.UserName())
//    .RuleFor(u => u.Email, (Internet i) => i.Email());

//new Faker<User>().RuleFor(u => u.FirstName).Use<Name>(n => n.FirstName())
//    .RuleFor(u => u.UserName).Use<Internet>(i => i.UserName())
//    .RuleFor( u => u.Email).Use<Internet>( i => i.UserName());

using System;
using System.Collections.Generic;
using System.Linq;
using FluentFaker.Generators;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    [TestFixture]
    public class Examples : SeededTest
    {
        [Test]
        public  void TestAPIDesign()
        {
            //Set the randomzier seed if you wish to generate repeatable data sets.
            Randomizer.Seed = new Random(3897234);

            var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

            var orderIds = 0;
            var testOrders = new Faker<Order>()
                //Ensure that all 
                .StrictMode(false)
                //OrderId is deterministic
                .RuleFor(o => o.OrderId, f => orderIds++)
                //Pick some fruit from a basket
                .RuleFor(o => o.Item, f => f.PickRandom(fruit))
                //A random quantity from 1 to 10
                .RuleFor(o => o.Quantity, f => f.Random.Number(1,10));


            var userIds = 0;
            var testUsers = new Faker<User>()
                // Optional: Call for objects that have complex initialization
                .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))

                // Basic rules using built-in generators
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor( u=> u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))

                // Use an enum outside scope.
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                // Use a method outside scope.
                .RuleFor(u => u.CartId, f => Guid.NewGuid())
                // Compound property with context, use the first name + random last name
                .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
                // And finally, composability of a complex collection.
                .RuleFor(u => u.Orders, f => testOrders.Generate(3).ToList());
            
            var user = testUsers.Generate();

            user.Dump();

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
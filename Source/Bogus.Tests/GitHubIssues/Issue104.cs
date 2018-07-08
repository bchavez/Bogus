using System.Collections.Generic;
using System.Linq;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue104 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue104(ITestOutputHelper console)
      {
         this.console = console;
      }

      public class Order2 : Order
      {
         public int Tax { get; set; }
      }

      [Fact]
      public void without_derived_faker()
      {
         var rootSeed = 0;
         var faker1 = new Faker<Order>()
            .UseSeed(rootSeed++)
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Item, f => f.Commerce.Product())
            .RuleFor(o => o.Quantity, f => f.Random.Int(1, 5));

         faker1.FinishWith((f, o) =>
            {
               faker1.UseSeed(rootSeed++);
            });


         var orders1 = faker1.Generate(3);
         console.Dump(orders1);
         CheckSequence(orders1);
         

         rootSeed = 0;
         var faker2 = new Faker<Order2>()
            .UseSeed(rootSeed++)
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Item, f => f.Commerce.Product())
            .RuleFor(o => o.Quantity, f => f.Random.Int(1, 5))
            .RuleFor(o => o.Tax, f=> f.Random.Int(9, 15) );

         faker2.FinishWith((f, o) =>
            {
               faker2.UseSeed(rootSeed++);
            });

         var orders2 = faker2.Generate(3);

         CheckSequence(orders2);

         console.Dump(orders2);
      }

      [Fact]
      public void adding_new_property_should_not_change_subsequent_items()
      {
         var faker1 = new CustomFaker<Order>()
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Item, f => f.Commerce.Product())
            .RuleFor(o => o.Quantity, f => f.Random.Int(1, 5));

         var orders1 = faker1.Generate(3);
         console.Dump(orders1);
         CheckSequence(orders1);

         var faker2 = new CustomFaker<Order2>()
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Item, f => f.Commerce.Product())
            .RuleFor(o => o.Quantity, f => f.Random.Int(1, 5))
            .RuleFor(o => o.Tax, f => f.Random.Int(9, 15));

         var orders2 = faker2.Generate(3);
         console.Dump(orders2);
         CheckSequence(orders2);
      }


      void CheckSequence(IEnumerable<Order> orders)
      {
         orders.Select(o => o.OrderId).Should().Equal(0, 1, 2);
         orders.Select(o => o.Item).Should().Equal("Fish", "Bike", "Cheese");
         orders.Select(o => o.Quantity).Should().Equal(5, 1, 3);
      }
      
   }

   public class CustomFaker<T> : Faker<T> where T : class
   {
      private int seed;
      protected override void PopulateInternal(T instance, string[] ruleSets)
      {
         this.UseSeed(seed++);
         base.PopulateInternal(instance, ruleSets);
      }
   }
}
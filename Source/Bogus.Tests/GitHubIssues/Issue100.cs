using System;
using System.Linq;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue100 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue100(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void dataset_test()
      {
         var c = new Commerce
            {
               Random = new Randomizer(54321)
            };

         c.Product().Should().Be("Shirt");
         Randomizer.Seed = new Random(7331);
         c.Product().Should().Be("Soap");

         Randomizer.Seed = new Random(1337);
         c.Random = new Randomizer(54321);

         c.Product().Should().Be("Shirt");
         Randomizer.Seed = new Random(3173);
         c.Product().Should().Be("Soap");
      }

      [Fact]
      public void faker_test()
      {
         var f = new Faker
            {
                Random = new Randomizer(54321)
            };

         f.Commerce.Product().Should().Be("Shirt");
         Randomizer.Seed = new Random(7331);
         f.Commerce.Product().Should().Be("Soap");

         Randomizer.Seed = new Random(1337);
         f.Random = new Randomizer(54321);

         f.Commerce.Product().Should().Be("Shirt");
         Randomizer.Seed = new Random(3173);
         f.Commerce.Product().Should().Be("Soap");
      }

      [Fact]
      public void randomizer_twice()
      {
         var r = new Randomizer(88);
         Enumerable.Range(1, 4)
            .Select(i => r.Number(1, 3))
            .Should().Equal(3, 2, 1, 2);

         r = new Randomizer(88);
         Enumerable.Range(1, 4)
            .Select(i => r.Number(1, 3))
            .Should().Equal(3, 2, 1, 2);
      }

      [Fact]
      public void faker_test_2()
      {
         var f = new Faker();
         f.Random = new Randomizer(88);

         Enumerable.Range(1, 4)
            .Select(i => f.Random.Number(1, 3))
            .Should().Equal(3, 2, 1, 2);

         f.Random = new Randomizer(88);

         Enumerable.Range(1, 4)
            .Select(i => f.Random.Number(1, 3))
            .Should().Equal(3, 2, 1, 2);
      }

      [Fact]
      public void simple_faker_t_test()
      {
         var orderFaker = new Faker<Examples.Order>()
            .UseSeed(88)
            .RuleFor(o => o.Quantity, f => { return f.Random.Number(1, 3); });

         var order = orderFaker.Generate();

         order.Quantity.Should().Be(3);

         orderFaker.UseSeed(88);

         order = orderFaker.Generate();

         order.Quantity.Should().Be(3);
      }

      [Fact]
      public void complex_faker_t_test()
      {
         Randomizer.Seed = new Random(7331);

         var orderFaker = new Faker<Examples.Order>()
            .UseSeed(88)
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product());

         var items = Enumerable.Range(1, 4)
            .Select(i => orderFaker.Generate())
            .ToArray();

         console.WriteLine(items.DumpString());

         items.Select(i => i.OrderId).Should().Equal(0, 1, 2, 3);

         CheckSequence(items);

         //using the same seed again should
         //reset the state.
         orderFaker.UseSeed(88);
         //and it should override a set seeded test
         Randomizer.Seed = new Random(1337);

         items = Enumerable.Range(1, 4)
            .Select(i => orderFaker.Generate())
            .ToArray();

         items.Select(i => i.OrderId).Should().Equal(4, 5, 6, 7);

         console.WriteLine(items.DumpString());

         CheckSequence(items);
      }

      [Fact]
      public void sequence_generate_list_of_4_vs_generate_4_times_should_produce_same_content()
      {
         Randomizer.Seed = new Random(7331);

         var orderFaker = new Faker<Examples.Order>()
            .UseSeed(88)
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product());

         var batch = orderFaker.Generate(4).ToArray();
         batch.Select(i => i.OrderId).Should().Equal(0, 1, 2, 3);
         CheckSequence(batch);

         var sequence = new Examples.Order[4];

         //reset
         Randomizer.Seed = new Random(7331);
         orderFaker.UseSeed(88);
         
         sequence[0] = orderFaker.Generate();
         sequence[1] = orderFaker.Generate();
         sequence[2] = orderFaker.Generate();
         sequence[3] = orderFaker.Generate();
         
         sequence.Select(i => i.OrderId).Should().Equal(4, 5, 6, 7);

         CheckSequence(sequence);
      }

      [Fact]
      public void parallel_determinism()
      {
         var orderFaker = new Faker<Examples.Order>()
            .RuleFor(o => o.OrderId, f => f.IndexVariable++)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 3))
            .RuleFor(o => o.Item, f => f.Commerce.Product());

         var orders = ParallelEnumerable.Range(1, 5)
            .Select(threadId =>
               orderFaker
                  .Clone()
                  .UseSeed(88)
                  .Generate(4).ToArray()
            ).ToArray();

         foreach( var orderOfFour in orders )
         {
            CheckSequence(orderOfFour);
         }
      }

      private void CheckSequence(Examples.Order[] items)
      {
         items.Select(i => i.Item)
            .Should().Equal("Tuna", "Pants", "Shoes", "Soap");

         items.Select(i => i.Quantity)
            .Should().Equal(3, 1, 3, 2);
      }
   }
}
using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class RuleSetTests : SeededTest
   {
      public RuleSetTests()
      {
         Faker.DefaultStrictMode = false;
      }

      public class Customer
      {
         public int Id { get; set; }
         public string Description { get; set; }
         public bool GoodCustomer { get; set; }
      }

      [Fact]
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

      [Fact]
      public void should_be_able_to_run_two_rules_with_last_one_taking_precedence()
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

      [Fact]
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

      [Fact]
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
         act.Should().Throw<ValidationException>();
      }

      [Fact]
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

      [Fact]
      public void should_be_able_to_use_rules_with_ruleset()
      {
         var testCustomers = new Faker<Customer>()
            .RuleSet("Good",
               (set) =>
                  {
                     set.Rules((f, c) =>
                        {
                           c.Description = f.Lorem.Sentence();
                           c.Description = "overridden";
                        });
                  });

         var results = testCustomers.Generate(5, "Good");

         results.Should().OnlyContain(c => c.Description == "overridden");
      }


      public class EmptyObject
      {
      }

      [Fact]
      public void can_create_a_fake_object_with_no_props_or_rules()
      {
         var f = new Faker<EmptyObject>()
            .StrictMode(true);

         f.Generate().Should().NotBeNull();
      }
   }
}

using System;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue218 : SeededTest
   {
      [Fact]
      public void global_strict_mode_should_throw_on_incomplete_rules()
      {
         Faker.DefaultStrictMode = true;

         var orderFaker = new Faker<Order>()
            .RuleFor(x => x.Item, f => f.Commerce.Product());

         Action gen = () => orderFaker.Generate();

         gen.Should().Throw<ValidationException>();

         Faker.DefaultStrictMode = false;
      }

      [Fact]
      public void local_struct_mode_faker_t_scope_should_throw_on_incomplete_rules()
      {
         Faker.DefaultStrictMode = false;

         var orderFaker = new Faker<Order>()
            .StrictMode(true)
            .RuleFor(x => x.Item, f => f.Commerce.Product());

         Action gen = () => orderFaker.Generate();

         gen.Should().Throw<ValidationException>();
      }


      [Fact]
      public void local_strict_mode_should_take_precedence_always()
      {
         Faker.DefaultStrictMode = true;

         var orderFaker = new Faker<Order>()
            .StrictMode(false)
            .RuleFor(x => x.Item, f => f.Commerce.Product());

         Action gen = () => orderFaker.Generate();

         gen.Should().NotThrow<ValidationException>();

         Faker.DefaultStrictMode = false;
      }
   }
}

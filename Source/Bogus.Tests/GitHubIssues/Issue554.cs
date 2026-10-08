using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues;

public class Issue554 : SeededTest
{
   public class Order
   {
      public string Tenant { get; set; }
      public int Max { get; set; }
   }

   [Fact]
   public void parameters_are_readable_in_rules()
   {
      var faker = new Faker<Order>()
         .WithParameter("tenant", "acme")
         .WithParameter("max", 5)
         .RuleFor(o => o.Tenant, f => f.GetParameter<string>("tenant"))
         .Rules((f, o) => o.Max = f.GetParameter<int>("MAX"));

      var orders = faker.Generate(3);
      orders.Should().OnlyContain(o => o.Tenant == "acme" && o.Max == 5);
   }

   [Fact]
   public void overwriting_changes_output()
   {
      var faker = new Faker<Order>().WithParameter("t", "a").RuleFor(o => o.Tenant, f => f.GetParameter<string>("t"));
      faker.Generate().Tenant.Should().Be("a");
      faker.WithParameter("t", "b");
      faker.Generate().Tenant.Should().Be("b");
   }

   [Fact]
   public void missing_parameter_throws_and_try_returns_false()
   {
      var f = new Faker();
      var act = () => f.GetParameter("nope");
      act.Should().Throw<KeyNotFoundException>();
      f.TryGetParameter<int>("nope", out _).Should().BeFalse();
   }

   [Fact]
   public void clone_copies_parameters_in_isolation()
   {
      var original = new Faker<Order>().WithParameter("t", "a").RuleFor(o => o.Tenant, f => f.GetParameter<string>("t"));
      var clone = original.Clone();
      clone.WithParameter("t", "b");
      clone.Generate().Tenant.Should().Be("b");
      original.Generate().Tenant.Should().Be("a");
   }
}

using System.Linq;
using FluentAssertions;
using Xunit;
using static Bogus.Tests.GitHubIssues.PullRequest259.DrinkRuleSets;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest259 : SeededTest
   {
      class Drink
      {
         public string Name { get; set; }
         public int FluidOunce { get; set; }
      }

      public static class DrinkRuleSets
      {
         public const string Cherry = nameof(Cherry);
         public const string Lemonade = nameof(Lemonade);
         public const string SmallDrink = nameof(SmallDrink);
         public const string LargeDrink = nameof(LargeDrink);
      }

      [Fact]
      public void ensure_generate_forever_applies_ruleset()
      {
         var drinkFaker = new Faker<Drink>()
            .RuleSet(Cherry, set =>
                  {
                     set.RuleFor(d => d.Name, f => "cherry coke");
                  })
            .RuleSet(Lemonade, set =>
               {
                  set.RuleFor(d => d.Name, f => "strawberry lemonade");
               })
            .RuleSet(SmallDrink, set =>
               {
                  set.RuleFor(d => d.FluidOunce, f => 12);
               });

         var cherryDrink = drinkFaker.GenerateForever($"{Cherry}, {SmallDrink}").First();
         cherryDrink.Name.Should().Be("cherry coke");
         cherryDrink.FluidOunce.Should().Be(12);

         var lemonadeDrink = drinkFaker.GenerateForever(Lemonade).First();
         lemonadeDrink.Name.Should().Be("strawberry lemonade");
         lemonadeDrink.FluidOunce.Should().Be(0);
      }
   }
}
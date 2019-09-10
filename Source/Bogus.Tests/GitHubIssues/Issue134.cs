using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using Z.ExtensionMethods;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue134 : SeededTest
   {
      [Fact]
      public void can_make_rules_from_string()
      {
         var rulesFromTextFile =
            @"
               FirstName, {{name.firstName}}
               LastName, {{name.lastName}}
               StreetAddress, {{address.StreetAddress}}
            ";
         var faker = new HandlebarFaker<Foo>()
            .LoadRulesFromString(rulesFromTextFile);

         var foo = faker.Generate();

         foo.FirstName.Should().Be("Lee");
         foo.LastName.Should().Be("Brekke");
         foo.StreetAddress.Should().Be("6439 Lindsey Cape");
      }
   }

   public class Foo
   {
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string StreetAddress { get; set; }
   }

   public class HandlebarFaker<T> : Faker<T> where T : class
   {
      public HandlebarFaker<T> LoadRulesFromString(string rules)
      {
         var lines = rules.Split(
            new[] {"\r\n", "\r", "\n"}, StringSplitOptions.RemoveEmptyEntries)
            .Where( s => !s.IsNullOrWhiteSpace());

         foreach( var line in lines )
         {
            var parts = line.Split(',');
            var memberName = parts[0].Trim();
            var handlebarRule = parts[1].Trim();

            this.RuleFor(memberName, f => f.Parse($"{handlebarRule}"));
         }

         return this;
      }
   }
}
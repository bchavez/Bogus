using System;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;
using Z.ExtensionMethods;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue279 : SeededTest
   {
      public class Bar
      {
         public string Prop1 { get; set; }
         public string Prop2 { get; set; }
         public string Prop3 { get; set; }
      }

      [Fact]
      public void last_rule_defined_for_a_property_wins()
      {
         var order = 0;
         var barFaker = new Faker<Bar>()
            .RuleFor(b => b.Prop1, f => "aaaa" + order++)
            .RuleFor(b => b.Prop3, f => "bbbb" + order++)
            .RuleFor(b => b.Prop1, f => "cccc" + order++)
            .RuleFor(b => b.Prop2, f => "cccc" + order++)
            .RuleFor(b => b.Prop1, f => "bbbb" + order++) // executed 1
            .RuleFor(b => b.Prop2, f => "aaaa" + order++)
            .RuleFor(b => b.Prop3, f => "aaaa" + order++)
            .RuleFor(b => b.Prop2, f => "bbbb" + order++) // executed 3
            .RuleFor(b => b.Prop3, f => "cccc" + order++) // executed 2
            ;

         var bar = barFaker.Generate();
         bar.Prop1.Should().Be("bbbb0");
         bar.Prop3.Should().Be("cccc1");
         bar.Prop2.Should().Be("bbbb2");
      }

      [Fact(Skip = "Works only if an OrderedDictionary is used.")]
      public void last_rule_defined_for_a_property_wins_ordered_dictionary()
      {
         var order = 0;
         var barFaker = new Faker<Bar>()
            .RuleFor(b => b.Prop1, f => "aaaa" + order++)
            .RuleFor(b => b.Prop3, f => "bbbb" + order++)
            .RuleFor(b => b.Prop1, f => "cccc" + order++)
            .RuleFor(b => b.Prop2, f => "cccc" + order++)
            .RuleFor(b => b.Prop1, f => "bbbb" + order++) // executed 1
            .RuleFor(b => b.Prop2, f => "aaaa" + order++)
            .RuleFor(b => b.Prop3, f => "aaaa" + order++)
            .RuleFor(b => b.Prop2, f => "bbbb" + order++) // executed 2
            .RuleFor(b => b.Prop3, f => "cccc" + order++) // executed 3
            ; 

         var bar = barFaker.Generate();
         bar.Prop1.Should().Be("bbbb0");
         bar.Prop2.Should().Be("bbbb1");
         bar.Prop3.Should().Be("cccc2");
      }

      [Fact]
      public void preserve_define_ordering_of_faker_t_rules()
      {
         var barFaker = new Faker<Bar>()
            .RuleFor(b => b.Prop1, f => f.Company.CompanyName())
            .RuleFor(b => b.Prop2, f => f.Commerce.Product())
            .RuleFor(b => b.Prop3, (f, b) => "Prop3 depends on Prop1: "+ b.Prop1);
         
         var bar1 = barFaker.Generate();

         bar1.Should().NotBeNull();
         bar1.Prop1.Should().Be("Brekke - Schultz");
         bar1.Prop2.Should().Be("Tuna");
         bar1.Prop3.Should().EndWith(bar1.Prop1);

         //reconfigure 
         barFaker
            .RuleFor(b => b.Prop2, f => f.Company.CompanyName() + " zzz")
            .RuleFor(b => b.Prop1, (f, b) => ReverseString(b.Prop2));

         //The current (and expected) behavior of Faker<T> throws an exception
         //due to developer trying to re-order the execution from the initially
         //defined order.
         Action generate = () => barFaker.Generate();
         generate.ShouldThrow<NullReferenceException>();

         //When an OrderedDictionary is used, the NullReferenceException is not thrown
         //and the following assertions can be made.
         //
         //bar2.Should().NotBeNull();
         //bar2.Prop1.Should().Be("zzz cnI relluM");
         //bar2.Prop2.Should().Be("Muller Inc zzz");
         //bar2.Prop3.Should().EndWith(bar2.Prop1);
      }


      private string ReverseString(string value)
      {
         return value.Reverse();
      }

      [Fact]
      public void modifying_rules_from_initial_order_can_affect_other_initial_rules_too()
      {
         var barFaker = new Faker<Bar>()
            .RuleFor(b => b.Prop1, f => f.Company.CompanyName())
            .RuleFor(b => b.Prop2, f => f.Commerce.Product())
            .RuleFor(b => b.Prop3, (f, b) => "Prop3 depends on Prop1: " + b.Prop1);

         var bar1 = barFaker.Generate();

         bar1.Should().NotBeNull();
         bar1.Prop1.Should().Be("Brekke - Schultz");
         bar1.Prop2.Should().Be("Tuna");
         bar1.Prop3.Should().EndWith(bar1.Prop1);

         //reconfigure 
         barFaker
            .RuleFor(b => b.Prop1, f => "Reconfigured Prop1 Value.");

         var bar2 = barFaker.Generate();
         bar2.Should().NotBeNull();
         bar2.Prop1.Should().Be("Reconfigured Prop1 Value.");
         bar2.Prop2.Should().Be("Chair");
         bar2.Prop3.Should().EndWith(bar2.Prop1);
      }
   }
}
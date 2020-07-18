using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Cache;
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
         var barFaker = new Faker<Bar>()
            .RuleFor(b => b.Prop1, f =>
               {
                  return "aaaa" + 1;
               })
            .RuleFor(b => b.Prop3, f =>
               {
                  return "bbbb" + 2;
               })
            .RuleFor(b => b.Prop1, f =>
               {
                  return "cccc" + 3;
               })
            .RuleFor(b => b.Prop2, f =>
               {
                  return "cccc" + 4;
               })
            .RuleFor(b => b.Prop1, f =>
               {
                  return "bbbb" + 5; // executed 1
               }) 
            .RuleFor(b => b.Prop2, f =>
               {
                  return "aaaa" + 6;
               })
            .RuleFor(b => b.Prop3, f =>
               {
                  return "aaaa" + 7;
               })
            .RuleFor(b => b.Prop2, f =>
               {
                  return "bbbb" + 8; // executed 3
               }) 
            .RuleFor(b => b.Prop3, f =>
               {
                  return "cccc" + 9; // executed 2
               }) 
            ;

         var bar = barFaker.Generate();
         bar.Prop1.Should().Be("bbbb5");
         bar.Prop2.Should().Be("bbbb8");
         bar.Prop3.Should().Be("cccc9");
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

      [Fact]
      public void redefine_ordering_other_than_initial_order_of_faker_t_rules_throws_by_default()
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
            .RuleFor(b => b.Prop2, f => f.Company.CompanyName() + " zzz")
            .RuleFor(b => b.Prop1, (f, b) => ReverseString(b.Prop2));

         //The current (and expected) behavior of Faker<T> throws an exception
         //due to developer trying to re-order the execution from the initially
         //defined order.
         Action generate = () => barFaker.Generate();

         generate.Should().Throw<NullReferenceException>();
      }

      [Fact]
      public void allow_redefine_ordering_of_faker_t_rules_using_derived_faker_t()
      {
         var barFaker = new ReorderFaker<Bar>()
            .RuleFor(b => b.Prop1, f => f.Company.CompanyName())
            .RuleFor(b => b.Prop2, f => f.Commerce.Product())
            .RuleFor(b => b.Prop3, (f, b) => "Prop3 depends on Prop1: "+ b.Prop1)
            as ReorderFaker<Bar>;
         
         var bar1 = barFaker.Generate();

         bar1.Should().NotBeNull();
         bar1.Prop1.Should().Be("Brekke - Schultz");
         bar1.Prop2.Should().Be("Tuna");
         bar1.Prop3.Should().EndWith(bar1.Prop1);

         //reconfigure 
         barFaker.Reorder()
            .RuleFor(b => b.Prop2, f => f.Company.CompanyName() + " zzz")
            .RuleFor(b => b.Prop1, (f, b) => ReverseString(b.Prop2));

         var bar2 = barFaker.Generate();

         bar2.Should().NotBeNull();
         bar2.Prop1.Should().Be("zzz cnI relluM");
         bar2.Prop2.Should().Be("Muller Inc zzz");
         bar2.Prop3.Should().EndWith(bar2.Prop1);
      }

      private string ReverseString(string value)
      {
         return value.Reverse();
      }

      public class ReorderFaker<T> : Faker<T> where T : class
      {
         private Dictionary<string, OrderedDictionary> reorderActions = new Dictionary<string, OrderedDictionary>();

         private MultiDictionary<string, string, PopulateAction<T>> initialRules;

         private bool loaded = false;
         public ReorderFaker<T> Reorder()
         {
            //save the very fist initial order, then faker is locked down by the initial order.
            this.initialRules ??= this.Actions.Clone();
            this.reorderActions.Clear();
            this.loaded = false;
            return this;
         }

         protected override Faker<T> AddRule(string propertyOrField, Func<Faker, T, object> invoker)
         {
            if (initialRules is null)
            {
               base.AddRule(propertyOrField, invoker);
               return this;
            }

            var rule = new PopulateAction<T>
               {
                  Action = invoker,
                  RuleSet = currentRuleSet,
                  PropertyName = propertyOrField,
               };

            if ( reorderActions.TryGetValue(currentRuleSet, out var currentActions) &&
                currentActions.Contains(propertyOrField))
            {
               currentActions.Remove(propertyOrField);
            }

            AddOrderedRule(reorderActions, currentRuleSet, propertyOrField, rule);

            return this;
         }

         public static void AddOrderedRule(Dictionary<string, OrderedDictionary> ruleSets, string currentRuleSet, string propertyOrField, PopulateAction<T> populateAction)
         {
            if (ruleSets.TryGetValue(currentRuleSet, out var currentActions))
            {
               currentActions.Add(propertyOrField, populateAction);
            }
            else
            {
               var newActionSet = new OrderedDictionary
                  {
                     {propertyOrField, populateAction}
                  };
               ruleSets.Add(currentRuleSet, newActionSet);
            }
         }

         protected override void PopulateInternal(T instance, string[] ruleSets)
         {
            if( !loaded && this.initialRules != null )
            {
               LoadRulesInNewOrder();
               this.loaded = true;
            }

            base.PopulateInternal(instance, ruleSets);
         }

         public ReorderFaker<T> LoadRulesInNewOrder()
         {
            this.Actions.Clear();

            //use our new rules first.
            foreach( var ruleSet in reorderActions )
            {
               var ruleSetName = ruleSet.Key;
               var orderedActions = ruleSet.Value;

               //first our ordered rules.
               foreach( PopulateAction<T> newRule in orderedActions.Values )
               {
                  this.Actions.Add(ruleSetName, newRule.PropertyName, newRule);
               }
            }

            //then the initial rules.
            foreach( var initialRuleSet in this.initialRules )
            {
               //but add the initial rule only if it doesn't exist.
               var initialRuleSetName = initialRuleSet.Key;
               var initialActions = initialRuleSet.Value;

               foreach( var initialRule in initialActions.Values )
               {
                  if( this.Actions.TryGetValue(initialRuleSetName, out var existingRules) &&
                      existingRules.ContainsKey(initialRule.PropertyName) )
                  {
                     continue;
                  }

                  this.Actions.Add(initialRuleSetName, initialRule.PropertyName, initialRule);
               }
            }

            return this;
         }

      }

   }

   public static class ExtensionsForMultiDictionary
   {
      public static MultiDictionary<T, U, V> Clone<T, U, V>(this MultiDictionary<T, U, V> source)
      {
         var target = new MultiDictionary<T, U, V>(source.Comparer);

         foreach( var level1 in source )
         {
            foreach( var level2 in level1.Value )
            {
               target.Add(level1.Key, level2.Key, level2.Value);
            }
         }

         return target;
      }
   }

}

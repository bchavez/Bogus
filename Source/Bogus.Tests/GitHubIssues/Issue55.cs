using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue55 : SeededTest
   {
      public class DerivedFaker : Faker<Bar>
      {
         public DerivedFaker()
         {
            this.RuleFor(o => o.Code, f => f.Random.AlphaNumeric(5));
            this.RuleFor(o => o.Dba, f => f.Company.CompanyName());
         }

         public override List<Bar> Generate(int count, string ruleSets = null)
         {
            var list = base.Generate(count, ruleSets)
               .OrderBy(o => o.Code)
               .ToList();
            list[0].Primary = true;
            return list;
         }
      }

      [Fact]
      public void issue_55_collection_finish_with_syntax()
      {
         var derivedFaker = new DerivedFaker();

         var fakes = derivedFaker.Generate(5);
      }

      public class Bar
      {
         public string Code { get; set; }
         public string Dba { get; set; }
         public bool Primary { get; set; }
      }

#if NETCOREAPP3_1
      [Fact]
      public void with_range_and_index_syntax()
      {
         var faker = new Faker<Bar>()
            .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(5))
            .RuleFor(o => o.Dba, f => f.Company.CompanyName());

         //set the first item to primary.
         var bars = faker.Generate(5).ToArray();
         bars[0].Apply(b => b.Primary = true);
         bars[0].Primary.Should().BeTrue();
         bars[1..5].Select(b => b.Primary).Should().OnlyContain(x => x == false);

         //set the last item to primary.
         bars = faker.Generate(5).ToArray();
         bars[^1].Apply(b => b.Primary = true);
         bars.Last().Primary.Should().BeTrue();

         //set all items to primary
         bars = faker.Generate(5).ToArray();
         bars[..].Apply(b => b.Primary = true);
         bars[..].Select(b => b.Primary).Should().OnlyContain(x => x == true);

         //set only the first 3
         bars = faker.Generate(5).ToArray();
         bars[..3].Apply(b => b.Primary = true);
         bars.Select(b => b.Primary).Should().Equal(true, true, true, false, false);
      }
#endif

   }

#if NETCOREAPP3_1
   public static class ExtensionsForCollection
   {
      public static T Apply<T>(this T item, Action<T> applyAction)
      {
         applyAction(item);
         return item;
      }

      public static T[] Apply<T>(this T[] array, Action<T> applyAction)
      {
         foreach( var item in array )
         {
            applyAction(item);
         }
         return array;
      }
      public static IEnumerable<T> Apply<T>(this IEnumerable<T> sequence, Action<T> applyAction)
      {
         foreach( var item in sequence )
         {
            applyAction(item);
         }

         return sequence;
      }
   }
#endif

}
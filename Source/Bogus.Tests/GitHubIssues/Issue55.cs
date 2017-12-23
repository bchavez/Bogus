using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue55 : SeededTest
   {
      public class DerivedFaker : Faker<Issue55Object>
      {
         public DerivedFaker()
         {
            this.RuleFor(o => o.Code, f => f.Random.AlphaNumeric(5));
            this.RuleFor(o => o.Dba, f => f.Company.CompanyName());
         }

         public override List<Issue55Object> Generate(int count, string ruleSets = null)
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

      public class Issue55Object
      {
         public string Code { get; set; }
         public string Dba { get; set; }
         public bool Primary { get; set; }
      }
   }
}
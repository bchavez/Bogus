using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue192 : SeededTest
   {

      [Fact]
      public void no_company_suffix_should_not_throw()
      {
         var f = new Faker("az");
         var s = f.Random.ArrayElement(f.Company.Suffixes());
         s.Should().Be("Holdinqlər");
      }

      [Fact]
      public void company_name_should_work_in_az()
      {
         var f = new Faker("az");

         f.Company.CompanyName(0).Should().Be("Əfəndiyeva Holdinqlər");
      }
   }
}
using Xunit;
using Bogus.Extensions.UnitedKingdom;
using FluentAssertions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue318 : SeededTest
   {
      [Fact]
      public void can_get_country_of_uk()
      {
         var f = new Faker("en_GB");
         var country = f.Address.CountryOfUnitedKingdom();
         country.Should().Be("Wales");
      }

      [Fact]
      public void can_get_country_of_uk_without_locale_specified()
      {
         var f = new Faker();
         var country = f.Address.CountryOfUnitedKingdom();
         country.Should().Be("Wales");
      }
   }
}

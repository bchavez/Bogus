using System.Linq;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.LocaleTests;

public class DaLocale : SeededTest
{
   [Fact]
   public void locale_is_discoverable()
   {
      Database.LocaleResourceExists("da").Should().BeTrue();
      Database.GetAllLocales().Should().Contain("da");
   }

   [Fact]
   public void address_test()
   {
      var a = new Address("da");

      a.ZipCode().Should().MatchRegex(@"^\d{4}$");
      a.Country().Should().NotBeNullOrWhiteSpace();
      a.State().Should().BeOneOf("Hovedstaden", "Midtjylland", "Nordjylland", "Sjælland", "Syddanmark");
      a.StreetAddress().Should().MatchRegex(@"\d");
   }

   [Fact]
   public void company_tests()
   {
      var c = new Company("da");

      c.CompanySuffix().Should().BeOneOf("A/S", "ApS");
      c.CompanyName().Should().NotBeNullOrWhiteSpace();
   }

   [Fact]
   public void name_tests()
   {
      var n = new Name("da");

      n.FirstName(Name.Gender.Female).Should().NotBeNullOrWhiteSpace();
      n.LastName().Should().NotBeNullOrWhiteSpace();
      n.FullName().Should().Contain(" ");
   }

   [Fact]
   public void phone_tests()
   {
      var p = new PhoneNumbers("da");

      p.PhoneNumber("## ## ## ##").Should().MatchRegex(@"^\d{2} \d{2} \d{2} \d{2}$");
      p.PhoneNumber().Should().NotBeNullOrWhiteSpace();
   }

   [Fact]
   public void internet_tests()
   {
      var i = new Internet("da");

      i.DomainSuffix().Should().BeOneOf("com", "dk", "info", "name", "net", "org");
   }

   [Fact]
   public void date_tests()
   {
      var d = new Date("da");

      Enumerable.Range(0, 20).Select(_ => d.Month()).Should().OnlyContain(
         m => new[]
            {
               "januar", "februar", "marts", "april", "maj", "juni",
               "juli", "august", "september", "oktober", "november", "december"
            }.Contains(m));

      Enumerable.Range(0, 20).Select(_ => d.Weekday(abbreviation: true)).Should().OnlyContain(
         w => new[] {"søn.", "man.", "tir.", "ons.", "tor.", "fre.", "lør."}.Contains(w));
   }

   [Fact]
   public void faker_facade_uses_da_locale()
   {
      var f = new Faker("da");

      f.Locale.Should().Be("da");
      f.Person.FullName.Should().NotBeNullOrWhiteSpace();
   }
}

using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.LocaleTests;

public class HyLocale : SeededTest
{
   private readonly ITestOutputHelper console;

   public HyLocale(ITestOutputHelper console)
   {
      this.console = console;
   }

   [Fact]
   public void name_should_support_gender_first_names()
   {
      var name = new Name("hy");
      name.SupportsGenderFirstNames.Should().BeTrue();
   }
   
   [Fact]
   public void name_should_not_support_gender_last_names()
   {
      var name = new Name("hy");
      name.SupportsGenderLastNames.Should().BeFalse();
   }
   
   [Fact]
   public void name_should_not_support_gender_prefix()
   {
      var name = new Name("hy");
      name.SupportsGenderPrefixes.Should().BeFalse();
   }
   
   [Fact]
   public void name_should_have_firstname_list()
   {
      var name = new Name("hy");
      name.HasFirstNameList.Should().BeTrue();
   }

   [Fact]
   public void male_first_name()
   {
      var name = new Name("hy");
      var firstName = name.FirstName(Name.Gender.Male);
      firstName.Should().Be("Գևորգ");
   }

   [Fact]
   public void female_first_name()
   {
      var name = new Name("hy");
      var firstName = name.FirstName(Name.Gender.Female);
      firstName.Should().Be("Կարինե");
   }

   [Fact]
   public void female_fullname_should_be_firstname_lastname()
   {
      var name = new Name("hy");
      var fullName = name.FullName(Name.Gender.Female);
      fullName.Should().Be("Կարինե Հակոբյան");
   }

   [Fact]
   public void male_fullname_should_be_firstname_lastname()
   {
      var name = new Name("hy");
      var fullName = name.FullName(Name.Gender.Male);
      fullName.Should().Be("Գևորգ Հակոբյան");
   }

   [Fact]
   public void should_have_city_prefix()
   {
      var address = new Address("hy");
      address.CityPrefix().Should().Be("Հարավային");
   }

   [Fact]
   public void should_have_city_suffix()
   {
      var address = new Address("hy");
      address.CitySuffix().Should().Be("աշեն");
   }

   [Fact]
   public void should_have_country()
   {
      var address = new Address("hy");
      address.Country().Should().Be("Մոնտսերատ");
   }

   [Fact]
   public void should_have_building_number()
   {
      var address = new Address("hy");
      address.BuildingNumber().Should().Be("18/6");
   }

   [Fact]
   public void should_have_street_suffix()
   {
      var address = new Address("hy");
      address.StreetSuffix().Should().Be(" պողոտա");
   }

   [Fact]
   public void should_have_secondary_address()
   {
      var address = new Address("hy");
      address.SecondaryAddress().Should().Be("Տուն 186");
   }

   [Fact]
   public void should_have_state()
   {
      var address = new Address("hy");
      address.State().Should().Be("Ստեփանակերտ");
   }

   [Fact]
   public void should_have_city()
   {
      var address = new Address("hy");
      address.City().Should().Be("Հակոբյանաշատ");
   }

   [Fact]
   public void should_have_street_name()
   {
      var address = new Address("hy");
      address.StreetName().Should().Be("Հակոբյան  պողոտա");
   }

   [Fact]
   public void should_have_street_address()
   {
      var address = new Address("hy");
      address.StreetAddress().Should().Be("Եղիազարյան  պողոտա 064 Տուն 391");
   }

   [Fact]
   public void should_have_direction()
   {
      var address = new Address("hy");
      address.Direction().Should().Be("Հյուսիսարևելյան");
   }

   [Fact]
   public void should_have_domain_suffix()
   {
      var internet = new Internet("hy");
      internet.DomainSuffix().Should().Be("am");
   }

   [Fact]
   public void should_have_words()
   {
      var lorem = new Lorem("hy");
      lorem.Word().Should().Be("ոչ");
   }

   [Fact]
   public void should_have_phone_number()
   {
      var phoneNumbers = new PhoneNumbers("hy");
      phoneNumbers.PhoneNumber().Should().Be("+374 186 06064");
   }

   [Fact]
   public void should_have_color()
   {
      var commerce = new Commerce("hy");
      commerce.Color().Should().Be("նարնջագույն");
   }

   [Fact]
   public void should_have_month()
   {
      var date = new Date("hy");
      date.Month().Should().Be("Օգոստոս");
   }

   [Fact]
   public void should_have_month_abbr()
   {
      var date = new Date("hy");
      date.Month(true).Should().Be("Օգս");
   }

   [Fact]
   public void should_have_weekday()
   {
      var date = new Date("hy");
      date.Weekday().Should().Be("Հինգշաբթի");
   }

   [Fact]
   public void should_have_weekday_abbr()
   {
      var date = new Date("hy");
      date.Weekday(true).Should().Be("հնգ");
   }
}
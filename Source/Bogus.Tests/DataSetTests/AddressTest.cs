using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.DataSetTests
{
   public class AddressTest : SeededTest
   {
      private readonly ITestOutputHelper console;

      public AddressTest(ITestOutputHelper console)
      {
         this.console = console;
         address = new Address();
      }

      private readonly Address address;

      [Fact]
      public void can_get_a_city_name()
      {
         address.City().Should().Be("Bernhardfort");
      }

      [Fact]
      public void can_get_a_country()
      {
         address.Country().Should().Be("Mozambique");
      }

      [Fact]
      public void can_get_a_county()
      {
         address.County().Should().Be("Borders");
      }

      [Fact]
      public void can_get_a_full_address()
      {
         address.FullAddress().Should().Be("60643 Oberbrunner Bypass, Danielchester, Monaco");
      }

      [Fact]
      public void can_get_a_full_street_address()
      {
         address.StreetAddress(true).Should().Be("60643 Oberbrunner Bypass Suite 175");
      }

      [Fact]
      public void can_get_a_latitude()
      {
         address.Latitude().Should().Be(18.634);
      }

      [Fact]
      public void can_get_a_longitude()
      {
         address.Latitude().Should().Be(18.634);
      }

      [Fact]
      public void can_get_a_random_country_code()
      {
         address.CountryCode().Should().Be("MR");
      }

      [Fact]
      public void can_get_a_random_country_code_alpha3()
      {
         address.CountryCode(Iso3166Format.Alpha3).Should().Be("CMR");
      }

      [Fact]
      public void can_get_a_state()
      {
         address.State().Should().Be("New Mexico");
      }

      [Fact]
      public void can_get_a_state_abbreviation()
      {
         address.StateAbbr().Should().Be("NM");
      }

      [Fact]
      public void can_get_a_street_address()
      {
         address.StreetAddress().Should().Be("60643 Oberbrunner Bypass");
      }

      [Fact]
      public void can_get_a_street_suffix()
      {
         address.StreetSuffix().Should().Be("Pines");
      }

      [Fact]
      public void can_get_a_zipcode()
      {
         address.ZipCode().Should().Be("18606-0643");
      }

      [Fact]
      public void can_get_canadian_zip_code()
      {
         var a = new Address("en_CA");
         a.ZipCode().Should().Be("N1V 6A6");
      }

      [Fact]
      public void can_get_a_city_prefix()
      {
         address.CityPrefix().Should().Be("New");
      }

      [Fact]
      public void can_get_a_city_suffix()
      {
         address.CitySuffix().Should().Be("stad");
      }

      [Fact]
      public void can_get_a_street_name()
      {
         address.StreetName().Should().Be("Brekke Stravenue");
      }

      [Fact]
      public void can_get_a_building_number()
      {
         address.BuildingNumber().Should().Be("1860");
      }

      [Fact]
      public void locales_with_no_state_should_return_null()
      {
         var a = new Address("az");
         a.State().Should().BeNullOrEmpty();
      }

      [Fact]
      public void can_generate_a_direction()
      {
         address.Direction().Should().Be("Northeast");
         address.Direction(true).Should().Be("N");
      }

      [Fact]
      public void can_generate_a_cardinal_direction()
      {
         address.CardinalDirection().Should().Be("South");
         address.CardinalDirection(true).Should().Be("N");
      }

      [Fact]
      public void can_generate_an_ordinal_direction()
      {
         address.OrdinalDirection().Should().Be("Southeast");
         address.OrdinalDirection(true).Should().Be("NE");
      }
   }
}
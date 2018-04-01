using Bogus.DataSets;
using Bogus.Extensions;
using Bogus.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static System.Math;

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
         address.CountryCode().Should().Be("MQ");
      }

      [Fact]
      public void can_get_a_random_country_code_alpha3()
      {
         address.CountryCode(Iso3166Format.Alpha3).Should().Be("MNP");
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
         a.ZipCode().Should().Be("C8Q 0Q0");
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
         address.StreetName().Should().Be("Brown Stravenue");
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
      public void can_generate_an_ordnial_direction()
      {
         address.OrdinalDirection().Should().Be("Southeast");
         address.OrdinalDirection(true).Should().Be("NE");
      }

      [Fact]
      public void can_generate_a_geohash()
      {
         address.Geohash().Should().Be("m3un1m1");
         address.Geohash(5).Should().Be("qg9y5");
      }

      [Fact]
      public void can_generate_depth()
      {
         address.Depth().Should().BeLessThan(0)
            .And.BeGreaterOrEqualTo(-10994);
      }

      [Fact]
      public void can_generate_height()
      {
         address.Altitude().Should().BeGreaterThan(0)
            .And.BeLessOrEqualTo(8848);
      }

      [Fact]
      public void can_get_range()
      {
         var center = new LatLon // somewhere in the middle of Colorado.
            {
               Latitude = 39, //north
               Longitude = -105, //west
            };
         var newPoint = address.AreaCircle(center.Latitude, center.Longitude, 1000 * 1000); //radial search around 1000 km.

         var distance = GetDistance(center, newPoint);
         console.Dump(distance);
         distance.Should().BeLessOrEqualTo(1000 * 1000);
      }

      double GetDistance(LatLon x, LatLon y)
      {
         // https://github.com/chrisveness/geodesy/blob/master/latlon-spherical.js
         // a = sin²(Δφ/2) + cos(φ1)⋅cos(φ2)⋅sin²(Δλ/2)
         // tanδ = √(a) / √(1−a)

         const int R = 6371000; // Earth's radius in meters
         var φ1 = x.Latitude.ToRadians(); var λ1 = x.Longitude.ToRadians();
         var φ2 = y.Latitude.ToRadians(); var λ2 = y.Longitude.ToRadians();
         var Δφ = φ2 - φ1;
         var Δλ = λ2 - λ1;

         var a = Sin(Δφ / 2) * Sin(Δφ / 2)
                 + Cos(φ1) * Cos(φ2)
                                * Sin(Δλ / 2) * Sin(Δλ / 2);
         var c = 2 * Atan2(Sqrt(a), Sqrt(1 - a));
         var d = R * c;

         return d;
      }
   }
}
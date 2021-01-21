using System;

namespace Bogus.DataSets
{
   /// <summary>
   /// Methods for generating an address.
   /// </summary>
   public class Address : DataSet
   {
      /// <summary>
      /// The source to pull names from.
      /// </summary>
      protected Name Name;

      /// <summary>
      /// Initializes a new instance of the <see cref="Address"/> class.
      /// </summary>
      /// <param name="locale">The locale used to generate the data.</param>
      public Address(string locale = "en") : base(locale)
      {
         this.Name = this.Notifier.Flow(new Name(locale));
      }

      /// <summary>
      /// Get a zipcode.
      /// </summary>
      /// <param name="format">
      /// If a format is provided it will fill the format with letters and numbers.
      /// Example "???? ##" can become "QYTE 78".
      /// </param>
      /// <returns>A random zipcode.</returns>
      public string ZipCode(string format = null)
      {
         return format == null ? GetFormattedValue("postcode") : Random.Replace(format);
      }

      /// <summary>
      /// Get a city name.
      /// </summary>
      /// <returns>A random city name.</returns>
      public string City()
      {
         return GetFormattedValue("city");
      }


      /// <summary>
      /// Get a street address.
      /// </summary>
      /// <param name="useFullAddress">If true, the returned value will also include a <see cref="SecondaryAddress"/>.</param>
      /// <returns>A random street address.</returns>
      public string StreetAddress(bool useFullAddress = false)
      {
         var streetAddress = GetFormattedValue("street_address");
         return useFullAddress ? $"{streetAddress} {SecondaryAddress()}" : streetAddress;
      }

      /// <summary>
      /// Get a city prefix.
      /// </summary>
      /// <returns>A random city prefix.</returns>
      public string CityPrefix()
      {
         return GetRandomArrayItem("city_prefix");
      }

      /// <summary>
      /// Get a city suffix.
      /// </summary>
      /// <returns>A random city suffix.</returns>
      public string CitySuffix()
      {
         return GetRandomArrayItem("city_suffix");
      }

      /// <summary>
      /// Get a street name.
      /// </summary>
      /// <returns>A random street name.</returns>
      public string StreetName()
      {
         return GetFormattedValue("street_name");
      }

      /// <summary>
      /// Get a building number.
      /// </summary>
      /// <returns>A random building number.</returns>
      public string BuildingNumber()
      {
         return GetFormattedValue("building_number");
      }

      /// <summary>
      /// Get a street suffix.
      /// </summary>
      /// <returns>A random street suffix.</returns>
      public string StreetSuffix()
      {
         return GetRandomArrayItem("street_suffix");
      }

      /// <summary>
      /// Get a secondary address like 'Apt. 2' or 'Suite 321'.
      /// </summary>
      /// <returns>A random secondary address.</returns>
      public string SecondaryAddress()
      {
         return GetFormattedValue("secondary_address");
      }

      /// <summary>
      /// Get a county.
      /// </summary>
      /// <returns>A random county.</returns>
      public string County()
      {
         return GetRandomArrayItem("county");
      }

      /// <summary>
      /// Get a country.
      /// </summary>
      /// <returns>A random country.</returns>
      public string Country()
      {
         return GetRandomArrayItem("country");
      }

      /// <summary>
      /// Get a full address like Street, City, Country.
      /// </summary>
      /// <returns>A random full address.</returns>
      public string FullAddress()
      {
         var street = StreetAddress();
         var city = City();
         var country = Country();
         return $"{street}, {city}, {country}";
      }


      /// <summary>
      /// Get a random ISO 3166-1 country code.
      /// </summary>
      /// <param name="format">The format the country code should be in.</param>
      /// <returns>A random country code.</returns>
      public string CountryCode(Iso3166Format format = Iso3166Format.Alpha2)
      {
         if( format == Iso3166Format.Alpha2 )
         {
            return GetRandomArrayItem("country_code");
         }

         if( format == Iso3166Format.Alpha3 )
         {
            return GetRandomArrayItem("country_code_alpha_3");
         }

         throw new ArgumentException("Invalid country code", nameof(format));
      }

      /// <summary>
      /// Get a random state state.
      /// </summary>
      /// <returns>A random state.</returns>
      public string State()
      {
         return GetRandomArrayItem("state");
      }

      /// <summary>
      /// Get a state abbreviation.
      /// </summary>
      /// <returns>An abbreviation for a random state.</returns>
      public string StateAbbr()
      {
         return GetRandomArrayItem("state_abbr");
      }

      /// <summary>
      /// Get a Latitude.
      /// </summary>
      /// <param name="min">The minimum value.</param>
      /// <param name="max">The maximum value.</param>
      /// <returns>A random latitude value.</returns>
      public double Latitude(double min = -90, double max = 90)
      {
         return Math.Round(Random.Double(min, max), 4);
      }

      /// <summary>
      /// Get a Longitude.
      /// </summary>
      /// <param name="min">The minimum value.</param>
      /// <param name="max">The maximum value.</param>
      /// <returns>A random longitude value.</returns>
      public double Longitude(double min = -180, double max = 180)
      {
         return Math.Round(Random.Double(min, max), 4);
      }

      /// <summary>
      /// Generates a cardinal or ordinal direction. IE: Northwest, South, SW, E.
      /// </summary>
      /// <param name="useAbbreviation">When true, directions such as Northwest turn into NW.</param>
      /// <returns>A random cardinal or ordinal direction.</returns>
      public string Direction(bool useAbbreviation = false)
      {
         if( useAbbreviation )
         {
            return GetRandomArrayItem("direction_abbr");
         }

         return GetRandomArrayItem("direction");
      }

      /// <summary>
      /// Generates a cardinal direction. IE: North, South, E, W.
      /// </summary>
      /// <param name="useAbbreviation">When true, directions such as West turn into W.</param>
      /// <returns>A random cardinal direction</returns>
      public string CardinalDirection(bool useAbbreviation = false)
      {
         if( useAbbreviation )
         {
            return GetRandomArrayItem("direction_abbr", min: 0, max: 4);
         }

         return GetRandomArrayItem("direction", min: 0, max: 4);
      }

      /// <summary>
      /// Generates an ordinal direction. IE: Northwest, Southeast, SW, NE.
      /// </summary>
      /// <param name="useAbbreviation">When true, directions such as Northwest turn into NW.</param>
      /// <returns>A random ordinal direction.</returns>
      public string OrdinalDirection(bool useAbbreviation = false)
      {
         if( useAbbreviation )
         {
            return GetRandomArrayItem("direction_abbr", min: 4, max: 8);
         }

         return GetRandomArrayItem("direction", min: 4, max: 8);
      }
   }

   /// <summary>
   /// Defines format for ISO 3166-1 country codes.
   /// </summary>
   public enum Iso3166Format
   {
      /// <summary>
      /// Two character ISO 3166-1 format.
      /// </summary>
      Alpha2 = 0x2,

      /// <summary>
      /// Three character ISO 3166-1 format.
      /// </summary>
      Alpha3
   }
}

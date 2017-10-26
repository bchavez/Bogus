using System;

namespace Bogus.DataSets
{
    /// <summary>
    /// Methods for generating an address
    /// </summary>
    public class Address : DataSet
    {
        /// <summary>
        /// The source to pull names from.
        /// </summary>
        protected Name Name;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
        public Address( string locale = "en" ) : base( locale )
        {
            this.Name = new Name( locale );
        }

        /// <summary>
        /// Get a zipcode.
        /// </summary>
        /// <returns></returns>
        public string ZipCode(string format = null)
        {
            return format == null ? GetFormattedValue( "postcode" ) : Random.Replace(format);
        }

        /// <summary>
        /// Get a city name.
        /// </summary>
        /// <returns></returns>
        public string City()
        {
            return GetFormattedValue( "city" );
        }

        

        /// <summary>
        /// Get a street address.
        /// </summary>
        /// <param name="useFullAddress"></param>
        /// <returns></returns>
        public string StreetAddress(bool useFullAddress = false)
        {
            var streetAddress = GetFormattedValue( "street_address" );
            return useFullAddress ? $"{streetAddress} {SecondaryAddress()}" : streetAddress;
        }

        /// <summary>
        /// Get a city prefix.
        /// </summary>
        /// <returns></returns>
        public string CityPrefix()
        {
            return GetRandomArrayItem("city_prefix");
        }

        /// <summary>
        /// Get a city suffix.
        /// </summary>
        /// <returns></returns>
        public string CitySuffix()
        {
            return GetRandomArrayItem("city_suffix");
        }

        /// <summary>
        /// Get a street name.
        /// </summary>
        /// <returns></returns>
        public string StreetName()
        {
            return GetFormattedValue( "street_name" );
        }

        /// <summary>
        /// Get a building number
        /// </summary>
        /// <returns></returns>
        public string BuildingNumber()
        {
            return GetFormattedValue( "building_number" );
        }

        /// <summary>
        /// Get a street suffix.
        /// </summary>
        /// <returns></returns>
        public string StreetSuffix()
        {
            return GetRandomArrayItem("street_suffix");
        }
        
        /// <summary>
        /// Get a secondary address like 'Apt. 2' or 'Suite 321'.
        /// </summary>
        /// <returns></returns>
        public string SecondaryAddress()
        {
            return GetFormattedValue( "secondary_address" );
        }

        /// <summary>
        /// Get a county.
        /// </summary>
        /// <returns></returns>
        public string County()
        {
            return GetRandomArrayItem("county");
        }

        /// <summary>
        /// Get a country.
        /// </summary>
        /// <returns></returns>
        public string Country()
        {
            return GetRandomArrayItem("country");
        }

        /// <summary>
        /// Get a full address like Street, City, Country.
        /// </summary>
        /// <returns></returns>
        public string FullAddress()
        {
            var street = StreetAddress();
            var city = City();
            var country = Country();
            return $"{street}, {city}, {country}";
        }

        /// <summary>
        /// Get a random country code.
        /// </summary>
        public string CountryCode()
        {
            return GetRandomArrayItem( "country_code" );
        }

        /// <summary>
        /// Get a state.
        /// </summary>
        /// <returns></returns>
        public string State()
        {
            return GetRandomArrayItem("state");
        }

        /// <summary>
        /// Get a state abbreviation.
        /// </summary>
        /// <returns></returns>
        public string StateAbbr()
        {
            return GetRandomArrayItem("state_abbr");
        }

        /// <summary>
        /// Get a Latitude
        /// </summary>
        /// <returns></returns>
        public double Latitude(double min = -90, double max = 90 )
        {
            return Math.Round(Random.Double(min, max), 4);
        }

        /// <summary>
        /// Get a Longitude
        /// </summary>
        /// <returns></returns>
        public double Longitude(double min = -180, double max = 180)
        {
           return Math.Round(Random.Double(min, max), 4);
        }
    }
}

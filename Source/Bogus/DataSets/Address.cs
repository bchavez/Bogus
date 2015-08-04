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
        protected Name Name = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
        public Address(string locale = "en") : base(locale)
        {
            this.Name = new Name(locale);
        }

        /// <summary>
        /// Get a zipcode.
        /// </summary>
        /// <returns></returns>
        public string ZipCode(string format = null)
        {
            if( format == null )
            {
                format = GetRandomArrayItem("postcode");
            }

            return Random.Replace(format);
        }

        /// <summary>
        /// Get a city name.
        /// </summary>
        /// <returns></returns>
        public string City()
        {
            var format = Random.Number(3);
            if( format == 0 )
                return string.Format("{0} {1} {2}", CityPrefix(), Name.FirstName(), CitySuffix());
            
            if( format == 1 )
                return string.Format("{0} {1}", CityPrefix(), Name.FirstName());
            
            if( format == 2 )
                return string.Format("{0} {1}", Name.FirstName(), CitySuffix());

            return string.Format("{0} {1}", Name.LastName(), CitySuffix());
        }

        /// <summary>
        /// Get a street address.
        /// </summary>
        /// <param name="useFullAddress"></param>
        /// <returns></returns>
        public string StreetAddress(bool useFullAddress = false)
        {
            var homeNumbers = new string('#', Random.Number(3, 5));

            var houseNumber = Random.Replace(homeNumbers);

            if( useFullAddress )
            {
                return string.Format("{0} {1} {2}", houseNumber, StreetName(), SecondaryAddress());
            }

            return string.Format("{0} {1}", houseNumber, StreetName());
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
	        return $"{(Random.Bool() ? Name.FirstName() : Name.LastName())} {StreetSuffix()}".Trim();
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
            var formats = new[] {"Apt. ###", "Suite ###"};

            var format = Random.ArrayElement(formats);

            return Random.Replace(format);
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
        public double Latitude()
        {
            return Random.Number(180 * 10000) / 10000.0 - 90.0;
        }

        /// <summary>
        /// Get a Longitude
        /// </summary>
        /// <returns></returns>
        public double Longitude()
        {
            return Random.Number(360 * 10000) / 10000.0 - 180.0;
        }
    }
}

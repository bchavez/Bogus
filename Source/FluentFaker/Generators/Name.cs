using Newtonsoft.Json.Linq;

namespace FluentFaker.Generators
{
    public class Name : DataSet
    {
        public Name(string locale = "en") : base(locale)
        {
        }

        /// <summary>
        /// Gets a first name
        /// </summary>
        /// <returns></returns>
        public string FirstName()
        {
            var get = (JArray)Get("first_name");

            return Random.ArrayElement(get);
        }

        /// <summary>
        /// Gets a last name
        /// </summary>
        /// <returns></returns>
        public string LastName()
        {
            return GetRandomArrayItem("last_name");
        }

        /// <summary>
        /// Gets a random prefix for a name
        /// </summary>
        /// <returns></returns>
        public string Prefix()
        {
            return GetRandomArrayItem("prefix");
        }

        /// <summary>
        /// Gets a random suffix for a name
        /// </summary>
        /// <returns></returns>
        public string Suffix()
        {
            return GetRandomArrayItem("suffix");
        }

        /// <summary>
        /// Gets a full name
        /// </summary>
        /// <param name="firstName">Use this first name.</param>
        /// <param name="lastName">use this last name.</param>
        /// <param name="withPrefix">Add a prefix?</param>
        /// <param name="withSuffix">Add a suffix?</param>
        /// <returns></returns>
        public string FindName(string firstName = "", string lastName = "", bool? withPrefix = null, bool? withSuffix = null)
        {
            if( string.IsNullOrWhiteSpace(firstName) )
                firstName = FirstName();
            if( string.IsNullOrWhiteSpace(lastName) )
                lastName = LastName();

            if( !withPrefix.HasValue && !withSuffix.HasValue )
            {
                withPrefix = Random.Bool();
                withSuffix = !withPrefix;
            }
        
            return string.Format("{0} {1} {2} {3}",
                withPrefix.GetValueOrDefault() ? Prefix() : "", firstName, lastName, withSuffix.GetValueOrDefault() ? Suffix() : "")
                .Trim();

        }
    }
}
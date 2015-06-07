using Newtonsoft.Json.Linq;

namespace FluentFaker
{
    public class Name : Category
    {
        public Name(string locale = "en") : base(locale)
        {
        }

        public string FirstName()
        {
            var get = (JArray)Get("first_name");

            return Random.ArrayElement(get);
        }

        public string LastName()
        {
            return GetArrayItem("last_name");
        }

        public string Prefix()
        {
            return GetArrayItem("prefix");
        }

        public string Suffix()
        {
            return GetArrayItem("suffix");
        }

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
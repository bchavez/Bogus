using System;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace FluentFaker
{
    public class Internet : Category
    {
        public Name Name = null;

        public Internet(string locale = "en") : base(locale)
        {
            this.Name = new Name(locale);
        }

        public string Avatar()
        {
            return GetArrayItem("avatar_uri");
        }

        public string Email(string firstName = null, string lastName = null, string provider = null)
        {
            provider = provider ?? GetArrayItem("free_email");

            return Helpers.Slugify(UserName(firstName, lastName)) + "@" + provider;
        }

        public string UserName(string firstName = null, string lastName = null)
        {
            firstName = firstName ?? Name.FirstName();
            lastName = lastName ?? Name.LastName();

            var val = Random.Number(2);

            string result;
            
            if( val == 0 )
            {
                result = firstName + Random.Number(99);
            }
            else if( val == 1 )
            {
                result = firstName + Random.ArrayElement(new[] { ".", "_" }) + lastName;
            }
            else
            {
                result = firstName + Random.ArrayElement(new[] {".", "_"}) + lastName + Random.Number(99);
            }

            result = result.Replace("'", "")
                .Replace(" ", "");

            return result;
        }

        public string DomainName()
        {
            return DomainWord() + "." + DomainSuffix();
        }

        public string DomainWord()
        {
            var domain = Name.FirstName().ToLower();

            return Regex.Replace(domain, @"([^a-z0-9._%+-])", "");
        }

        public string DomainSuffix()
        {
            return GetArrayItem("domain_suffix");
        }

        public string Ip()
        {
            return string.Format("{0}.{1}.{2}.{3}",
                Random.Number(255),
                Random.Number(255),
                Random.Number(255),
                Random.Number(255));
        }

        public string Color(byte baseRed = 0, byte baseGreen = 0, byte baseBlue = 0)
        {
            var red = Math.Floor(( Random.Number(256) + (double)baseRed ) / 2);
            var green = Math.Floor(( Random.Number(256) + (double)baseGreen ) / 2);
            var blue = Math.Floor(( Random.Number(256) + (double)baseBlue ) / 2);

            return string.Format("#{0:x02}{1:x02}{2:x02}", (byte)red, (byte)green, (byte)blue);
        }       
    }
}
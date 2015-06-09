using System;
using System.Text.RegularExpressions;

namespace FluentFaker.Generators
{
    public class Internet : DataSet
    {
        protected Name Name = null;

        public Internet(string locale = "en") : base(locale)
        {
            this.Name = new Name(locale);
        }

        /// <summary>
        /// Generates a legit Internet URL avatar from twitter accounts.
        /// </summary>
        /// <returns></returns>
        public string Avatar()
        {
            return GetRandomArrayItem("avatar_uri");
        }

        /// <summary>
        /// Generates an email address.
        /// </summary>
        /// <param name="firstName">Always use this first name.</param>
        /// <param name="lastName">Sometimes used depending on randomness. See 'UserName'.</param>
        /// <param name="provider">Always use the provider.</param>
        public string Email(string firstName = null, string lastName = null, string provider = null)
        {
            provider = provider ?? GetRandomArrayItem("free_email");

            return Utils.Slugify(UserName(firstName, lastName)) + "@" + provider;
        }

        /// <summary>
        /// Generates user names.
        /// </summary>
        /// <param name="firstName">Always used.</param>
        /// <param name="lastName">Sometimes used depending on randomness.</param>
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

        /// <summary>
        /// Generates a random domain name.
        /// </summary>
        /// <returns></returns>
        public string DomainName()
        {
            return DomainWord() + "." + DomainSuffix();
        }


        /// <summary>
        /// Generates a domain word used for domain names.
        /// </summary>
        /// <returns></returns>
        public string DomainWord()
        {
            var domain = Name.FirstName().ToLower();

            return Regex.Replace(domain, @"([^a-z0-9._%+-])", "");
        }

        /// <summary>
        /// Generates a domain name suffix like .com, .net, .org
        /// </summary>
        /// <returns></returns>
        public string DomainSuffix()
        {
            return GetRandomArrayItem("domain_suffix");
        }

        /// <summary>
        /// Gets a random IP address.
        /// </summary>
        /// <returns></returns>
        public string Ip()
        {
            return string.Format("{0}.{1}.{2}.{3}",
                Random.Number(255),
                Random.Number(255),
                Random.Number(255),
                Random.Number(255));
        }

        /// <summary>
        /// Gets a random aesthetically pleasing color near the base R,G.B. See: http://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette
        /// </summary>
        /// <param name="baseRed">Red base color</param>
        /// <param name="baseGreen">Green base color</param>
        /// <param name="baseBlue">Blue base color</param>
        /// <returns></returns>
        public string Color(byte baseRed = 0, byte baseGreen = 0, byte baseBlue = 0)
        {
            var red = Math.Floor(( Random.Number(256) + (double)baseRed ) / 2);
            var green = Math.Floor(( Random.Number(256) + (double)baseGreen ) / 2);
            var blue = Math.Floor(( Random.Number(256) + (double)baseBlue ) / 2);

            return string.Format("#{0:x02}{1:x02}{2:x02}", (byte)red, (byte)green, (byte)blue);
        }       
    }
}
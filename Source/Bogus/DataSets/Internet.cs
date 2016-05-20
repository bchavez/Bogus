using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bogus.DataSets
{
    /// <summary>
    /// Random Internet things like email addresses
    /// </summary>
    public class Internet : DataSet
    {
        /// <summary>
        /// The source to pull names from.
        /// </summary>
        protected Name Name = null;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
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
        /// Generates an example email with @example.com
        /// </summary>
        /// <param name="firstName">Optional: first name of the user</param>
        /// <param name="lastName">Optional: last name of the user</param>
        /// <returns></returns>
        public string ExampleEmail(string firstName = null, string lastName = null)
        {
            var provider = GetRandomArrayItem("example_email");
            return Email(firstName, lastName, provider);
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

            return Regex.Replace(domain, @"([\\~#&*{}/:<>?|\""'])", "");
        }

        /// <summary>
        /// Generates a domain name suffix like .com, .net, .org
        /// </summary>
        public string DomainSuffix()
        {
            return GetRandomArrayItem("domain_suffix");
        }

        /// <summary>
        /// Gets a random IP address.
        /// </summary>
        public string Ip()
        {
            return string.Format("{0}.{1}.{2}.{3}",
                Random.Number(255),
                Random.Number(255),
                Random.Number(255),
                Random.Number(255));
        }

        /// <summary>
        /// Gets a random mac address
        /// </summary>
        public string Mac()
        {
            var sb = new StringBuilder();

            var arr = Enumerable.Range(0, 6)
                .Select(s => this.Random.Number(0, 255).ToString("x2"));

            return string.Join(":", arr);
        }


        /// <summary>
        /// Gets a random aesthetically pleasing color near the base R,G.B. See [here](http://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette).
        /// </summary>
        /// <param name="baseRed">Red base color</param>
        /// <param name="baseGreen">Green base color</param>
        /// <param name="baseBlue">Blue base color</param>
        public string Color(byte baseRed = 0, byte baseGreen = 0, byte baseBlue = 0)
        {
            var red = Math.Floor(( Random.Number(256) + (double)baseRed ) / 2);
            var green = Math.Floor(( Random.Number(256) + (double)baseGreen ) / 2);
            var blue = Math.Floor(( Random.Number(256) + (double)baseBlue ) / 2);

            return string.Format("#{0:x02}{1:x02}{2:x02}", (byte)red, (byte)green, (byte)blue);
        }

        /// <summary>
        /// Returns a random protocol. HTTP or HTTPS.
        /// </summary>
        public string Protocol()
        {
            var protocols = new[] {"http", "https"};

            return Random.ArrayElement(protocols);
        }

        /// <summary>
        /// Generates a random URL.
        /// </summary>
        public string Url()
        {
            return string.Format("{0}://{1}", Protocol(), DomainName());
        }
    }
}

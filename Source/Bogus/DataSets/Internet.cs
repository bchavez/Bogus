using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bogus.Vendor;

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
         this.Name = this.Notifier.Flow(new Name(locale));
         this.userAgentGenerator = new UserAgentGenerator(() => this.Random);
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
      /// Generates an example email with @example.com.
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
            result = firstName + Random.ArrayElement(new[] {".", "_"}) + lastName;
         }
         else
         {
            result = firstName + Random.ArrayElement(new[] {".", "_"}) + lastName + Random.Number(99);
         }

         result = result.Replace(" ", "");

         return Utils.Slugify(result);
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
         return $"{Random.Number(255)}.{Random.Number(255)}.{Random.Number(255)}.{Random.Number(255)}";
      }

      /// <summary>
      /// Generates a random IPv6 address.
      /// </summary>
      /// <returns></returns>
      public string Ipv6()
      {
         var bytes = Random.Bytes(16);
         return
            $"{bytes[0]:x}{bytes[1]:x}:{bytes[2]:x}{bytes[3]:x}:{bytes[4]:x}{bytes[5]:x}:{bytes[6]:x}{bytes[7]:x}:{bytes[8]:x}{bytes[9]:x}:{bytes[10]:x}{bytes[11]:x}:{bytes[12]:x}{bytes[13]:x}:{bytes[14]:x}{bytes[15]:x}";
      }

      private UserAgentGenerator userAgentGenerator;

      /// <summary>
      /// Generates a random user agent.
      /// </summary>
      public string UserAgent()
      {
         return userAgentGenerator.Generate();
      }

      /// <summary>
      /// Gets a random mac address.
      /// </summary>
      public string Mac(string separator = ":")
      {
         var arr = Enumerable.Range(0, 6)
            .Select(s => this.Random.Number(0, 255).ToString("x2"));

         return string.Join(separator, arr);
      }

      /// <summary>
      /// Generates a random password.
      /// </summary>
      /// <param name="length">Length of the password.</param>
      /// <param name="memorable">A memorable password (ie: all lower case).</param>
      /// <param name="regexPattern">Regex pattern that the password should follow.</param>
      /// <param name="prefix">Password prefix.</param>
      public string Password(int length = 10, bool memorable = false, string regexPattern = "\\w", string prefix = "")
      {
         string consonant, vowel;
         vowel = "[aeiouAEIOU]$";
         consonant = "[bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ]";

         string c;
         int n;

         if( prefix.Length >= length )
         {
            return prefix;
         }
         if( memorable )
         {
            if( Regex.IsMatch(prefix, consonant) )
            {
               regexPattern = vowel;
            }
            else
            {
               regexPattern = consonant;
            }
         }
         n = this.Random.Number(32, 126); //ascii
         c = Convert.ToChar(n).ToString();
         if( memorable )
         {
            c = c.ToLower();
         }
         if( !Regex.IsMatch(c, regexPattern) )
         {
            return Password(length, memorable, regexPattern, prefix);
         }
         return Password(length, memorable, regexPattern, "" + prefix + c);
      }

      /// <summary>
      /// Gets a random aesthetically pleasing color near the base RGB. See [here](http://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette).
      /// </summary>
      /// <param name="baseRed">Red base color</param>
      /// <param name="baseGreen">Green base color</param>
      /// <param name="baseBlue">Blue base color</param>
      /// <param name="grayscale">Output a gray scale color</param>
      /// <param name="format">The color format</param>
      public string Color(byte baseRed = 0, byte baseGreen = 0, byte baseBlue = 0, bool grayscale = false, ColorFormat format = ColorFormat.Hex)
      {
         var red = Math.Floor((Random.Number(256) + (double)baseRed) / 2);
         var green = Math.Floor((Random.Number(256) + (double)baseGreen) / 2);
         var blue = Math.Floor((Random.Number(256) + (double)baseBlue) / 2);

         if( grayscale )
         {
            green = red;
            blue = red;
         }

         if( format == ColorFormat.Hex )
         {
            return string.Format("#{0:x02}{1:x02}{2:x02}", (byte)red, (byte)green, (byte)blue);
         }

         if( format == ColorFormat.Delimited )
         {
            return DelimitedRgb();
         }

         return $"rgb({DelimitedRgb()})";

         string DelimitedRgb()
         {
            return $"{(byte)red},{(byte)green},{(byte)blue}";
         }
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
         return Url(null, null);
      }

      private string Url(string protocol, string domain)
      {
         return $"{protocol ?? Protocol()}://{domain ?? DomainName()}";
      }

      /// <summary>
      /// Get a random URL with random path.
      /// </summary>
      /// <param name="protocol">Protocol part of the URL, random if null</param>
      /// <param name="domain">Domain part of the URL, random if null</param>
      public string UrlWithPath(string protocol = null, string domain = null)
      {
         var words = Random.WordsArray(1, 3)
            .Select(Utils.Slugify)
            .Select(s => s.ToLower())
            .ToArray();

         return $"{Url(protocol, domain)}/{Utils.Slashify(words, "/")}";
      }
   }
}
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Bogus.Extensions;
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
      /// Initializes a new instance of the <see cref="Internet"/> class.
      /// </summary>
      /// <param name="locale">The locale used to generate values.</param>
      public Internet(string locale = "en") : base(locale)
      {
         this.Name = this.Notifier.Flow(new Name(locale));
         this.userAgentGenerator = new UserAgentGenerator(() => this.Random);
      }

      /// <summary>
      /// Generates a legit Internet URL avatar from twitter accounts.
      /// </summary>
      /// <returns>A string containing a URL avatar from twitter accounts.</returns>
      public string Avatar()
      {
         var n = this.Random.Number(0, 1249);
         return $"https://cloudflare-ipfs.com/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/{n}.jpg";
      }

      /// <summary>
      /// Generates an email address.
      /// </summary>
      /// <param name="firstName">Always use this first name.</param>
      /// <param name="lastName">Sometimes used depending on randomness. See 'UserName'.</param>
      /// <param name="provider">Always use the provider.</param>
      /// <param name="uniqueSuffix">This parameter is appended to
      /// the email account just before the @ symbol. This is useful for situations
      /// where you might have a unique email constraint in your database or application.
      /// Passing var f = new Faker(); f.UniqueIndex is a good choice. Or you can supply
      /// your own unique changing suffix too like Guid.NewGuid; just be sure to change the
      /// <paramref name="uniqueSuffix"/> value each time before calling this method
      /// to ensure that email accounts that are generated are totally unique.</param>
      /// <returns>An email address</returns>
      public string Email(string firstName = null, string lastName = null, string provider = null, string uniqueSuffix = null)
      {
         provider ??= GetRandomArrayItem("free_email");

         return UserName(firstName, lastName) + uniqueSuffix + "@" + provider;
      }

      /// <summary>
      /// Generates an example email with @example.com.
      /// </summary>
      /// <param name="firstName">Optional: first name of the user.</param>
      /// <param name="lastName">Optional: last name of the user.</param>
      /// <returns>An example email ending with @example.com.</returns>
      public string ExampleEmail(string firstName = null, string lastName = null)
      {
         var provider = GetRandomArrayItem("example_email");
         return Email(firstName, lastName, provider);
      }

      /// <summary>
      /// Generates user names.
      /// </summary>
      /// <param name="firstName">First name is always part of the returned user name.</param>
      /// <param name="lastName">Last name may or may not be used.</param>
      /// <returns>A random user name.</returns>
      public string UserName(string firstName = null, string lastName = null)
      {
         firstName ??= Name.FirstName();
         lastName ??= Name.LastName();

         firstName = firstName.Transliterate(this.Locale);
         lastName = lastName.Transliterate(this.Locale);

         return Utils.Slugify(UserNameUnicode(firstName, lastName));
      }

      /// <summary>
      /// Generates a user name preserving Unicode characters.
      /// </summary>
      /// <param name="firstName">First name is always part of the returned user name.</param>
      /// <param name="lastName">Last name may or may not be used.</param>
      public string UserNameUnicode(string firstName = null, string lastName = null)
      {
         firstName ??= Name.FirstName();
         lastName ??= Name.LastName();

         var val = Random.Number(2);

         string result;

         if (val == 0)
         {
            result = firstName + Random.Number(99);
         }
         else if (val == 1)
         {
            result = firstName + Random.ArrayElement(new[] { ".", "_" }) + lastName;
         }
         else
         {
            result = firstName + Random.ArrayElement(new[] { ".", "_" }) + lastName + Random.Number(99);
         }

         result = result.Replace(" ", string.Empty);
         return result;
      }

      /// <summary>
      /// Generates a random domain name.
      /// </summary>
      /// <returns>A random domain name.</returns>
      public string DomainName()
      {
         return DomainWord() + "." + DomainSuffix();
      }

      /// <summary>
      /// Generates a domain word used for domain names.
      /// </summary>
      /// <returns>A random domain word.</returns>
      public string DomainWord()
      {
         var domain = Name.FirstName().ToLower();

         return Regex.Replace(domain, @"([\\ ~#&*{}/:<>?|\""'])", string.Empty);
      }

      /// <summary>
      /// Generates a domain name suffix like .com, .net, .org
      /// </summary>
      /// <returns>A random domain suffix.</returns>
      public string DomainSuffix()
      {
         return GetRandomArrayItem("domain_suffix");
      }

      /// <summary>
      /// Gets a random IPv4 address string.
      /// </summary>
      /// <returns>A random IPv4 address.</returns>
      public string Ip()
      {
         return $"{Random.Number(1, 255)}.{Random.Number(255)}.{Random.Number(255)}.{Random.Number(255)}";
      }

      /// <summary>
      /// Generates a random port number.
      /// </summary>
      /// <returns>A random port number</returns>
      public int Port()
      {
         return this.Random.Number(min: IPEndPoint.MinPort + 1, max: IPEndPoint.MaxPort);
      }

      /// <summary>
      /// Gets a random IPv4 IPAddress type.
      /// </summary>
      public IPAddress IpAddress()
      {
         var bytes = this.Random.Bytes(4);
         if( bytes[0] == 0 ) bytes[0]++;
         var address = new IPAddress(bytes);
         return address;
      }

      /// <summary>
      /// Gets a random IPv4 IPEndPoint.
      /// </summary>
      /// <returns>A random IPv4 IPEndPoint.</returns>
      public IPEndPoint IpEndPoint()
      {
         var address = this.IpAddress();
         var port = this.Random.Int(IPEndPoint.MinPort + 1, IPEndPoint.MaxPort);
         return new IPEndPoint(address, port);
      }

      /// <summary>
      /// Generates a random IPv6 address string.
      /// </summary>
      /// <returns>A random IPv6 address.</returns>
      public string Ipv6()
      {
         var bytes = this.Random.Bytes(16);
         return
            $"{bytes[0]:x}{bytes[1]:x}:{bytes[2]:x}{bytes[3]:x}:{bytes[4]:x}{bytes[5]:x}:{bytes[6]:x}{bytes[7]:x}:{bytes[8]:x}{bytes[9]:x}:{bytes[10]:x}{bytes[11]:x}:{bytes[12]:x}{bytes[13]:x}:{bytes[14]:x}{bytes[15]:x}";
      }

      /// <summary>
      /// Generate a random IPv6 IPAddress type.
      /// </summary>
      /// <returns></returns>
      public IPAddress Ipv6Address()
      {
         var address = new IPAddress(this.Random.Bytes(16));
         return address;
      }

      /// <summary>
      /// Gets a random IPv6 IPEndPoint.
      /// </summary>
      /// <returns>A random IPv6 IPEndPoint.</returns>
      public IPEndPoint Ipv6EndPoint()
      {
         var address = this.Ipv6Address();
         var port = this.Random.Int(IPEndPoint.MinPort + 1, IPEndPoint.MaxPort);
         return new IPEndPoint(address, port);
      }

      private UserAgentGenerator userAgentGenerator;

      /// <summary>
      /// Generates a random user agent.
      /// </summary>
      /// <returns>A random user agent.</returns>
      public string UserAgent()
      {
         return userAgentGenerator.Generate();
      }

      /// <summary>
      /// Gets a random mac address.
      /// </summary>
      /// <param name="separator">The string the mac address should be separated with.</param>
      /// <returns>A random mac address.</returns>
      public string Mac(string separator = ":")
      {
         var arr = Enumerable.Range(0, 6)
            .Select(_ => this.Random.Number(0, 255).ToString("x2"));

         return string.Join(separator, arr);
      }

      /// <summary>
      /// Generates a random password.
      /// </summary>
      /// <param name="length">Length of the password.</param>
      /// <param name="memorable">A memorable password (ie: all lower case).</param>
      /// <param name="regexPattern">Regex pattern that the password should follow.</param>
      /// <param name="prefix">Password prefix.</param>
      /// <returns>A random password.</returns>
      public string Password(int length = 10, bool memorable = false, string regexPattern = "\\w", string prefix = "")
      {
         string consonant, vowel;
         vowel = "[aeiouAEIOU]$";
         consonant = "[bcdfghjklmnpqrstvwxyzBCDFGHJKLMNPQRSTVWXYZ]";

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

         var asciiNumber = this.Random.Number(32, 126); //ascii
         var character = Convert.ToChar(asciiNumber).ToString();
         if( memorable )
         {
            character = character.ToLowerInvariant();
         }

         if( !Regex.IsMatch(character, regexPattern) )
         {
            return Password(length, memorable, regexPattern, prefix);
         }

         return Password(length, memorable, regexPattern, prefix + character);
      }

      /// <summary>
      /// Gets a random aesthetically pleasing color near the base RGB. See [here](http://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette).
      /// </summary>
      /// <param name="baseRed">Red base color</param>
      /// <param name="baseGreen">Green base color</param>
      /// <param name="baseBlue">Blue base color</param>
      /// <param name="grayscale">Output a gray scale color</param>
      /// <param name="format">The color format</param>
      /// <returns>A random color.</returns>
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

         var r = (byte)red;
         var g = (byte)green;
         var b = (byte)blue;

         if( format == ColorFormat.Hex )
         {
            return $"#{r:x02}{g:x02}{b:x02}";
         }

         if( format == ColorFormat.Delimited )
         {
            return DelimitedRgb();
         }

         return $"rgb({DelimitedRgb()})";

         string DelimitedRgb()
         {
            return $"{r},{g},{b}";
         }
      }

      /// <summary>
      /// Returns a random protocol. HTTP or HTTPS.
      /// </summary>
      /// <returns>A random protocol.</returns>
      public string Protocol()
      {
         var protocols = new[] {"http", "https"};

         return Random.ArrayElement(protocols);
      }

      /// <summary>
      /// Generates a random URL.
      /// </summary>
      /// <returns>A random URL.</returns>
      public string Url()
      {
         return Url(null, null);
      }

      /// <summary>
      /// Get an absolute URL with random path.
      /// </summary>
      /// <param name="protocol">Protocol part of the URL, random if null</param>
      /// <param name="domain">Domain part of the URL, random if null</param>
      /// <param name="fileExt">The file extension to use in the path, directory if null</param>
      /// <returns>An URL with a random path.</returns>
      public string UrlWithPath(string protocol = null, string domain = null, string fileExt = null)
      {
         var path = UrlRootedPath(fileExt);
         return $"{Url(protocol, domain)}{path}";
      }

      /// <summary>
      /// Get a rooted URL path like: /foo/bar. Optionally with file extension.
      /// </summary>
      /// <param name="fileExt">Optional: The file extension to use. If <paramref name="fileExt"/> is null, then a rooted URL directory is returned.</param>
      /// <returns>Returns a rooted URL path like: /foo/bar; optionally with a file extension.</returns>
      public string UrlRootedPath(string fileExt = null)
      {
         var words = Random.WordsArray(1, 3)
            .Select(Utils.Slugify)
            .Select(s => s.ToLower())
            .ToArray();

         var path = $"/{Utils.Slashify(words)}";

         return Path.ChangeExtension(path, fileExt);
      }

      private string Url(string protocol, string domain)
      {
         return $"{protocol ?? Protocol()}://{domain ?? DomainName()}";
      }
   }
}
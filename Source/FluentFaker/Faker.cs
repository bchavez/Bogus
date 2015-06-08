using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentFaker
{
    public class Card
    {
        public class CardAddress
        {
            public class CardGeo
            {
                public double Lat;
                public double Lng;
            }

            public string Street;
            public string Suite;
            public string City;
            public string ZipCode;
            public CardGeo Geo;
        }

        public class CardCompany
        {
            public string Name;
            public string CatchPhrase;
            public string Bs;
        }

        public Card(string locale = "en")
        {
            var gname = new Name(locale);

            this.Name = gname.FirstName();

            var ginternet = new Internet(locale);

            this.UserName = ginternet.UserName(this.Name);
            this.Email = ginternet.Email(this.UserName);
            this.Website = ginternet.DomainName();
            this.Avatar = ginternet.Avatar();
            
            var gdate = new Date(){Locale = locale};

            this.DateOfBirth = gdate.Past(50, DateTime.Parse("9/20/1992"));

            var gphone = new PhoneNumbers(locale);
            this.Phone = gphone.PhoneNumber();

            var gaddress = new Address(locale);

            this.Address = new CardAddress
                {
                    Street = gaddress.StreetAddress(),
                    Suite = gaddress.SecondaryAddress(),
                    City = gaddress.City(),
                    ZipCode = gaddress.ZipCode(),
                    Geo = new CardAddress.CardGeo
                        {
                            Lat = gaddress.Latitude(),
                            Lng = gaddress.Longitude()
                        }
                };

            var gcompany = new Company(locale);

            this.Company = new CardCompany
                {
                    Name = gcompany.CompanyName(),
                    CatchPhrase = gcompany.CatchPhrase(),
                    Bs = gcompany.Bs()
                };
        }

        public string Name;
        public string UserName;
        public string Avatar;
        public string Email;
        public DateTime DateOfBirth;
        public CardAddress Address;
        public string Phone;
        public string Website;
        public CardCompany Company;
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }



    public static class Faker
    {
        private static Dictionary<string, string[]>
            definitions = new Dictionary<string, string[]>
                {
                    {"name", new[] {"first_name", "last_name", "prefix", "suffix"}},
                    {"address", new[] {"city_prefix", "city_suffix", "street_suffix", "county", "country", "state", "state_abbr"}},
                    {"company", new[] {"adjective", "noun", "descriptor", "bs_adjective", "bs_noun", "bs_verb"}},
                    {"lorem", new[] {"words"}},
                    {"hacker", new[] {"abbreviation", "adjective", "noun", "verb", "ingverb"}},
                    {"phone_number", new[] {"formats"}},
                    {"finance", new[] {"account_type", "transaction_type", "currency"}},
                    {"internet", new[] {"avatar_uri", "domain_suffix", "free_email", "password"}}
                };


        public static Lazy<JObject> Data = new Lazy<JObject>(Initialize);

        private static JObject Initialize()
        {
            using( var s = typeof(Faker).Assembly.GetManifestResourceStream("FluentFaker.data.locales.json"))
            using( var sr = new StreamReader(s) )
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<JObject>(new JsonTextReader(sr));
            }
        }

        public static JToken Get(string category, string subKind, string locale = "en", string localeFallback = "en" )
        {
            var path = string.Format("{0}.{1}.{2}", locale, category, subKind);
            var jtoken = Data.Value.SelectToken(path);

            if( jtoken != null )
            {
                return jtoken;
            }

            //fallback path
            var fallbackPath = string.Format("{0}.{1}.{2}", localeFallback, category, subKind);

            return Data.Value.SelectToken(fallbackPath, errorWhenNoMatch: true);
        }

        
    }
    
    public class Faker<T> where T : class
    {
        public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> prop, Func<TProperty> generator )
        {

            return this;
        }
    }
}
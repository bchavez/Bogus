#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Bogus.DataSets;
using Newtonsoft.Json;

namespace Bogus
{
    /// <summary>
    /// Uses Faker to generate a person with contextually relevant fields.
    /// </summary>
    public class Person
    {
        //context variable to store state from Bogus.Extensions so, they
        //keep returning the result on each person.
        internal Dictionary<string, object> context = new Dictionary<string, object>();

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

        public Person(string locale = "en")
        {
            Initialize(locale);
        }

        protected virtual void Initialize(string locale)
        {
            var gname = new Name(locale);

            this.FirstName = gname.FirstName();
            this.LastName = gname.LastName();

            var ginternet = new Internet(locale);

            this.UserName = ginternet.UserName(this.FirstName, this.LastName);
            this.Email = ginternet.Email(this.UserName, this.LastName);
            this.Website = ginternet.DomainName();
            this.Avatar = ginternet.Avatar();

            var gdate = new Date() { Locale = locale };

            this.DateOfBirth = gdate.Past(50, DateTime.Now.AddYears(-20));

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

        public string FirstName;
        public string LastName;
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
}

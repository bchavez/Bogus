#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Bogus.DataSets;

namespace Bogus
{
   /// <summary>
   /// Uses Faker to generate a person with contextually relevant fields.
   /// </summary>
   public class Person : IHasRandomizer
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

      protected Name DsName { get; set; }
      protected Internet DsInternet { get; set; }
      protected Date DsDate { get; set; }
      protected PhoneNumbers DsPhoneNumbers { get; set; }
      protected Address DsAddress { get; set; }
      protected Company DsCompany { get; set; }

      public Person(string locale = "en")
      {
         this.GetDataSources(locale);
         this.Populate();
      }

      internal Person(Randomizer randomizer, string locale = "en")
      {
         this.GetDataSources(locale);
         this.Random = randomizer;
         this.Populate();
      }

      private void GetDataSources(string locale)
      {
         this.DsName = this.Notifier.Flow(new Name(locale));
         this.DsInternet = this.Notifier.Flow(new Internet(locale));
         this.DsInternet = this.Notifier.Flow(new Internet(locale));
         this.DsDate = this.Notifier.Flow(new Date { Locale = locale });
         this.DsPhoneNumbers = this.Notifier.Flow(new PhoneNumbers(locale));
         this.DsAddress = this.Notifier.Flow(new Address(locale));
         this.DsCompany = this.Notifier.Flow(new Company(locale));
      }

      protected internal virtual void Populate()
      {
         this.FirstName = this.DsName.FirstName();
         this.LastName = this.DsName.LastName();

         this.UserName = this.DsInternet.UserName(this.FirstName, this.LastName);
         this.Email = this.DsInternet.Email(this.FirstName, this.LastName);
         this.Website = this.DsInternet.DomainName();
         this.Avatar = this.DsInternet.Avatar();

         this.DateOfBirth = this.DsDate.Past(50, DateTime.Now.AddYears(-20));

         this.Phone = this.DsPhoneNumbers.PhoneNumber();

         this.Address = new CardAddress
         {
            Street = this.DsAddress.StreetAddress(),
            Suite = this.DsAddress.SecondaryAddress(),
            City = this.DsAddress.City(),
            ZipCode = this.DsAddress.ZipCode(),
            Geo = new CardAddress.CardGeo
            {
               Lat = this.DsAddress.Latitude(),
               Lng = this.DsAddress.Longitude()
            }
         };

         this.Company = new CardCompany
         {
            Name = this.DsCompany.CompanyName(),
            CatchPhrase = this.DsCompany.CatchPhrase(),
            Bs = this.DsCompany.Bs()
         };
      }

      protected SeedNotifier<DataSet> Notifier = new SeedNotifier<DataSet>();

      private Randomizer randomizer;
      public Randomizer Random
      {
         get => this.randomizer ?? (this.Random = new Randomizer());
         set
         {
            this.randomizer = value;
            this.Notifier.Notify(value);
         }
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
   }
}

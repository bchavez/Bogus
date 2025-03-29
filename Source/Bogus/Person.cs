#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Bogus.DataSets;

namespace Bogus;

/// <summary>
/// Uses Faker to generate a person with contextually relevant fields.
/// </summary>
public class Person : IHasRandomizer, IHasContext
{
   //context variable to store state from Bogus.Extensions so, they
   //keep returning the result on each person.
   internal Dictionary<string, object> context = new();

   Dictionary<string, object> IHasContext.Context => this.context;

   private bool isPopulated = false;

   private void EnsurePopulated()
   {
      if (!isPopulated)
      {
         Populate();
         isPopulated = true;
      }
   }

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
      public string State;
      public string ZipCode;
      public CardGeo Geo;
   }

   public class CardCompany
   {
      public string Name;
      public string CatchPhrase;
      public string Bs;
   }

   protected internal Name DsName { get; set; }
   protected internal Internet DsInternet { get; set; }
   protected internal Date DsDate { get; set; }
   protected internal PhoneNumbers DsPhoneNumbers { get; set; }
   protected internal Address DsAddress { get; set; }
   protected internal Company DsCompany { get; set; }

   /// <summary>
   /// Creates a new Person object.
   /// </summary>
   /// <param name="locale">The locale to use. Defaults to 'en'.</param>
   /// <param name="seed">The seed used to generate person data. When a <paramref name="seed"/> is specified,
   /// the Randomizer.Seed global static is ignored and a locally isolated derived seed is used to derive randomness.
   /// However, if the <paramref name="seed"/> parameter is null, then the Randomizer.Seed global static is used to derive randomness.
   /// </param>
   public Person(string locale = "en", int? seed = null, DateTime? refDate = null)
   {
      this.GetDataSources(locale);
      if( seed.HasValue )
      {
         this.Random = new Randomizer(seed.Value);
      }
      if( refDate.HasValue )
      {
         this.DsDate.LocalSystemClock = () => refDate.Value;
      }
      this.Populate();
   }

   internal Person(Randomizer randomizer, DateTime? refDate, string locale = "en", bool skipPopulate = false)
   {
      this.GetDataSources(locale);
      this.Random = randomizer;
      if( refDate.HasValue )
      {
         this.DsDate.LocalSystemClock = () => refDate.Value;
      }
      if (!skipPopulate)
      {
         this.Populate();
      }
   }

   private void GetDataSources(string locale)
   {
      this.DsName = this.Notifier.Flow(new Name(locale));
      this.DsInternet = this.Notifier.Flow(new Internet(locale));
      this.DsDate = this.Notifier.Flow(new Date {Locale = locale});
      this.DsPhoneNumbers = this.Notifier.Flow(new PhoneNumbers(locale));
      this.DsAddress = this.Notifier.Flow(new Address(locale));
      this.DsCompany = this.Notifier.Flow(new Company(locale));
   }

   protected internal virtual void Populate()
   {
      this.Gender = this.Random.Enum<Name.Gender>();
      this.FirstName = this.DsName.FirstName(this.Gender);
      this.LastName = this.DsName.LastName(this.Gender);
      this.FullName = $"{this.FirstName} {this.LastName}";

      this.UserName = this.DsInternet.UserName(this.FirstName, this.LastName);
      this.Email = this.DsInternet.Email(this.FirstName, this.LastName);
      this.Website = this.DsInternet.DomainName();
      this.Avatar = this.DsInternet.Avatar();

      this.DateOfBirth = this.DsDate.Past(50, this.DsDate.GetTimeReference().AddYears(-20));

      this.Phone = this.DsPhoneNumbers.PhoneNumber();

      this.Address = new CardAddress
         {
            Street = this.DsAddress.StreetAddress(),
            Suite = this.DsAddress.SecondaryAddress(),
            City = this.DsAddress.City(),
            State = this.DsAddress.State(),
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

   protected SeedNotifier Notifier = new();

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

   SeedNotifier IHasRandomizer.GetNotifier()
   {
      return this.Notifier;
   }

   public Name.Gender Gender;
   public string FirstName;
   public string LastName;
   public string FullName;
   public string UserName;
   public string Avatar;
   public string Email;
   public DateTime DateOfBirth;
   public CardAddress Address;
   public string Phone;
   public string Website;
   public CardCompany Company;

   public string GetGender()
   {
      if (this.Gender == default)
      {
         EnsurePopulated();
      }
      return this.Gender.ToString();
   }

   public string GetFirstName()
   {
      if (this.FirstName == null)
      {
         EnsurePopulated();
      }
      return this.FirstName;
   }

   public string GetLastName()
   {
      if (this.LastName == null)
      {
         EnsurePopulated();
      }
      return this.LastName;
   }

   public string GetFullName()
   {
      if (this.FullName == null)
      {
         EnsurePopulated();
      }
      return this.FullName;
   }

   public string GetUserName()
   {
      if (this.UserName == null)
      {
         EnsurePopulated();
      }
      return this.UserName;
   }

   public string GetAvatar()
   {
      if (this.Avatar == null)
      {
         EnsurePopulated();
      }
      return this.Avatar;
   }

   public string GetEmail()
   {
      if (this.Email == null)
      {
         EnsurePopulated();
      }
      return this.Email;
   }

   public DateTime GetDateOfBirth()
   {
      if (this.DateOfBirth == default(DateTime))
      {
         EnsurePopulated();
      }
      return this.DateOfBirth;
   }

   public double GetAddressGeoLat()
   {
      if (this.Address == null || this.Address.Geo == null)
      {
         EnsurePopulated();
      }
      return this.Address.Geo.Lat;
   }

   public double GetAddressGeoLng()
   {
      if (this.Address == null || this.Address.Geo == null)
      {
         EnsurePopulated();
      }
      return this.Address.Geo.Lng;
   }

   public string GetAddressStreet()
   {
      if (this.Address == null)
      {
         EnsurePopulated();
      }
      return this.Address.Street;
   }

   public string GetAddressSuite()
   {
      if (this.Address == null)
      {
         EnsurePopulated();
      }
      return this.Address.Suite;
   }

   public string GetAddressCity()
   {
      if (this.Address == null)
      {
         EnsurePopulated();
      }
      return this.Address.City;
   }

   public string GetAddressState()
   {
      if (this.Address == null)
      {
         EnsurePopulated();
      }
      return this.Address.State;
   }

   public string GetAddressZipCode()
   {
      if (this.Address == null)
      {
         EnsurePopulated();
      }
      return this.Address.ZipCode;
   }

   public string GetPhone()
   {
      if (this.Phone == null)
      {
         EnsurePopulated();
      }
      return this.Phone;
   }

   public string GetWebsite()
   {
      if (this.Website == null)
      {
         EnsurePopulated();
      }
      return this.Website;
   }

   public string GetCompanyName()
   {
      if (this.Company == null)
      {
         EnsurePopulated();
      }
      return this.Company.Name;
   }

   public string GetCompanyCatchPhrase()
   {
      if (this.Company == null)
      {
         EnsurePopulated();
      }
      return this.Company.CatchPhrase;
   }

   public string GetCompanyBs()
   {
      if (this.Company == null)
      {
         EnsurePopulated();
      }
      return this.Company.Bs;
   }
}
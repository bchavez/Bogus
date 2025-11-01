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

   public class CardAddress
   {
      public class CardGeo
      {
         public double Lat { get; set; }
         public double Lng { get; set; }
      }

      public string Street { get; set; }
      public string Suite { get; set; }
      public string City { get; set; }
      public string State { get; set; }
      public string ZipCode { get; set; }
      public CardGeo Geo { get; set; }
   }

   public class CardCompany
   {
      public string Name { get; set; }
      public string CatchPhrase { get; set; }
      public string Bs { get; set; }
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

   internal Person(Randomizer randomizer, DateTime? refDate, string locale = "en")
   {
      this.GetDataSources(locale);
      this.Random = randomizer;
      if( refDate.HasValue )
      {
         this.DsDate.LocalSystemClock = () => refDate.Value;
      }
      this.Populate();
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
   [RegisterPersonProperty]
   public Name.Gender Gender { get; set; }
   [RegisterPersonProperty]
   public string FirstName { get; set; }
   [RegisterPersonProperty]
   public string LastName { get; set; }
   [RegisterPersonProperty]
   public string FullName { get; set; }
   [RegisterPersonProperty]
   public string UserName { get; set; }
   [RegisterPersonProperty]
   public string Avatar { get; set; }
   [RegisterPersonProperty]
   public string Email { get; set; }
   [RegisterPersonProperty]
   public DateTime DateOfBirth { get; set; }
   [RegisterPersonProperty]
   public CardAddress Address { get; set; }
   [RegisterPersonProperty]
   public string Phone { get; set; }
   [RegisterPersonProperty]
   public string Website { get; set; }
   [RegisterPersonProperty]
   public CardCompany Company { get; set; }
}
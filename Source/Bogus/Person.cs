//#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Bogus.DataSets;

namespace Bogus;

/// <summary>
/// Uses Faker to generate a person with contextually relevant fields.
/// </summary>
public class Person : IHasRandomizer, IHasContext
{
   /// <summary>
   /// Context variable to store state from Bogus.Extensions so, they
   /// keep returning the result on each person.
   /// </summary>
   internal Dictionary<string, object> context = new();

   private Randomizer randomizer;

   /// <summary>
   ///    Creates a new Person object.
   /// </summary>
   /// <param name="locale">The locale to use. Defaults to 'en'.</param>
   /// <param name="seed">
   ///    The seed used to generate person data. When a <paramref name="seed" /> is specified,
   ///    the Randomizer.Seed global static is ignored and a locally isolated derived seed is used to derive randomness.
   ///    However, if the <paramref name="seed" /> parameter is null, then the Randomizer.Seed global static is used to derive
   ///    randomness.
   /// </param>
   public Person(string locale = "en", int? seed = null, DateTime? refDate = null)
   {
      InitializeDataSources(locale);
      ConfigureRandomization(seed);
      ConfigureReferenceDate(refDate);
      Populate();
   }

   /// <summary>
   /// .
   /// </summary>
   /// <param name="randomizer"></param>
   /// <param name="refDate"></param>
   /// <param name="locale"></param>
   internal Person(Randomizer randomizer, DateTime? refDate, string locale = "en")
   {
      InitializeDataSources(locale);
      Random = randomizer;
      ConfigureReferenceDate(refDate);
      Populate();
   }

   Dictionary<string, object> IHasContext.Context => context;

   public string Avatar { get; set; }
   public string Email { get; set; }
   public string FirstName { get; set; }
   public string FullName { get; set; }
   public string LastName { get; set; }
   public string Phone { get; set; }
   public string UserName { get; set; }
   public string Website { get; set; }
   public DateTime DateOfBirth { get; set; }
   public Name.Gender Gender { get; set; }
   public CardAddress Address { get; set; }
   public CardCompany Company { get; set; }

   protected internal Address DsAddress { get; set; }
   protected internal Company DsCompany { get; set; }
   protected internal Date DsDate { get; set; }
   protected internal Internet DsInternet { get; set; }
   protected internal Name DsName { get; set; }
   protected internal PhoneNumbers DsPhoneNumbers { get; set; }

   protected SeedNotifier Notifier { get; } = new();

   public Randomizer Random
   {
      get => randomizer ?? (Random = new Randomizer());
      set
      {
         randomizer = value;
         Notifier.Notify(value);
      }
   }

   SeedNotifier IHasRandomizer.GetNotifier() => Notifier;

   /// <summary>
   /// Generates the Person data.
   /// </summary>
   protected internal virtual void Populate()
   {
      // Generate consistent person data
      Gender = Random.Enum<DataSets.Name.Gender>();
      FirstName = DsName.FirstName(Gender);
      LastName = DsName.LastName(Gender);
      FullName = $"{FirstName} {LastName}";

      UserName = DsInternet.UserName(FirstName, LastName);
      Email = DsInternet.Email(FirstName, LastName);
      Website = DsInternet.DomainName();
      Avatar = DsInternet.Avatar();

      DateOfBirth = DsDate.Past(50, DsDate.GetTimeReference().AddYears(-20));
      Phone = DsPhoneNumbers.PhoneNumber();

      InitializeAddress();
      InitializeCompany();
   }

   private void InitializeDataSources(string locale)
   {
      DsName = Notifier.Flow(new DataSets.Name(locale));
      DsInternet = Notifier.Flow(new DataSets.Internet(locale));
      DsDate = Notifier.Flow(new DataSets.Date { Locale = locale });
      DsPhoneNumbers = Notifier.Flow(new DataSets.PhoneNumbers(locale));
      DsAddress = Notifier.Flow(new DataSets.Address(locale));
      DsCompany = Notifier.Flow(new DataSets.Company(locale));
   }

   private void ConfigureRandomization(int? seed)
   {
      if (seed.HasValue)
      {
         Random = new Randomizer(seed.Value);
      }
   }

   private void ConfigureReferenceDate(DateTime? refDate)
   {
      if (refDate.HasValue)
      {
         DsDate.LocalSystemClock = () => refDate.Value;
      }
   }

   private void InitializeAddress()
   {
      Address = new CardAddress
      {
         Street = DsAddress.StreetAddress(),
         Suite = DsAddress.SecondaryAddress(),
         City = DsAddress.City(),
         State = DsAddress.State(),
         ZipCode = DsAddress.ZipCode(),
         Geo = new CardAddress.CardGeo
         {
            Lat = DsAddress.Latitude(),
            Lng = DsAddress.Longitude()
         }
      };
   }

   private void InitializeCompany()
   {
      Company = new CardCompany
      {
         Name = DsCompany.CompanyName(),
         CatchPhrase = DsCompany.CatchPhrase(),
         Bs = DsCompany.Bs()
      };
   }

   public class CardAddress
   {
      public string City { get; set; }
      public string State { get; set; }
      public string Street { get; set; }
      public string Suite { get; set; }
      public string ZipCode { get; set; }
      public CardGeo Geo { get; set; }

      public class CardGeo
      {
         public double Lat { get; set; }
         public double Lng { get; set; }
      }
   }

   public class CardCompany
   {
      public string Bs { get; set; }
      public string CatchPhrase { get; set; }
      public string Name { get; set; }
   }
}
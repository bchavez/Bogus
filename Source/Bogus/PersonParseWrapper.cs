#pragma warning disable 1591

using System;

namespace Bogus.PersonParseWrapper
{

   /// <summary>
   /// Creates a wrapper around Person to allow for lazy loading of Bogus.Person which facilitates parsing.
   /// </summary>
   public class Person
   {

      Lazy<Bogus.Person> lazyPerson;

      /// <summary>
      /// Initialising Lazy loading of Bogus.Person to prevent it from interfering with seeds during unit tests.
      /// </summary>
      /// <param name="randomizer"></param>
      /// <param name="refDate"></param>
      /// <param name="locale"></param>
      internal Person(Randomizer randomizer, DateTime? refDate, string locale = "en")
      {
         lazyPerson = new(() => new Bogus.Person(randomizer, refDate, locale));
      }

      public string Gender()
      {
         return lazyPerson.Value.Gender.ToString();
      }

      public string FirstName()
      {
         return lazyPerson.Value.FirstName;
      }

      public string LastName()
      {
         return lazyPerson.Value.LastName;
      }

      public string FullName()
      {
         return lazyPerson.Value.FullName;
      }

      public string UserName()
      {
         return lazyPerson.Value.UserName;
      }

      public string Avatar()
      {
         return lazyPerson.Value.Avatar;
      }

      public string Email()
      {
         return lazyPerson.Value.Email;
      }

      public DateTime DateOfBirth()
      {
         return lazyPerson.Value.DateOfBirth;
      }

      public double AddressGeoLat()
      {
         return lazyPerson.Value.Address.Geo.Lat;
      }

      public double AddressGeoLng()
      {
         return lazyPerson.Value.Address.Geo.Lng;
      }

      public string AddressStreet()
      {
         return lazyPerson.Value.Address.Street;
      }

      public string AddressSuite()
      {
         return lazyPerson.Value.Address.Suite;
      }

      public string AddressCity()
      {
         return lazyPerson.Value.Address.City;
      }

      public string AddressState()
      {
         return lazyPerson.Value.Address.State;
      }

      public string AddressZipCode()
      {
         return lazyPerson.Value.Address.ZipCode;
      }

      public string Phone()
      {
         return lazyPerson.Value.Phone;
      }

      public string Website()
      {
         return lazyPerson.Value.Website;
      }

      public string CompanyName()
      {
         return lazyPerson.Value.Company.Name;
      }

      public string CompanyCatchPhrase()
      {
         return lazyPerson.Value.Company.CatchPhrase;
      }

      public string CompanyBs()
      {
         return lazyPerson.Value.Company.Bs;
      }
   }
}
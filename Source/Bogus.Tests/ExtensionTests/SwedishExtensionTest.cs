using System;
using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions.Sweden;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests;

public class SwedishExtensionTest : SeededTest
{
   [Fact]
   public void can_create_valid_swedish_personnummer()
   {
      var f = new Faker("sv");
      var person = f.Person;

      var personnummer = person.Personnummer();

      CheckLuhn(personnummer.Substring(2)).Should().BeTrue();
   }

   [Fact]
   public void can_create_valid_swedish_samordningsnummer()
   {
      var f = new Faker("sv");
      var person = f.Person;

      var samordningsnummer = person.Samordningsnummer();

      CheckLuhn(samordningsnummer.Substring(2)).Should().BeTrue();
   }

   [Fact]
   public void personnummer_should_contain_valid_date_of_birth()
   {
      var f = new Faker("sv");
      var person = f.Person;

      var personnummer = person.Personnummer();
      var (year, month, day) = ExtractDateParts(personnummer);
      var dateOfBirth = new DateTime(year, month, day);

      dateOfBirth.Date.Should().Be(person.DateOfBirth.Date);
   }

   [Fact]
   public void samordningsnummer_should_contain_offset_date_of_birth()
   {
      var f = new Faker("sv");
      var person = f.Person;

      var samordningsnummer = person.Samordningsnummer();
      var (year, month, day) = ExtractDateParts(samordningsnummer);
      var dateOfBirth = new DateTime(year, month, day - 60);

      dateOfBirth.Date.Should().Be(person.DateOfBirth.Date);
   }

   [Theory]
   [InlineData(false)]
   [InlineData(true)]
   public void when_person_is_male_second_last_number_is_odd(bool isSamordningsnummer)
   {
      var f = new Faker("sv");
      var person = f.Person;
      person.Gender = Name.Gender.Male;

      var identificationNumber = isSamordningsnummer ? person.Samordningsnummer() : person.Personnummer();

      var secondLast = int.Parse(identificationNumber.Substring(identificationNumber.Length - 2, 1));

      secondLast.Should()
         .Match(x => x % 2 == 1)
         .And.BeLessThan(10)
         .And.BeGreaterThan(0);
   }

   [Theory]
   [InlineData(false)]
   [InlineData(true)]
   public void when_person_is_female_second_last_number_is_even(bool isSamordningsnummer)
   {
      var f = new Faker("sv");
      var person = f.Person;
      person.Gender = Name.Gender.Female;

      var identificationNumber = isSamordningsnummer ? person.Samordningsnummer() : person.Personnummer();

      var secondLast = int.Parse(identificationNumber.Substring(identificationNumber.Length - 2, 1));

      secondLast.Should()
         .Match(x => x % 2 == 0)
         .And.BeLessThan(10)
         .And.BeGreaterThan(0);
   }

   private static bool CheckLuhn(string digits)
   {
      return digits.All(char.IsDigit) && digits.Reverse()
         .Select(c => c - 48)
         .Select(
            (thisNum, i) => i % 2 == 0
               ? thisNum
               : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
         )
         .Sum() % 10 == 0;
   }

   private static (int year, int month, int day) ExtractDateParts(string date)
   {
      return (int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(4, 2)), int.Parse(date.Substring(6, 2)));
   }
}
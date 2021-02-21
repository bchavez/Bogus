using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions.Sweden;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests
{
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
      public void when_person_is_male_second_last_number_is_even()
      {
         var f = new Faker("sv");
         var person = f.Person;
         person.Gender = Name.Gender.Male;

         var personnummer = person.Personnummer();

         var secondLast = int.Parse(personnummer.Substring(personnummer.Length - 2, 1));


         secondLast.Should()
            .Match(x => x % 2 == 0)
            .And.BeLessThan(10)
            .And.BeGreaterThan(0);
      }

      [Fact]
      public void when_person_is_female_second_last_number_is_odd()
      {
         var f = new Faker("sv");
         var person = f.Person;
         person.Gender = Name.Gender.Female;

         var personnummer = person.Personnummer();

         var secondLast = int.Parse(personnummer.Substring(personnummer.Length - 2, 1));

         secondLast.Should()
            .Match(x => x % 2 == 1)
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
   }
}
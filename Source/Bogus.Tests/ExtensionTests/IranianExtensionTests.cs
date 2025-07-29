using Bogus.Extensions.Iran;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Bogus.Tests.ExtensionTests;

public class IranianExtensionTests : SeededTest
{
   [Fact]
   public void can_create_valid_iranian_national_number()
   {
      //Arrange 
      var faker = new Faker("fa");

      //Act
      var nationalNumber = faker.Person.IranianNationalNumber();

      //Assert
      nationalNumber.Should().MatchRegex("^[0-9]{10}$");
      ValidateIranianNationalNumber(nationalNumber).Should().BeTrue();
   }

   [Fact]
   public void iranian_national_number_generated_twice_are_equal()
   {
      //Arrange
      var faker = new Faker("fa");
      var person = faker.Person;

      //Act
      var nationalNumber1 = person.IranianNationalNumber();
      var nationalNumber2 = person.IranianNationalNumber();

      //Assert
      nationalNumber1.Should().Be(nationalNumber2);
   }

   [Fact]
   public void iranian_national_number_should_be_different_for_different_persons()
   {
      //Arrange
      var faker1 = new Faker("fa");
      var faker2 = new Faker("fa");
      var person1 = faker1.Person;
      var person2 = faker2.Person;

      //Act
      var nationalNumber1 = person1.IranianNationalNumber();
      var nationalNumber2 = person2.IranianNationalNumber();

      //Assert
      nationalNumber1.Should().NotBe(nationalNumber2);
   }

   /// <summary>
   /// Validates an Iranian National Number using the official algorithm.
   /// The checksum is calculated as: sum of (digit * position_weight) mod 11
   /// where position_weight starts from 10 and decreases to 2.
   /// The last digit should be the remainder, or 11 - remainder if remainder >= 2.
   /// </summary>
   private static bool ValidateIranianNationalNumber(string nationalNumber)
   {
      if (string.IsNullOrEmpty(nationalNumber) || nationalNumber.Length != 10 || !nationalNumber.All(char.IsDigit))
         return false;

      var digits = nationalNumber.Select(c => c - '0').ToArray();

      var sum = 0;
      for (int i = 0; i < 9; i++)
      {
         sum += digits[i] * (10 - i);
      }

      var remainder = sum % 11;
      var expectedChecksum = remainder < 2 ? remainder : 11 - remainder;

      return digits[9] == expectedChecksum;
   }
}

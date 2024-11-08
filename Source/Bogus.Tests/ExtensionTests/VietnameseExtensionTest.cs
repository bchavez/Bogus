using System;
using Bogus.DataSets;
using Bogus.Extensions.Vietnam;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests;

public class VietnameseExtensionTest : SeededTest
{
   #region Setup test cases

   public static TheoryData<int, Name.Gender, string> ValidVietnameseCitizenIdTestCases => new()
   {
      { 1999, Name.Gender.Male, "Hà Nội" },
      { 2000, Name.Gender.Female, "An Giang" },
      { 2001, Name.Gender.Male, "TP. Hồ Chí Minh" },
      { 2099, Name.Gender.Female, "Bà Rịa-Vũng Tàu" },
      { 2100, Name.Gender.Female, "Thanh Hoá" },
      { 2101, Name.Gender.Female, "Thừa Thiên-Huế" },
      { 2199, Name.Gender.Female, "Khánh Hoà" }
   };
   
   public static TheoryData<int, Name.Gender, string> InvalidVietnameseCitizenIdTestCases => new()
   {
      { 1899, Name.Gender.Male, "Hà Nội" }, // invalid birth year
      { 2000, Name.Gender.Male, "London" }, // invalid city
   };

   #endregion
   
   #region Tests
   
   [Fact]
   public void can_generate_vietnamese_citizen_id()
   {
      // Arrange
      var faker = new Faker("vi");
      var person = faker.Person;
      var x = faker.Person.DateOfBirth;
      
      // Act
      var obtained = person.Cccd();
      obtained.Dump();
      
      // Assert
      obtained.Should().NotBeNullOrWhiteSpace();
      ShouldBeLegalVietnameseCitizenId(obtained);
      ShouldHaveCorrectInformationPart(obtained, faker.Person);
   }
   
   [Theory]
   [MemberData(nameof(ValidVietnameseCitizenIdTestCases))]
   public void can_generate_vietnamese_citizen_id_with_valid_info(int birthYear, Name.Gender gender, string city)
   {
      // Arrange
      var faker = new Faker();
      var person = faker.Person;
      person.DateOfBirth = new DateTime(birthYear, 1, 1);
      person.Address.City = city;
      person.Gender = gender;
      
      // Act
      var obtained = person.Cccd();
      obtained.Dump();
      
      // Assert
      obtained.Should().NotBeNullOrWhiteSpace();
      ShouldBeLegalVietnameseCitizenId(obtained);
      ShouldHaveCorrectInformationPart(obtained, faker.Person);
   }

   [Theory]
   [MemberData(nameof(InvalidVietnameseCitizenIdTestCases))]
   public void cannot_generate_vietnamese_citizen_id_with_invalid_info(int birthYear, Name.Gender gender, string city)
   {
      // Arrange
      var faker = new Faker();
      var person = faker.Person;
      person.DateOfBirth = new DateTime(birthYear, 1, 1);
      person.Gender = gender;
      person.Address.City = city;
      
      // Act
      Action act = () => faker.Person.Cccd();
      
      // Assert
      act.Should().Throw<ArgumentException>();
   }

   [Fact]
   public void cannot_generate_vietnamese_citizen_id_without_vietnamese_locale()
   {
      // Arrange
      var faker = new Faker("en");
      
      // Act
      Action act = () => faker.Person.Cccd();
      
      // Assert
      act.Should().Throw<ArgumentException>();
   }

   #endregion

   #region Private methods
   
   private static void ShouldBeLegalVietnameseCitizenId(string candidate)
   {
      candidate.Should().NotBeNullOrWhiteSpace();
      candidate.Should().MatchRegex(@"^\d{12}$");
   }

   private static void ShouldHaveCorrectInformationPart(string candidate, Person p)
   {
      var provinceCityCode = candidate.Substring(0, 3);
      var genderCode = candidate.Substring(3, 1);
      var birthYearCode = candidate.Substring(4, 2);
      var randomNumber = int.Parse(candidate.Substring(6, 6));

      provinceCityCode
         .Should()
         .BeEquivalentTo(VietnameseCityCodes.CityCodes[p.Address.City]);
      genderCode
         .Should()
         .BeEquivalentTo(GetGenderCode(p.Gender, p.DateOfBirth.Year).ToString());
      birthYearCode
         .Should()
         .BeEquivalentTo(p.DateOfBirth.Year.ToString().Substring(2));
      randomNumber
         .Should()
         .BeInRange(1, 999999);
   }
   
   private static int GetGenderCode(Name.Gender gender, int year)
   {
      return year switch
      {
         >= 1900 and <= 1999 => (int)gender,
         >= 2000 and <= 2099 => (int)gender + 2,
         >= 2100 and <= 2199 => (int)gender + 4,
         _ => throw new ArgumentException($"Year of birth {year} is not supported.")
      };
   }
   
   #endregion
}
using Bogus.DataSets;
using Bogus.Extensions.Belgium;
using FluentAssertions;

namespace Bogus.Tests.ExtensionTests;

public class BelgianExtensionTests : SeededTest
{
   private readonly Faker _faker;

   public BelgianExtensionTests()
   {
      _faker = new Faker();
   }

   [Fact]
   public void can_generate_national_number_for_belgium()
   {
      // Act
      var obtained = _faker.Person.NationalNumber(includeFormatSymbols: false);

      obtained.Dump();

      // Assert
      obtained.Should().NotBeNullOrWhiteSpace();
      ShouldBeLegalBelgianNationalNumber(obtained);
      ShouldBeCorrectGenderCode(_faker.Person.Gender, obtained);
      ShouldHaveCorrectChecksum(obtained);
   }

   [Fact]
   public void excludes_formatting_national_number()
   {
      var result = _faker.Person.NationalNumber(includeFormatSymbols: false);
      result.Should().NotContainAny("-", ".");
   }

   [Fact]
   public void includes_formatting_national_number()
   {
      var result = _faker.Person.NationalNumber(includeFormatSymbols: true);
      result.Should().ContainAll("-", ".");
   }

   [Theory]
   [InlineData("850103725", "07")]
   public void checksum_is_zero_padded(string givenNationalNumber, string expectedChecksum)
   {
      var year = int.Parse(givenNationalNumber.Substring(0, 2));
      var month = int.Parse(givenNationalNumber.Substring(2, 2));
      var day = int.Parse(givenNationalNumber.Substring(4, 2));
      var dateOfBirth = new DateTime(year, month, day);

      var checkNumber = ExtensionsForBelgium.CalculateCheckNumber(givenNationalNumber, dateOfBirth);

      checkNumber.Should().Be(expectedChecksum);
   }

   private void ShouldHaveCorrectChecksum(string candidate)
   {
      var baseNumber = long.Parse(candidate.Substring(0, 9));
      var checkNumber = candidate.Substring(9);
      var birthYear = int.Parse(candidate.Substring(0, 2));

      if (birthYear == 00)
         baseNumber += 2000000000L;

      var expectedCheckNumber = 97 - (int)(baseNumber % 97);

      checkNumber.Should().Be(expectedCheckNumber.ToString());
   }

   private void ShouldBeLegalBelgianNationalNumber(string candidate)
   {
      // Check if the first 6 digits represent a valid date.
      var year = int.Parse(candidate.Substring(0, 2));
      var month = int.Parse(candidate.Substring(2, 2));
      var day = int.Parse(candidate.Substring(4, 2));

      day.Should().BeInRange(1, 31);
      month.Should().BeInRange(1, 12);
      year.Should().BeInRange(0, 99);
   }

   private void ShouldBeCorrectGenderCode(Name.Gender gender, string candidate)
   {
      var sequenceNumber = int.Parse(candidate.Substring(6, 3));

      if (gender == Name.Gender.Female)
      {
         sequenceNumber.Should()
            .Match(x => x % 2 == 0);
      }

      if (gender == Name.Gender.Male)
      {
         sequenceNumber.Should()
            .Match(x => x % 2 == 1);
      }
   }
}

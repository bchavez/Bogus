using System;
using Bogus.DataSets;
using Bogus.Extensions.Denmark;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests;

public class DanishExtensionTest : SeededTest
{
   private readonly Faker _faker;

   public DanishExtensionTest()
   {
      _faker = new Faker();
   }

   [Fact]
   public void can_generate_cpr_number_for_denmark()
   {
      // Act
      var obtained = _faker.Person.Cpr(validChecksum: false);

      obtained.Dump();

      // Assert
      obtained.Should().NotBeNullOrWhiteSpace();
      ShouldBeLegalDanishCprNumber(obtained);
      ShouldBeCorrectGenderCode(_faker.Person.Gender, obtained);
   }

   [Fact]
   public void excludes_dash_cpr_number()
   {
      var result = _faker.Person.Cpr(includeDash: false);
      result.Should().NotContain("-");
   }

   [Theory]
   [InlineData("080165-0058", Name.Gender.Female)]
   [InlineData("080165-0066", Name.Gender.Female)]
   [InlineData("080165-0074", Name.Gender.Female)]
   [InlineData("080165-0082", Name.Gender.Female)]
   [InlineData("080165-0090", Name.Gender.Female)]
   [InlineData("250665-3595", Name.Gender.Male)]
   [InlineData("250665-3617", Name.Gender.Male)]
   [InlineData("250665-3633", Name.Gender.Male)]
   [InlineData("250665-3641", Name.Gender.Male)]
   [InlineData("250665-3749", Name.Gender.Male)]
   public void is_valid_danish_cpr_number(string candidate, Name.Gender gender)
   {
      ShouldBeCorrectGenderCode(gender, candidate);
      ShouldBeLegalDanishCprNumber(candidate);
      ShouldHaveCorrectChecksum(candidate);
   }

   [Theory]
   [InlineData("000000-0000", Name.Gender.Female)]
   [InlineData("111111-1111", Name.Gender.Male)]
   [InlineData("999999-9999", Name.Gender.Female)]
   [InlineData("AAAAAA-AAAA", Name.Gender.Female)]
   [InlineData("241212-1234", Name.Gender.Female)]
   public void is_invalid_danish_cpr_number(string candidate, Name.Gender gender)
   {
      Action action = () =>
      {
         ShouldBeCorrectGenderCode(gender, candidate);
         ShouldBeLegalDanishCprNumber(candidate);
         ShouldHaveCorrectChecksum(candidate);
      };

      action.Should().Throw<Exception>();
   }
         
   [Theory]
   [InlineData("080165", Name.Gender.Female)]
   [InlineData("080166", Name.Gender.Female)]
   [InlineData("080167", Name.Gender.Female)]
   [InlineData("080168", Name.Gender.Female)]
   [InlineData("080169", Name.Gender.Female)]
   [InlineData("250665", Name.Gender.Male)]
   [InlineData("250607", Name.Gender.Male)]
   [InlineData("250608", Name.Gender.Male)]
   [InlineData("250609", Name.Gender.Male)]
   [InlineData("250610", Name.Gender.Male)]
   public void can_generate_valid_danish_cpr_numbers(string birthDate, Name.Gender gender)
   {;
      int day = int.Parse(birthDate.Substring(0, 2));
      int month = int.Parse(birthDate.Substring(2, 2));
      int year = int.Parse(birthDate.Substring(4, 2));

      year += year < DateTime.Now.Year % 100 ? 2000 : 1900;

      var bd = new DateTime(year, month, day);

      _faker.Person.DateOfBirth = bd;
      _faker.Person.Gender = gender;

      var actual = _faker.Person.Cpr(true);

      ShouldBeCorrectGenderCode(gender, actual);
      ShouldBeLegalDanishCprNumber(actual);
      ShouldHaveCorrectChecksum(actual);
   }

   private void ShouldHaveCorrectChecksum(string candidate)
   {
      var factors = new[] { 4, 3, 2, 7, 6, 5, 4, 3, 2, 1 };
      var digits = candidate.Replace("-", "").Substring(0, 10).ToCharArray();

      int cs = 0;
      for (int i = 0; i < 10; i++)
      {
         cs += (digits[i] - '0') * factors[i];
      }

      (cs % 11).Should().Be(0);
   }

   private void ShouldBeLegalDanishCprNumber(string candidate)
   {
      var parts = candidate.Split('-');
      parts[0].Should().HaveLength(6);
      parts[1].Should().HaveLength(4);

      // Check if the first 6 digits represent a valid date.
      int day = int.Parse(parts[0].Substring(0, 2));
      int month = int.Parse(parts[0].Substring(2, 2));
      int year = int.Parse(parts[0].Substring(4, 2));

      day.Should().BeInRange(1, 31);
      month.Should().BeInRange(1, 12);
      year.Should().BeInRange(0, 99);
   }

   private void ShouldBeCorrectGenderCode(Name.Gender gender, string candidate)
   {
      var lastPart = int.Parse(candidate.Split('-')[1]);

      if (gender == Name.Gender.Female)
      {
         lastPart.Should()
            .Match(x => x % 2 == 0);
      }

      if (gender == Name.Gender.Male)
      {
         lastPart.Should()
            .Match(x => x % 2 == 1);
      }
   }
}

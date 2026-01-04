using System;
using Bogus.Extensions.Netherlands;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.ExtensionTests;

public class DutchVehicleExtensionTests : SeededTest
{
   private readonly Faker faker;
   private readonly ITestOutputHelper console;

   public DutchVehicleExtensionTests(ITestOutputHelper console)
   {
      faker = new Faker("nl");
      this.console = console;
   }

   [Fact]
   public void should_generate_valid_dutch_registration_plate()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(2020, 1, 1);
      var dateTo = new DateTime(2024, 12, 31);

      // Act
      var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);

      // Assert
      kenteken.Should().NotBeNullOrEmpty();
      kenteken.Should().MatchRegex(@"^[A-Z0-9]+-[A-Z0-9]+-[A-Z0-9]+$", "Dutch kenteken should follow the pattern with dashes");
      
      console.WriteLine($"Generated kenteken: {kenteken}");
   }

   [Theory]
   [InlineData("1951-01-01", "1964-12-31")] // Serie 1: XX-99-99
   [InlineData("1965-01-01", "1972-12-31")] // Serie 2: 99-99-XX
   [InlineData("1973-01-01", "1977-12-31")] // Serie 3: 99-XX-99
   [InlineData("1978-07-01", "1990-12-31")] // Serie 4: XX-99-XX
   [InlineData("1991-01-01", "1998-12-31")] // Serie 5: XX-XX-99
   [InlineData("1999-01-01", "2004-10-28")] // Serie 6: 99-XX-XX
   [InlineData("2004-10-29", "2013-03-04")] // Serie 7: 99-XXX-9
   [InlineData("2013-03-05", "2015-03-29")] // Serie 8: 9-XXX-99
   [InlineData("2015-03-30", "2019-08-18")] // Serie 9: XX-999-X
   [InlineData("2019-08-19", "2024-01-07")] // Serie 10: X-999-XX
   [InlineData("2024-01-08", "2024-06-03")] // Serie 12: X-99-XXX (starts before Serie 11)
   [InlineData("2024-06-04", "2024-12-31")] // Serie 11: XXX-99-X
   public void should_generate_correct_format_for_date_ranges(string fromDate, string toDate)
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = DateTime.Parse(fromDate);
      var dateTo = DateTime.Parse(toDate);

      // Act
      var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);

      // Assert
      kenteken.Should().NotBeNullOrEmpty();
      ValidateKentekenFormatByDate(kenteken, dateFrom, dateTo);
      
      console.WriteLine($"Date range: {fromDate} to {toDate}, Generated kenteken: {kenteken}");
   }

   [Fact]
   public void should_generate_serie_1_format_for_1950s()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(1951, 1, 1);
      var dateTo = new DateTime(1964, 12, 31);

      // Act
      var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);

      // Assert
      kenteken.Should().MatchRegex(@"^[A-Z]{2}-\d{2}-\d{2}$", "Serie 1 should follow XX-99-99 format");
      
      console.WriteLine($"Serie 1 kenteken: {kenteken}");
   }

   [Fact]
   public void should_generate_serie_7_format_for_2000s()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(2004, 10, 29);
      var dateTo = new DateTime(2013, 3, 4);

      // Act
      var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);

      // Assert
      kenteken.Should().MatchRegex(@"^\d{2}-[A-Z]{3}-\d{1}$", "Serie 7 should follow 99-XXX-9 format");
      
      console.WriteLine($"Serie 7 kenteken: {kenteken}");
   }

   [Fact]
   public void should_generate_serie_8_format_for_2010s()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(2013, 3, 5);
      var dateTo = new DateTime(2015, 3, 29);

      // Act
      var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);

      // Assert
      kenteken.Should().MatchRegex(@"^\d{1}-[A-Z]{3}-\d{2}$", "Serie 8 should follow 9-XXX-99 format");
      
      // Serie 8 for personenauto's should start with specific letters
      var firstLetter = kenteken.Split('-')[1][0];
      new[] { 'K', 'S', 'T', 'X', 'Z' }.Should().Contain(firstLetter, "Serie 8 personenauto should start with K, S, T, X, or Z");
      
      console.WriteLine($"Serie 8 kenteken: {kenteken}");
   }

   [Fact]
   public void should_generate_current_format_for_recent_dates()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(2024, 6, 4);
      var dateTo = new DateTime(2025, 12, 31);

      // Act
      var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);

      // Assert
      kenteken.Should().MatchRegex(@"^[A-Z]{3}-\d{2}-[A-Z]{1}$", "Current format should follow XXX-99-X format");
      
      console.WriteLine($"Current kenteken: {kenteken}");
   }

   [Fact]
   public void should_avoid_forbidden_combinations()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(2020, 1, 1);
      var dateTo = new DateTime(2024, 12, 31);

      // Act & Assert - Generate multiple kentekens to test forbidden combinations
      for (int i = 0; i < 100; i++)
      {
         var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);
         
         // Check that forbidden combinations are not present
         kenteken.Should().NotContain("SA");
         kenteken.Should().NotContain("SD");
         kenteken.Should().NotContain("SS");
         kenteken.Should().NotContain("GVD");
         kenteken.Should().NotContain("KKK");
         kenteken.Should().NotContain("LPF");
         kenteken.Should().NotContain("NSB");
         kenteken.Should().NotContain("PKK");
         kenteken.Should().NotContain("PSV");
         kenteken.Should().NotContain("PVV");
         kenteken.Should().NotContain("TBS");
         kenteken.Should().NotContain("BBB");
      }
   }

   [Fact]
   public void should_generate_different_kentekens_for_different_vehicles()
   {
      // Arrange
      var vehicle1 = faker.Vehicle;
      var vehicle2 = faker.Vehicle;
      var dateFrom = new DateTime(2020, 1, 1);
      var dateTo = new DateTime(2024, 12, 31);

      // Act
      var kenteken1 = vehicle1.NlRegistrationPlate(dateFrom, dateTo);
      var kenteken2 = vehicle2.NlRegistrationPlate(dateFrom, dateTo);

      // Assert
      kenteken1.Should().NotBe(kenteken2, "Different vehicles should have different kentekens");
      
      console.WriteLine($"Vehicle 1 kenteken: {kenteken1}");
      console.WriteLine($"Vehicle 2 kenteken: {kenteken2}");
   }

   [Fact]
   public void should_handle_date_swap_correctly()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var laterDate = new DateTime(2024, 12, 31);
      var earlierDate = new DateTime(2020, 1, 1);

      // Act - Pass dates in wrong order
      var kenteken = vehicle.NlRegistrationPlate(laterDate, earlierDate);

      // Assert
      kenteken.Should().NotBeNullOrEmpty("Method should handle swapped dates gracefully");
      
      console.WriteLine($"Kenteken with swapped dates: {kenteken}");
   }

   [Theory]
   [InlineData("1950-12-31")] // Before earliest registration
   [InlineData("2031-01-01")] // After latest registration
   public void should_throw_exception_for_invalid_date_ranges(string invalidDate)
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var validDate = new DateTime(2020, 1, 1);
      var invalidDateTime = DateTime.Parse(invalidDate);

      // Act & Assert
      Assert.Throws<ArgumentOutOfRangeException>(() => vehicle.NlRegistrationPlate(invalidDateTime, validDate));
      Assert.Throws<ArgumentOutOfRangeException>(() => vehicle.NlRegistrationPlate(validDate, invalidDateTime));
   }

   [Fact]
   public void should_generate_valid_kentekens_consistently()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(2020, 1, 1);
      var dateTo = new DateTime(2024, 12, 31);

      // Act & Assert - Generate multiple kentekens to ensure consistency
      for (int i = 0; i < 100; i++)
      {
         var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);
         
         kenteken.Should().NotBeNullOrEmpty();
         kenteken.Should().MatchRegex(@"^[A-Z0-9]+-[A-Z0-9]+-[A-Z0-9]+$");
         kenteken.Split('-').Should().HaveCount(3, "Kenteken should have exactly 3 parts separated by dashes");
      }
   }

   [Fact]
   public void should_only_use_consonants_in_modern_formats()
   {
      // Arrange
      var vehicle = faker.Vehicle;
      var dateFrom = new DateTime(2004, 10, 29); // Serie 7 onwards
      var dateTo = new DateTime(2024, 12, 31);

      // Act & Assert
      for (int i = 0; i < 50; i++)
      {
         var kenteken = vehicle.NlRegistrationPlate(dateFrom, dateTo);
         var parts = kenteken.Split('-');
         
         foreach (var part in parts)
         {
            foreach (var character in part)
            {
               if (char.IsLetter(character))
               {
                  // Should not contain vowels (A, E, I, O, U) or forbidden letters
                  character.Should().NotBe('A');
                  character.Should().NotBe('E');
                  character.Should().NotBe('I');
                  character.Should().NotBe('O');
                  character.Should().NotBe('U');
               }
            }
         }
      }
   }

   private void ValidateKentekenFormatByDate(string kenteken, DateTime dateFrom, DateTime dateTo)
   {
      // Use the middle date of the range to determine expected format
      var midDate = dateFrom.AddDays((dateTo - dateFrom).TotalDays / 2);

      if (midDate >= new DateTime(2024, 6, 4))
      {
         kenteken.Should().MatchRegex(@"^[A-Z]{3}-\d{2}-[A-Z]{1}$", "Serie 11 format");
      }
      else if (midDate >= new DateTime(2024, 1, 8))
      {
         kenteken.Should().MatchRegex(@"^[A-Z]{1}-\d{2}-[A-Z]{3}$", "Serie 12 format");
      }
      else if (midDate >= new DateTime(2019, 8, 19))
      {
         kenteken.Should().MatchRegex(@"^[A-Z]{1}-\d{3}-[A-Z]{2}$", "Serie 10 format");
      }
      else if (midDate >= new DateTime(2015, 3, 30))
      {
         kenteken.Should().MatchRegex(@"^[A-Z]{2}-\d{3}-[A-Z]{1}$", "Serie 9 format");
      }
      else if (midDate >= new DateTime(2013, 3, 5))
      {
         kenteken.Should().MatchRegex(@"^\d{1}-[A-Z]{3}-\d{2}$", "Serie 8 format");
      }
      else if (midDate >= new DateTime(2004, 10, 29))
      {
         kenteken.Should().MatchRegex(@"^\d{2}-[A-Z]{3}-\d{1}$", "Serie 7 format");
      }
      else if (midDate >= new DateTime(1999, 1, 1))
      {
         kenteken.Should().MatchRegex(@"^\d{2}-[A-Z]{2}-[A-Z]{2}$", "Serie 6 format");
      }
      else if (midDate >= new DateTime(1991, 1, 1))
      {
         kenteken.Should().MatchRegex(@"^[A-Z]{2}-[A-Z]{2}-\d{2}$", "Serie 5 format");
      }
      else if (midDate >= new DateTime(1978, 7, 1))
      {
         kenteken.Should().MatchRegex(@"^[A-Z]{2}-\d{2}-[A-Z]{2}$", "Serie 4 format");
      }
      else if (midDate >= new DateTime(1973, 1, 1))
      {
         kenteken.Should().MatchRegex(@"^\d{2}-[A-Z]{2}-\d{2}$", "Serie 3 format");
      }
      else if (midDate >= new DateTime(1965, 1, 1))
      {
         kenteken.Should().MatchRegex(@"^\d{2}-\d{2}-[A-Z]{2}$", "Serie 2 format");
      }
      else
      {
         kenteken.Should().MatchRegex(@"^[A-Z]{2}-\d{2}-\d{2}$", "Serie 1 format");
      }
   }
}

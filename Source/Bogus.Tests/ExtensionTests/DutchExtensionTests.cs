using Bogus.Extensions.Netherlands;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.ExtensionTests;

public class DutchExtensionTests : SeededTest
{
   private readonly Faker faker;
   private readonly ITestOutputHelper console;

   public DutchExtensionTests(ITestOutputHelper console)
   {
      faker = new Faker("nl");
      this.console = console;
   }
   
   [Fact]
   public void should_generate_valid_bsn()
   {
      // Arrange
      var person = faker.Person;
      
      // Act
      var bsn = person.Bsn();
      
      // Assert
      bsn.Should().NotBeNullOrEmpty();
      bsn.Should().HaveLength(9);
      bsn.Should().MatchRegex(@"^\d{9}$");
      
      // Validate the BSN using 11-proof
      IsValidBsn(bsn).Should().BeTrue($"BSN {bsn} should be valid according to 11-proof");
      
      console.WriteLine($"Generated BSN: {bsn}");
   }

   [Fact]
   public void should_generate_different_bsns_for_different_people()
   {
      // Arrange
      var person1 = faker.Person;
      var person2 = faker.Person;
      
      // Act
      var bsn1 = person1.Bsn();
      var bsn2 = person2.Bsn();
      
      // Assert
      bsn1.Should().NotBe(bsn2, "Different people should have different BSNs");
      
      console.WriteLine($"Person 1 BSN: {bsn1}");
      console.WriteLine($"Person 2 BSN: {bsn2}");
   }

   [Fact]
   public void should_generate_multiple_valid_bsns()
   {
      // Arrange & Act
      var bsns = new string[100];
      for (int i = 0; i < 100; i++)
      {
         var person = faker.Person;
         bsns[i] = person.Bsn();
      }
      
      // Assert
      foreach (var bsn in bsns)
      {
         bsn.Should().HaveLength(9);
         bsn.Should().MatchRegex(@"^\d{9}$");
         IsValidBsn(bsn).Should().BeTrue($"BSN {bsn} should be valid");
      }
      
      console.WriteLine($"Generated {bsns.Length} valid BSNs");
   }

   [Theory]
   [InlineData("123456782")] // Known valid BSN
   [InlineData("111222333")] // Valid test BSN
   public void should_validate_known_valid_bsns(string validBsn)
   {
      // Act & Assert
      IsValidBsn(validBsn).Should().BeTrue($"BSN {validBsn} should be valid");
   }

   [Theory]
   [InlineData("123456789")] // Invalid BSN (fails 11-proof)
   [InlineData("111111111")] // Invalid BSN (all ones)
   public void should_invalidate_known_invalid_bsns(string invalidBsn)
   {
      // Act & Assert
      IsValidBsn(invalidBsn).Should().BeFalse($"BSN {invalidBsn} should be invalid");
   }

   [Fact]
   public void should_handle_edge_cases_gracefully()
   {
      // Test with many different people to ensure robustness
      for (int i = 0; i < 1000; i++)
      {
         var person = faker.Person;
         var bsn = person.Bsn();
         
         bsn.Should().NotBeNullOrEmpty();
         bsn.Should().HaveLength(9);
         IsValidBsn(bsn).Should().BeTrue($"Generated BSN {bsn} should always be valid");
      }
   }

   /// <summary>
   /// Validates a BSN using the 11-proof algorithm
   /// </summary>
   /// <param name="bsn">The BSN to validate</param>
   /// <returns>True if the BSN is valid, false otherwise</returns>
   private static bool IsValidBsn(string bsn)
   {
      if (string.IsNullOrEmpty(bsn) || bsn.Length != 9)
         return false;

      if (!long.TryParse(bsn, out _))
         return false;

      // Calculate the 11-proof
      int sum = 0;
      for (int i = 0; i < 8; i++)
      {
         int digit = int.Parse(bsn[i].ToString());
         int weight = 9 - i; // Weights: 9, 8, 7, 6, 5, 4, 3, 2
         sum += digit * weight;
      }

      int checkDigit = int.Parse(bsn[8].ToString());
      int calculatedCheckDigit = sum % 11;

      // BSN is valid if calculated check digit equals the actual check digit
      // and the check digit is not 10 or 11 (which would be invalid)
      return calculatedCheckDigit == checkDigit && calculatedCheckDigit < 10;
   }
}

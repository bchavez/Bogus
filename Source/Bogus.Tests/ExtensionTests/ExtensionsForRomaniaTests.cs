using Bogus.DataSets;
using System;
using System.Linq;
using Bogus.Extensions.Romania;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.ExtensionTests;

public class ExtensionsForRomaniaTests : SeededTest
{
   private readonly ITestOutputHelper testOutputHelper;

   public ExtensionsForRomaniaTests(ITestOutputHelper testOutputHelper)
   {
      this.testOutputHelper = testOutputHelper;
   }

   [Fact]
   public void cnp_generator_for_person()
   {
      //Arrange
      var f = new Faker("ro");
      var person = f.Person;
      person.DateOfBirth = new DateTime(1986, 8, 12);
      person.Gender = Name.Gender.Male;

      //Act
      var cnpNumber = person.Cnp();

      //Assert
      cnpNumber.Should()
         .HaveLength(13)
         .And.StartWith("1")
         .And.Match(p => p.Substring(1, 2) == person.DateOfBirth.ToString("yy"))
         .And.Match(p => p.Substring(3, 2) == person.DateOfBirth.ToString("MM"))
         .And.Match(p => p.Substring(5, 2) == person.DateOfBirth.ToString("dd"));
   }

   [Fact]
   public void romania_random_licence_plate()
   {
      //Arrange
      var faker = new Faker("ro");

      //Act
      var randomValidLicencesPlateDataSet = Make(10, () => faker.Vehicle.RoRegistrationPlate())
         .Distinct().ToList();

      //Assert
      randomValidLicencesPlateDataSet.Should().NotBeNullOrEmpty();
      foreach (var plate in randomValidLicencesPlateDataSet)
      {
         testOutputHelper.WriteLine($"Generated LicencesPlate: {plate}");

         plate.Should().NotBeNullOrEmpty();
         plate.Should().MatchRegex("^(AB|AG|AR|B|BC|BH|BN|BR|BT|BV|BZ|CJ|CL|CS|CT|CV|DB|DJ|GJ|GL|GR|HD|HR|IF|IL|IS|MH|MM|MS|NT|OT|PH|SB|SJ|SM|SV|TL|TM|TR|VL|VN|VS)[0-9]{2,3}[A-Z]{3}$", "Not respect the rules.");
         
      }
   }

}
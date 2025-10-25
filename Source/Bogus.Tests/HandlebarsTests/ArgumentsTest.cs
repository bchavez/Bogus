using FluentAssertions;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.HandlebarsTests;

public class ArgumentsTest : SeededTest
{
   private readonly ITestOutputHelper console;

   public ArgumentsTest(ITestOutputHelper console)
   {
      this.console = console;
   }


   [Fact]
   public void can_parse_random_number_request_without_arguments()
   {
      var faker = new Faker();
      var result = faker.Parse("{{randomizer.number}}");
      int.Parse(result);
   }

   [Fact]
   public void can_parse_random_bool_request_without_arguments()
   {
      var faker = new Faker();
      var result = faker.Parse("{{randomizer.bool}}");
      bool.Parse(result);
   }


   [Fact]
   public void can_parse_random_number_parameterized_request()
   {
      var faker = new Faker();
      var result = faker.Parse("{{randomizer.number (100)}}");
      int.Parse(result).Should().Be(60);
   }

   [Fact]
   public void can_parse_random_number_parameterized_request_overload()
   {
      var faker = new Faker();
      var result = faker.Parse("{{randomizer.number (50, 100)}}");
      int.Parse(result).Should().Be(80);
   }

   [Fact]
   public void cant_parse_random_number_parameterized_request_incorrect_overload()
   {
      var faker = new Faker();
      Assert.Throws<ArgumentException>(() => faker.Parse("{{randomizer.number (50, 100, 1000)}}"));
   }

   [Fact]
   public void cant_parse_random_number_parameterized_request_incorrect_type()
   {
      var faker = new Faker();
      Assert.Throws<ArgumentException>(() => faker.Parse("{{randomizer.number (a)}}"));
   }

   [Fact]
   public void cant_parse_random_string_request_with_not_enough_arguments()
   {
      var faker = new Faker();
      Assert.Throws<ArgumentException>(() => faker.Parse("{{randomizer.ReplaceNumbers}}"));
   }

   [Fact]
   public void can_parse_random_string_request_with_enough_arguments()
   {
      var faker = new Faker();
      faker.Parse("{{randomizer.string (5, 10)}}")
           .Length.Should()
           .BeGreaterOrEqualTo(5)
           .And
           .BeLessOrEqualTo(10);
   }

   [Fact]
   public void can_parse_random_string_request_with_full_arguments()
   {
      var faker = new Faker();
      faker.Parse("{{randomizer.string (5, 10, a, z)}}").Should().Be("cvqaqbrm");
   }

   [Fact]
   public void can_parse_enum_argument()
   {
      var faker = new Faker();
      faker.Parse("{{name.firstname(Female)}} {{name.firstname(Male)}}").Should().Be("Lindsay Jonathan");
   }

   [Fact]
   public void can_parse_string_argument()
   {
      var faker = new Faker();
      faker.Parse(@"{{randomizer.string2(9, wxyz)}}").Should().Be("ywzywywyx");
   }

   [Fact]
   public void can_parse_bool_argument()
   {
      var faker = new Faker();
      faker.Parse(@"{{randomizer.hash(5, true)}}").Should().Be("91DA0");
   }

   [Fact]
   public void can_parse_datetime_with_arguments()
   {
      var faker = new Faker();
      var dtString = faker.Parse("{{date.between(2015-10-04, 2017-11-03) }}");
      var dt = DateTimeOffset.Parse(dtString);
      dt.Should()
         .BeAfter(new DateTime(2015, 10, 04))
         .And
         .BeBefore(new DateTime(2017, 11, 03));
   }

   [Fact]
   public void unmached_brace_should_throw()
   {
      var faker = new Faker();
      //easy to miss the closing ) with }} handle bars.
      Action a = () => faker.Parse("{{randomizer.number(100 }}");
      a.Should().Throw<ArgumentException>();
   }

   [Fact]
   public void can_parse_timespan_with_arguments()
   {
      var faker = new Faker();

      faker.Parse("{{date.timespan(00:00:25)}}")
         .Should().Be("00:00:15.0880571");
   }


   [Fact]
   public void can_parse_vehicle()
   {
      var faker = new Faker();
      var result = faker.Parse("{{vehicle.manufacturer}}");
      Assert.NotNull(result);
   }

   [Fact]
   public void parse_vehicle_returns_expected_value()
   {
      var faker = new Faker();
      var result = faker.Parse("{{vehicle.manufacturer}}");
      result.Should().Be("Maserati");
   }

   [Fact]
   public void can_parse_music()
   {
      var faker = new Faker();
      var result = faker.Parse("{{music.genre}}");
      Assert.NotNull(result);
   }

   [Fact]
   public void parse_music_returns_expected_value()
   {
      var faker = new Faker();
      var result = faker.Parse("{{music.genre}}");
      result.Should().Be("Hip Hop");
   }

   [Fact]
   public void can_parse_person()
   {
      var faker = new Faker();
      var firstname = faker.Parse("{{person.firstname}}");
      var lastname = faker.Parse("{{person.lastname}}");
      var fullname = faker.Parse("{{person.fullname}}");

      Assert.Multiple(() =>
      {
         Assert.NotNull(firstname);
         Assert.NotNull(lastname);
         Assert.NotNull(fullname);
         Assert.True($"{firstname} {lastname}" == fullname);
      });
   }
   private class TestClass
   {
      public string Name { get; set; }
   }

   [Fact]
   public void can_parse_person_in_rulefor()
   {
      var faker = new Faker<TestClass>();
      faker.RuleFor(o => o.Name, (f, o) =>
      {
         return f.Parse("{{person.firstname}} {{person.lastname}}");
      });

      var fakes = faker.Generate(10);

      fakes.Should().OnlyHaveUniqueItems(o => o.Name);
   }

   [Fact]
   public void can_parse_all_person_values()
   {
      var faker = new Faker();

      var firstname = faker.Parse("{{person.FirstName}}");
      var lastname = faker.Parse("{{person.LastName}}");
      var fullname = faker.Parse("{{person.FullName}}");
      var gender = faker.Parse("{{person.Gender}}");
      var username = faker.Parse("{{person.UserName}}");
      var avatar = faker.Parse("{{person.Avatar}}");
      var email = faker.Parse("{{person.Email}}");
      var dateofbirth = faker.Parse("{{person.DateOfBirth}}");
      var geolat = faker.Parse("{{person.AddressGeoLat}}");
      var geolng = faker.Parse("{{person.AddressGeoLng}}");
      var street = faker.Parse("{{person.AddressStreet}}");
      var suit = faker.Parse("{{person.AddressSuite}}");
      var city = faker.Parse("{{person.AddressCity}}");
      var state = faker.Parse("{{person.AddressState}}");
      var zipcode = faker.Parse("{{person.AddressZipCode}}");
      var phone = faker.Parse("{{person.Phone}}");
      var website = faker.Parse("{{person.Website}}");
      var companyname = faker.Parse("{{person.CompanyName}}");
      var companycatchphrase = faker.Parse("{{person.CompanyCatchPhrase}}");
      var companybs = faker.Parse("{{person.CompanyBs}}");


      firstname.Should().Be("Doris");
      lastname.Should().Be("Schultz");
      fullname.Should().Be("Doris Schultz");
      gender.Should().Be("Female");
      username.Should().Be("Doris.Schultz");
      avatar.Should().Be("https://ipfs.io/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/avatar/1180.jpg");
      email.Should().Be("Doris69@yahoo.com");
      //dateofbirth.Should().Be(""); Not sure the best way to test DOB.
      geolat.Should().Be("-38.4622");
      geolng.Should().Be("-0.6396");
      street.Should().Be("31681 Little Flats");
      suit.Should().Be("Suite 926");
      city.Should().Be("Port Simone");
      state.Should().Be("Rhode Island");
      zipcode.Should().Be("42504-1131");
      phone.Should().Be("1-607-290-8836 x5069");
      website.Should().Be("javier.biz");
      companyname.Should().Be("MacGyver and Sons");
      companycatchphrase.Should().Be("Programmable multi-tasking implementation");
      companybs.Should().Be("e-enable enterprise mindshare");
   }
}

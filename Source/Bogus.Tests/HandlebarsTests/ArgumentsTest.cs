using Bogus.DataSets;
using FluentAssertions;
using System;
using Xunit;

namespace Bogus.Tests.HandlebarsTests
{
   public class ArgumentsTest : SeededTest
   {
      public ArgumentsTest()
      {
        
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
      public void can_parse_random_number_parametized_request()
      {
         var faker = new Faker();
         var result = faker.Parse("{{randomizer.number (100)}}");
         int.Parse(result).Should().Be(60);
      }

      [Fact]
      public void can_parse_random_number_parametized_request_overload()
      {
         var faker = new Faker();
         var result = faker.Parse("{{randomizer.number (50, 100)}}");
         int.Parse(result).Should().Be(80);
      }

      [Fact]
      public void cant_parse_random_number_parametized_request_incorrect_overload()
      {
         var faker = new Faker();
         Assert.Throws<ArgumentException>(() => faker.Parse("{{randomizer.number (50, 100, 1000)}}"));
      }

      [Fact]
      public void cant_parse_random_number_parametized_request_incorrect_type()
      {
         var faker = new Faker();
         Assert.Throws<ArgumentException>(() => faker.Parse("{{randomizer.number (a)}}"));
      }

      [Fact]
      public void cant_parse_random_string_request_with_not_enough_arguments()
      {
         var faker = new Faker();
         Assert.Throws<ArgumentException>(() => faker.Parse("{{randomizer.string (5)}}"));
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
      public void can_parse_datetime_request_with_arguments()
      {
         var faker = new Faker();
         faker.Parse("{{date.between(2015-10-04, 2017-11-03)}}").Should().Be("05-Jan-17 6:43:51 AM");
      }
   }
}
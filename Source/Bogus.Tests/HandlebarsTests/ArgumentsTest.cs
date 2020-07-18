using FluentAssertions;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.HandlebarsTests
{
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
   }
}

using System;
using System.Globalization;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class DateTest : SeededTest
   {
      public DateTest()
      {
         date = new Date();
      }

      private readonly Date date;

      [Fact]
      public void can_get_a_random_month()
      {
         date.Month().Should().NotBeNullOrEmpty();
         date.Month(true).Should().NotBeNullOrEmpty();
      }

      [Fact]
      public void can_get_a_random_weekday()
      {
         date.Weekday().Should().NotBeNullOrEmpty();
         date.Weekday(true).Should().NotBeNullOrEmpty();
      }

      [Fact]
      public void can_get_a_timespan()
      {
         date.Timespan().Should().BePositive()
            .And
            .BeGreaterThan(TimeSpan.Zero)
            .And
            .BeLessThan(TimeSpan.FromDays(7));
      }


      [Fact]
      public void can_get_date_in_future()
      {
         var starting = DateTime.Parse("6/7/2015 4:17:41 PM");
         date.Future(refDate: starting).Should()
            .BeOnOrBefore(starting.AddYears(1))
            .And
            .BeOnOrAfter(starting);
      }

      [Fact]
      public void can_get_date_in_future_with_options()
      {
         var starting = DateTime.Parse("6/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);
         date.Future(refDate: starting, yearsToGoForward: 5).Should()
            .BeOnOrBefore(starting.AddYears(5))
            .And
            .BeOnOrAfter(starting);
      }

      [Fact]
      public void can_get_date_in_past()
      {
         var starting = DateTime.Parse("6/7/2015 4:17:41 PM");
         date.Past(refDate: starting).Should()
            .BeOnOrBefore(starting)
            .And
            .BeOnOrAfter(starting.AddYears(-1));
      }

      [Fact]
      public void can_get_date_in_past_0_days_results_in_random_time()
      {
         date.Recent(0).Should()
            .BeOnOrBefore(DateTime.Now)
            .And
            .BeOnOrAfter(DateTime.Now.Date);
      }

      [Fact]
      public void can_get_date_in_past_with_custom_options()
      {
         var starting = DateTime.Parse("6/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);
         date.Past(refDate: starting, yearsToGoBack: 5).Should()
            .BeOnOrBefore(starting)
            .And
            .BeOnOrAfter(starting.AddYears(-5));
      }

      [Fact]
      public void can_get_date_recently_within_the_year()
      {
         var start = DateTime.Now;
         date.Recent()
            .Should()
            .BeOnOrBefore(start)
            .And
            .BeOnOrAfter(start.AddDays(-1));
      }


      [Fact]
      public void can_get_random_time_between_two_dates()
      {
         var start = DateTime.Parse("6/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);
         var end = DateTime.Parse("8/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);

         date.Between(start, end)
            .Should()
            .BeOnOrAfter(start)
            .And
            .BeOnOrBefore(end);

         //and reverse...
         date.Between(end, start)
            .Should()
            .BeOnOrAfter(start)
            .And
            .BeOnOrBefore(end);
      }

      [Fact]
      public void get_a_date_time_that_will_happen_soon()
      {
         var now = DateTime.Now;
         date.Soon(3).Should().BeAfter(now).And.BeBefore(now.AddDays(3));
      }
   }
}
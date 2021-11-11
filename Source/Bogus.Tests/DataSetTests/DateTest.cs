using System;
using System.Globalization;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public partial class DateTest : SeededTest
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
      public void can_get_dateOffset_in_future()
      {
         var starting = DateTimeOffset.Parse("6/7/2015 4:17:41 PM");
         date.FutureOffset(refDate: starting).Should()
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
      public void can_get_dateOffset_in_future_with_options()
      {
         var starting = DateTimeOffset.Parse("6/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);
         date.FutureOffset(refDate: starting, yearsToGoForward: 5).Should()
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
      public void can_get_dateOffset_in_past()
      {
         var starting = DateTimeOffset.Parse("6/7/2015 4:17:41 PM");
         date.PastOffset(refDate: starting).Should()
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
      public void can_get_dateOffset_in_past_0_days_results_in_random_time()
      {
         date.RecentOffset(0).Should()
            .BeOnOrBefore(DateTimeOffset.Now)
            .And
            .BeOnOrAfter(DateTimeOffset.Now.Date);
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
      public void can_get_dateOffset_in_past_with_custom_options()
      {
         var starting = DateTimeOffset.Parse("6/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);
         date.PastOffset(refDate: starting, yearsToGoBack: 5).Should()
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
      public void can_get_dateOffset_recently_within_the_year()
      {
         var start = DateTimeOffset.Now;
         date.RecentOffset()
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
      public void can_get_random_time_between_two_dateOffsets()
      {
         var start = DateTimeOffset.Parse("6/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);
         var end = DateTimeOffset.Parse("8/15/2000 4:17:41 PM", CultureInfo.InvariantCulture);

         date.BetweenOffset(start, end)
            .Should()
            .BeOnOrAfter(start)
            .And
            .BeOnOrBefore(end);

         //and reverse...
         date.BetweenOffset(end, start)
            .Should()
            .BeOnOrAfter(start)
            .And
            .BeOnOrBefore(end);
      }

      [Fact]
      public void get_a_date_that_will_happen_soon()
      {
         var now = DateTime.Now;
         date.Soon(3).Should().BeAfter(now).And.BeBefore(now.AddDays(3));
      }

      [Fact]
      public void get_a_dateOffsets_that_will_happen_soon()
      {
         var now = DateTimeOffset.Now;
         date.SoonOffset(3).Should().BeAfter(now).And.BeBefore(now.AddDays(3));
      }

      [Fact]
      public void soon_explicit_refdate_in_utc_should_return_utc_kind()
      {
         const int days = 3;
         var dt = DateTime.Parse("7/5/2018 9:00 AM");
         var utc = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
         var utcEnd = utc.AddDays(days);
         var result = date.Soon(days, utc);

         result.Kind.Should().Be(DateTimeKind.Utc);

         result.Should()
            .BeOnOrAfter(utc)
            .And
            .BeOnOrBefore(utcEnd);
      }

      [Fact]
      public void soon_explicit_refdate_offset_should_return_explicit_offset()
      {
         const int days = 3;
         var offset = TimeSpan.FromHours(-3);
         var utc = DateTimeOffset.Parse("7/5/2018 9:00 AM").ToOffset(offset);
         var utcEnd = utc.AddDays(days);
         var result = date.SoonOffset(days, utc);

         result.Offset.Should().Be(offset);

         result.Should()
            .BeOnOrAfter(utc)
            .And
            .BeOnOrBefore(utcEnd);
      }

      [Fact]
      public void recent_explicit_refdate_in_utc_should_return_utc_kind()
      {
         const int days = 3;
         var dt = DateTime.Parse("7/5/2018 9:00 AM");
         var utcEnd = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
         var utcPast = utcEnd.AddDays(-days);
         var result = date.Recent(days, utcEnd);

         result.Kind.Should().Be(DateTimeKind.Utc);

         result.Should()
            .BeOnOrAfter(utcPast)
            .And
            .BeOnOrBefore(utcEnd);
      }

      [Fact]
      public void recent_explicit_refdate_offset_should_return_explicit_offset()
      {
         const int days = 3;
         var offset = TimeSpan.FromHours(-3);
         var utcEnd = DateTimeOffset.Parse("7/5/2018 9:00 AM").ToOffset(offset);
         var utcPast = utcEnd.AddDays(-days);
         var result = date.RecentOffset(days, utcEnd);

         result.Offset.Should().Be(offset);

         result.Should()
            .BeOnOrAfter(utcPast)
            .And
            .BeOnOrBefore(utcEnd);
      }

      [Fact]
      public void between_explicit_refdate_in_utc_should_return_utc_kind()
      {
         const int days = 3;
         var dt = DateTime.Parse("8/8/2017 10:00 AM");
         var utc = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
         var utcEnd = utc.AddDays(days);
         var result = date.Between(utc, utcEnd);

         result.Kind.Should().Be(DateTimeKind.Utc);

         result.Should()
            .BeOnOrAfter(utc)
            .And
            .BeOnOrBefore(utcEnd);
      }

      [Fact]
      public void between_explicit_start_offset_should_return_explicit_offset()
      {
         const int days = 3;
         var offset = TimeSpan.FromHours(-3);
         var utc = DateTimeOffset.Parse("8/8/2017 10:00 AM").ToOffset(offset);
         var utcEnd = utc.AddDays(days);
         var result = date.BetweenOffset(utc, utcEnd);

         result.Offset.Should().Be(offset);

         result.Should()
            .BeOnOrAfter(utc)
            .And
            .BeOnOrBefore(utcEnd);
      }


      [Fact]
      public void can_set_global_static_time_source()
      {
         Date.SystemClock = () => DateTime.UtcNow;

         var d = new Date();

         d.Soon().Kind.Should().Be(DateTimeKind.Utc);
         d.Future().Kind.Should().Be(DateTimeKind.Utc);
         d.Past().Kind.Should().Be(DateTimeKind.Utc);
         d.Recent().Kind.Should().Be(DateTimeKind.Utc);

         d.SoonOffset().Offset.Should().Be(DateTimeOffset.UtcNow.Offset);
         d.FutureOffset().Offset.Should().Be(DateTimeOffset.UtcNow.Offset);
         d.PastOffset().Offset.Should().Be(DateTimeOffset.UtcNow.Offset);
         d.RecentOffset().Offset.Should().Be(DateTimeOffset.UtcNow.Offset);

         Date.SystemClock = () => DateTime.Now;

         d.Soon().Kind.Should().Be(DateTimeKind.Local);
         d.Future().Kind.Should().Be(DateTimeKind.Local);
         d.Past().Kind.Should().Be(DateTimeKind.Local);
         d.Recent().Kind.Should().Be(DateTimeKind.Local);

         d.SoonOffset().Offset.Should().Be(DateTimeOffset.Now.Offset);
         d.FutureOffset().Offset.Should().Be(DateTimeOffset.Now.Offset);
         d.PastOffset().Offset.Should().Be(DateTimeOffset.Now.Offset);
         d.RecentOffset().Offset.Should().Be(DateTimeOffset.Now.Offset);
      }

      [Fact]
      public void can_get_timezone_string()
      {
         date.TimeZoneString().Should().Be("Asia/Yerevan");
      }
   }
}
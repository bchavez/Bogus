using System;
using System.Globalization;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public partial class DateTest
   {
#if NET6_0
      [Fact]
      public void can_get_dateonly_in_past()
      {
         var now = DateOnly.Parse("7/7/1972", CultureInfo.InvariantCulture);
         var maxBehind = now.AddYears(-1);

         var somePastDate = date.PastDateOnly(refDate: now);
         somePastDate.Should().BeInRange(maxBehind, now);
      }

      [Fact]
      public void can_get_dateonly_in_past_with_custom_options()
      {
         var now = DateOnly.Parse("7/7/1972", CultureInfo.InvariantCulture);
         var maxBehind = now.AddYears(-5);

         var somePastDate = date.PastDateOnly(5, now);

         somePastDate.Should().BeInRange(maxBehind, now);
      }

      [Fact]
      public void get_a_dateonly_that_will_happen_soon()
      {
         var now = DateOnly.Parse("7/7/1972", CultureInfo.InvariantCulture);
         var maxDate = now.AddDays(3);

         var someDateSoon = date.SoonDateOnly(3, now);

         someDateSoon.Should().BeInRange(now, maxDate);
      }

      [Fact]
      public void can_get_dateonly_in_future()
      {
         var now = DateOnly.Parse("1/1/1990", CultureInfo.InvariantCulture);
         var maxDate = now.AddYears(1);

         var someFutureDate = date.FutureDateOnly(refDate: now);
         someFutureDate.Should().BeInRange(now, maxDate);
      }

      [Fact]
      public void can_get_dateonly_in_future_with_options()
      {
         var now = DateOnly.Parse("7/7/1972", CultureInfo.InvariantCulture);
         var maxDate = now.AddYears(5);

         var someFutureDate = date.FutureDateOnly(5, now);
         someFutureDate.Should().BeInRange(now, maxDate);
      }

      [Fact]
      public void can_get_random_dateonly_between_two_dates()
      {
         var start = DateOnly.Parse("8/8/2020", CultureInfo.InvariantCulture);
         var end = DateOnly.Parse("12/12/2021", CultureInfo.InvariantCulture);

         var someDate = date.BetweenDateOnly(start, end);

         someDate.Should().BeInRange(start, end);

         //and reverse...
         var otherDate = date.BetweenDateOnly(end, start);
         otherDate.Should().BeInRange(start, end);
      }

      [Fact]
      public void can_get_dateonly_recently_within_the_year()
      {
         var now = DateOnly.Parse("7/7/1972", CultureInfo.InvariantCulture);
         var maxBehind = now.AddDays(-1);

         var someRecentDate = date.RecentDateOnly(refDate: now);

         someRecentDate.Should().BeInRange(maxBehind, now);
      }

      [Fact]
      public void can_get_random_timeonly_between_two_times_basic()
      {
         var start = TimeOnly.Parse("1:00 PM", CultureInfo.InvariantCulture);
         var end = TimeOnly.Parse("2:00 PM", CultureInfo.InvariantCulture);

         var someTimeBetween = date.BetweenTimeOnly(start, end);
         someTimeBetween.IsBetween(start, end).Should().BeTrue();

         var outside = TimeOnly.Parse("2:30 PM", CultureInfo.InvariantCulture);
         outside.IsBetween(start, end).Should().BeFalse();
      }

      [Fact]
      public void can_get_random_timeonly_between_two_times_wrap_around()
      {
         //wrap around from 2:00 PM to 1:00 PM; times from 1:00 PM -> 2:00 PM is excluded.
         var start = TimeOnly.Parse("2:00 PM", CultureInfo.InvariantCulture);
         var end = TimeOnly.Parse("1:00 PM", CultureInfo.InvariantCulture);

         var someTimeBetween = date.BetweenTimeOnly(end, start);
         someTimeBetween.IsBetween(end, start).Should().BeTrue();

         var outside = TimeOnly.Parse("1:30 PM", CultureInfo.InvariantCulture);
         outside.IsBetween(start, end).Should().BeFalse();
      }

      [Fact]
      public void can_get_a_timeonly_that_will_happen_soon()
      {
         var now = TimeOnly.Parse("1:00 PM", CultureInfo.InvariantCulture);
         var maxTime = now.AddMinutes(5);

         var timeSoon = date.SoonTimeOnly(5, now);

         timeSoon.IsBetween(now, maxTime).Should().BeTrue();
      }

      [Fact]
      public void can_get_a_timeonly_that_happened_recently()
      {
         var now = TimeOnly.Parse("2:00 PM", CultureInfo.InvariantCulture);
         var maxBehind = now.AddMinutes(-5);

         var timeRecent = date.RecentTimeOnly(5, now);
         timeRecent.IsBetween(maxBehind, now).Should().BeTrue();
      }
#endif
   }
}

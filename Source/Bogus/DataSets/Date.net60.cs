using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bogus.DataSets
{
   public partial class Date
   {
#if NET6_0
      /// <summary>
      /// Get a random <see cref="DateOnly"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start date</param>
      /// <param name="end">End date</param>
      public DateOnly BetweenDateOnly(DateOnly start, DateOnly end)
      {
         var maxDay = Math.Max(start.DayNumber, end.DayNumber);
         var minDay = Math.Min(start.DayNumber, end.DayNumber);

         var someDayNumber = this.Random.Number(minDay, maxDay);

         var dateBetween = DateOnly.FromDayNumber(someDayNumber);
         return dateBetween;
      }

      /// <summary>
      /// Get a <see cref="DateOnly"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
      public DateOnly PastDateOnly(int yearsToGoBack = 1, DateOnly? refDate = null)
      {
         var start = refDate ?? DateOnly.FromDateTime(SystemClock());
         var maxBehind = start.AddYears(-yearsToGoBack);

         return BetweenDateOnly(maxBehind, start);
      }

      /// <summary>
      /// Get a <see cref="DateOnly"/> that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
      /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
      public DateOnly SoonDateOnly(int days = 1, DateOnly? refDate = null)
      {
         var start = refDate ?? DateOnly.FromDateTime(SystemClock());
         var maxForward = start.AddDays(days);

         return BetweenDateOnly(start, maxForward);
      }

      /// <summary>
      /// Get a <see cref="DateOnly"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
      public DateOnly FutureDateOnly(int yearsToGoForward = 1, DateOnly? refDate = null)
      {
         var start = refDate ?? DateOnly.FromDateTime(SystemClock());
         var maxForward = start.AddYears(yearsToGoForward);

         return BetweenDateOnly(start, maxForward);
      }

      /// <summary>
      /// Get a random <see cref="DateOnly"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDate">The date to start calculations. Default is from <see cref="DateTime.Now"/>.</param>
      public DateOnly RecentDateOnly(int days = 1, DateOnly? refDate = null)
      {
         var start = refDate ?? DateOnly.FromDateTime(SystemClock());
         var maxBehind = start.AddDays(-days);

         return BetweenDateOnly(maxBehind, start);
      }

      /// <summary>
      /// Get a random <see cref="TimeOnly"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time</param>
      /// <param name="end">End time</param>
      public TimeOnly BetweenTimeOnly(TimeOnly start, TimeOnly end)
      {
         var diff = end - start;
         var diffTicks = diff.Ticks;

         var part = RandomTimeSpanFromTicks(diffTicks);

         return start.Add(part);
      }

      /// <summary>
      /// Get a <see cref="TimeOnly"/> that will happen soon.
      /// </summary>
      /// <param name="mins">Minutes no more than <paramref name="mins"/> ahead.</param>
      /// <param name="refTime">The time to start calculations. Default is time from <see cref="DateTime.Now"/>.</param>
      public TimeOnly SoonTimeOnly(int mins = 60, TimeOnly? refTime = null)
      {
         var start = refTime ?? TimeOnly.FromDateTime(SystemClock());
         var maxForward = start.AddMinutes(mins);

         return BetweenTimeOnly(start, maxForward);
      }

      /// <summary>
      /// Get a random <see cref="TimeOnly"/> within the last few Minutes.
      /// </summary>
      /// <param name="mins">Minutes <paramref name="mins"/> of the day to go back.</param>
      /// <param name="refTime">The Time to start calculations. Default is time from <see cref="DateTime.Now"/>.</param>
      public TimeOnly RecentTimeOnly(int mins = 60, TimeOnly? refTime = null)
      {
         var start = refTime ?? TimeOnly.FromDateTime(SystemClock());
         var maxBehind = start.AddMinutes(-mins);

         return BetweenTimeOnly(maxBehind, start);
      }
#endif
   }
}

using System;
using System.Globalization;

namespace Bogus.DataSets
{
   /// <summary>
   /// Methods for generating dates
   /// </summary>
   public partial class Date : DataSet
   {
      private bool hasMonthWideContext;
      private bool hasMonthAbbrContext;
      private bool hasWeekdayWideContext;
      private bool hasWeekdayAbbrContext;

      /// <summary>
      /// Sets the system clock time Bogus uses for date calculations.
      /// This value is normally <seealso cref="DateTime.Now"/>. If deterministic times are desired,
      /// set the <seealso cref="SystemClock"/> to a single instance in time.
      /// IE: () => new DateTime(2018, 4, 23);
      /// Setting this static value will effect and apply to all Faker[T], Faker,
      /// and new Date() datasets instances.
      /// </summary>
      public static Func<DateTime> SystemClock = () => DateTime.Now;

      /// <summary>
      /// Create a Date dataset
      /// </summary>
      public Date(string locale = "en") : base(locale)
      {
         this.hasMonthWideContext = HasKey("month.wide_context", false);
         this.hasMonthAbbrContext = HasKey("month.abbr_context", false);
         this.hasWeekdayWideContext = HasKey("weekday.wide_context", false);
         this.hasWeekdayAbbrContext = HasKey("weekday.abbr_context", false);
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
      {
         var maxDate = refDate ?? SystemClock();

         var minDate = maxDate.AddYears(-yearsToGoBack);

         return Between(minDate, maxDate);
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset PastOffset(int yearsToGoBack = 1, DateTimeOffset? refDate = null)
      {
         var maxDate = refDate ?? SystemClock();

         var minDate = maxDate.AddYears(-yearsToGoBack);

         return BetweenOffset(minDate, maxDate);
      }

      /// <summary>
      /// Gets an random timespan from ticks.
      /// </summary>
      protected internal TimeSpan RandomTimeSpanFromTicks(long totalTimeSpanTicks)
      {
         //find % of the timespan
         var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;
         return TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTime Soon(int days = 1, DateTime? refDate = null)
      {
         var dt = refDate ?? SystemClock();
         return Between(dt, dt.AddDays(days));
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset SoonOffset(int days = 1, DateTimeOffset? refDate = null)
      {
         var dt = refDate ?? SystemClock();
         return BetweenOffset(dt, dt.AddDays(days));
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
      {
         var minDate = refDate ?? SystemClock();

         var maxDate = minDate.AddYears(yearsToGoForward);

         return Between(minDate, maxDate);
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset FutureOffset(int yearsToGoForward = 1, DateTimeOffset? refDate = null)
      {
         var minDate = refDate ?? SystemClock();

         var maxDate = minDate.AddYears(yearsToGoForward);

         return BetweenOffset(minDate, maxDate);
      }

      /// <summary>
      /// Get a random <see cref="DateTime"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time - The returned <seealso cref="DateTimeKind"/> is used from this parameter.</param>
      /// <param name="end">End time</param>
      public DateTime Between(DateTime start, DateTime end)
      {
         ComputeRealRange(ref start, ref end, start.Kind, out var preferRangeBoundary);

         var startTicks = start.ToUniversalTime().Ticks;
         var endTicks = end.ToUniversalTime().Ticks;

         var minTicks = Math.Min(startTicks, endTicks);
         var maxTicks = Math.Max(startTicks, endTicks);

         var totalTimeSpanTicks = maxTicks - minTicks;

         // Right around daylight savings time transition, there can be two different local DateTime values
         // that are actually exactly the same DateTime. The ToLocalTime conversion might pick the wrong
         // one in edge cases; it will pick the later one, and if the caller's window includes the earlier
         // one, we should return that instead to follow the principle of least surprise.
         if (totalTimeSpanTicks == 0)
            return preferRangeBoundary;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         var value = new DateTime(minTicks, DateTimeKind.Utc) + partTimeSpan;

         if (start.Kind != DateTimeKind.Utc)
         {
            value = value.ToLocalTime();

            if (value > end)
               value = end;
         }

         return value;
      }

      /// <summary>
      /// Takes a date/time range, as indicated by <paramref name="start"/> and <paramref name="end"/>,
      /// and ensures that the range indicators are in the correct order and both reference actual
      /// <see cref="DateTime"/> values. This takes into account the fact that when Daylight Savings Time
      /// comes into effect, there is a 1-hour interval in the local calendar which does not exist, and
      /// <see cref="DateTime"/> values in this change are not meaningful.
      /// 
      /// This function only worries about the start and end times. Impossible <see cref="DateTime"/>
      /// values within the range are excluded automatically by means of the <see cref="DateTime.ToLocalTime"/>
      /// function.
      /// 
      /// The version of this function built targeting .NET Standard 1.3 does not check Daylight Savings Time
      /// transitions, as this API does not expose Daylight Savings Time information.
      /// </summary>
      /// <param name="start">A ref <see cref="DateTime"/> to be adjusted forward out of an impossible date/time range if necessary.</param>
      /// <param name="end">A ref <see cref="DateTime"/> to be adjusted backward out of an impossible date/time range if necessary.</param>
      /// <param name="kind">A <see cref="DateTimeKind"/> indicating how the supplied <see cref="DateTime"/> values should be interpreted with respect to DST change windows.</param>
      /// <param name="preferRangeBoundary">An out <see cref="DateTime"/> that indicates which value should be used if the supplied range is empty due to being entirely contained within a DST transition range.</param>
      private void ComputeRealRange(ref DateTime start, ref DateTime end, DateTimeKind kind, out DateTime preferRangeBoundary)
      {
         preferRangeBoundary = end;

         if (start > end)
         {
            var tmp = start;

            start = end;
            end = tmp;
         }

#if !NETSTANDARD1_3
         if (kind == DateTimeKind.Local)
         {
            var window = GetForwardDSTTransitionWindow(start);

            var startLocal = start.ToLocalTime();
            var endLocal = end.ToLocalTime();

            if ((startLocal >= window.Start) && (startLocal <= window.End))
            {
               start = new DateTime(window.Start.Ticks, start.Kind);

               if (start == end)
                  return;
            }

            window = GetForwardDSTTransitionWindow(end);

            if ((end >= window.Start) && (end < window.End))
            {
               end = new DateTime(window.End.Ticks, end.Kind);

               // We had to bump the end, meaning that the end was not already a valid
               // DateTime value (within the DST transition range), so prefer the start
               // instead.
               preferRangeBoundary = start;
            }

            if (start > end)
               throw new Exception("DateTime range does not contain any real DateTime values due to daylight savings transitions");
         }
#endif
      }

#if !NETSTANDARD1_3
      struct DateTimeRange
      {
         public DateTime Start;
         public DateTime End;
      }

      /// <summary>
      /// Finds the window of time that doesn't exist in the local timezone due to Daylight Savings Time coming into
      /// effect. In timezones that do not have Daylight Savings Time transitions, this function returns <see cref="null"/>.
      /// </summary>
      /// <param name="dateTime">
      /// A reference <see cref="DateTime"/> value for determining the DST transition window accurately. Daylight Savings Time
      /// rules can change over time, and the <see cref="TimeZoneInfo"/> API exposes information about which Daylight Savings
      /// Time rules are in effect for which date ranges.
      /// </param>
      /// <returns>
      /// A <see cref="DateTimeRange"/> that indicates the start &amp; end of the interval of date/time values that do not
      /// exist in the local calendar in the interval indicated by the supplied <paramref name="dateTime"/>, or <see cref="null"/>
      /// if no such range exists.
      /// </returns>
      private DateTimeRange GetForwardDSTTransitionWindow(DateTime dateTime)
      {
         // Based on code found at: https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.transitiontime.isfixeddaterule
         var rule = FindEffectiveTimeZoneAdjustmentRule(dateTime);

         if (rule == null)
            return default(DateTimeRange);

         var transition = rule.DaylightTransitionStart;

         DateTime startTime;

         if (transition.IsFixedDateRule)
         {
            startTime = new DateTime(
               dateTime.Year,
               transition.Month,
               transition.Day,
               transition.TimeOfDay.Hour,
               transition.TimeOfDay.Minute,
               transition.TimeOfDay.Second,
               transition.TimeOfDay.Millisecond);
         }
         else
         {
            var calendar = CultureInfo.CurrentCulture.Calendar;

            var startOfWeek = transition.Week * 7 - 6;

            var firstDayOfWeek = (int)calendar.GetDayOfWeek(new DateTime(dateTime.Year, transition.Month, 1));
            var changeDayOfWeek = (int)transition.DayOfWeek;

            int transitionDay =
               firstDayOfWeek <= changeDayOfWeek
               ? startOfWeek + changeDayOfWeek - firstDayOfWeek
               : startOfWeek + changeDayOfWeek - firstDayOfWeek + 7;

            if (transitionDay > calendar.GetDaysInMonth(dateTime.Year, transition.Month))
               transitionDay -= 7;

            startTime = new DateTime(
               dateTime.Year,
               transition.Month,
               transitionDay,
               transition.TimeOfDay.Hour,
               transition.TimeOfDay.Minute,
               transition.TimeOfDay.Second,
               transition.TimeOfDay.Millisecond);
         }

         return
            new DateTimeRange()
            {
               Start = startTime,
               End = startTime + rule.DaylightDelta,
            };
      }

      /// <summary>
      /// Identifies the timezone adjustment rule in effect in the local timezone at the specified
      /// <paramref name="dateTime"/>. If no adjustment rule is in effect, returns <see cref="null"/>.
      /// </summary>
      /// <param name="dateTime">The <see cref="DateTime"/> value for which to find an adjustment rule.</param>
      private TimeZoneInfo.AdjustmentRule FindEffectiveTimeZoneAdjustmentRule(DateTime dateTime)
      {
         foreach (var rule in TimeZoneInfo.Local.GetAdjustmentRules())
            if ((dateTime >= rule.DateStart) && (dateTime <= rule.DateEnd))
               return rule;

         return default;
      }
#endif

      /// <summary>
      /// Get a random <see cref="DateTimeOffset"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time - The returned <seealso cref="DateTimeOffset"/> offset value is used from this parameter</param>
      /// <param name="end">End time</param>
      public DateTimeOffset BetweenOffset(DateTimeOffset start, DateTimeOffset end)
      {
         var startTime = start.ToUniversalTime().DateTime;
         var endTime = end.ToUniversalTime().DateTime;

         if (startTime > endTime)
            return end;
         else
         {
            ComputeRealRange(ref startTime, ref endTime, start.DateTime.Kind, out var preferRangeBoundary);

            var sample = Between(startTime, endTime) + start.Offset;

            // In practice, we will only ever get samples that are exactly equal to the end time
            // if the range is empty due to being contained within a DST transition window.
            if (sample == end)
               sample = preferRangeBoundary;

            return new DateTimeOffset(new DateTime(sample.Ticks, DateTimeKind.Unspecified), start.Offset);
         }
      }

      /// <summary>
      /// Get a random <see cref="DateTime"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Recent(int days = 1, DateTime? refDate = null)
      {
         var systemClock = SystemClock();

         var maxDate = refDate ?? systemClock;

         var minDate = days == 0 ? systemClock.Date : maxDate.AddDays(-days);

         return Between(minDate, maxDate);
      }

      /// <summary>
      /// Get a random <see cref="DateTimeOffset"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset RecentOffset(int days = 1, DateTimeOffset? refDate = null)
      {
         var systemClock = SystemClock();

         var maxDate = refDate ?? systemClock;

         var minDate = days == 0 ? systemClock.Date : maxDate.AddDays(-days);

         return BetweenOffset(minDate, maxDate);
      }

      /// <summary>
      /// Get a random <see cref="TimeSpan"/>.
      /// </summary>
      /// <param name="maxSpan">Maximum of time to span. Default 1 week/7 days.</param>
      public TimeSpan Timespan(TimeSpan? maxSpan = null)
      {
         var span = maxSpan ?? TimeSpan.FromDays(7);

         return RandomTimeSpanFromTicks(span.Ticks);
      }

      /// <summary>
      /// Get a random month.
      /// </summary>
      public string Month(bool abbreviation = false, bool useContext = false)
      {
         var type = "wide";
         if( abbreviation )
            type = "abbr";

         if( useContext &&
             (type == "wide" && hasMonthWideContext) ||
             (type == "abbr" && hasMonthAbbrContext) )
         {
            type += "_context";
         }

         return GetRandomArrayItem("month." + type);
      }

      /// <summary>
      /// Get a random weekday.
      /// </summary>
      public string Weekday(bool abbreviation = false, bool useContext = false)
      {
         var type = "wide";
         if( abbreviation )
            type = "abbr";

         if( useContext &&
             (type == "wide" && hasWeekdayWideContext) ||
             (type == "abbr" && hasWeekdayAbbrContext) )
         {
            type += "_context";
         }

         return GetRandomArrayItem("weekday." + type);
      }

      /// <summary>
      /// Get a timezone string. Eg: America/Los_Angeles
      /// </summary>
      public string TimeZoneString()
      {
         return GetRandomArrayItem("address", "time_zone");
      }
   }
}
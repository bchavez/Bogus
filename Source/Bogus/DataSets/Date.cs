using System;

namespace Bogus.DataSets
{
   /// <summary>
   /// Methods for generating dates
   /// </summary>
   public class Date : DataSet
   {
      private bool hasMonthWideContext;
      private bool hasMonthAbbrContext;
      private bool hasWeekdayWideContext;
      private bool hasWeekdayAbbrContext;
      private static Func<DateTimeOffset> dateTimeOffsetNow = () => DateTimeOffset.Now;
      private static Func<DateTime> dateTimeNow=() => DateTime.Now;

      /// <summary>
      /// Create a Date dataset
      /// </summary>
      public Date(string locale = "en") : base(locale)
      {
         this.hasMonthWideContext = Get("month.wide_context") != null;
         this.hasMonthAbbrContext = Get("month.abbr_context") != null;
         this.hasWeekdayWideContext = Get("weekday.wide_context") != null;
         this.hasWeekdayAbbrContext = Get("weekday.abbr_context") != null;
      }

      /// <summary>
      /// Sets the default global datetime "Now" generation.
      /// </summary>
      public static void SetDefaultNowGeneration(Func<DateTime> dateTimeNow, Func<DateTimeOffset> dateTimeOffsetNow)
      {
         if (dateTimeNow == null)
            throw new ArgumentNullException(nameof(dateTimeNow));
         if (dateTimeOffsetNow == null)
            throw new ArgumentNullException(nameof(dateTimeOffsetNow));
         Date.dateTimeOffsetNow = dateTimeOffsetNow;
         Date.dateTimeNow = dateTimeNow;
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
      {
         var maxDate = refDate ?? dateTimeNow();

         var minDate = maxDate.AddYears(-yearsToGoBack);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> in the past between <paramref name="refDate"/> and <paramref name="yearsToGoBack"/>.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset PastOffset(int yearsToGoBack = 1, DateTimeOffset? refDate = null)
      {
         var maxDate = refDate ?? dateTimeOffsetNow();

         var minDate = maxDate.AddYears(-yearsToGoBack);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
      }

      TimeSpan RandomTimeSpanFromTicks(long totalTimeSpanTicks)
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
         var dt = refDate ?? dateTimeNow();
         return Between(dt, dt.AddDays(days));
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than <paramref name="days"/> ahead.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset SoonOffset(int days = 1, DateTimeOffset? refDate = null)
      {
         var dt = refDate ?? dateTimeOffsetNow();
         return BetweenOffset(dt, dt.AddDays(days));
      }

      /// <summary>
      /// Get a <see cref="DateTime"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
      {
         var minDate = refDate ?? dateTimeNow();

         var maxDate = minDate.AddYears(yearsToGoForward);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return minDate + partTimeSpan;
      }

      /// <summary>
      /// Get a <see cref="DateTimeOffset"/> in the future between <paramref name="refDate"/> and <paramref name="yearsToGoForward"/>.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from <paramref name="refDate"/>. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset FutureOffset(int yearsToGoForward = 1, DateTimeOffset? refDate = null)
      {
         var minDate = refDate ?? dateTimeOffsetNow();

         var maxDate = minDate.AddYears(yearsToGoForward);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return minDate + partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTime"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time - The returned <seealso cref="DateTimeKind"/> is used from this parameter.</param>
      /// <param name="end">End time</param>
      public DateTime Between(DateTime start, DateTime end)
      {
         var minTicks = Math.Min(start.Ticks, end.Ticks);
         var maxTicks = Math.Max(start.Ticks, end.Ticks);

         var totalTimeSpanTicks = maxTicks - minTicks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return new DateTime(minTicks, start.Kind) + partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTimeOffset"/> between <paramref name="start"/> and <paramref name="end"/>.
      /// </summary>
      /// <param name="start">Start time - The returned <seealso cref="DateTimeOffset"/> offset value is used from this parameter</param>
      /// <param name="end">End time</param>
      public DateTimeOffset BetweenOffset(DateTimeOffset start, DateTimeOffset end)
      {
         var minTicks = Math.Min(start.Ticks, end.Ticks);
         var maxTicks = Math.Max(start.Ticks, end.Ticks);

         var totalTimeSpanTicks = maxTicks - minTicks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return new DateTimeOffset(minTicks, start.Offset) + partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTime"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTime.Now"/>.</param>
      public DateTime Recent(int days = 1, DateTime? refDate = null)
      {
         var maxDate = refDate ?? dateTimeNow();

         var minDate = days == 0 ? dateTimeNow().Date : maxDate.AddDays(-days);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Get a random <see cref="DateTimeOffset"/> within the last few days.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <param name="refDate">The date to start calculations. Default is <see cref="DateTimeOffset.Now"/>.</param>
      public DateTimeOffset RecentOffset(int days = 1, DateTimeOffset? refDate = null)
      {
         var maxDate = refDate ?? dateTimeOffsetNow();

         var minDate = days == 0 ? dateTimeOffsetNow().Date : maxDate.AddDays(-days);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         var partTimeSpan = RandomTimeSpanFromTicks(totalTimeSpanTicks);

         return maxDate - partTimeSpan;
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
      /// Get a random month
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
      /// Get a random weekday
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

         return GetRandomArrayItem("month." + type);
      }
   }
}
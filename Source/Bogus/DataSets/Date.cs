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


      /// <summary>
      /// Create a Date dataset
      /// </summary>
      /// <param name="locale"></param>
      public Date(string locale = "en") : base(locale)
      {
         this.hasMonthWideContext = Get("month.wide_context") != null;
         this.hasMonthAbbrContext = Get("month.abbr_context") != null;
         this.hasWeekdayWideContext = Get("weekday.wide_context") != null;
         this.hasWeekdayAbbrContext = Get("weekday.abbr_context") != null;
      }

      /// <summary>
      /// Get a date in the past between refDate and years past that date.
      /// </summary>
      /// <param name="yearsToGoBack">Years to go back from refDate. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is now.</param>
      /// <returns></returns>
      public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
      {
         var maxDate = refDate ?? DateTime.Now;

         var minDate = maxDate.AddYears(-yearsToGoBack);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         //find % of the timespan
         var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;

         var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Get a date and time that will happen soon.
      /// </summary>
      /// <param name="days">A date no more than N days ahead.</param>
      public DateTime Soon(int days = 1)
      {
         return Between(DateTime.Now, DateTime.Now.AddDays(days));
      }

      /// <summary>
      /// Get a date in the future between refDate and years forward of that date.
      /// </summary>
      /// <param name="yearsToGoForward">Years to go forward from refDate. Default is 1 year.</param>
      /// <param name="refDate">The date to start calculations. Default is now.</param>
      public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
      {
         var minDate = refDate ?? DateTime.Now;

         var maxDate = minDate.AddYears(yearsToGoForward);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         //find % of the timespan
         var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;

         var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

         return minDate + partTimeSpan;
      }

      /// <summary>
      /// Get a random date between start and end.
      /// </summary>
      /// <param name="start">Starting</param>
      /// <param name="end">Ending</param>
      /// <returns></returns>
      public DateTime Between(DateTime start, DateTime end)
      {
         var minTicks = Math.Min(start.Ticks, end.Ticks);
         var maxTicks = Math.Max(start.Ticks, end.Ticks);

         var totalTimeSpanTicks = maxTicks - minTicks;

         var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;

         var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

         return new DateTime(minTicks) + partTimeSpan;
      }

      /// <summary>
      /// Get a random date/time within the last few days since now.
      /// </summary>
      /// <param name="days">Number of days to go back.</param>
      /// <returns></returns>
      public DateTime Recent(int days = 1)
      {
         var maxDate = DateTime.Now;

         var minDate = days == 0 ? DateTime.Now.Date : maxDate.AddDays(-days);

         var totalTimeSpanTicks = (maxDate - minDate).Ticks;

         //find % of the timespan
         var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;

         var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

         return maxDate - partTimeSpan;
      }

      /// <summary>
      /// Get a random span of time.
      /// </summary>
      /// <param name="maxSpan">Maximum of time to span. Default 1 week/7 days.</param>
      public TimeSpan Timespan(TimeSpan? maxSpan = null)
      {
         var span = maxSpan ?? TimeSpan.FromDays(7);

         var totalTimeSpanTicks = span.Ticks;

         var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;

         var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

         return partTimeSpan;
      }

      /// <summary>
      /// Get a random month
      /// </summary>
      public string Month(bool abbrivation = false, bool useContext = false)
      {
         var type = "wide";
         if( abbrivation )
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
      public string Weekday(bool abbrivation = false, bool useContext = false)
      {
         var type = "wide";
         if( abbrivation )
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
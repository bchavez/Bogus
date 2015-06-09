using System;

namespace Bogus.Generators
{
    /// <summary>
    /// Methods for generating dates
    /// </summary>
    public class Date : DataSet
    {
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
        /// Get a date in the future between refDate and years forward of that date.
        /// </summary>
        /// <param name="yearsToGoForward">Years to go forward from refDate. Default is 1 year.</param>
        /// <param name="refDate">The date to start calculations. Default is now.</param>
        public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
        {
            var minDate = refDate ?? DateTime.Now;

            var maxDate = minDate.AddYears(yearsToGoForward);

            var totalTimeSpanTicks = ( maxDate - minDate ).Ticks;

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

            var minDate = maxDate.AddDays(-days);

            var totalTimeSpanTicks = ( maxDate - minDate ).Ticks;

            //find % of the timespan
            var partTimeSpanTicks = Random.Double() * totalTimeSpanTicks;

            var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

            return maxDate - partTimeSpan;
        }
    }
}

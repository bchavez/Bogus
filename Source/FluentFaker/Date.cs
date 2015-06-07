using System;

namespace FluentFaker
{
    public class Date : Category
    {

        public DateTime Past(int yearsToGoBack = 1, DateTime? refDate = null)
        {
            var maxDate = refDate ?? DateTime.Now;

            var minDate = maxDate.AddYears(-yearsToGoBack);

            var totalTimeSpanTicks = (maxDate - minDate).Ticks;

            //find % of the timespan
            var partTimeSpanTicks = Random.Generator.NextDouble() * totalTimeSpanTicks;

            var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

            return maxDate - partTimeSpan;
        }

        public DateTime Future(int yearsToGoForward = 1, DateTime? refDate = null)
        {
            var minDate = refDate ?? DateTime.Now;

            var maxDate = minDate.AddYears(yearsToGoForward);

            var totalTimeSpanTicks = ( maxDate - minDate ).Ticks;

            //find % of the timespan
            var partTimeSpanTicks = Random.Generator.NextDouble() * totalTimeSpanTicks;

            var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

            return minDate + partTimeSpan;
        }

        public DateTime Between(DateTime start, DateTime end)
        {
            var minTicks = Math.Min(start.Ticks, end.Ticks);
            var maxTicks = Math.Max(start.Ticks, end.Ticks);

            var totalTimeSpanTicks = maxTicks - minTicks;

            var partTimeSpanTicks = Random.Generator.NextDouble() * totalTimeSpanTicks;

            var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

            return new DateTime(minTicks) + partTimeSpan;
        }

        public DateTime Recent(int days = 1)
        {
            var maxDate = DateTime.Now;

            var minDate = maxDate.AddDays(-days);

            var totalTimeSpanTicks = ( maxDate - minDate ).Ticks;

            //find % of the timespan
            var partTimeSpanTicks = Random.Generator.NextDouble() * totalTimeSpanTicks;

            var partTimeSpan = TimeSpan.FromTicks(Convert.ToInt64(partTimeSpanTicks));

            return maxDate - partTimeSpan;
        }
    }
}
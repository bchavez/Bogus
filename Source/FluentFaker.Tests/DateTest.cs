using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    [TestFixture]
    public class DateTest : ConsistentTest
    {
        private Date date;

        [SetUp]
        public void BeforeEachTest()
        {
            date = new Date();
        }


        [Test]
        public void can_get_date_in_past()
        {
            var starting = DateTime.Parse("6/7/2015 4:17:41 PM");
            date.Past(refDate: starting).Should()
                .BeOnOrBefore(starting)
                .And
                .BeOnOrAfter(starting.AddYears(-1));
        }

        [Test]
        public void can_get_date_in_past_with_custom_options()
        {
            var starting = DateTime.Parse("6/15/2000 4:17:41 PM");
            date.Past(refDate: starting, yearsToGoBack: 5).Should()
                .BeOnOrBefore(starting)
                .And
                .BeOnOrAfter(starting.AddYears(-5));
        }


        [Test]
        public void can_get_date_in_future()
        {
            var starting = DateTime.Parse("6/7/2015 4:17:41 PM");
            date.Future(refDate: starting).Should()
                .BeOnOrBefore(starting.AddYears(1))
                .And
                .BeOnOrAfter(starting);
        }

        [Test]
        public void can_get_date_in_future_with_options()
        {
            var starting = DateTime.Parse("6/15/2000 4:17:41 PM");
            date.Future(refDate: starting, yearsToGoForward: 5).Should()
                .BeOnOrBefore(starting.AddYears(5))
                .And
                .BeOnOrAfter(starting);
        }


        [Test]
        public void can_get_random_time_between_two_dates()
        {
            var start = DateTime.Parse("6/15/2000 4:17:41 PM");
            var end = DateTime.Parse("8/15/2000 4:17:41 PM");

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

        [Test]
        public void can_get_date_recently_within_the_year()
        {
            var start = DateTime.Now;
            date.Recent()
                .Should()
                .BeOnOrBefore(start)
                .And
                .BeOnOrAfter(start.AddDays(-1));
        }
    }
}
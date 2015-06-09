using System;
using System.Net;
using FluentAssertions;
using Bogus.Generators;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class FinanceTest : SeededTest
    {
        private Finance finance;

        [SetUp]
        public void BeforeEachTest()
        {
            finance = new Finance();
        }

        [Test]
        public void can_get_random_currency()
        {
            var cur = finance.Currency();

            cur.Description.Should().Be("Nepalese Rupee");
            cur.Code.Should().Be("NPR");
            cur.Symbol.Should().NotBeNullOrWhiteSpace();
        }

        [Test]
        public void get_random_amount()
        {
            finance.Amount().Should().Be(603.52m);
        }

        [Test]
        public void get_random_amount_with_options()
        {
            var val = finance.Amount(200, 300, 3);

            val.Should()
                .BeInRange(200, 300);
            
            //get decimal places.
            var decimals = BitConverter.GetBytes(decimal.GetBits(val)[3])[2];

            decimals.Should().Be(3);
        }

        [Test]
        public void should_be_able_to_get_a_transaction_type()
        {
            finance.TransactionType().Should().Be("payment");
        }
    }
}

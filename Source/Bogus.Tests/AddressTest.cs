using Bogus.DataSets;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class AddressTest : SeededTest
    {
        private Address address;

        [SetUp]
        public void BeforeEachTest()
        {
            address = new Address();
        }

        [Test]
        public void can_get_a_zipcode()
        {
            address.ZipCode().Should().Be("18606-0643");
        }

        [Test]
        public void can_get_canadian_zip_code()
        {
            var a = new Address("en_CA");
            a.ZipCode().Should().Be("C8Q 0Q0");
        }

        [Test]
        public void can_get_a_city_name()
        {
            address.City().Should().Be("Bernhardfort");
        }

        [Test]
        public void can_get_a_street_address()
        {
            address.StreetAddress().Should().Be("60643 Oberbrunner Bypass");
        }

        [Test]
        public void can_get_a_full_street_address()
        {
            address.StreetAddress(useFullAddress: true).Should().Be("60643 Oberbrunner Bypass Suite 175");
        }

        [Test]
        public void can_get_a_county()
        {
            address.County().Should().Be("Borders");
        }

        [Test]
        public void can_get_a_country()
        {
            address.Country().Should().Be("Mozambique");
        }

        [Test]
        public void can_get_a_state()
        {
            address.State().Should().Be("New Mexico");
        }

        [Test]
        public void can_get_a_state_abbreviation()
        {
            address.StateAbbr().Should().Be("NM");
        }

        [Test]
        public void can_get_a_latitude()
        {
            address.Latitude().Should().Be(18.634);
        }

        [Test]
        public void can_get_a_longitude()
        {
            address.Latitude().Should().Be(18.634);
            
        }

        [Test]
        public void can_get_a_street_suffix()
        {
            address.StreetSuffix().Should().Be("Pines");
        }

        [Test]
        public void can_get_a_random_country_code()
        {
            address.CountryCode().Should().Be( "MQ" );
        }

        [Test]
        public void can_get_a_full_address()
        {
            address.FullAddress().Should().Be("60643 Oberbrunner Bypass, Danielchester, Monaco");
        }

    }
}

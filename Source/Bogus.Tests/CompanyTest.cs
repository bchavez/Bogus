using System.Net;
using Bogus.DataSets;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class CompanyTest : SeededTest
    {
        private Company company;

        [SetUp]
        public void BeforeEachTest()
        {
            company = new Company();
        }

        [Test]
        public void can_get_company_suffix_array()
        {
            var arr = company.Suffexes();

            arr.Length.Should().NotBe(0);
        }

        [Test]
        public void can_get_company_name()
        {
            company.CompanyName().Should().Be("Brown - Schultz");
        }

        [Test]
        public void can_get_a_company_name_with_custom_format()
        {
            company.CompanyName(0).Should().Be("Mitchell Inc");
        }

        [Test]
        public void can_get_a_catch_phrase()
        {
            company.CatchPhrase().Should().Be("Phased background protocol");
        }

        [Test]
        public void can_get_company_bs_phrase()
        {
            company.Bs().Should().Be("seamless transform schemas"); //lol
        }

    }
}

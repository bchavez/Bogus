using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
    public class CompanyTest : SeededTest
    {
        public CompanyTest()
        {
            company = new Company();
        }

        private readonly Company company;

        [Fact]
        public void can_get_a_catch_phrase()
        {
            company.CatchPhrase().Should().Be("Phased background protocol");
        }

        [Fact]
        public void can_get_a_company_name_with_custom_format()
        {
            company.CompanyName(0).Should().Be("Mitchell Inc");
        }

        [Fact]
        public void can_get_company_bs_phrase()
        {
            company.Bs().Should().Be("seamless transform schemas"); //lol
        }

        [Fact]
        public void can_get_company_name()
        {
            company.CompanyName().Should().Be("Brown - Schultz");
        }

        [Fact]
        public void can_get_company_suffix_array()
        {
            var arr = company.Suffexes();

            arr.Length.Should().NotBe(0);
        }
    }
}
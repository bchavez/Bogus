using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using Bogus.Extensions.UnitedStates;
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
         company.Bs().Should().Be("maximize leading-edge networks"); //lol
      }

      [Fact]
      public void can_get_company_name()
      {
         company.CompanyName().Should().Be("Brekke - Schultz");
      }

      [Fact]
      public void can_get_company_suffix_array()
      {
         var arr = company.Suffixes();

         arr.Length.Should().NotBe(0);
      }

      [Fact]
      public void can_generate_cnpj_for_brazil()
      {
         company.Cnpj().Should().Be("61.860.606/0001-91");
      }

      [Fact]
      public void can_generate_cnpj_for_brazil_without_formatting()
      {
         company.Cnpj(includeFormatSymbols: false).Should().Be("61860606000191");
      }

      [Fact]
      public void can_generate_an_EIN()
      {
         company.Ein().Should().Be("61-8606064");
      }
   }
}
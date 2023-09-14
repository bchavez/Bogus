using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.LocaleTests
{
   public class PtLocale : SeededTest
   {
      [Fact]
      public void address_test()
      {
         var a = new Address("pt_PT");
         a.CityPrefix().Should().Be("Vila Nova de");
         a.CitySuffix().Should().Be("do Douro");
         a.StreetSuffix().Should().Be("Viela");
         a.SecondaryAddress().Should().Be("Lote 06");
         a.State().Should().Be("Aveiro");
      }

      [Fact]
      public void company_tests()
      {
         var c = new Company("pt_PT");

         c.CompanySuffix().Should().Be("e Associados");
         c.CompanyName().Should().Be("Raposo e Associados");
      }

      [Fact]
      public void name_tests()
      {
         var n = new Name("pt_PT");

         n.Suffix().Should().Be("Neto");
         n.Prefix(Name.Gender.Female).Should().Be("Sra.");
      }

      [Fact]
      public void date_tests()
      {
         var d = new Date("pt_PT");

         d.Month().Should().Be("Agosto");
         d.Month(abbreviation: true).Should().Be("Fev");
         d.Weekday().Should().Be("Sábado");
         d.Weekday(abbreviation:true).Should().Be("Sex");
      }
   }
}
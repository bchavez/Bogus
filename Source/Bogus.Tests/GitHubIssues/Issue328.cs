using System;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue328 : SeededTest
   {
      [Fact]
      public void can_use_pt_br_locale_address_city_without_exception_thrown()
      {
         var a = new Address("pt_BR");
         Action act = () => a.City();

         act.Should().NotThrow();

         a.CityPrefix().Should().BeEmpty(because: "Current locale for pt_BR.address.city_prefix is empty. If it's not empty, then the data_extend/pt_BR.address.city name formats should be removed.");
         a.CitySuffix().Should().BeEmpty(because: "Current locale for pt_BR.address.city_suffix is empty. If it's not empty, then the data_extend/pt_BR.address.city name formats should be removed.");
      }
   }
}
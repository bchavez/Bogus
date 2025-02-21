using Bogus.DataSets;
using FluentAssertions;

namespace Bogus.Tests.GitHubIssues;

public class Issue342 : SeededTest
{
   [Fact]
   public void more_realistic_pt_BR_city_name()
   {
      var a = new Address("pt_BR");
      a.City().Should().Be("Pelotas");
   }
}
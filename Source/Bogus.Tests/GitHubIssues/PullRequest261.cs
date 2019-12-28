using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest261 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public PullRequest261(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void can_generate_ipv4_endpoint()
      {
         var i = new Internet();
         var ep = i.IpEndPoint();
         ep.ToString().Should().Be("218.35.156.76:2333");
      }

      [Fact]
      public void can_generate_ipv6_endpoint()
      {
         var i = new Internet();
         var ep = i.Ipv6EndPoint();
         ep.ToString().Should().Be("[da23:9c4c:e0c4:2dd7:e3c4:a896:17f2:55b2]:45956");
      }
   }
}
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues;

public class PullRequest513 : SeededTest
{
   [Fact]
   public void sv_has_gendered_names()
   {
      var n = new Name("sv");
      n.SupportsGenderFirstNames.Should().BeTrue();
   }
}
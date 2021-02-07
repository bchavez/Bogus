using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue355 : SeededTest
   {
      [Fact]
      public void pt_BR_has_gendered_names()
      {
         var n = new Name("pt_BR");
         n.SupportsGenderFirstNames.Should().BeTrue();
      }
   }
}
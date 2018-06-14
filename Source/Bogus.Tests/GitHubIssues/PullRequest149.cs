using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest149
   {
      [Fact]
      public void can_use_arabic_locale()
      {
         var f = new Faker("ar");

         f.Name.FirstName().Should().Be("عبير");
      }
   }
}
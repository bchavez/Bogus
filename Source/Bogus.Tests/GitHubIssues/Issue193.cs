using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue193 : SeededTest
   {
      [Fact]
      public void tr_locale_should_have_real_state_name()
      {
         var f = new Faker("tr");
         
         f.Address.State().Should().Be("Kirklareli");
      }
   }
}
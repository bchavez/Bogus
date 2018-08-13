using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue168 : SeededTest
   {
      [Fact]
      public void make_sure_person_card_has_a_state()
      {
         var p = new Bogus.Person();

         p.Address.State.Should().NotBeNullOrWhiteSpace();
      }
   }
}
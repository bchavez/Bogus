using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue169 : SeededTest
   {
      [Fact]
      public void date_weekday_should_generate_a_weekday()
      {
         var d = new DataSets.Date();

         d.Weekday().Should().Be("Thursday");
      }
   }
}
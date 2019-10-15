using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest258 : SeededTest
   {
      [Fact]
      public void can_get_rooted_url_path_with_fileExt()
      {
         var i = new Internet();

         i.UrlRootedPath(".txt").Should().Be("/soft/deposit.txt");
      }

      [Fact]
      public void get_rooted_path_with_no_ext()
      {
         var i = new Internet();

         i.UrlRootedPath().Should().Be("/soft/deposit");
      }

      [Fact]
      public void check_UrlWithPath_fileExt_parameter()
      {
         var i = new Internet();
         i.UrlWithPath(fileExt: ".mp3").Should().Be("https://ambrose.net/soft/deposit.mp3");
      }
   }
}
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest113
   {
      [Fact]
      public void category_exists_in_image_url()
      {
         var images = new Images();

         images.Cats().Should().Contain("cat");
      }
   }
}
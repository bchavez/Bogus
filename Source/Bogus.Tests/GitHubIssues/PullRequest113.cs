using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using static Bogus.DataSets.LoremPixelCategory;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest113 : SeededTest
   {
      [Fact]
      public void category_exists_in_image_url()
      {
         var images = new Images();

         images.LoremPixelUrl(Cats).Should().Contain("cat");
      }
   }
}
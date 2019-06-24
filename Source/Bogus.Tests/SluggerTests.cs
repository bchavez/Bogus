using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class SluggerTests
   {
      [Theory]
      [InlineData("Ánhanguera", "anhanguera")]
      [InlineData("foo Ánhanguera bar", "foo-anhanguera-bar")]
      [InlineData("Ánhanguera foo bar", "anhanguera-foo-bar")]
      [InlineData("Ánhanguera fooá", "anhanguera-fooa")]
      [InlineData("Schöner Titel läßt grüßen!? Bel été !", "schoener-titel-laesst-gruessen-bel-ete")]
      public void can_slug(string input, string expected)
      {
         var slug = Slugger.GetSlug(input);
         slug.Should().Be(expected);
      }

      [Fact]
      public void debug_slug()
      {
         var slug = Slugger.GetSlug("Schöner Titel läßt grüßen!? Bel été !");
      }
   }
}
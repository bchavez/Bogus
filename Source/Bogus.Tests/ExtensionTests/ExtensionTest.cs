using Bogus.DataSets;
using Bogus.Extensions.UnitedKingdom;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests
{
   public class ExtensionTest : SeededTest
   {
      [Fact]
      public void can_create_shortcode()
      {
         var f = new Finance();
         f.ShortCode().Should().Be("61-86-06");
         f.ShortCode(false).Should().Be("064391");
      }
   }
}
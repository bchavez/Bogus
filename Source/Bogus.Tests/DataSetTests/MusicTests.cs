using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.DataSetTests
{
   public class MusicTests : SeededTest
   {
      private readonly ITestOutputHelper console;
      private Music music;

      public MusicTests(ITestOutputHelper console)
      {
         this.console = console;
         this.music = new Music();
      }

      [Fact]
      public void can_generate_genre()
      {
         this.music.Genre().Should().Be("Hip Hop");
      }   
   }
}
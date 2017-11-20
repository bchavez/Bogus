using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests
{
   public class HandleBarTests : SeededTest
   {
      private readonly ITestOutputHelper console;

      public HandleBarTests(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void parse_test()
      {
         var f = new Faker();
         var s = Tokenizer.Parse("{{name.lastName}}, {{name.firstName}} {{name.suffix}}", f.Name);

         console.Dump(s);
         s.Should().Be("Mitchell, Bernhard DDS");
      }
   }
}
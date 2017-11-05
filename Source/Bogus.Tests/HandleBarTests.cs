using Xunit;

namespace Bogus.Tests
{
   public class HandleBarTests
   {
      [Fact]
      public void parse_test()
      {
         var f = new Faker();
         Tokenizer.Parse("{{name.lastName}}, {{name.firstName}} {{name.suffix}}", f.Name)
            .Dump();
      }
   }
}
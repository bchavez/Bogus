using Bogus.Tests.AutoFakerTests.Models.Complex;

namespace Bogus.Tests.AutoFakerTests.Helpers
{
   public static class GenerateExtensions
   {
      public static GenerateAssertions Should(this Order order)
      {
         return new GenerateAssertions(order);
      }
   }
}
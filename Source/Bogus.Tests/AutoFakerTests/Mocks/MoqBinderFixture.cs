using Bogus.Moq;
using Bogus.Tests.AutoFakerTests.Helpers;
using Bogus.Tests.AutoFakerTests.Models.Complex;
using Xunit;

namespace Bogus.Tests.AutoFakerTests.Mocks
{
   public class MoqBinderFixture : SeededTest
   {
      [Fact]
      public void Should_Create_With_Mocks()
      {
         var binder = new MoqBinder();

         AutoFaker.Generate<Order>(binder).Should().BePopulatedWithMocks();
      }
   }
}
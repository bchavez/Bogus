using Bogus.FakeItEasy;
using Bogus.Tests.AutoFakerTests.Helpers;
using Bogus.Tests.AutoFakerTests.Models.Complex;
using Xunit;

namespace Bogus.Tests.AutoFakerTests.Mocks
{
   public class FakeItEasyBinderFixture : SeededTest
   {
      [Fact]
      public void Should_Create_With_Mocks()
      {
         var binder = new FakeItEasyBinder();

         AutoFaker.Generate<Order>(binder).Should().BePopulatedWithMocks();
      }
   }
}
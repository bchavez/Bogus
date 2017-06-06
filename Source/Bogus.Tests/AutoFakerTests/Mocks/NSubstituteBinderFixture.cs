using Bogus.NSubstitute;
using Bogus.Tests.AutoFakerTests.Helpers;
using Bogus.Tests.AutoFakerTests.Models.Complex;
using Xunit;

namespace Bogus.Tests.AutoFakerTests.Mocks
{
   public class NSubstituteBinderFixture : SeededTest
   {
      [Fact]
      public void Should_Create_With_Mocks()
      {
         var binder = new NSubstituteBinder();

         AutoFaker.Generate<Order>(binder).Should().BePopulatedWithMocks();
      }
   }
}
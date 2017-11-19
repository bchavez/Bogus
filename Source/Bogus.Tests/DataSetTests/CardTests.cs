using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class CardTests : SeededTest
   {
      [Fact]
      public void should_be_able_to_get_a_contextually_bogus_person()
      {
         var card = new Person();

         card.FirstName.Should().Be("Doris");
         card.LastName.Should().Be("Schultz");
         card.UserName.Should().Be("Doris.Schultz");
         card.Email.Should().Be("Doris69@yahoo.com");

         card.Dump();
      }
   }
}
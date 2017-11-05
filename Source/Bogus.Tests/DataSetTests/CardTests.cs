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

         card.FirstName.Should().Be("Lee");
         card.LastName.Should().Be("Brown");
         card.UserName.Should().Be("Lee_Brown3");
         card.Email.Should().Be("Lee69@yahoo.com");

         card.Dump();
      }
   }
}
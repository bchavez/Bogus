using Bogus.DataSets;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class CardTests : SeededTest
    {
        [Test]
        public void should_be_able_to_get_a_contextually_bogus_person()
        {
            var card = new Person();

            card.FirstName.Should().Be("Lee");
            card.LastName.Should().Be("Brown");
            card.UserName.Should().Be("Lee_Brown3");
            card.Email.Should().Be("Lee_Brown369@yahoo.com");

            card.Dump();
        }
    }
}

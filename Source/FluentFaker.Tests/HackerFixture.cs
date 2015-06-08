using FluentAssertions;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    [TestFixture]
    public class HackerFixture : ConsistentTest
    {
        private Hacker hacker;


        [SetUp]
        public void BeforeEachTest()
        {
            hacker = new Hacker();
        }

        [Test]
        public void can_get_a_hacker_phrase()
        {
            hacker.Phrase().Should().Be("Use the neural RAM driver, then you can calculate the neural driver!");
        }

    }
}
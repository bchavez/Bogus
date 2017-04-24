using Bogus.DataSets;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class HackerTests : SeededTest
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

        [Test]
        public void make_sure_we_have_updated_ru_hacker_locale()
        {
            var ruhacker = new Hacker("ru");
            ruhacker.Adjective().Should().Be("многобайтный");
            ruhacker.Noun().Should().Be("пропускная способность");
            ruhacker.Verb().Should().Be("передавать");
            ruhacker.IngVerb().Should().Be("определение количества");
        }

    }
}
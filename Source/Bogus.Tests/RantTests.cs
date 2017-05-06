using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class RantTests : SeededTest
    {
        private DataSets.Rant rant;

        [SetUp]
        public void BeforeEachTest()
        {
            rant = new DataSets.Rant();
        }

        [Test]
        public void can_get_random_product_review()
        {
            rant.Review("foobar").Should().Be("one of my hobbies is poetry. and when i'm writing poems this works great.");
        }

        [Test]
        public void can_get_an_array_of_reviews()
        {
            var reviews = rant.Reviews("foobar", 3);

            var truth = new[]
                {
                    "one of my hobbies is poetry. and when i'm writing poems this works great.",
                    "I tried to annihilate it but got bonbon all over it.",
                    "My co-worker Merwin has one of these. He says it looks bubbly."
                };
            //reviews.Length.Should().Be(3);
            reviews.Should().BeEquivalentTo(truth);
        }
    }
}
using FluentAssertions;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    [TestFixture]
    public class LoremTests : ConsistentTest
    {
        private Lorem lorem;

        [SetUp]
        public void BeforeEachTest()
        {
            lorem = new Lorem();
        }

        [Test]
        public void can_get_3_words()
        {
            var test = lorem.Words();
            
            test.Dump();

            test.Should()
                .HaveCount(3)
                .And
                .ContainInOrder("id","fugit","illum");
        }

        [Test]
        public void can_get_5_words()
        {
            var test = lorem.Words(5);

            test.Dump();

            test.Should()
                .HaveCount(5)
                .And
                .ContainInOrder("id","fugit","illum","est","ab");
            
        }

        [Test]
        public void can_get_a_sentance()
        {
            lorem.Sentance().Should().Be("aut illum est quae et quasi optio");
        }

        [Test]
        public void can_get_a_sentance_with_options()
        {
            lorem.Sentance(5, 0)
                .Should().Be("aut illum est quae et");
        }

        [Test]
        public void should_get_some_sentances()
        {
            lorem.Sentances(5)
                .Should().Be("aut illum est quae et quasi optio\ncorporis rerum dolor minus deserunt fugit\ncumque repudiandae eaque\nvoluptas voluptatem et animi aut eligendi sapiente ea magnam\nperspiciatis incidunt voluptatem sed est consequatur amet");
        }

        [Test]
        public void can_get_a_paragraph()
        {
            lorem.Paragraph()
                .Should().Be("vel est ipsa\nab eligendi atque enim rerum consectetur id\nexplicabo ipsa nihil repudiandae consequatur pariatur nulla\nlaborum mollitia explicabo est sapiente\ntempora qui unde labore voluptas consequuntur");
        }

        [Test]
        public void can_get_paragraphs()
        {
            lorem.Paragraphs()
                .Should().Be("vel est ipsa\nab eligendi atque enim rerum consectetur id\nexplicabo ipsa nihil repudiandae consequatur pariatur nulla\nlaborum mollitia explicabo est sapiente\ntempora qui unde labore voluptas consequuntur\n \r	dolorem non delectus et et molestiae consequatur saepe dolor\ntotam ad error architecto iusto sed numquam voluptatem eos\net modi error ea libero\nlaudantium eveniet omnis porro eos et et enim\nassumenda hic quibusdam non iusto in est dolorem et\n \r	dolorem ipsum neque qui ab aperiam repellat esse\nrerum quis et sunt voluptatibus\ndoloremque eos et voluptatem pariatur eum quis numquam nam sit");
        }
    }
}
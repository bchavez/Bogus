using Bogus.DataSets;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class LoremTests : SeededTest
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
        public void can_get_a_sentence()
        {
            lorem.Sentence().Should().Be("Aut illum est quae et quasi optio.");
        }

        [Test]
        public void can_get_a_sentence_with_options()
        {
            lorem.Sentence(5, 0)
                .Should().Be("Aut illum est quae et.");
        }

        [Test]
        public void should_get_some_sentences()
        {
            lorem.Sentences(5)
                .Should().Be("Aut illum est quae et quasi optio.\nCorporis rerum dolor minus deserunt fugit.\nCumque repudiandae eaque.\nVoluptas voluptatem et animi aut eligendi sapiente ea magnam.\nPerspiciatis incidunt voluptatem sed est consequatur amet.");
        }

        [Test]
        public void can_get_a_paragraph()
        {
            lorem.Paragraph()
                .Should().Be("Vel est ipsa.\nAb eligendi atque enim rerum consectetur id.\nExplicabo ipsa nihil repudiandae consequatur pariatur nulla.\nLaborum mollitia explicabo est sapiente.\nTempora qui unde labore voluptas consequuntur.");
        }

        [Test]
        public void can_get_paragraphs()
        {
            lorem.Paragraphs()
                .Should().Be("Vel est ipsa.\nAb eligendi atque enim rerum consectetur id.\nExplicabo ipsa nihil repudiandae consequatur pariatur nulla.\nLaborum mollitia explicabo est sapiente.\nTempora qui unde labore voluptas consequuntur.\r\nDolorem non delectus et et molestiae consequatur saepe dolor.\nTotam ad error architecto iusto sed numquam voluptatem eos.\nEt modi error ea libero.\nLaudantium eveniet omnis porro eos et et enim.\nAssumenda hic quibusdam non iusto in est dolorem et.\r\nDolorem ipsum neque qui ab aperiam repellat esse.\nRerum quis et sunt voluptatibus.\nDoloremque eos et voluptatem pariatur eum quis numquam nam sit.");
        }

        [Test]
        public void can_get_some_letters()
        {
            var c = lorem.Letter();
            c.Should().Be("i");


            var chars = lorem.Letter(100);
            chars.Length.Should().Be(100);
            chars.Should().Be("eiblrueeulrtiorismecntonniaeaaumrumclrquoqaeoiehdtueuteisquagsieuiuturutunuuaiuamisseqvnqeratepilptt");
        }
    }
}

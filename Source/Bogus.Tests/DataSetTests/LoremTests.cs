using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Z.ExtensionMethods;

namespace Bogus.Tests.DataSetTests
{
    public class LoremTests : SeededTest
    {
        public LoremTests()
        {
            lorem = new Lorem();
        }

        private readonly Lorem lorem;

        [Fact]
        public void can_get_3_words()
        {
            var test = lorem.Words();

            test.Dump();

            test.Should()
                .HaveCount(3);

            test.Should().Equal("id", "aut", "vel");

            //test.ForEach(w => w.Should().NotBeNullOrWhiteSpace());
        }

        [Fact]
        public void can_get_5_words()
        {
            var test = lorem.Words(5);

            test.Dump();

            test.Should()
                .HaveCount(5);

            test.Should().Equal("id",
                "aut",
                "vel",
                "facilis",
                "aperiam");

            //test.ForEach(w => w.Should().NotBeNullOrWhiteSpace());
        }

        [Fact]
        public void can_get_a_paragraph()
        {
            lorem.Paragraph()
                .Split(". ").Length.Should().Be(5); // para of 5 sentances.
        }

        [Fact]
        public void can_get_a_random_word()
        {
            lorem.Word().Should().Be("id");
            lorem.Word().Should().Be("aut");
            lorem.Word().Should().Be("vel");
        }

        [Fact]
        public void can_get_a_sentence()
        {
            lorem.Sentence().Split(' ').Length.Should().BeGreaterThan(3);
        }

        [Fact]
        public void can_get_a_sentence_with_options()
        {
            lorem.Sentence(5).Split(' ').Length.Should().Be(5);
        }

        [Fact]
        public void can_get_a_setnance_with_range_option()
        {
            lorem.Sentence(10, 5).Split(' ').Length.Should().Be(13);
        }

        [Fact]
        public void can_get_paragraphs()
        {
            lorem.Paragraphs()
                .Split("\n\n").Length.Should().Be(3);
        }

        [Fact]
        public void can_get_some_letters()
        {
            var c = lorem.Letter();
            c.Should().Be("i");


            var chars = lorem.Letter(100);
            chars.Length.Should().Be(100);
            chars.Should().Be("eiblrueeulrtiorismecntonniaeaaumrumclrquoqaeoiehdtueuteisquagsieuiuturutunuuaiuamisseqvnqeratepilptt");
        }

        [Fact]
        public void can_get_some_lorem_lines()
        {
            lorem.Lines().Split(" ").Length.Should().BeGreaterThan(5);
        }

        [Fact]
        public void can_get_some_lorem_text()
        {
            lorem.Text().Split(" ").Length.Should().BeGreaterThan(5);
        }

        [Fact]
        public void can_slugify_lorem()
        {
            lorem.Slug(5).Should().Be("id-aut-vel-facilis-aperiam");
        }
    }
}
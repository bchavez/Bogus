using System;
using System.Linq;
using Bogus.DataSets;
using FluentAssertions;
using NUnit.Framework;
using Z.ExtensionMethods;

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
                .HaveCount(3);

            test.Should().Equal("id", "aut", "vel");

            //test.ForEach(w => w.Should().NotBeNullOrWhiteSpace());
        }

        [Test]
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

        [Test]
        public void can_get_a_sentence()
        {
            lorem.Sentence().Split(' ').Length.Should().BeGreaterThan(3);
        }

        [Test]
        public void can_get_a_sentence_with_options()
        {
            lorem.Sentence(5).Split(' ').Length.Should().Be(5);
        }

        [Test]
        public void can_get_a_paragraph()
        {
            lorem.Paragraph()
                .Split(". ").Length.Should().Be(5); // para of 5 sentances.
        }

        [Test]
        public void can_get_paragraphs()
        {
            lorem.Paragraphs()
                .Split("\n\n").Length.Should().Be(3);
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

        [Test]
        public void can_get_a_random_word()
        {
            lorem.Word().Should().Be("id");
            lorem.Word().Should().Be("aut");
            lorem.Word().Should().Be("vel");

        }

        [Test]
        public void can_get_some_lorem_text()
        {
            lorem.Text().Split(" ").Length.Should().BeGreaterThan(5);
        }

        [Test]
        public void can_get_some_lorem_lines()
        {
            lorem.Lines().Split(" ").Length.Should().BeGreaterThan(5);
        }

        [Test]
        public void can_slugify_lorem()
        {
            lorem.Slug(5).Should().Be("id-aut-vel-facilis-aperiam");
        }
    }
}

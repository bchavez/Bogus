using System.Linq;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Z.ExtensionMethods;

namespace Bogus.Tests.DataSetTests
{
   public class LoremTests : SeededTest
   {
      private readonly ITestOutputHelper console;

      public LoremTests(ITestOutputHelper console)
      {
         this.console = console;
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
            .Split(". ").Length.Should().Be(5); // para of 5 sentences.
      }

      [Fact]
      public void paragraph_with_zero_sentences()
      {
         var text = lorem.Paragraph(0);

         text.Count(c => c == '.')
            .Should()
            .BeGreaterOrEqualTo(0)
            .And
            .BeLessOrEqualTo(3);
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
      public void can_get_a_sentences_with_range_option()
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
      public void can_get_random_number_of_paragraphs()
      {
         var text = lorem.Paragraphs(5, 7);

         console.Dump(text);

         text.Split("\n\n")
            .Length.Should()
            .BeGreaterOrEqualTo(5)
            .And
            .BeLessOrEqualTo(7);
      }

      [Fact]
      public void check_separator_works()
      {
         var text = lorem.Paragraphs(5, 7, "<br/>");

         text.Split("<br/>").Length
            .Should()
            .BeGreaterOrEqualTo(5)
            .And
            .BeLessOrEqualTo(7);
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
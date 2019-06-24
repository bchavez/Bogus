using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class SluggerTests
   {
      [Theory]
      [InlineData("Ánhanguera", "anhanguera")]
      [InlineData("foo Ánhanguera bar", "foo-anhanguera-bar")]
      [InlineData("Ánhanguera foo bar", "anhanguera-foo-bar")]
      [InlineData("Ánhanguera fooá", "anhanguera-fooa")]
      [InlineData("Schöner Titel läßt grüßen!? Bel été !", "schoener-titel-laesst-gruessen-bel-ete")]
      [InlineData("Foo ♥ Bar", "foo-love-bar")]
      [InlineData("NEXUS4 only $299", "nexus4-only-usd299")]
      public void can_slug(string input, string expected)
      {
         var slug = Slugger.GetSlug(input);
         slug.Should().Be(expected);
      }

      [Fact]
      public void debug_slug()
      {
         var slug = Slugger.GetSlug("Foo ♥ Bar");
      }

      [Fact]
      public void separator_test()
      {
         Slugger.GetSlug("Schöner Titel läßt grüßen!? Bel été !", "*")
            .Should().Be("schoener*titel*laesst*gruessen*bel*ete");
      }

      [Fact]
      public void uric()
      {
         Slugger.GetSlug("Schöner Titel läßt grüßen!? Bel été !", uric: true)
            .Should().Be("schoener-titel-laesst-gruessen-?-bel-ete");
      }
      [Fact]
      public void uric_noslash()
      {
         Slugger.GetSlug("Schöner Titel läßt grüßen!? Bel été !", uricNoSlash: true)
            .Should().Be("schoener-titel-laesst-gruessen-?-bel-ete");
      }

      [Fact]
      public void truncate()
      {
         Slugger.GetSlug("Schöner Titel läßt grüßen!? Bel été !", truncate: 20)
            .Should().Be("schoener-titel");
      }

      [Fact]
      public void maintain_case()
      {
         Slugger.GetSlug("Schöner Titel läßt grüßen!? Bel été !", maintainCase: true)
            .Should().Be("Schoener-Titel-laesst-gruessen-Bel-ete");
      }

      [Fact]
      public void lang_test()
      {
         Slugger.GetSlug("Äpfel & Birnen!", lang: "de")
            .Should().Be("aepfel-und-birnen");
      }

      [Fact]
      public void lang_my()
      {
         Slugger.GetSlug("မြန်မာ သာဓက", lang: "my")
            .Should().Be("myanma-thadak");
      }

      [Fact]
      public void lang_dv()
      {
         Slugger.GetSlug("މިއަދަކީ ހދ ރީތި ދވހކވ", lang: "dv")
            .Should().Be("miaadhakee-hdh-reethi-dhvhkv");
      }

      [Fact]
      public void lang_en()
      {
         Slugger.GetSlug("Apple & Pear!", lang: "en")
            .Should().Be("apple-and-pear");
      }

      [Fact]
      public void nexus4_maitnain_case()
      {
         Slugger.GetSlug("NEXUS4 only €299", maintainCase: true)
            .Should().Be("NEXUS4-only-EUR299");
      }

      [Fact]
      public void title_case()
      {
         Slugger.GetSlug("Don't drink and drive", titleCase: true)
            .Should().Be("Don-t-Drink-And-Drive");
      }

      [Fact]
      public void symbols_false()
      {
         Slugger.GetSlug("Foo & Bar ♥ Foo < Bar", symbols: false)
            .Should().Be("foo-bar-foo-bar");
      }
   }

   public class TransliterateTests
   {
      [Fact]
      public void Test()
      {
         Transliterate.Translate("À").Should().Be("A");
         Transliterate.Translate("ден").Should().Be("MKD");
      }
   }
}
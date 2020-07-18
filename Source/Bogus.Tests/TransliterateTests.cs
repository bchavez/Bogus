using System;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class TransliterateTests
   {
      [Fact]
      public void Test()
      {
         Transliterater.Translate("À").Should().Be("A");
         Transliterater.Translate("ден").Should().Be("MKD");
         Transliterater.Translate("စျ").Should().Be("za");
      }

      [Fact]
      public void index_test()
      {
         Transliterater.Translate("ေါင်ူ").Should().Be("aungu");
      }

      [Fact]
      public void simple_test()
      {
         Transliterater.Translate("ာ").Should().Be("a");
      }

      [Fact]
      public void basic_ru_test()
      {
         Transliterater.Translate("Анна Фомина").Should().Be("Anna Fomina");
      }

      [Fact]
      public void index2_test()
      {
         Transliterater.Translate("ေါင်ff").Should().Be("aungff");
      }

      [Fact]
      public void transliterate_with_unknown_langauge_doesnt_throw()
      {
         Action a = () => Transliterater.Translate("fefefe", "gggg");
         a.Should().NotThrow();
      }

      [Fact]
      public void can_translate_symbol()
      {
         Transliterater.Translate("♥").Should().Be("love");
      }

      [Fact]
      public void can_translate_symbol_with_locale()
      {
         Transliterater.Translate("♥", "es").Should().Be("amor");
      }

      [Fact]
      public void can_translate_with_langchar_map()
      {
         Transliterater.Translate("Ä").Should().Be("Ae");
         Transliterater.Translate("Ä", lang: "fi").Should().Be("A");
         Transliterater.Translate("Ä", lang: "hu").Should().Be("A");
      }
   }
}

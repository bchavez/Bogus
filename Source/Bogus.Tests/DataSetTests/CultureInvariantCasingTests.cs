using System.Globalization;
using System.Linq;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests;

public class CultureInvariantCasingTests : SeededTest
{
   // In the Turkish culture "I".ToLower() is "ı" and "i".ToUpper() is "İ",
   // so the generated values must not depend on the machine's culture.
   private static readonly char[] TurkishLetters = { 'ı', 'İ' };

   [Fact]
   public void domain_words_are_the_same_under_turkish_culture()
   {
      var domainWords = WithTurkishCulture(() => Enumerable.Range(0, 500).Select(_ => new Internet().DomainWord()).ToArray());

      domainWords.Should().Contain(w => w.StartsWith("i"));
      domainWords.Should().NotContain(w => w.IndexOfAny(TurkishLetters) >= 0);
   }

   [Fact]
   public void lorem_sentences_are_the_same_under_turkish_culture()
   {
      var sentences = WithTurkishCulture(() => Enumerable.Range(0, 500).Select(_ => new Lorem().Sentence()).ToArray());

      sentences.Should().Contain(s => s.StartsWith("I"));
      sentences.Should().NotContain(s => s.IndexOfAny(TurkishLetters) >= 0);
   }

   [Fact]
   public void file_names_and_url_paths_are_the_same_under_turkish_culture()
   {
      var values = WithTurkishCulture(() => Enumerable.Range(0, 200)
         .SelectMany(_ => new[] { new DataSets.System().FileName(), new DataSets.System().CommonFileName(), new Internet().UrlRootedPath() })
         .ToArray());

      values.Should().NotContain(v => v.IndexOfAny(TurkishLetters) >= 0);
   }

   private static T WithTurkishCulture<T>(global::System.Func<T> action)
   {
      var previous = CultureInfo.CurrentCulture;
      CultureInfo.CurrentCulture = new CultureInfo("tr-TR");
      try
      {
         return action();
      }
      finally
      {
         CultureInfo.CurrentCulture = previous;
      }
   }
}

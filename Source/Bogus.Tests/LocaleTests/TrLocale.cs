using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.LocaleTests
{
   public class TrLocale : SeededTest
   {
      private readonly ITestOutputHelper console;

      public TrLocale(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void ensure_tr_locale_lorem_is_used()
      {
         var l = new Lorem("tr");
         l.Sentence().Should().Be("Değerli voluptatem quia değirmeni sequi mi numquam.");
      }
   }
}
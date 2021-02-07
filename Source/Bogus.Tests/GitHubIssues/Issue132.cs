using System;
using System.Globalization;
using Bogus.DataSets;
using Bogus.Extensions;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue132 : SeededTest
   {
      [Fact]
      public void should_throw_exception_on_invalid_locale_dataset()
      {
         Action a = () => new Lorem("wtf_locale");
         a.Should().Throw<BogusException>();
      }

      [Fact]
      public void should_throw_exception_on_invalid_locale_with_faker_t()
      {
         Action a = () => new Faker<Models.Order>("yo yo yo");

         a.Should().Throw<BogusException>();
      }

      [Fact]
      public void should_throw_exception_on_invalid_locate_with_faker()
      {
         Action a = () => new Faker("fe fi fo fum");

         a.Should().Throw<BogusException>();
      }

      [Fact]
      public void ensure_the_project_url_exists()
      {

         Action a = () => new Lorem("LOCALE");

         //make sure the message has a link back to the project site.
         //test exists here because we're using AssemblyDescription attribute
         //and in case that changes, we need to be aware of it.

         a.Should().Throw<BogusException>()
            .And.Message
            .Should().Contain("https://github.com/bchavez/Bogus");

      }

      [Fact]
      public void fr_locale()
      {
         CultureInfo.GetCultureInfo("fr-CA")
            .ToBogusLocale()
            .Should().Be("fr_CA");

         CultureInfo.GetCultureInfo("fr-BE")
            .ToBogusLocale()
            .Should().Be("fr");
      }


      [Fact]
      public void nb_NO_locale()
      {
         CultureInfo.GetCultureInfo("nn-NO")
            .ToBogusLocale()
            .Should().Be("nb_NO");

         CultureInfo.GetCultureInfo("nb-NO")
            .ToBogusLocale()
            .Should().Be("nb_NO");

         CultureInfo.GetCultureInfo("no")
            .ToBogusLocale()
            .Should().Be("nb_NO");

         CultureInfo.GetCultureInfo("nb")
            .ToBogusLocale()
            .Should().Be("nb_NO");

         CultureInfo.GetCultureInfo("nn")
            .ToBogusLocale()
            .Should().Be("nb_NO");
      }


      [Fact]
      public void id_ID_locale()
      {
         CultureInfo.GetCultureInfo("id-ID")
            .ToBogusLocale()
            .Should().Be("id_ID");

         CultureInfo.GetCultureInfo("id")
            .ToBogusLocale()
            .Should().Be("id_ID");
      }

      [Fact]
      public void ne_locale()
      {
         CultureInfo.GetCultureInfo("ne-NP")
            .ToBogusLocale()
            .Should().Be("ne");

         CultureInfo.GetCultureInfo("ne")
            .ToBogusLocale()
            .Should().Be("ne");
      }

      [Fact]
      public void ge_locale()
      {
         CultureInfo.GetCultureInfo("ka-GE")
            .ToBogusLocale()
            .Should().Be("ge");

         CultureInfo.GetCultureInfo("ka")
            .ToBogusLocale()
            .Should().Be("ge");
      }

      [Fact]
      public void ind_locale()
      {
         CultureInfo.GetCultureInfo("en-IN")
            .ToBogusLocale()
            .Should().Be("en_IND");
      }

      [Fact]
      public void cz_locale()
      {
         CultureInfo.GetCultureInfo("cs")
            .ToBogusLocale()
            .Should().Be("cz");

         CultureInfo.GetCultureInfo("cs-CZ")
            .ToBogusLocale()
            .Should().Be("cz");
      }

      [Fact]
      public void en_US_locale()
      {
         CultureInfo.GetCultureInfo("en-US")
            .ToBogusLocale()
            .Should().Be("en_US");
      }

      [Fact]
      public void de_locale()
      {
         CultureInfo.GetCultureInfo("de-AT")
            .ToBogusLocale()
            .Should().Be("de_AT");

         CultureInfo.GetCultureInfo("de-CH")
            .ToBogusLocale()
            .Should().Be("de_CH");

         CultureInfo.GetCultureInfo("de-LI")
            .ToBogusLocale()
            .Should().Be("de");
      }
   }
}

using Bogus.DataSets;
using Bogus.Extensions;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue225 : SeededTest
   {
      [Fact]
      public void can_generate_sane_email_addresses_in_different_locales()
      {
         var p = new Bogus.Person("ru");
         p.FullName.Should().Be("Анастасия Евсеева");
         p.Email.Should().Be("Anastasiya69@gmail.com");
      }

      [Fact]
      public void can_generate_sane_email_address_from_ru()
      {
         var i = new Internet();
         i.Email("Анна", "Фомина").Should().Be("Anna81@yahoo.com");
      }

      [Fact]
      public void can_generate_email_without_transliteration()
      {
         var i = new Internet();
         i.Email("Анна", "Фомина").Should().Be("Anna81@yahoo.com");
      }

      [Fact]
      public void simple_translation()
      {
         "Анна Фомина".Transliterate().Should().Be("Anna Fomina");
      }
   }
}
using Bogus.DataSets;
using Bogus.Extensions;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue86 : SeededTest
   {
      private Internet internet;

      public Issue86()
      {
         internet = new Internet();
      }

      [Fact]
      public void should_remove_diacritic_marks()
      {
         "hello ���� world".RemoveDiacritics().Should().Be("hello AOUi world");
      }

      [Fact]
      public void should_remove_diacritic_marks_in_email()
      {
         internet.Email("�ra'in����", "����chavez").Should().Be("rainAOUi81@yahoo.com");
      }

      [Fact]
      public void should_remove_diacritic_marks_in_username()
      {
         internet.UserName("�ri'����an", "cha����ez").Should().Be("riAOUian.chaAOUiez");
      }
   }
}
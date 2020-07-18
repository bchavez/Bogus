using System;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest149 : SeededTest
   {
      [Fact]
      public void ensure_arabic_locale_exists()
      {
         Action a = () => new Faker("ar");

         a.Should().NotThrow();
      }
   }
}

using System;
using System.Globalization;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest180 : SeededTest, IDisposable
   {
      private CultureInfo current;

      public PullRequest180()
      {
         current = Thread.CurrentThread.CurrentCulture;
      }

      [Fact]
      public void lower_invarant_should_be_used_to_avoid_locale_issues_in_json_path()
      {
         var turkish = CultureInfo.GetCultureInfo("tr");
         Thread.CurrentThread.CurrentCulture = turkish;
         var faker = new Faker("tr");
         faker.Internet.Url().Should().NotBeNullOrWhiteSpace();
      }

      public void Dispose()
      {
         Thread.CurrentThread.CurrentCulture = current;
      }
   }
}
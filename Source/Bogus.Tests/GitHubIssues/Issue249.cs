using FluentAssertions;
using System;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue249 : SeededTest
   {
      public class UrlModel
      {
         public string UrlTest1 { get; set; }
         public string UrlTest2 { get; set; }
         public string UrlTest3 { get; set; }
         public string UrlTest4 { get; set; }
      }

      [Fact]
      public void should_have_valid_url_for_pt_BR()
      {
         var FakerUrl = new Faker<UrlModel>("pt_BR")
            .RuleFor(u => u.UrlTest1, f => f.Internet.Url())
            .RuleFor(u => u.UrlTest2, f => f.Internet.Url())
            .RuleFor(u => u.UrlTest3, f => f.Internet.Url())
            .RuleFor(u => u.UrlTest4, f => f.Internet.Url());

         var x = FakerUrl.Generate(1000);

         foreach (var u in x)
         {
            Uri.TryCreate(u.UrlTest1, UriKind.Absolute, out _).Should().BeTrue($"Wrong URL format: {u.UrlTest1}");
            Uri.TryCreate(u.UrlTest2, UriKind.Absolute, out _).Should().BeTrue($"Wrong URL format: {u.UrlTest2}");
            Uri.TryCreate(u.UrlTest3, UriKind.Absolute, out _).Should().BeTrue($"Wrong URL format: {u.UrlTest3}");
            Uri.TryCreate(u.UrlTest4, UriKind.Absolute, out _).Should().BeTrue($"Wrong URL format: {u.UrlTest4}");
         }
      }
   }
}
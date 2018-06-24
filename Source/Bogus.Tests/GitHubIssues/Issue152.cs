using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue152 : SeededTest
   {
      [Fact]
      public void make_unique_email_easier_to_generate()
      {
         var f = new Faker();

         var email = f.Internet.Email(uniqueSuffix: "fff");
         email.Should().Contain("fff@");

         f.Internet.Email(uniqueSuffix: f.UniqueIndex.ToString());
      }
   }
}
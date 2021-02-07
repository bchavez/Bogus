using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest194 : SeededTest
   {
      [Fact]
      public void can_generate_nl_locale_with_modifications()
      {
         var f = new Faker("nl");

         f.Address.StreetSuffix().Should().Be("sloot");
         f.Company.Suffixes().Should().Equal(
            new[]
               {
                  "Bank",
                  "BV",
                  "B.V.",
                  "NV",
                  "N.V.",
                  "V.O.F.",
                  "International",
                  "Groep",
                  "Group",
                  "HRM",
                  "ICT",
                  "IT",
                  "Maatschappij",
                  "Online",
                  "en Zonen"
               });

         f.Name.FirstName().Should().Be("Bram");
      }
   }
}
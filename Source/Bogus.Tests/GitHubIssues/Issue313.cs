using Xunit;
using Bogus.Extensions.Extras;
using FluentAssertions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue313 : SeededTest
   {
      [Fact]
      public void can_create_custom_credit_card_number_PAN()
      {
         var f = new Faker();
         Visa16Digit(f).Should()
            .HaveLength(16)
            .And
            .Be("4618606064391758");
      }

      public static string Visa16Digit(Faker f)
      {
         const string format = "4##############";
         return f.Random.ReplaceNumbers(format).AppendCheckDigit();
      }
   }

}
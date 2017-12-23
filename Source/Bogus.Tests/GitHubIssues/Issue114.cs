using Bogus.DataSets;
using Bogus.Extensions.Extras;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue114 : SeededTest
   {
      [Fact]
      public void should_be_able_to_get_an_obfuscated_credit_card_number()
      {
         var finance = new Finance();
         finance.CreditCardNumberObfuscated().Should().Be("****-****-****-6186");
      }
      
      [Fact]
      public void cn_get_last_for_credit_card_digits()
      {
         var finance = new Finance();
         finance.CreditCardNumberLastFourDigits().Should().Be("6186");
      }
   }
}
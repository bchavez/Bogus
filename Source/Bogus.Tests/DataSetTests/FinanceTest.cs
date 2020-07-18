using System;
using System.Collections.Generic;
using System.Linq;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class FinanceTest : SeededTest
   {
      public FinanceTest()
      {
         finance = new Finance();
      }

      private readonly Finance finance;

      [Fact]
      public void can_get_a_random_account_default()
      {
         finance.Account().Should().Be("61860606");
      }

      [Fact]
      public void can_get_a_random_account_12()
      {
         finance.Account(12).Should().Be("618606064391");
      }

        [Fact]
      public void can_get_a_random_account_name()
      {
         finance.AccountName().Should().Be("Home Loan Account");
      }

      [Fact]
      public void can_generate_a_random_bitcoin_address()
      {
         var btc = finance.BitcoinAddress();
         btc.Should().MatchRegex("^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$");
         btc.Should().Be("17PE5D8HxpXjM3igcWbTeyKns2YdAfLC");
      }

      [Fact]
      public void can_generate_a_random_ethereum_address()
      {
         finance.EthereumAddress().Should().Be("0x91da090b74f2b910be0dd5991af6398351ac2ef3");
      }

      [Fact]
      public void can_generate_a_random_litecoin_address()
      {
         finance.LitecoinAddress().Should().Be("L7PE5D8HxpXjM3igcWbTeyKns2YdAfL");
      }

      [Fact]
      public void can_generate_aba_routing_number()
      {
         finance.RoutingNumber().Should().Be("618606061");
      }

      [Fact]
      public void can_generate_bic()
      {
         finance.Bic().Should().Be("CVQAMUB1");
      }

      [Fact]
      public void can_generate_iban()
      {
         finance.Iban().Should().Be("MT78CVQA0491707AV6092536EZ69UM5");

         finance.Iban(true).Should().Be("BH95 LCFH 2236 87QH UU47 F6");
      }

      [Fact]
      public void can_generate_iban2()
      {
         finance.Iban(countryCode: "ro").Should().Be("RO36PCVQ663E5098I56K5218");
         finance.Iban(countryCode: "sa", formatted: true).Should().Be("SA90 19G4 X820 1322 3687 QHUU");
      }

      [Fact]
      public void iban_throws_key_not_found_on_invalid_iso3166()
      {
         Action a = () => finance.Iban(countryCode: "zz");
         a.Should().Throw<KeyNotFoundException>();
      }

      [Fact]
      public void iban_thows_on_invalid_iso3166_length()
      {
         Action a = () => finance.Iban(countryCode: "fff");
         a.Should().Throw<ArgumentOutOfRangeException>();
      }

      [Fact]
      public void can_get_random_currency()
      {
         var cur = finance.Currency();

         cur.Description.Should().Be("Nepalese Rupee");
         cur.Code.Should().Be("NPR");
         cur.Symbol.Should().NotBeNullOrWhiteSpace();
      }

      [Fact]
      public void get_random_amount()
      {
         finance.Amount().Should().Be(603.52m);
      }

      [Fact]
      public void get_random_amount_with_options()
      {
         var val = finance.Amount(200, 300, 3);

         val.Should()
            .BeInRange(200, 300);

         //get decimal places.
         var decimals = BitConverter.GetBytes(decimal.GetBits(val)[3])[2];

         decimals.Should().Be(3);
      }

      [Fact]
      public void get_random_amount_with_zero_decimals()
      {
         finance.Amount(decimals: 0).Should().Be(604);
      }

      [Fact]
      public void should_be_able_to_get_a_transaction_type()
      {
         finance.TransactionType().Should().Be("payment");
      }

      [Fact]
      public void can_get_random_credit_card_number()
      {
         finance.CreditCardNumber(CardType.Switch)
            .Should().Be("6759-1860-6064-3917-52")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.AmericanExpress)
            .Should().Be("3407-908836-50694")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Instapayment)
            .Should().Be("6375-5231-6819-9268")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Maestro)
            .Should().Be("6759-9878-4250-4118")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Jcb)
            .Should().Be("3528-1242-5366-4879")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Visa)
            .Should().Be("4869-2879-7143-7822")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Mastercard)
            .Should().Be("5481-1400-9339-3651")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Solo)
            .Should().Be("6767-9010-0832-1613-169")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.DinersClub)
            .Should().Be("5474-3198-2736-8655")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Discover)
            .Should().Be("6493-6232-2435-7233-5952")
            .And.Match(f => Luhn(f));
         finance.CreditCardNumber(CardType.Laser)
            .Should().Be("6771693455045167")
            .And.Match(f => Luhn(f));

         finance.CreditCardNumber().Should().Be("3731-282228-18252")
            .And.Match(f => Luhn(f));
      }

      [Fact]
      public void can_get_random_credit_card_cvv()
      {
         finance.CreditCardCvv().Should().Be("618");
      }

      private static bool Luhn(string digits)
      {
         return digits.Where(char.IsDigit).Reverse()
                   .Select(c => c - 48)
                   .Select((thisNum, i) => i % 2 == 0
                      ? thisNum
                      : ((thisNum *= 2) > 9 ? thisNum - 9 : thisNum)
                   ).Sum() % 10 == 0;
      }
   }
}

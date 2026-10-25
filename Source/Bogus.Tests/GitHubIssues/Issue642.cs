using System.Linq;
using System.Numerics;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues;

public class Issue642 : SeededTest
{
   [Fact]
   public void can_generate_valid_turkish_iban()
   {
      // Turkish IBANs follow the SWIFT BBAN format 5!n1!n16!c:
      // 5-digit bank code, a 1-digit numeric reserve field, 16-character account number.
      // e.g. TR33 0006 1005 1978 6457 8413 26
      var finance = new Finance();

      for( var i = 0; i < 1000; i++ )
      {
         var iban = finance.Iban(countryCode: "TR");

         iban.Should().HaveLength(26);
         iban.Should().MatchRegex("^TR[0-9]{2}[0-9]{5}[0-9][0-9A-Z]{16}$");
         IsValidChecksum(iban).Should().BeTrue(because: $"{iban} should pass the ISO 13616 mod-97 check");
      }
   }

   private static bool IsValidChecksum(string iban)
   {
      var rearranged = iban.Substring(4) + iban.Substring(0, 4);
      var digits = string.Concat(rearranged.Select(c => char.IsDigit(c) ? c.ToString() : (c - 'A' + 10).ToString()));
      return BigInteger.Parse(digits) % 97 == 1;
   }
}

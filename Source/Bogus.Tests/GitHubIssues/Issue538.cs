using Bogus.DataSets;
using FluentAssertions;

namespace Bogus.Tests.GitHubIssues;

public class Issue538 : SeededTest
{
    [Fact]
    public void can_generate_valid_costa_rican_iban()
    {
        // Costa Rican IBANs should match the following format:
        // https://bank-code.net/iban/structure/costa-rica-international-bank-account-number
        // e.g. CR05 0152 0200 1026 2840 66
        var finance = new Finance();
        var iban = finance.Iban(countryCode: "CR");

        var countryCode = iban.Substring(0, 2);
        var digitsAfterCountryCode = iban.Substring(2, 20);

        iban.Should().HaveLength(22);
        countryCode.Should().Be("CR");
        digitsAfterCountryCode.Should().MatchRegex("^[0-9]{20}$");
    }
}
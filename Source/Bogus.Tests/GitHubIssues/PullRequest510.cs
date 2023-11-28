using Bogus.DataSets;
using Bogus.Extensions.UnitedKingdom;

using FluentAssertions;

using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues;

public class PullRequest510
{
   private readonly ITestOutputHelper console;

   public PullRequest510(ITestOutputHelper console)
   {
      this.console = console;
   }

   [Fact]
   public void can_generate_formatted_government_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.GovernmentDepartment);

      Assert(ep, @"GB GD ([01][0-9][0-9]|4[0-9][0-9]|[0-9])");
   }

   [Fact]
   public void can_generate_unformatted_government_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.GovernmentDepartment, includeSeparator: false);

      Assert(ep, @"GBGD([01][0-9][0-9]|4[0-9][0-9]|[0-9])");
   }

   [Fact]
   public void can_generate_formatted_health_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.HealthAuthority);

      Assert(ep, @"GB HA ([5][0-9][0-9]|9[0-9][0-9]|[0-9])");
   }

   [Fact]
   public void can_generate_unformatted_health_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.HealthAuthority, includeSeparator: false);

      Assert(ep, @"GBHA([5][0-9][0-9]|9[0-9][0-9]|[0-9])");
   }
   
   [Fact]
   public void can_generate_unformatted_standard_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.Standard, includeSeparator: false);

      Assert(ep, @"GB([0-9]){9}");
   }

   [Fact]
   public void can_generate_formatted_standard_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.Standard, includeSeparator: true);

      Assert(ep, @"GB ([0-9]){3} ([0-9]){4} ([0-9]){2}");
   }

   [Fact]
   public void can_generate_unformatted_branchTrader_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.BranchTrader, includeSeparator: false);

      Assert(ep, @"GB([0-9]){12}");
   }

   [Fact]
   public void can_generate_formatted_branchTrader_vat_number()
   {
      Finance i = new Finance();

      string ep = i.VatNumber(VatRegistrationNumberType.BranchTrader, includeSeparator: true);

      Assert(ep, @"GB ([0-9]){3} ([0-9]){4} ([0-9]){2} ([0-9]){3}");
   }

   private void Assert(string ep, string regex)
   {
      this.console.WriteLine($"Generated {ep}");

      ep.ToString().Should().MatchRegex(regex);
   }
}
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class PullRequest197 : SeededTest
   {
      public class Internet2 : Internet
      {
         public Name DependentDataSet => this.Name;
      }

      [Fact]
      public void ensure_randomizer_propagates_to_dependent_datasets()
      {
         var internet = new Internet2();

         var internetRandomizer = internet.Random;

         var dependentRandomizer = internet.DependentDataSet.Random;

         dependentRandomizer.Should().BeSameAs(internetRandomizer);
      }
   }
}
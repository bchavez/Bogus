using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue49 : SeededTest
   {
      [Fact]
      public void issue_49_pr_51_pick_random_subset()
      {
         var items = Enumerable.Range(1, 10).ToArray();

         var f = new Faker();

         Action bounds1 = () => { f.PickRandom(items, 25).ToList(); };

         bounds1.Should().Throw<ArgumentOutOfRangeException>();

         Action bounds2 = () => { f.PickRandom(items, -1).ToList(); };

         bounds2.Should().Throw<ArgumentOutOfRangeException>();

         var picked = f.PickRandom(items, 4).ToArray();
         picked.Dump();
         picked.Should().Equal(7, 2, 9, 8);
      }
   }
}

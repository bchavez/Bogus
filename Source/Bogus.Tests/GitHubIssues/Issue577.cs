using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Bogus.Tests.GitHubIssues;

public class Issue577 : SeededTest
{
   [Fact]
   public void issue_577()
   {
      var items = Enumerable.Range(1, 10).ToArray();

      var f = new Faker();

      // The minimum number is more than the number of items in the list.
      Action minimum_bounds = () => f.PickRandom(items, 15, 25).ToList();
      minimum_bounds.Should().Throw<ArgumentOutOfRangeException>();

      // The maximum number is less than zero.
      Action maximum_bounds = () => f.PickRandom(items, 2, -1).ToList();
      maximum_bounds.Should().Throw<ArgumentOutOfRangeException>();

      // Should return an empty list.
      var pickedEmpty = f.PickRandom(items, 0, 0);
      pickedEmpty.Dump();
      pickedEmpty.Should().BeEmpty();

      // Should return NULL.
      var pickedNull = f.PickRandom(items, 0, 0, true);
      pickedNull.Dump();
      pickedNull.Should().BeNull();

      // Should return a list with some items.
      var picked = f.PickRandom(items, 2, 5);
      picked.Dump();
      picked.Should().Equal(7, 2, 1, 4, 9);
   }
}

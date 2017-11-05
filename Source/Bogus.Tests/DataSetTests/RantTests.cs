using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class RantTests : SeededTest
   {
      public RantTests()
      {
         rant = new DataSets.Rant();
      }

      private readonly DataSets.Rant rant;

      [Fact]
      public void can_get_an_array_of_reviews()
      {
         var reviews = rant.Reviews("foobar", 3);

         var truth = new[]
            {
               "one of my hobbies is poetry. and when i'm writing poems this works great.",
               "I tried to annihilate it but got bonbon all over it.",
               "My co-worker Merwin has one of these. He says it looks bubbly."
            };
         //reviews.Length.Should().Be(3);
         reviews.Should().BeEquivalentTo(truth);
      }

      [Fact]
      public void can_get_random_product_review()
      {
         rant.Review("foobar").Should().Be("one of my hobbies is poetry. and when i'm writing poems this works great.");
      }
   }
}
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class CommerceTest : SeededTest
   {
      public CommerceTest()
      {
         commerce = new Commerce();
      }

      private readonly Commerce commerce;

      [Fact]
      public void can_get_a_color()
      {
         commerce.Color().Should().Be("plum");
      }

      [Fact]
      public void can_get_a_product()
      {
         commerce.Product().Should().Be("Soap");
      }

      [Fact]
      public void can_get_a_product_adj()
      {
         commerce.ProductAdjective().Should().Be("Generic");
      }

      [Fact]
      public void can_get_a_product_material()
      {
         commerce.ProductMaterial().Should().Be("Rubber");
      }

      [Fact]
      public void can_get_a_product_name()
      {
         commerce.ProductName().Should().Be("Generic Wooden Bacon");
      }

      [Fact]
      public void can_get_categories()
      {
         commerce.Categories(3).ShouldBeEquivalentTo(
            new[] {"Kids", "Music", "Jewelery"});
      }

      [Fact]
      public void can_get_list_of_departments()
      {
         commerce.Department(5).Should().Be("Music, Jewelery, Baby & Books");
      }

      [Fact]
      public void can_get_price()
      {
         commerce.Price(symbol: "$").Dump();
      }
   }
}
using Bogus.DataSets;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{

    [TestFixture]
    public class CommerceTest: SeededTest
    {
        private Commerce commerce;

        [SetUp]
        public void BeforeEachTest()
        {
            this.commerce = new Commerce();
        }

        [Test]
        public void can_get_a_product()
        {
            commerce.Product().Should().Be( "Soap" );
        }

        [Test]
        public void can_get_a_product_adj()
        {
            commerce.ProductAdjective().Should().Be("Generic");
        }

        [Test]
        public void can_get_a_product_material()
        {
            commerce.ProductMaterial().Should().Be( "Rubber" );
        }

        [Test]
        public void can_get_a_color()
        {
            commerce.Color().Should().Be( "plum" );
        }

        [Test]
        public void can_get_a_product_name()
        {
            commerce.ProductName().Should().Be("Generic Wooden Bacon");
        }

        [Test]
        public void can_get_categories()
        {
            commerce.Categories( 3 ).ShouldBeEquivalentTo(
                new[] {"Kids", "Music", "Jewelery"} );
        }

        [Test]
        public void can_get_price()
        {
            commerce.Price(symbol:"$").Dump();
        }

        [Test]
        public void can_get_list_of_departments()
        {
            commerce.Department(5).Should().Be("Music, Jewelery, Baby & Books");
        }
    }

}
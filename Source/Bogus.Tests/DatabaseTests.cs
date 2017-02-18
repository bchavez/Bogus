using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{

    [TestFixture]
    public class DatabaseTests : SeededTest
    {
        private DataSets.Database database;

        [SetUp]
        public void BeforeEachTest()
        {
            database = new DataSets.Database();
        }

        [Test]
        public void Test()
        {
            
        }

        [Test]
        public void can_generate_a_column_name()
        {
            this.database.Column().Should().Be("password");
        }

        [Test]
        public void can_generate_a_type()
        {
            this.database.Type().Should().Be("real");
        }

        [Test]
        public void can_generate_collation()
        {
            this.database.Collation().Should().Be("ascii_general_ci");
        }

        [Test]
        public void can_generate_engine()
        {
            this.database.Engine().Should().Be("CSV");
        }
    }

}
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests
{
   public class DatabaseTests : SeededTest
   {
      public DatabaseTests()
      {
         database = new DataSets.Database();
      }

      private readonly DataSets.Database database;

      [Fact]
      public void can_generate_a_column_name()
      {
         database.Column().Should().Be("password");
      }

      [Fact]
      public void can_generate_a_type()
      {
         database.Type().Should().Be("real");
      }

      [Fact]
      public void can_generate_collation()
      {
         database.Collation().Should().Be("ascii_general_ci");
      }

      [Fact]
      public void can_generate_engine()
      {
         database.Engine().Should().Be("CSV");
      }
   }
}
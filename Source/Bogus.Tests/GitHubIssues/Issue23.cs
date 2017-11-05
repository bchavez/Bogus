using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue23 : SeededTest
   {
      public class TestClass
      {
         public string Value { get; set; }
      }

      [Fact]
      public void issue_23_should_be_able_to_generate_random_word_without_exception()
      {
         var faker = new Faker<TestClass>();
         faker.RuleFor(x => x.Value, faker1 => faker1.Random.Word());
         foreach( var item in faker.Generate(1000) )
         {
            item.Value.Should().NotBeNullOrWhiteSpace();
         }
      }
   }
}
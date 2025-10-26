using FluentAssertions;
using Xunit;

namespace Bogus.Tests;

public class RecordTest
{
   private record User(string FirstName, string Email, string LastName);

   [Fact]
   public void Can_generate_a_record()
   {
      var faker = new Faker<User>()
         .RuleFor(d => d.FirstName, f => f.Person.FirstName)
         .RuleFor(d => d.LastName, _ => "x");

      var user = faker.Generate();
      user.FirstName.Should().NotBeNull();
      user.LastName.Should().Be("x");
      user.Email.Should().BeNull();
   }
}
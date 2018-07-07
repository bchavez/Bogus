using System;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue121 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue121(ITestOutputHelper console)
      {
         this.console = console;
      }
      public class TestObject
      {
         public Guid Id { get; set; }
         public string Name { get; set; }
      }

      [Fact(Skip = "Their example")]
      public void Test()
      {
         var faker = new Faker<TestObject>()
            .StrictMode(true)
            .RuleFor(x => x.Id, Guid.NewGuid)
            .RuleFor(x => x.Name, x => x.Person.FirstName)
            .RuleSet("update", f => f.Ignore(x => x.Id));

         var obj = faker.Generate();

         var id = obj.Id; // value copy
         var name = obj.Name; // value copy

         faker.Populate(obj, "default,update");

         Assert.True(id == obj.Id); // fails
         Assert.False(name == obj.Name);
      }

      [Fact]
      public void github_issue121_workaround()
      {
         var faker = new Faker<TestObject>()
            .StrictMode(true)
            .RuleFor(x => x.Id, x => Guid.NewGuid())
            .RuleFor(x => x.Name, x => x.Person.FirstName);

         var updateFaker = faker.Clone()
            .Ignore(x => x.Id);

         var obj = faker.Generate();

         var id = obj.Id; // value copy
         var name = obj.Name; // value copy

         updateFaker.Populate(obj);

         console.Dump($"Copy values - id: {id}, name: {name}");
         console.Dump(obj);

         id.Should().Be(obj.Id);
         name.Should().NotBe(obj.Name);
      }

      [Fact]
      public void last_call_ignoring_a_prop_should_be_ignored()
      {
         var faker = new Faker<TestObject>()
            .RuleFor(x => x.Name, x => x.Person.FirstName)
            .Ignore(x => x.Name);

         //who wins? last call to ignore should win.

         var obj = faker.Generate();

         obj.Id.Should().BeEmpty();
         obj.Name.Should().BeNull();
      }

   }
}
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   //https://github.com/bchavez/Bogus/issues/93
   public class Issue93 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue93(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void should_be_able_to_use_list_of_objects_and_pick_random()
      {
         var myObjFaker = new Faker<MyObj>()
            .RuleFor(m => m.SomeString, f => f.Lorem.Sentence());

         var objects = myObjFaker.Generate(20);

         var faker = new Faker();
         var picked = faker.PickRandom(objects);

         console.WriteLine(picked.DumpString());
         picked.GetType().Should().Be<MyObj>();
      }

      class MyObj
      {
         public string SomeString { get; set; }
      }
   }
}
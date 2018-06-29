using System.Linq;
using FluentAssertions;
using Xunit;
using Z.ExtensionMethods;
using Z.ExtensionMethods.ObjectExtensions;

namespace Bogus.Tests
{
   public class UniquenessTests : SeededTest
   {
      public class User
      {
         public string FirstName { get; set; }
         public string LastName { get; set; }
         public string Email { get; set; }
         public string Username { get; set; }
      }

      public UniquenessTests()
      {
         Faker.GlobalUniqueIndex = -1;
      }

      [Fact]
      public void every_new_generation_should_have_a_new_unique_index()
      {
         var faker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Username, f => f.IndexGlobal + f.Person.UserName);

         var fakes = faker.Generate(10);

         fakes.Dump();

         faker.FakerHub.IndexGlobal.Should().Be(9);

         var values = fakes
            .Select(u => u.Username.Left(1).ToInt32())
            .ToArray();

         values.Should().BeEquivalentTo(0, 1, 2, 3, 4, 5, 6, 7, 8, 9);

         var morefakes = faker.Generate(3);

         morefakes.Dump();

         faker.FakerHub.IndexGlobal.Should().Be(12);
      }

      public class Video
      {
         public int Index { get; set; }
         public string VideoId { get; set; }
         public string Summary { get; set; }
      }

      [Fact]
      public void should_be_able_to_create_some_hash_ids()
      {
         var faker = new Faker<Video>()
            .RuleFor(v => v.Index, f => f.IndexGlobal)
            .RuleFor(v => v.VideoId, f => f.Hashids.EncodeLong(f.IndexGlobal))
            .RuleFor(v => v.Summary, f => f.Lorem.Sentence());

         var fakes = faker.Generate(5);

         fakes.Dump();

         var ids = fakes.Select(v => v.VideoId).ToArray();

         ids.Should().BeEquivalentTo("gY", "jR", "k5", "l5", "mO");
      }

      [Fact]
      public void should_be_able_to_drive_manual_index()
      {
         int indexer = 0;
         var faker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => new[] {"A", "B", "C", "D"}[indexer % 4])
            .FinishWith((f, u) => indexer++);

         var fakes = faker.Generate(10);

         fakes.Dump();
      }


      [Fact]
      public void should_be_able_to_drive_internal_index()
      {
         var faker = new Faker<User>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => new[] {"A", "B", "C", "D"}[f.IndexFaker % 4]);
         var fakes = faker.Generate(5);

         fakes.Dump();

         fakes.Select(f => f.LastName).ToList().Should().Equal("A", "B", "C", "D", "A");
      }

      [Fact]
      public void issue_57_unique_index_not_really_unique_in_parent_child_generation()
      {
         var childFaker = new Faker<Issue57Child>()
            .RuleFor(u => u.Id, f => f.IndexGlobal);

         var parentFaker = new Faker<Issue57Parent>()
            .RuleFor(u => u.Id, f => f.IndexGlobal)
            .RuleFor(u => u.Child, f => childFaker.Generate());

         var ids = parentFaker.Generate(3).Select(o => new {o.Id, CId = o.Child.Id});

         var allIds = ids.SelectMany(x => new[] {x.Id, x.CId}).ToList();

         ids.Dump();
         allIds.Dump();

         allIds.Distinct().Count().Should().Be(6);
      }

      [Fact]
      public void issue_57_reordering_rules_shouldnt_matter()
      {
         var childFaker = new Faker<Issue57Child>()
            .RuleFor(u => u.Id, f => f.IndexGlobal);

         var parentFaker = new Faker<Issue57Parent>()
            .RuleFor(u => u.Child, f => childFaker.Generate())
            .RuleFor(u => u.Id, f => f.IndexGlobal);

         var ids = parentFaker.Generate(3).Select(o => new {o.Id, CId = o.Child.Id});

         var allIds = ids.SelectMany(x => new[] {x.Id, x.CId}).ToList();

         ids.Dump();
         allIds.Dump();

         allIds.Distinct().Count().Should().Be(6);
      }

      public class Issue57Parent
      {
         public int Id { get; set; }
         public Issue57Child Child { get; set; }
      }

      public class Issue57Child
      {
         public int Id { get; set; }
      }

      [Fact]
      public void should_be_able_to_control_index_variable()
      {
         var childFaker = new Faker<Issue57Child>()
            .RuleFor(u => u.Id, f => f.IndexVariable++ + 50);


         var parentFaker = new Faker<Issue57Parent>()
            .RuleFor(u => u.Child, f => childFaker.Generate())
            .RuleFor(u => u.Id, f => f.IndexVariable++);

         var ids = parentFaker.Generate(3).Select(o => new {o.Id, CId = o.Child.Id});

         var allIds = ids.SelectMany(x => new[] {x.Id, x.CId}).ToList();

         ids.Dump();
         allIds.Dump();

         allIds.Distinct().Count().Should().Be(6);
      }
   }
}
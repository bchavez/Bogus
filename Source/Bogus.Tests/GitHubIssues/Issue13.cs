using System;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue13 : SeededTest
   {
      public class ReadOnly
      {
         public string Name;

         public string NameReadOnly => Name;
      }

      [Fact]
      public void issue_13_readonly_property()
      {
         var faker = new Faker<ReadOnly>()
            .StrictMode(true)
            .RuleFor(ro => ro.Name, f => f.Name.FirstName());

         faker.Validate().Should().BeTrue();
         faker.TypeProperties.Count.Should().Be(1);
      }

      [Fact]
      public void issue_13_with_model()
      {
         var counter = 0;

         var faker = new Faker<TestObject>()
            .StrictMode(true)
            .RuleFor(c => c.SomeOtherId, f => counter++)
            .RuleFor(c => c.SomeId, f => Guid.NewGuid())
            .RuleFor(c => c.SomeFutureDate, f => f.Date.Future())
            .RuleFor(c => c.SomePastDate, (f, b) => b.SomeFutureDate.AddHours(f.Random.Number(1, 24)))
            .RuleFor(c => c.SomeStatusInt, (f, b) => (int)b.SomeExplicitInt)
            .RuleFor(c => c.SomeExplicitInt, f => 2)
            .RuleFor(c => c.SomeBool3, f => f.Random.Bool())
            .RuleFor(c => c.SomeBool2, f => f.Random.Bool())
            .RuleFor(c => c.SomeBool1, f => f.Random.Bool())
            .RuleFor(c => c.SomeOtherInt, f => f.Random.Number(1, 5))
            .RuleFor(c => c.SomeInt, f => 0)
            .RuleFor(c => c.SomeOtherString, f => null)
            .RuleFor(c => c.SomeOtherGuid, f => Guid.NewGuid())
            .RuleFor(c => c.SomeString, f => null)
            .RuleFor(c => c.SomeComment, f => f.Lorem.Sentence())
            .RuleFor(c => c.SomeGuid, f => null)
            .RuleFor(c => c.SomeTimestamp, f => null);

         faker.TypeProperties.Count.Should().Be(17);

         var fake = faker.Generate();
         fake.Dump();
      }

      public class TestObject
      {
         private DateTime? _lastTimeToUnbook;

         public int SomeOtherId { get; set; }

         public Guid SomeId { get; set; }

         public DateTime SomeFutureDate { get; set; }

         public DateTime SomePastDate { get; set; }

         public int SomeStatusInt { get; set; }

         public int SomeExplicitInt
         {
            get { return this.SomeStatusInt; }
            set { this.SomeStatusInt = value; }
         }

         public bool SomeBool3 { get; set; }

         public bool SomeBool2 { get; set; }

         public bool SomeBool1 { get; set; }

         public int SomeOtherInt { get; set; }

         public DateTime? ReadOnlyDateTime
         {
            get { return _lastTimeToUnbook; }
         }

         public int SomeInt { get; set; }

         public string SomeOtherString { get; set; }

         public Guid SomeOtherGuid { get; set; }

         public string SomeString { get; set; }

         public bool Someboolean
         {
            get { return !this.SomeTimestamp.HasValue; }
         }

         public DateTime? SomeTimestamp { get; set; }

         public Guid? SomeGuid { get; set; }

         public string SomeComment { get; set; }
      }
   }
}
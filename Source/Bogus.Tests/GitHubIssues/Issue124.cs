using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Bogus.Extensions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue124 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue124(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void test_nullable()
      {
         var faker = new Faker<Person>()
            .RuleFor(p => p.Guid, f => f.Random.NullableUuid())
            .RuleFor(p => p.Name, f => f.Person.FullName);

         var fakes = faker.Generate(10);

         fakes.Should()
            .Contain(p => p.Guid == null)
            .And
            .Contain(p => p.Guid != null);
      }

      [Fact]
      public void test_nullable_struct()
      {
         var faker = new Faker<Person>()
            .RuleFor(p => p.Guid, f => f.Random.Uuid().OrNull(f))
            .RuleFor(p => p.Name, f => f.Person.FullName);

         var fakes = faker.Generate(10);

         console.Dump(fakes);

         fakes.Should()
            .Contain(p => p.Guid == null)
            .And
            .Contain(p => p.Guid != null);
      }

      [Fact]
      public void test_null_reference_type()
      {
         var faker = new Faker<Foo>()
            .RuleFor(x => x.Id, f => f.Random.Uuid())
            .RuleFor(x => x.Bar, f => new object().OrNull(f));

         var fakes = faker.Generate(10);

         console.Dump(fakes);

         fakes.Should()
            .Contain(f => f.Bar == null)
            .And
            .Contain(f => f.Bar != null);
      }

      [Fact]
      public void test_null_reference_type_between_fakers()
      {
         var personFaker = new Faker<Person>()
            .RuleFor(p => p.Guid, f => f.Random.Uuid().OrNull(f))
            .RuleFor(p => p.Name, f => f.Person.FullName);

         var barFaker = new Faker<Bar>()
            .RuleFor(x => x.Id, f => f.Random.Uuid())
            .RuleFor(x => x.Person, f => personFaker.Generate().OrNull(f) );

         var fakes = barFaker.Generate(20);
         console.Dump(fakes);

         fakes.Should()
            .Contain(f => f.Person == null)
            .And
            .Contain(f => f.Person != null)
            .And
            .Contain(f => f.Person != null && f.Person.Guid == null)
            .And
            .Contain(f => f.Person != null && f.Person.Guid != null);
      }

      [Fact]
      public void test_deterministic_ornull()
      {
         var faker = new Faker<Foo>()
            .RuleFor(x => x.Id, f => f.Random.Uuid())
            .RuleFor(x => x.Bar, f => new object().OrNull(f));

         var fakes = faker.Generate(10);

         console.Dump(fakes);

         var bars = fakes.Select(f => f.Bar).ToArray();

         var notNullObjects = bars.Where(b => b != null).ToArray();

         bars.Should()
            .ContainInOrder(
               null,
               notNullObjects[0],
               notNullObjects[1],
               null,
               notNullObjects[2],
               null,
               null,
               notNullObjects[3],
               notNullObjects[4],
               null);
      }

      public class Foo
      {
         public Guid Id { get; set; }
         public object Bar { get; set; }
      }

      public class Bar
      {
         public Guid Id { get; set; }
         public Person Person { get; set; }
      }
   }

   public class Person
   {
      public Guid? Guid { get; set; }
      public string Name { get; set; }
   }

   public static class ObjectExtensions
   {
      public static Guid? NullableUuid(this Randomizer r)
      {
         return r.Bool() ? r.Uuid() : (Guid?)null;
      }
   }
}
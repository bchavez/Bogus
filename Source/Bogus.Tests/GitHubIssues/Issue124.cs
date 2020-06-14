using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Bogus.Extensions;
using Z.ExtensionMethods.ObjectExtensions;

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
      public void test_deterministic_or_null()
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
               notNullObjects[0],
               null,
               null,
               notNullObjects[1],
               null,
               notNullObjects[2],
               notNullObjects[3],
               null,
               null,
               notNullObjects[4]
               );
      }

      [Fact]
      public void nullable_int_and_nullable_reference_type()
      {
         var faker = new Faker<Qux>()
            .RuleFor(x => x.Id, f => f.Random.Int().OrNull(f))
            .RuleFor(x => x.Gud, f => f.Random.Guid().OrNull(f, .8f))
            .RuleFor(x => x.Obj, f => new object().OrNull(f))
            .RuleFor(x => x.Str, f => f.Random.Word().OrNull(f));

         var q = faker.Generate(5);

         console.Dump(q);

         q[0].Id.Should().NotBeNull();
         q[0].Gud.Should().NotBeNull();
         q[0].Obj.Should().NotBeNull();
         q[0].Str.Should().NotBeNull();

         q[1].Id.Should().NotBeNull();
         q[1].Gud.Should().NotBeNull();
         q[1].Obj.Should().NotBeNull();
         q[1].Str.Should().BeNull();

         q[2].Id.Should().BeNull();
         q[2].Gud.Should().BeNull();
         q[2].Obj.Should().NotBeNull();
         q[2].Str.Should().NotBeNull();

         q[3].Id.Should().BeNull();
         q[3].Gud.Should().NotBeNull();
         q[3].Obj.Should().BeNull();
         q[3].Str.Should().NotBeNull();

         q[4].Id.Should().BeNull();
         q[4].Gud.Should().NotBeNull();
         q[4].Obj.Should().NotBeNull();
         q[4].Str.Should().BeNull();
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

      public class Qux
      {
         public int? Id { get; set; }
         public Guid? Gud { get; set; }
         public object Obj { get; set; }
         public string Str { get; set; }
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
using System;
using Xunit;
using Xunit.Abstractions;

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
      }

      [Fact]
      public void test_nullable2()
      {
         var faker = new Faker<Person>()
            .RuleFor(p => p.Guid, f => f.Random.Uuid().OrNull())
            .RuleFor(p => p.Name, f => f.Person.FullName);
      }
   }

   public class Person
   {
      public Guid? Guid { get; set; }
      public string Name { get; set; }
   }

   public static class ObjectExtensions
   {
      public static T? OrNull<T>(this T? value)
         where T : struct
      {
         return value.GetHashCode() % 2 == 0 ? value : null;
      }

      public static T? OrNull<T>(this T value)
         where T : struct
      {
         return value.GetHashCode() % 2 == 0 ? (T?)value : null;
      }

      public static Guid? NullableUuid(this Randomizer r)
      {
         return r.Bool() ? r.Uuid() : (Guid?)null;
      }
   }


}
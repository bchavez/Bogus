using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue309 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue309(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void ruleforlist_addrange()
      {
         var hobbyFaker = new Faker<Hobby>()
            .RuleFor(h => h.Id, f => f.IndexFaker)
            .RuleFor(h => h.Text, f => f.Random.Word());

         var userFaker = new Faker<User>()
            .RuleFor(u => u.Id, f => f.IndexFaker)
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleForList(u => u.Hobbies, f => hobbyFaker.Generate(3));

         var user = userFaker.Generate(1);
         user.Should().NotBeNull();
         console.Dump(user);
      }

      public class Hobby
      {
         public int Id { get; set; }
         public string Text { get; set; }
      }
      public class User
      {
         public int Id { get; set; }
         public string Name { get; set; }
         public List<Hobby> Hobbies { get; } = new List<Hobby>();
      }
   }

   public static class ExtensionsForIssue309
   {
      public static Faker<T> RuleForList<T, U>(this Faker<T> fakerT, Expression<Func<T, List<U>>> propertyOfListU, Func<Faker, List<U>> itemsGetter) 
         where T : class
      {
         var func = propertyOfListU.Compile();

         fakerT.RuleFor(propertyOfListU, (f, t) =>
            {

               var list = func(t);
               var items = itemsGetter(f);

               list.AddRange(items);

               return items;
            });

         return fakerT;
      }
   }
}
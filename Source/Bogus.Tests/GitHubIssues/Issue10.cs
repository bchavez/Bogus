using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue10 : SeededTest
   {
      public class Bar
      {
         public int Id { get; set; }
         public string Name;
         public string Email { get; set; }
         internal string LastName;
      }

      [Fact]
      public void issue_10_should_be_able_to_fake_fields()
      {
         var faker = new Faker<Bar>()
            .RuleFor(b => b.Email, f => f.Internet.Email())
            .RuleFor(b => b.Name, f => f.Name.FirstName())
            .RuleFor(b => b.LastName, f => f.Name.LastName());

         var bar = faker.Generate();

         bar.Dump();
         bar.Name.Should().NotBeNullOrEmpty();
         bar.Email.Length.Should().BeGreaterOrEqualTo(2);
         bar.Email.Should().NotBeNullOrEmpty();
         bar.Email.Length.Should().BeGreaterOrEqualTo(2);

         bar.LastName.Should().NotBeNullOrEmpty();
         bar.LastName.Length.Should().BeGreaterOrEqualTo(2);
      }


      [Fact]
      public void issue_12_bogus_should_be_thread_safe()
      {
         int threadCount = 20;

         var barId = 0;
         var faker = new Faker<Bar>()
            .RuleFor(b => b.Id, f => barId++)
            .RuleFor(b => b.Email, f => f.Internet.Email())
            .RuleFor(b => b.Name, f => f.Name.FirstName())
            .RuleFor(b => b.LastName, f => f.Name.LastName());

         var threads = new List<Task>();
         for( var x = 0; x < threadCount; x++ )
         {
            var thread = Task.Run(() =>
               {
                  var fakes = faker.Generate(3);
                  fakes.Dump();
               });
            threads.Add(thread);
         }

         Task.WaitAll(threads.ToArray());

         Console.WriteLine(barId);
         barId.Should().Be(60);

         var result = Parallel.For(0, threadCount, i =>
            {
               var fakes = faker.Generate(3);
            });

         Console.WriteLine(barId);
         barId.Should().Be(120);
      }
   }
}
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue48 : SeededTest
   {
      [Fact]
      public void issue_48()
      {
         Faker<Client> clients = new Faker<Client>()
            .RuleFor(x => x.Description, y => y.Lorem.Paragraphs(1));
         var users = new List<User>();

         users.AddRange(
            new Faker<User>()
               .RuleFor(x => x.Name, y =>
                  {
                     var i = y.IndexFaker;
                     i.Dump();
                     return new[] {"John", "Mary", "Mike", "Tom"}[i % 4];
                  })
               .RuleFor(x => x.Email, (y, x) => $"{x.Name}@xyz.com".Replace(" ", "").ToLower())
               .RuleFor(x => x.Client, y => y.Random.Bool() ? clients.Generate(1).First() : null)
               .RuleFor(x => x.UserName, (y, x) => x.Email)
               .Generate(4)
         );

         users.Dump();


         users.Select(f => f.Name).ToList().Should().Equal("John", "Mary", "Mike", "Tom");
      }

      public class User
      {
         public string Name { get; set; }
         public string Email { get; set; }
         public Client Client { get; set; }
         public string UserName { get; set; }
      }

      public class Client
      {
         public string Description { get; set; }
      }
   }
}
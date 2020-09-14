using System;
using Xunit;
using Bogus.Extensions;
using Xunit.Abstractions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue321 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue321(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void reuse_data_from_custom_instantiator()
      {
         var unions = new[]
            {
               new Union("Married"),
               new Union("Single"),
               new Union("Divorced"),
            };

         var memberFaker = new Faker<Member>()
            .CustomInstantiator(f =>
               {
                  //Store intermediate state here.
                  var email = f.Internet.ExampleEmail();
                  var selectedUnion = f.PickRandom(unions);

                  return new Member(
                     f.Random.Guid().ToString("N"),
                     f.Name.FullName(),
                     f.Date.Between(new DateTime(1950, 3, 9), new DateTime(2010, 4, 2)),
                     f.Address.FullAddress(),
                     f.Phone.PhoneNumber(),
                     email,
                     CreateAppUser(email).Id,
                     selectedUnion,
                     selectedUnion.Id,
                     f.Random.ReplaceNumbers("######"),
                     f.Rant.Random.Words(5)
                  );
               });

         var testMembers = memberFaker.GenerateBetween(4, 10);
         console.Dump(testMembers);
      }

      private AppUser CreateAppUser(string email)
      {
         return new AppUser(email);
      }

      public class AppUser
      {
         public AppUser(string email)
         {
            this.Email = email;
            this.Id = $"appuser_id:{this.Email.ToLower()}";
         }

         public string Email { get; set; }

         public string Id { get; set; }
      }

      public class Union
      {
         public Union(string description)
         {
            this.Description = description;
            this.Id = $"union_id:{this.Description.ToLower()}";
         }

         public string Description { get; set; }

         public string Id { get; set; }
      }

      public class Member
      {
         public string Id { get; }
         public string Name { get; }
         public DateTime Dob { get; }
         public string Address { get; }
         public string Phone { get; }
         public string Email { get; }
         public string AppUserId { get; }
         public Union Union { get; }
         public string UnionId { get; }
         public string Code { get; }
         public string Description { get; }

         public Member(
            string id, 
            string name, 
            DateTime dob, 
            string address,
            string phone, 
            string email, 
            string appUserId, 
            Union union,
            string unionId,
            string code, 
            string description)
         {
            this.Id = id;
            this.Name = name;
            this.Dob = dob;
            this.Address = address;
            this.Phone = phone;
            this.Email = email;
            this.AppUserId = appUserId;
            this.Union = union;
            this.UnionId = unionId;
            this.Code = code;
            this.Description = description;
         }
      }
   }
}
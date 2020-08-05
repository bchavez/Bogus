using System;
using System.IO;
using Bogus.Bson;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue315 : SeededTest
   {
      [Fact]
      public void can_create_new_locale_at_runtime()
      {
         var customJsonLocale = @"
{
  name:{
      male_first_name: ['bob', 'bill'],
      female_first_name: ['marry', 'kate'],
      first_name:['sam', 'karla'],
      last_name:['morris', 'adams']
  }
}";
         var bsonBytes = BsonJsonConverter.ToBson(customJsonLocale);
         var bson = Bogus.Bson.Bson.Load(bsonBytes);
         bson.Should().BeOfType<BObject>();

         Database.Data.Value.TryAdd("my_custom_locale", bson);

         Action act = () => new Faker("my_custom_locale");

         act.Should().NotThrow();

         //test faker facade
         var f = new Faker("my_custom_locale");
         f.Name.LastName().Should().ContainAny("morris", "adams");
         f.Name.FirstName().Should().ContainAny("bob", "bill", "marry", "kate", "sam", "karla");
         
         //test dependent dataset
         f.Company.CompanyName().Should().Be("adams, morris and adams");

         //test fallback
         f.Finance.AccountName().Should().Be("Checking Account");

         //test faker<t>
         var userFaker = new Faker<User>("my_custom_locale")
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Age, f => f.Random.Number(18, 38));

         var fakeUser = userFaker.Generate();
         fakeUser.Name.Should().Be("karla morris");
         fakeUser.Age.Should().Be(24);
      }

      private class User
      {
         public string Name { get; set; }
         public int Age { get; set; }
      }

      private static class BsonJsonConverter
      {
         public static byte[] ToBson(string json)
         {
            using (MemoryStream ms = new MemoryStream())
            using (BsonDataWriter datawriter = new BsonDataWriter(ms))
            {
               var obj = JsonConvert.DeserializeObject(json);
               JsonSerializer serializer = new JsonSerializer();
               serializer.Serialize(datawriter, obj);
               return ms.ToArray();
            }
         }
      }
   }
}
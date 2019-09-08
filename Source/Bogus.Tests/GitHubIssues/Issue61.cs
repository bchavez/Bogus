using Xunit;
using static Bogus.Tests.GitHubIssues.Issue61.HashIdExtension;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue61 : SeededTest
   {
      public static class HashIdExtension
      {
         public static Hashids Hashid { get; set; }
      }

      [Fact]
      public void can_have_custom_hashid_extension()
      {
         //Custom
         Hashid = new Hashids(alphabet: "~!@#$%.&*()_+-=;<>");

         var faker = new Faker<SomeUser>()
            .RuleFor(o => o.Id, f => Hashid.Encode(f.Random.Digits(3)))
            .RuleFor(o => o.TrackingNumber, f => f.Random.Number(3));

         faker.Generate(5).Dump();
      }

      public class SomeUser
      {
         public string Id { get; set; }
         public int TrackingNumber { get; set; }
      }
   }
}
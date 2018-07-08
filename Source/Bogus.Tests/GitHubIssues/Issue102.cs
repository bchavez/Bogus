using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue102 : SeededTest
   {
      [Fact]
      public void deterministic_uuid_using_global_seed()
      {
         var r = new Randomizer();
         r.Uuid().Should().Be("{4c9c23da-c4e0-d72d-e3c4-a89617f255b2}");
         ResetGlobalSeed(); //should have an effect only if a new randomizer is created
         r.Uuid().Should().Be("{bd59c865-cda5-44f4-a28e-db17c014d586}");

         r = new Randomizer();
         r.Uuid().Should().Be("{4c9c23da-c4e0-d72d-e3c4-a89617f255b2}");
      }

      [Fact]
      public void deterministic_uuid_using_local_seed()
      {
         var r = new Randomizer(1337);
         r.Uuid().Should().Be("{c7f40068-5e43-aa02-c27c-4fd927fc2227}");
         ResetGlobalSeed(); //should have no effect
         r.Uuid().Should().Be("{b254896a-12e5-1eef-9af7-227ef036e328}");

         ResetGlobalSeed(); //should have no effect
         r = new Randomizer(1337);
         r.Uuid().Should().Be("{c7f40068-5e43-aa02-c27c-4fd927fc2227}");
      }
   }
}
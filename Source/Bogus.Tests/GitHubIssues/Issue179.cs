using FluentAssertions;
using Xunit;
using Xunit.Abstractions;


namespace Bogus.Tests.GitHubIssues
{
   public class Issue179 : SeededTest
   {
      private readonly ITestOutputHelper console;

      public Issue179(ITestOutputHelper console)
      {
         this.console = console;
      }

      [Fact]
      public void should_be_able_to_set_non_public_members_of_T()
      {
         var fooFaker = new Faker<Buz>()
               .RuleFor(x => x.Baz, f => f.Random.String2(10))
               .RuleFor(x => x.Bar, f => f.Random.String2(10))
               .RuleFor( x => x.Normal, f => f.Random.String2(10))
             ;

         var foos = fooFaker.Generate(10);

         foreach (var foo in foos)
         {
            console.WriteLine($"{foo.Baz} / {foo.Bar} / {foo.Normal}");
            foo.Baz.Should().NotBeNullOrWhiteSpace();
            foo.Bar.Should().NotBeNullOrWhiteSpace();
            foo.Normal.Should().NotBeNullOrWhiteSpace();
         }
      }

      [Fact]
      public void can_set_members_on_internal_class()
      {
         var boxFaker = new Faker<Box>()
            .RuleFor(x => x.Bub, f => f.Random.String2(10))
            .RuleFor(x => x.Normal, f => f.Random.String2(10))
            ;

         var boxes = boxFaker.Generate(10);

         foreach( var box in boxes )
         {
            console.WriteLine($"{box.Bub} / {box.Normal}");
            box.Bub.Should().NotBeNullOrWhiteSpace();
            box.Normal.Should().NotBeNullOrWhiteSpace();
         }
      }
   }


   public class Buz
   {
      public string Baz { get; internal set; }

      internal string Bar { get; set; }

      public string Normal { get; set; }
   }

   internal class Box
   {
      internal string Bub { get; private set; }
      public string Normal { get; set; }
   }
}
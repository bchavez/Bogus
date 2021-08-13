using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue389
   {
      [Fact]
      public void Property_with_private_setter_in_base_class_is_assigned()
      {
         var foo = new Faker<Foo>()
           .RuleFor(f => f.Bar, 42)
           .RuleFor(f => f.Baz, 123)
           .Generate();

         foo.Bar.Should().Be(42);
         foo.Baz.Should().Be(123);
      }

      public class FooBase
      {
         public int Baz { get; private set; }
      }

      public class Foo : FooBase
      {
         public int Bar { get; private set; }
      }
   }
}

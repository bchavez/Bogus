using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue389
   {
      [Fact]
      public void property_with_private_setter_in_base_class_is_assigned()
      {
         var foo = new Faker<Foo>()
           .RuleFor(f => f.PropInFoo, _ => 42)
           .RuleFor(f => f.PropInFooBase, _ => 123)
           .Generate();

         foo.PropInFoo.Should().Be(42);
         foo.PropInFooBase.Should().Be(123);
      }

      public class FooBase
      {
         public int PropInFooBase { get; private set; }
      }

      public class Foo : FooBase
      {
         public int PropInFoo { get; private set; }
      }

      public class Zoo : Foo
      {
         public int PropInZoo { get; private set; }
      }

      [Fact]
      public void property_with_private_setter_inheritance_chain_is_assigned()
      {
         var zoo = new Faker<Zoo>()
            .RuleFor(f => f.PropInFoo, _ => 42)
            .RuleFor(f => f.PropInFooBase, _ => 123)
            .RuleFor(f => f.PropInZoo, _ => 77)
            .Generate();

         zoo.PropInFoo.Should().Be(42);
         zoo.PropInFooBase.Should().Be(123);
         zoo.PropInZoo.Should().Be(77);
      }
   }
}

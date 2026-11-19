using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues;

public class Issue502
{
   public class BaseWithReadOnlyIndexers
   {
      public int BaseProp { get; private set; }
      public int this[int index] => index;
      public int this[string key] => key.Length;
   }

   public class DerivedWithReadOnlyIndexers : BaseWithReadOnlyIndexers
   {
      public int Prop { get; set; }
   }

   [Fact]
   public void object_with_readonly_overloaded_indexers_in_base_class_does_not_throw()
   {
      var faker = new Faker<DerivedWithReadOnlyIndexers>()
         .RuleFor(f => f.Prop, _ => 42)
         .RuleFor(f => f.BaseProp, _ => 123);

      var result = faker.Generate();

      result.Prop.Should().Be(42);
      result.BaseProp.Should().Be(123);
   }

   public class BaseWithPrivateSetterIndexers
   {
      public int this[int index] { get => index; private set { } }
      public int this[string key] { get => key.Length; private set { } }
   }

   public class DerivedWithPrivateSetterIndexers : BaseWithPrivateSetterIndexers
   {
      public string Name { get; set; }
   }

   [Fact]
   public void object_with_private_setter_overloaded_indexers_in_base_class_does_not_throw()
   {
      var faker = new Faker<DerivedWithPrivateSetterIndexers>()
         .RuleFor(f => f.Name, _ => "hello");

      var result = faker.Generate();

      result.Name.Should().Be("hello");
   }

   public class GrandParentWithProperty
   {
      public int Value { get; private set; }
   }

   public class ParentHidingProperty : GrandParentWithProperty
   {
      public new string Value { get; private set; }
   }

   public class ChildOfHidingParent : ParentHidingProperty
   {
      public int Id { get; set; }
   }

   [Fact]
   public void object_with_hidden_property_of_different_type_in_base_class_does_not_throw()
   {
      var faker = new Faker<ChildOfHidingParent>()
         .RuleFor(c => c.Id, _ => 1)
         .RuleFor(c => c.Value, _ => "hello");

      var result = faker.Generate();

      result.Id.Should().Be(1);
      result.Value.Should().Be("hello");
   }
}

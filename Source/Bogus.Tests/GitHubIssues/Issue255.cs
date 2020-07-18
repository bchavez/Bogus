using FluentAssertions;
using System;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue255 : SeededTest
   {
      private interface IParent
      {
         string Name { get; set; }
      }

      private interface IChild : IParent
      {
         string City { get; set; }
      }

      private class ChildWithExplicitInterface : IChild
      {
         string IParent.Name { get; set; }
         string IChild.City { get; set; }
      }

      [Fact]
      public void explicit_interface_properties_in_parent_interfaces()
      {
         var child = new Faker<IChild>()
             .CustomInstantiator(f => new ChildWithExplicitInterface())
             .RuleFor(e => e.Name, f => f.Person.FullName)
             .Generate();

         child.Name.Should().NotBeNullOrWhiteSpace();
      }

      [Fact]
      public void explicit_interface_properties_in_child_interfaces()
      {
         var child = new Faker<IChild>()
             .CustomInstantiator(f => new ChildWithExplicitInterface())
             .RuleFor(e => e.City, f => f.Address.City())
             .Generate();

         child.City.Should().NotBeNullOrWhiteSpace();
      }

      [Fact]
      public void explicit_interface_properties_in_child_interfaces_should_throw_when_strictmode_true()
      {
         var childFaker = new Faker<IChild>()
            .StrictMode(true)
            .CustomInstantiator(f => new ChildWithExplicitInterface())
            .RuleFor(e => e.City, f => f.Address.City());

         Action act = () => childFaker.AssertConfigurationIsValid();
         act.Should().Throw<ValidationException>();
      }

      [Fact]
      public void explicit_interface_properties_in_child_interfaces_should_throw_when_strictmode_true2()
      {
         var childFaker = new Faker<IChild>()
            .StrictMode(true)
            .CustomInstantiator(f => new ChildWithExplicitInterface())
            .RuleFor(e => e.Name, f => f.Address.City());

         Action act = () => childFaker.AssertConfigurationIsValid();
         act.Should().Throw<ValidationException>();
      }

      public class ChildWithNormalInterface : IChild
      {
         public string Name { get; set; }
         public string City { get; set; }
      }

      [Fact]
      public void regular_interface_properties_in_parent()
      {
         var child = new Faker<IChild>()
            .StrictMode(true)
            .CustomInstantiator(f => new ChildWithNormalInterface())
            .RuleFor(e => e.City, f => f.Address.City())
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .Generate();

         child.City.Should().NotBeNullOrWhiteSpace();
         child.Name.Should().Be("Lupe");
      }

      private interface IParent2
      {
         string Name2 { get; set; }
      }

      public class ChildWithMixedInterface : IChild, IParent2
      {
         public string Name { get; set; }
         string IParent2.Name2 { get; set; }
         string IChild.City { get; set; }
      }

      [Fact]
      public void strictmode_only_sees_ichild()
      {
         var child = new Faker<IChild>()
            .StrictMode(true)
            .CustomInstantiator(f => new ChildWithMixedInterface())
            .RuleFor(e => e.City, f => f.Address.City())
            .RuleFor(e => e.Name, f => f.Name.FirstName())
            .Generate();

         child.City.Should().NotBeNullOrWhiteSpace();
         child.Name.Should().Be("Lupe");
      }

      [Fact]
      public void strictmode_only_sees_iparent2()
      {
         var child = new Faker<IParent2>()
            .StrictMode(true)
            .CustomInstantiator(f => new ChildWithMixedInterface())
            .RuleFor(e => e.Name2, f => f.Name.FirstName())
            .Generate();

         child.Name2.Should().Be("Lee");
      }

   }
}

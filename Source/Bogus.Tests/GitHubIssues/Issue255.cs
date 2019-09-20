using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue255
   {
      [Fact]
      public void explicit_interface_properties_in_parent_interfaces()
      {
         var child = new Faker<IChild>()
             .CustomInstantiator(f => new Child())
             .RuleFor(e => e.Name, f => f.Person.FullName)
             .Generate();

         child.Name.Should().NotBeNullOrWhiteSpace();
      }

      [Fact]
      public void explicit_interface_properties_in_child_interfaces()
      {
         var child = new Faker<IChild>()
             .CustomInstantiator(f => new Child())
             .RuleFor(e => e.City, f => f.Address.City())
             .Generate();

         child.City.Should().NotBeNullOrWhiteSpace();
      }

      // Define other methods and classes here
      private interface IParent
      {
         string Name { get; set; }
      }

      private interface IChild : IParent
      {
         string City { get; set; }
      }

      private class Child : IChild
      {
         string IParent.Name { get; set; }
         string IChild.City { get; set; }
      }
   }
}

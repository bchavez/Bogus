using System;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue115
   {
      public class Customer
      {
         public string Name { get; set; }
         public int OrderId { get; set; }
         public Order Order { get; set; }
      }
      
      [Fact]
      public void should_throw_with_nested_expression()
      {
         Action fakerMaker = () => new Faker<Customer>()
            .RuleFor(o => o.Order.Item, f => f.Random.Int(1, 1000).ToString());

         fakerMaker.ShouldThrow<ArgumentException>();

      }
   }
}
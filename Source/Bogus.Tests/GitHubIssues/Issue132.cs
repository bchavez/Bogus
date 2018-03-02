using System;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue132 : SeededTest
   {
      [Fact]
      public void should_throw_exception_on_invalid_locale_dataset()
      {
         Action a = () => new Lorem("wtf_locale");
         a.ShouldThrow<BogusException>();
      }

      [Fact]
      public void should_throw_exception_on_invalid_locale_with_faker_t()
      {
         Action a = () => new Faker<Models.Order>("yo yo yo");

         a.ShouldThrow<BogusException>();
      }

      [Fact]
      public void should_throw_exception_on_invalid_loclate_with_faker()
      {
         Action a = () => new Faker("fe fi fo fum");

         a.ShouldThrow<BogusException>();
      }

      [Fact]
      public void ensure_the_project_url_exists()
      {

         Action a = () => new Lorem("LOCALE");

         //make sure the message has a link back to the project site.
         //test exists here because we're using AssemblyDescription attribute
         //and in case that changes, we need to be aware of it.

         a.ShouldThrow<BogusException>()
            .And.Message
            .Should().Contain("https://github.com/bchavez/Bogus");

      }
   }
}
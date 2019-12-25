using Xunit;
using Bogus.Extensions.Brazil;
using FluentAssertions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue271 : SeededTest
   {
      [Fact]
      public void brazil_cpf_format_parameter_should_respect_person_context()
      {
         var p = new Bogus.Person();

         p.Cpf().Should().Be("869.287.971-18");
         p.Cpf(includeFormatSymbols:false).Should().Be("86928797118");
      }
   }
}
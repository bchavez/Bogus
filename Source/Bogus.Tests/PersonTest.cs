using System;
using System.Collections.Generic;
using System.Linq;
using Bogus.Extensions.Brazil;
using Bogus.Extensions.Canada;
using Bogus.Extensions.Denmark;
using Bogus.Extensions.Finland;
using Bogus.Extensions.UnitedStates;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests
{
   public class PersonTest : SeededTest
   {
      private readonly ITestOutputHelper console;

      public PersonTest(ITestOutputHelper console)
      {
         this.console = console;
      }

      public class User
      {
         public string FirstName { get; set; }
         public string Email { get; set; }
         public string LastName { get; set; }
      }

      [Fact]
      public void new_person_on_every_generate()
      {
         var faker = new Faker<User>()
            .RuleFor(b => b.Email, f => f.Person.Email)
            .RuleFor(b => b.FirstName, f => f.Person.FirstName)
            .RuleFor(b => b.LastName, f => f.Person.LastName);

         var fakes = faker.Generate(3);

         fakes.Select(f => f.Email).Distinct().Count().Should().Be(3);
         fakes.Select(f => f.FirstName).Distinct().Count().Should().Be(3);
         fakes.Select(f => f.LastName).Distinct().Count().Should().Be(3);
      }


      [Fact]
      public void check_ssn_on_person()
      {
         var p = new Person();
         p.Ssn().Should().Be("786-92-8797");
      }

      [Fact]
      public void can_generate_valid_sin()
      {
         var obtained = Get(10, p => p.Sin());

         console.Dump(obtained);

         var truth = new[]
            {
               "746 924 794",
               "377 136 593",
               "307 629 840",
               "586 063 471",
               "372 429 126",
               "320 968 522",
               "485 558 597",
               "400 037 271",
               "678 373 663",
               "488 539 966"
            };

         obtained.Should().Equal(truth);
      }

      [Fact]
      public void can_generate_cpf_for_brazil()
      {
         var obtained = Get(10, p => p.Cpf());

         console.Dump(obtained);

         var expect = new[]
            {
               "786.928.797-03",
               "359.526.934-90",
               "307.329.888-61",
               "412.365.760-55",
               "818.542.835-29",
               "989.340.967-56",
               "475.157.598-87",
               "400.035.257-16",
               "658.676.631-16",
               "847.792.478-37"
            };

         obtained.Should().Equal(expect);
      }

      [Fact]
      public void can_generate_cpr_nummer_for_denmark()
      {
         var p = new Person();
         var obtained = p.Cpr();

         obtained.Dump();

         var a = obtained.Split('-')[0];
         var b = obtained.Split('-')[1];

         a.Length.Should().Be(6);
         b.Length.Should().Be(4);
      }

      [Fact]
      public void can_generate_henkilötunnus_for_finland()
      {
         var p = new Person();
         var obtained = p.Henkilötunnus();

         var a = obtained.Split('-')[0];
         var b = obtained.Split('-')[1];

         a.Length.Should().Be(6);
         b.Length.Should().Be(4);
      }

      [Fact]
      public void check_emails()
      {
         var emails = Get(10, p => p.Email);

         console.Dump(emails);

         emails.Should().ContainInOrder(
            "Doris69@yahoo.com",
            "Betsy_Mraz19@hotmail.com",
            "Alvin_Fahey@yahoo.com",
            "Malcolm96@gmail.com",
            "Rosemarie_Rath@hotmail.com",
            "Kelley_Emard84@hotmail.com",
            "Stacey.Kerluke79@hotmail.com",
            "Bernadette8@hotmail.com",
            "Sylvia94@yahoo.com",
            "Angelo_Hyatt@hotmail.com");

         console.WriteLine(emails.DumpString());
      }
      
      [Fact]
      public void person_has_full_name()
      {
         var p = new Person();
         p.FullName.Should().Be($"{p.FirstName} {p.LastName}");
      }
      

      IEnumerable<string> Get(int times, Func<Person, string> a)
      {
         return Enumerable.Range(0, times)
            .Select(i =>
               {
                  var p = new Person();
                  return a(p);
               }).ToArray();
      }
   }
}

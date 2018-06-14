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
               "595 169 327",
               "083 194 845",
               "335 860 631",
               "865 181 952",
               "320 968 522",
               "949 882 807",
               "003 727 773",
               "678 373 663",
               "250 448 792"
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
               "693.455.045-59",
               "073.298.888-85",
               "365.760.664-57",
               "352.829.147-86",
               "096.754.915-90",
               "575.989.784-50",
               "003.525.756-38",
               "919.359.823-89",
               "399.334.225-96"
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
            "Jeremy.Klein@gmail.com",
            "Armando_Waelchi79@yahoo.com",
            "Cecil91@gmail.com",
            "Genevieve_Marvin@yahoo.com",
            "Elijah56@hotmail.com",
            "Gerardo_Leannon@hotmail.com",
            "Laverne25@yahoo.com");

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
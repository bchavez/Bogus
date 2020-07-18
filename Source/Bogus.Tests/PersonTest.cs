using System;
using System.Collections.Generic;
using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using Bogus.Extensions.Canada;
using Bogus.Extensions.Denmark;
using Bogus.Extensions.Finland;
using Bogus.Extensions.UnitedStates;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Z.ExtensionMethods.ObjectExtensions;

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
         p.Ssn().Should().Be("771-62-9016");
      }

      [Fact]
      public void can_generate_valid_sin()
      {
         var obtained = Get(10, p => p.Sin());

         console.Dump(obtained);

         var truth = new[]
            {
               "839 188 984",
               "325 702 553",
               "319 484 895",
               "586 063 471",
               "518 195 482",
               "964 093 777",
               "779 498 823",
               "920 006 517",
               "933 919 375",
               "399 632 215"
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
               "869.287.971-18",
               "595.269.345-80",
               "798.307.329-16",
               "885.844.123-01",
               "818.542.835-29",
               "963.989.340-40",
               "006.475.157-09",
               "629.400.035-13",
               "658.676.631-16",
               "792.478.139-05"
            };

         obtained.Should().Equal(expect);
      }

      [Fact]
      public void can_generate_cpf_for_brazil_without_formatting()
      {
         var obtained = Get(10, p => p.Cpf(includeFormatSymbols: false));

         console.Dump(obtained);

         var expect = new[]
            {
               "86928797118",
               "59526934580",
               "79830732916",
               "88584412301",
               "81854283529",
               "96398934040",
               "00647515709",
               "62940003513",
               "65867663116",
               "79247813905"
            };

         obtained.Should().Equal(expect);
      }

      [Fact]
      public void can_generate_numeric_cpf_for_brazil()
      {
         var obtained = Get(10, p => p.Cpf(includeFormatSymbols: false).ToULong());

         console.Dump(obtained);

         var expect = new ulong[]
            {
               86928797118,
               59526934580,
               79830732916,
               88584412301,
               81854283529,
               96398934040,
               00647515709,
               62940003513,
               65867663116,
               79247813905
            };

         obtained.Should().Equal(expect);
      }

      [Fact]
      public void can_generate_cpr_number_for_denmark()
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
            "Natasha_Turcotte19@hotmail.com",
            "Melba47@gmail.com",
            "Ismael.Murray3@gmail.com",
            "Brendan.Beer51@yahoo.com",
            "Kathleen_Nader@yahoo.com",
            "Genevieve_Marvin@yahoo.com",
            "Regina_Kirlin44@yahoo.com",
            "Gerardo_Leannon@hotmail.com",
            "Theodore_Gaylord24@hotmail.com"
         );
         console.WriteLine(emails.DumpString());
      }

      [Fact]
      public void person_has_full_name()
      {
         var p = new Person();
         p.FullName.Should().Be($"{p.FirstName} {p.LastName}");
      }

      [Fact]
      public void can_use_local_seed_for_person()
      {
         Date.SystemClock = () => new DateTime(2019, 3, 21, 1, 1, 1);

         var p1 = new Person(seed: 1337);
         var p2 = new Person(seed: 1337);
         var q = new Person(seed: 7331);

         p1.FullName.Should().Be("Samuel Haley");
         p2.FullName.Should().Be(p1.FullName);
         q.FullName.Should().Be("Lynette Beatty");
         q.FullName.Should().NotBe(p1.FullName);

         p1.FirstName.Should().Be(p2.FirstName);
         p1.LastName.Should().Be(p2.LastName);
         p1.Avatar.Should().Be(p2.Avatar);
         p1.DateOfBirth.Should().Be(p2.DateOfBirth);
         p1.Email.Should().Be(p2.Email);
         p1.Phone.Should().Be(p2.Phone);
         p1.UserName.Should().Be(p2.UserName);
         p1.Gender.Should().Be(p2.Gender);
         p1.Website.Should().Be(p2.Website);

         p1.Should().BeEquivalentTo(p2);

         Date.SystemClock = () => DateTime.Now;
      }


      IEnumerable<T> Get<T>(int times, Func<Person, T> a)
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
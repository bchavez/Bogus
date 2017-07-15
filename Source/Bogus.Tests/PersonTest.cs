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

            var fakes = faker.Generate(3).ToList();

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

            var truth = new[]
                {
                    "746 924 794",
                    "595 169 327",
                    "947 986 089",
                    "845 442 110",
                    "035 435 247",
                    "386 828 776",
                    "045 289 626",
                    "079 899 753",
                    "620 761 643",
                    "574 964 227"
                };

            obtained.Should().Equal(truth);
        }

        [Fact]
        public void can_generate_cpf_for_brazil()
        {
            var obtained = Get(10, p => p.Cpf());


            var expect = new[]
                {
                    "786.928.797-03",
                    "595.269.345-80",
                    "073.298.888-85",
                    "365.760.664-57",
                    "835.282.914-94",
                    "340.967.549-35",
                    "989.784.800-20",
                    "003.525.756-38",
                    "658.676.631-16",
                    "210.847.792-69"
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

         emails.Should().ContainInOrder(
             "Lee69@yahoo.com",
             "Michale_Stiedemann94@gmail.com",
             "Grayson_Harvey@yahoo.com",
             "Anne.Ruecker@gmail.com",
             "Vilma.Beer51@yahoo.com",
             "Loraine_Sipes@yahoo.com",
             "Haylie_Reilly29@yahoo.com",
             "Sandrine_Watsica76@gmail.com",
             "Madisyn6@yahoo.com",
             "Austin.Marks1@hotmail.com");

          console.WriteLine(emails.DumpString());
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
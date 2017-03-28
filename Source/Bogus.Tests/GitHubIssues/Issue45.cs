using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests.GitHubIssues
{
    public class Issue45 : SeededTest
    {
        public class Issue45Object
        {
            public int Id { get; set; }
            public List<string> Phones { get; set; } // PROBLEM !!!
        }

        [Test]
        public void issue_45_better_fluency()
        {
            var ids = 0;

            var test = new Faker<Issue45Object>()
                .RuleFor(p => p.Id, f => ids++)
                .RuleFor(p => p.Phones, f => f.Generate(5, () => f.Phone.PhoneNumber()).ToList());

            test.Generate(1).First().Phones.Count.Should().Be(5);
        }

        [Test]
        public void with_int_argument()
        {
            var test = new Faker<Issue45Object>()
                .RuleFor(p => p.Id, f => f.IndexVariable++)
                .RuleFor(p => p.Phones, f => f.Generate(5, i => i.ToString()).ToList());

            test.Generate(1).First().Phones.Select(int.Parse)
                .Distinct().Should().Equal(1, 2, 3, 4, 5);
        }
    }
}
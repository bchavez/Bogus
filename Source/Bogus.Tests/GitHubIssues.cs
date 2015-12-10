using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class GitHubIssues : SeededTest
    {
        public class Bar
        {
            public string Name;
            public string Email { get; set; }
            internal string LastName;
        }

        [Test]
        public void issue_10_should_be_able_to_fake_fields()
        {
            var faker = new Faker<Bar>()
                .RuleFor(b => b.Email, f => f.Internet.Email())
                .RuleFor(b => b.Name, f => f.Name.FirstName())
                .RuleFor(b => b.LastName, f => f.Name.LastName());

            var bar = faker.Generate();

            bar.Dump();
            bar.Name.Should().NotBeNullOrEmpty();
            bar.Email.Length.Should().BeGreaterOrEqualTo(2);
            bar.Email.Should().NotBeNullOrEmpty();
            bar.Email.Length.Should().BeGreaterOrEqualTo(2);

            bar.LastName.Should().NotBeNullOrEmpty();
            bar.LastName.Length.Should().BeGreaterOrEqualTo(2);

        }
    }
}
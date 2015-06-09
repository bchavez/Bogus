using FluentAssertions;
using Bogus.Generators;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class NameTests : SeededTest
    {
        private Name name;

        [SetUp]
        public void BeforeEachTest()
        {
            name = new Name();
        }

        [Test]
        public void can_get_first_name()
        {
            name.FirstName().Should().Be("Lee");
        }

        [Test]
        public void can_get_last_name()
        {
            name.LastName().Should().Be("Mitchell");
        }

        [Test]
        public void can_get_prefix()
        {
            name.Prefix().Should().Be("Miss");
        }

        [Test]
        public void can_get_suffix()
        {
            name.Suffix().Should().Be("V");
        }

        [Test]
        public void should_be_able_to_get_any_full_name()
        {
            name.FindName().Should().Be("Lee Brown MD");
        }

        [Test]
        public void should_be_able_to_get_any_name_with_options()
        {
            name.FindName(firstName: "cowboy")
                .Should().Be("Dr. cowboy Mitchell");

            name.FindName(lastName: "cowboy")
                .Should().Be("Miss Lupe cowboy");

            name.FindName(withPrefix: false, withSuffix: false)
                .Should().Be("Ambrose Pollich");

            name.FindName(firstName: "cowboy", withPrefix: false, withSuffix: false)
                .Should().Be("cowboy Kreiger");

            name.FindName(lastName: "cowboy", withPrefix: false, withSuffix: false)
                .Should().Be("Eliza cowboy");
        }
    }
}

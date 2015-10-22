using Bogus.DataSets;
using FluentAssertions;
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
            var n = name.FindName();
            n.Length.Should().BeGreaterThan(4);
            n.Should().Contain(" ");
        }

        [Test]
        public void should_be_able_to_get_locale_full_name()
        {
            var n = new Name("ru");
            n.FindName().Should().Contain(" ");
        }

        [Test]
        public void should_be_able_to_get_any_name_with_options()
        {
            name.FindName(firstName: "cowboy")
                .Should().StartWith("cowboy");

            name.FindName(lastName: "cowboy")
                .Should().EndWith("cowboy");

            name.FindName(withPrefix: false, withSuffix: false)
                .Should().Contain(" ");

            name.FindName(firstName: "cowboy", withPrefix: false, withSuffix: false)
                .Should().StartWith("cowboy");

            name.FindName(lastName: "cowboy", withPrefix: false, withSuffix: false)
                .Should().EndWith("cowboy");
        }

        [Test]
        public void should_be_able_to_get_job_title()
        {
            name.JobTitle().Should().Be("Investor Research Assistant");
        }

        [Test]
        public void should_be_able_to_get_job_description()
        {
            name.JobDescriptor().Should().Be("Investor");
        }

        [Test]
        public void should_be_able_to_get_job_area()
        {
            name.JobArea().Should().Be("Communications");
        }

        [Test]
        public void should_be_able_to_get_job_type()
        {
            name.JobType().Should().Be("Orchestrator");
        }

        [Test]
        public void can_get_first_name_when_locale_dataset_is_split_in_male_female()
        {
            var n = new Name("ru");

            n.FirstName().Should().Be("Анастасия");
        }
        [Test]
        public void can_get_last_name_when_locale_dataset_is_split_in_male_female()
        {
            var n = new Name("ru");

            n.LastName().Should().Be("Киселева");
        }
    }
}

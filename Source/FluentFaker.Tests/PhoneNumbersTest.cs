using FluentAssertions;
using NUnit.Framework;

namespace FluentFaker.Tests
{
    [TestFixture]
    public class PhoneNumbersTest: ConsistentTest
    {
        private PhoneNumbers phone;

        [SetUp]
        public void BeforeEachTest()
        {
            phone = new PhoneNumbers();
        }

        [Test]
        public void can_get_phone_number()
        {
            phone.PhoneNumber()
                .Should().Be("186-060-6439 x1750");
        }

        [Test]
        public void can_get_phone_number_of_specific_format()
        {
            phone.PhoneNumber("## ### ####")
                .Should().Be("61 860 6064");
        }

        [Test]
        public void can_get_phone_number_via_formats_index()
        {
            phone.PhoneNumberFormat(1)
                .Should().Be("(618) 606-0643");
        }
    }
}
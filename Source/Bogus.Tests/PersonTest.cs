using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    public class PersonTest: SeededTest
    {
        [Test]
        public void check_ssn_on_person()
        {
            var p = new Person();
            p.SSN.Should().Be("778-69-2879");
        }
    }

}
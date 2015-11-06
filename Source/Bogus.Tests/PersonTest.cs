using System.Linq;
using Bogus.Extensions.Canada;
using Bogus.Extensions.UnitedStates;
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
            p.Ssn().Should().Be("778-69-2879");
        }

        [Test]
        public void can_generate_valid_sin()
        {
            Enumerable.Range(1, 10000)
                .Select(s =>
                    {
                        var p = new Person();

                        return p.Sin();
                    })
                .Dump();
        }

    }

}
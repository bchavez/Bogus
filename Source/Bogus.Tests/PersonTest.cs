using System.Linq;
using Bogus.Extensions.Brazil;
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
            var obtained =Enumerable.Range(1, 10)
                .Select(s =>
                    {
                        var p = new Person();

                        return p.Sin();
                    })
                .ToArray();

            var truth = new[]
                {
                    "788 391 886",
                    "465 826 378",
                    "059 694 794",
                    "388 842 841",
                    "254 884 844",
                    "699 577 375",
                    "001 827 872",
                    "248 908 691",
                    "069 387 884",
                    "108 829 094",
                };
            obtained.Should().Equal(truth);
        }

        [Test]
        public void can_generate_cpf_for_brazil()
        {

            var obtained = Enumerable.Range(1, 10)
                .Select(s =>
                    {
                        var p = new Person();
                        return p.Cpf();
                    }).ToArray();


            var expect = new[]
                {
                    "778.692.879-03",
                    "357.233.595-76",
                    "019.398.798-84",
                    "273.787.448-32",
                    "214.788.888-57",
                    "699.175.356-40",
                    "001.725.853-76",
                    "337.805.979-69",
                    "361.678.676-23",
                    "094.805.520-00"
                };

            obtained.Should().Equal(expect);

        }

    }

}
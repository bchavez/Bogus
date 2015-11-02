using System;
using FluentAssertions;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class RandomizerTest : SeededTest
    {
        private Randomizer r;

        [TestFixtureSetUp]
        public void BeforeRunningTestSession()
        {
            r = new Randomizer();
        }

        public enum Foo
        {
            ExcludeMe,
            A,B,C,D
        }

        [Test]
        public void pick_an_enum()
        {
            var f = r.Enum<Foo>();
            f.Should().Be(Foo.C);
        }

        [Test]
        public void exclude_an_enum()
        {
            //seeded value of 14 gets "ExcludeMe", ensure exclude works.
            Randomizer.Seed = new Random(14);
            var f = r.Enum( exclude: Foo.ExcludeMe);
            f.ToString().Dump();

            f.Should().NotBe(Foo.ExcludeMe);
        }

        [Test]
        public void exclude_all_throws_an_error()
        {
            Action act = () => r.Enum(Foo.ExcludeMe, Foo.A, Foo.B, Foo.C, Foo.D);

            act.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void can_replace_numbers_or_letters_using_asterisk()
        {
            r.Replace("***")
                .Should().Be("CQ6");
        }
    }



}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        [Test]
        public void can_get_a_random_word()
        {
            r.Word().Should().Be("Court");
            r.Word().Should().Be("bluetooth");
            r.Word().Should().Be("Movies & Clothing");
        }

        [Test]
        public void can_get_some_random_words()
        {
            r.Words().Should().Be("Soft deposit");
            r.Words().Should().Be("Handcrafted Granite Gloves Directives");
            r.Words().Should().Be("Corner Handcrafted Frozen Chair transmitting");
        }

        [Test]
        public void can_shuffle_some_enumerable()
        {
            new string(r.Shuffle("123456789").ToArray())
                .Should().Be("628753491");
        }

        [Test]
        public void can_get_random_locale()
        {
            r.RandomLocale().Should().Be("ru");
        }

        [Test]
        public void exclusive_int_maxvalue_number()
        {
            var max = r.Number(int.MaxValue - 1, int.MaxValue);
            max.Should().Be(int.MaxValue - 1);
        }

        [Test]
        public void random_bool()
        {
            r.Bool().Should().BeFalse();
        }

        [Test]
        public void can_get_some_alpha_chars()
        {
            r.AlphaNumeric(20).Should().Be("l3tn1m1ohax6ql31pw1u");
        }

        [Test]
        public void generate_a_double_with_min_and_max()
        {
            r.Double(2.5, 2.9).Should().BeInRange(2.74140891332244, 2.74140891332246);
        }

        [Test]
        public void generate_a_decimal_with_min_and_max()
        {
            r.Decimal(2.2m, 5.2m).Should().Be(4.0105668499183690m);
        }

        [Test]
        public void generate_a_float_with_min_and_max()
        {
            r.Float(2.7f, 3.9f).Should().BeInRange(3.424226f, 3.424228f);

        }

        [Test]
        public void generate_a_byte()
        {
            r.Byte(1, 128).Should().Be(78);
        }

        [Test]
        public void generate_some_bytes()
        {
            r.Bytes(20).Should()
                .Equal(218, 35, 156, 76, 224, 196, 45, 215, 227, 196, 168, 150, 23, 242, 85, 178, 101, 200, 89, 189);
        }

        [Test]
        public void generate_an_sbyte()
        {
            r.SByte(max:0).Should().Be(-51);
        }

        [Test]
        public void generate_a_uint32()
        {
            r.UInt(99, 200).Should().Be(160);
        }

        [Test]
        public void generate_a_int32()
        {
            r.Int(max: 0).Should().Be(-851429413);
        }

        [Test]
        public void generate_a_uint64()
        {
            r.ULong(99, 9999).Should().Be(6074);
        }

        [Test]
        public void generate_an_int64()
        {
            r.Long(max: 0).Should().Be(-3656861485390336000);
        }

        [Test]
        public void generate_a_int16()
        {
            r.Short(max: 0).Should().Be(-12992);
        }

        [Test]
        public void generate_a_uint16()
        {
            r.UShort().Should().Be(39552);
        }

        [Test]
        public void generate_a_char()
        {
            r.Char().Should().Be('\u9a80');
        }

        [Test]
        public void generate_some_chars()
        {
            r.Chars(count: 10).Should().Equal(
                '\u9a80',
                '\u1c17',
                '\ud089',
                '\ua576',
                '\u091c',
                '\u9fdb',
                '\u0cfa',
                '\ub0d6',
                '\u7a91',
                '\u4d58');
        }
    }



}
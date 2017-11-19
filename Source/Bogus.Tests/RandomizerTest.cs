﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests
{
   public class RandomizerTest : SeededTest
   {
      private Randomizer r;

      public RandomizerTest()
      {
         r = new Randomizer();
      }

      public enum Foo
      {
         ExcludeMe,
         A,
         B,
         C,
         D
      }

      [Fact]
      public void pick_an_enum()
      {
         var f = r.Enum<Foo>();
         f.Should().Be(Foo.C);
      }

      [Fact]
      public void exclude_an_enum()
      {
         //seeded value of 14 gets "ExcludeMe", ensure exclude works.
         Randomizer.Seed = new Random(14);
         var f = r.Enum(exclude: Foo.ExcludeMe);
         f.ToString().Dump();

         f.Should().NotBe(Foo.ExcludeMe);
      }

      [Fact]
      public void exclude_all_throws_an_error()
      {
         Action act = () => r.Enum(Foo.ExcludeMe, Foo.A, Foo.B, Foo.C, Foo.D);

         act.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void can_replace_numbers_or_letters_using_asterisk()
      {
         r.Replace("***")
            .Should().Be("CQ6");
      }

      [Fact]
      public void can_get_random_word()
      {
         r.Word().Should().Be("Court");
         r.Word().Should().Be("bluetooth");
         r.Word().Should().Be("Movies & Clothing");
      }

      [Fact]
      public void can_get_some_random_words()
      {
         r.Words().Should().Be("Soft deposit");
         r.Words().Should().Be("Handcrafted Granite Gloves Directives");
         r.Words().Should().Be("Corner Handcrafted Frozen Chair transmitting");
      }

      [Fact]
      public void can_shuffle_some_enumerable()
      {
         new string(r.Shuffle("123456789").ToArray())
            .Should().Be("628753491");
      }

      [Fact]
      public void can_get_random_locale()
      {
         r.RandomLocale().Should().Be("lv");
      }

      [Fact]
      public void exclusive_int_maxvalue_number()
      {
         var max = r.Number(int.MaxValue - 1, int.MaxValue);
         max.Should().Be(int.MaxValue - 1);
      }

      [Fact]
      public void random_bool()
      {
         r.Bool().Should().BeFalse();
      }

      [Fact]
      public void can_get_some_alpha_chars()
      {
         r.AlphaNumeric(20).Should().Be("l3tn1m1ohax6ql31pw1u");
      }

      [Fact]
      public void generate_double_with_min_and_max()
      {
         r.Double(2.5, 2.9).Should().BeInRange(2.74140891332244, 2.74140891332246);
      }

      [Fact]
      public void generate_decimal_with_min_and_max()
      {
         r.Decimal(2.2m, 5.2m).Should().Be(4.0105668499183690m);
      }

      [Fact]
      public void generate_float_with_min_and_max()
      {
         r.Float(2.7f, 3.9f).Should().BeInRange(3.424226f, 3.424228f);
      }

      [Fact]
      public void generate_byte()
      {
         r.Byte(1, 128).Should().Be(78);
      }

      [Fact]
      public void generate_some_bytes()
      {
         r.Bytes(20).Should()
            .Equal(218, 35, 156, 76, 224, 196, 45, 215, 227, 196, 168, 150, 23, 242, 85, 178, 101, 200, 89, 189);
      }

      [Fact]
      public void generate__sbyte()
      {
         r.SByte(max: 0).Should().Be(-51);
      }

      [Fact]
      public void generate_uint32()
      {
         r.UInt(99, 200).Should().Be(160);
      }

      [Fact]
      public void generate_unit32_many()
      {
         r.UInt().Should().Be(2592108469u);
         r.UInt().Should().Be(471320134u);
         r.UInt().Should().Be(3498684729u);
         r.UInt().Should().Be(2775978649u);
      }

      [Fact]
      public void generate_int32()
      {
         r.Int(max: 0).Should().Be(-425714706);
      }

      [Fact]
      public void generate_int32_many()
      {
         r.Int().Should().Be(1296054233);
         r.Int().Should().Be(-1749342366);
         r.Int().Should().Be(-76446690);
         r.Int().Should().Be(108870444);
      }

      [Fact]
      public void generate_uint64()
      {
         r.ULong(99, 9999).Should().Be(6074);
      }

      [Fact]
      public void generate_uint64_many()
      {
         r.ULong().Should().Be(11133021102928879616UL);
         r.ULong().Should().Be(2024304562418978048UL);
         r.ULong().Should().Be(15026736492772024320UL);
         r.ULong().Should().Be(11922737513106253824UL);
      }

      [Fact]
      public void generate_int64()
      {
         r.Long(max: 0).Should().Be(-3656861485390335055L);
      }

      [Fact]
      public void generate_int64_many()
      {
         r.Long().Should().Be(1909649066074105698L);
         r.Long().Should().Be(-7199067474435792608L);
         r.Long().Should().Be(5803364455917250112L);
         r.Long().Should().Be(2699365476251477286L);
         r.Long().Should().Be(-8566699986853958425L);
      }

      [Fact]
      public void generate_int16()
      {
         r.Short(max: 0).Should().Be(-12992);
      }

      [Fact]
      public void generate_int16_many()
      {
         r.Short().Should().Be(6784);
         r.Short().Should().Be(-25576);
         r.Short().Should().Be(20617);
         r.Short().Should().Be(9589);
      }

      [Fact]
      public void generate_uint16()
      {
         r.UShort().Should().Be(39552);
      }

      [Fact]
      public void generate_char()
      {
         r.Char().Should().Be('\u9a80');
      }

      [Fact]
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

      [Fact]
      public void random_word_tests()
      {
         //r.Words(3).Should().Be("");
         //r.Words(5).Split(' ').Length.Should().Be(4);
         r.WordsArray(5).Length.Should().Be(5);
         r.WordsArray(1, 80).Length.Should().BeInRange(1, 80); //.Should().BeInRange(1, 80);
         r.WordsArray(10, 20).Length.Should().BeInRange(10, 20);
      }

      [Fact]
      public void can_pick_random_item_from_ICollection()
      {
         var x = new List<int> {1, 2, 3} as ICollection<int>;

         r.CollectionItem(x).Should().BeOneOf(1, 2, 3);
      }

      [Fact]
      public void throw_an_exception_when_picking_nothing_from_collection()
      {
         var x = new List<int>() as ICollection<int>;
         Action act = () => r.CollectionItem(x);
         act.ShouldThrow<InvalidOperationException>();
      }

      [Fact]
      public void can_pick_random_item_from_ilist()
      {
         var x = new List<int> {1, 2, 3} as IList<int>;

         r.ListItem(x).Should().BeOneOf(1, 2, 3);
      }

      [Fact]
      public void can_pick_random_item_from_list()
      {
         var x = new List<int> {1, 2, 3};

         r.ListItem(x).Should().BeOneOf(1, 2, 3);
      }

      [Fact]
      public void can_generate_hexdec_string()
      {
         r.Hexadecimal().Should().StartWith("0x").And.Be("0x9");
         r.Hexadecimal(20).Should().Be("0x1da090b74f2b910be0dd");
         r.Hexadecimal(prefix: "").Should().Be("5");
      }

      [Fact]
      public void can_get_random_subset_of_an_array()
      {
         var a = new[] {"a", "b", "c"};
         r.ArrayElements(a).Should().Equal("a");

         r.ArrayElements(a, 2).Should().Equal("c", "a");
      }

      [Fact]
      public void can_get_random_subset_of_a_list()
      {
         var a = new List<string>() {"a", "b", "c"};
         r.ListItems(a).Should().Equal("a");

         r.ListItems(a, 2).Should().Equal("c", "a");
      }

      [Fact]
      public void can_get_a_weighted_true_value()
      {
         var bools = Enumerable.Range(1, 100).Select(i => r.Bool(0.20f));
         var truths = bools.Count(b => b) / 100f;

         truths.Should().BeLessThan(0.25f); // roughly
      }
   }
}
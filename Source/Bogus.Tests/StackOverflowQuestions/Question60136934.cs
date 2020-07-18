
using System;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.StackOverflowQuestions
{
   //https://stackoverflow.com/questions/60136934/generate-a-random-number-of-addresses-each-with-a-unique-type-value-with-bogus-f
   public class Question60136934 : SeededTest
   {
      private Randomizer r;

      enum Foo
      {
         A,
         B,
         C,
         D
      }

      public Question60136934()
      {
         r = new Randomizer();
      }

      [Fact]
      public void pick_subset_of_enum_values_except_c_and_d()
      {
         var chosen = r.EnumValues(2, Foo.C, Foo.D);

         chosen.Length.Should().Be(2);
         chosen[0].Should().Be(Foo.B);
         chosen[1].Should().Be(Foo.A);
      }

      [Fact]
      public void pick_subset_of_enum_values_except_c()
      {
         var chosen = r.EnumValues(2, exclude: Foo.C);
         chosen.Length.Should().Be(2);
         chosen[0].Should().Be(Foo.B);
         chosen[1].Should().Be(Foo.A);
      }

      [Fact]
      public void pick_any_two_random_enums()
      {
         var chosen = r.EnumValues<Foo>(2);

         chosen.Length.Should().Be(2);
         chosen.Should().Equal(Foo.C, Foo.B);
      }

      [Theory]
      [InlineData(-3)]
      [InlineData(9)]
      public void pick_invalid_number_of_enums(int count)
      {
         Action act = () => r.EnumValues<Foo>(count);
         act.Should().Throw<ArgumentOutOfRangeException>();
      }
   }
}

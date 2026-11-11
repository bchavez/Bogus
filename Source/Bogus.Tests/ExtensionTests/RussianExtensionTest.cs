using System.Linq;
using Bogus.Extensions.Russia;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests;

public class RussianExtensionTest : SeededTest
{
   private static readonly int[] TinIndividualFirstWeights = {7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
   private static readonly int[] TinIndividualSecondWeights = {3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
   private static readonly int[] TinLegalWeights = {2, 4, 10, 3, 5, 9, 4, 6, 8};

   [Fact]
   public void can_generate_inila()
   {
      var faker = new Faker("ru");
      var inila = faker.Person.Inila();

      inila.Should().MatchRegex(@"^\d{3}-\d{3}-\d{3} \d{2}$");
      ShouldHaveValidInilaChecksum(inila.Replace("-", "").Replace(" ", ""));
   }

   [Fact]
   public void can_generate_inila_without_formatting()
   {
      var faker = new Faker("ru");
      var inila = faker.Person.Inila(includeFormatSymbols: false);

      inila.Should().HaveLength(11);
      inila.Should().MatchRegex(@"^\d{11}$");
      ShouldHaveValidInilaChecksum(inila);
   }

   [Fact]
   public void inila_is_cached_on_person()
   {
      var faker = new Faker("ru");
      var person = faker.Person;

      person.Inila().Should().Be(person.Inila());
   }

   [Fact]
   public void can_generate_individual_tin()
   {
      var faker = new Faker("ru");
      var tin = faker.Person.Tin();

      tin.Should().HaveLength(12);
      tin.Should().MatchRegex(@"^\d{12}$");
      tin[0].Should().NotBe('0');
      ShouldHaveValidIndividualTinChecksum(tin);
   }

   [Fact]
   public void individual_tin_is_cached_on_person()
   {
      var faker = new Faker("ru");
      var person = faker.Person;

      person.Tin().Should().Be(person.Tin());
   }

   [Fact]
   public void can_generate_legal_tin()
   {
      var faker = new Faker("ru");
      var tin = faker.Company.Tin();

      tin.Should().HaveLength(10);
      tin.Should().MatchRegex(@"^\d{10}$");
      tin[0].Should().NotBe('0');
      ShouldHaveValidLegalTinChecksum(tin);
   }

   private static void ShouldHaveValidInilaChecksum(string digits)
   {
      var sum = 0;
      for( var i = 0; i < 9; i++ )
         sum += (int)char.GetNumericValue(digits[i]) * (9 - i);

      var check = sum % 101;
      if( check == 100 )
         check = 0;

      check.Should().Be(int.Parse(digits.Substring(9, 2)));
   }

   private static void ShouldHaveValidIndividualTinChecksum(string tin)
   {
      var digits = tin.Select(c => (int)char.GetNumericValue(c)).ToArray();

      WeightedMod(digits, TinIndividualFirstWeights).Should().Be(digits[10]);
      WeightedMod(digits, TinIndividualSecondWeights).Should().Be(digits[11]);
   }

   private static void ShouldHaveValidLegalTinChecksum(string tin)
   {
      var digits = tin.Select(c => (int)char.GetNumericValue(c)).ToArray();

      WeightedMod(digits, TinLegalWeights).Should().Be(digits[9]);
   }

   private static int WeightedMod(int[] digits, int[] weights)
   {
      var sum = 0;
      for( var i = 0; i < weights.Length; i++ )
         sum += digits[i] * weights[i];

      return (sum % 11) % 10;
   }
}

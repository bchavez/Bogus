using System.Linq;
using Bogus.Extensions.Russia;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests;

public class RussianExtensionTest : SeededTest
{
   private static readonly int[] InnIndividualFirstWeights = {7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
   private static readonly int[] InnIndividualSecondWeights = {3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
   private static readonly int[] InnLegalWeights = {2, 4, 10, 3, 5, 9, 4, 6, 8};

   [Fact]
   public void can_generate_snils()
   {
      var faker = new Faker("ru");
      var snils = faker.Person.Snils();

      snils.Should().MatchRegex(@"^\d{3}-\d{3}-\d{3} \d{2}$");
      ShouldHaveValidSnilsChecksum(snils.Replace("-", "").Replace(" ", ""));
   }

   [Fact]
   public void can_generate_snils_without_formatting()
   {
      var faker = new Faker("ru");
      var snils = faker.Person.Snils(includeFormatSymbols: false);

      snils.Should().HaveLength(11);
      snils.Should().MatchRegex(@"^\d{11}$");
      ShouldHaveValidSnilsChecksum(snils);
   }

   [Fact]
   public void snils_is_cached_on_person()
   {
      var faker = new Faker("ru");
      var person = faker.Person;

      person.Snils().Should().Be(person.Snils());
   }

   [Fact]
   public void can_generate_individual_inn()
   {
      var faker = new Faker("ru");
      var inn = faker.Person.Inn();

      inn.Should().HaveLength(12);
      inn.Should().MatchRegex(@"^\d{12}$");
      inn[0].Should().NotBe('0');
      ShouldHaveValidIndividualInnChecksum(inn);
   }

   [Fact]
   public void individual_inn_is_cached_on_person()
   {
      var faker = new Faker("ru");
      var person = faker.Person;

      person.Inn().Should().Be(person.Inn());
   }

   [Fact]
   public void can_generate_legal_inn()
   {
      var faker = new Faker("ru");
      var inn = faker.Company.Inn();

      inn.Should().HaveLength(10);
      inn.Should().MatchRegex(@"^\d{10}$");
      inn[0].Should().NotBe('0');
      ShouldHaveValidLegalInnChecksum(inn);
   }

   private static void ShouldHaveValidSnilsChecksum(string digits)
   {
      var sum = 0;
      for( var i = 0; i < 9; i++ )
         sum += (int)char.GetNumericValue(digits[i]) * (9 - i);

      var check = sum % 101;
      if( check == 100 )
         check = 0;

      check.Should().Be(int.Parse(digits.Substring(9, 2)));
   }

   private static void ShouldHaveValidIndividualInnChecksum(string inn)
   {
      var digits = inn.Select(c => (int)char.GetNumericValue(c)).ToArray();

      WeightedMod(digits, InnIndividualFirstWeights).Should().Be(digits[10]);
      WeightedMod(digits, InnIndividualSecondWeights).Should().Be(digits[11]);
   }

   private static void ShouldHaveValidLegalInnChecksum(string inn)
   {
      var digits = inn.Select(c => (int)char.GetNumericValue(c)).ToArray();

      WeightedMod(digits, InnLegalWeights).Should().Be(digits[9]);
   }

   private static int WeightedMod(int[] digits, int[] weights)
   {
      var sum = 0;
      for( var i = 0; i < weights.Length; i++ )
         sum += digits[i] * weights[i];

      return (sum % 11) % 10;
   }
}

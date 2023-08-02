using Bogus.DataSets;
using Bogus.Extensions.Poland;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Bogus.Extensions.Poland.ExtensionsForPoland;

namespace Bogus.Tests.ExtensionTests;

public class PolishExtensionTest : SeededTest
{
   [Fact]
   public void PeselWhenPersonIsMaleSecondLastNumberIsOdd()
   {
      Faker f = new Faker("pl");
      Person person = f.Person;
      person.Gender = Name.Gender.Male;

      string pesel = person.Pesel();

      int secondLast = int.Parse(pesel.Substring(pesel.Length - 2, 1));

      secondLast.Should().Match(x => x % 2 == 1);
   }
   
   [Fact]
   public void PeselTest()
   {
      Faker f = new Faker("pl");
      Person person = f.Person;

      string pesel = person.Pesel();
      int sum = 0;

      for (int i = 0; i < pesel.Length; i++)
         sum += pesel[i] * PeselWeights[i];

      string sumString = sum.ToString();

      sumString[sumString.Length - 1].Should().Be('0');
   }

   private static readonly int[] PeselWeights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3, 1 };


   [Fact]
   public void NipTest()
   {
      Faker f = new Faker("pl");
      Company company = f.Company;

      string nip = company.Nip();

      nip.Length.Should().Be(10);

      int sum = nip.Zip(NipWeights, (digit, weight) => (int)char.GetNumericValue(digit) * weight).Sum();

      ((sum % 11) == (int)char.GetNumericValue(nip[9])).Should().BeTrue();
   }

   private static readonly int[] NipWeights = { 6, 5, 7, 2, 3, 4, 5, 6, 7, 0 };

   [Fact]
   public void Regon9Test()
   {
      Faker f = new Faker("pl");

      string regon9 = f.Company.Regon();
      regon9.Length.Should().Be(9);

      int sum = regon9.Zip(RegonWeights[RegonType.Regon9], (digit, weight) => (int)char.GetNumericValue(digit) * weight).Sum();

      int expected = (sum % 11);
      expected = expected == 10 ? 0 : expected;

      (expected == (int)char.GetNumericValue(regon9[8])).Should().BeTrue();  
   }

   [Fact]
   public void Regon14Test()
   {
      Faker f = new Faker("pl");

      string regon14 = f.Company.Regon(RegonType.Regon14);
      regon14.Length.Should().Be(14);

      int sum = regon14.Zip(RegonWeights[RegonType.Regon14], (digit, weight) => (int)char.GetNumericValue(digit) * weight).Sum();

      int expected = (sum % 11);
      expected = expected == 10 ? 0 : expected;

      (expected == (int)char.GetNumericValue(regon14[13])).Should().BeTrue();
   }

   private static readonly Dictionary<RegonType, int[]> RegonWeights = new()
   {
      [RegonType.Regon9] = new int[] { 8, 9, 2, 3, 4, 5, 6, 7, 0 },
      [RegonType.Regon14] = new int[] { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8, 0 },
   };
}

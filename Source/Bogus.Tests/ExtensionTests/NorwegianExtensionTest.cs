using Bogus.DataSets;
using Bogus.Extensions.Norway;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.ExtensionTests
{
   public class NorwegianExtensionTest : SeededTest
   {
      private void IsLegalIndividualNumber(int readIndNo, int birthYear, Person p)
      {
         // Check that birth year is in the correct range given individual number.
         if (0 <= readIndNo && readIndNo <= 499)
         {
            birthYear.Should().BeInRange(0, 99);
         }
         else if (750 <= readIndNo && readIndNo <= 999)
         {
            birthYear.Should().BeInRange(0, 39);
         }
         else if (500 <= readIndNo && readIndNo <= 749)
         {
            if (0 <= birthYear && birthYear <= 39)
            {
               birthYear.Should().BeInRange(0, 39);
            }
            else
            {
               birthYear.Should().BeInRange(54, 99);
            }
         }

         // Check odd/even individual number given gender.
         if (p.Gender == Name.Gender.Female)
         {
            (readIndNo % 2 == 0).Should().BeTrue();
         }
         else
         {
            (readIndNo % 2 == 0).Should().BeFalse();
         }
      }

      private void IsLegalChecksum(string readFødselsnummer)
      {
         string readCs = readFødselsnummer.Substring(9, 2);

         int d1 = int.Parse(readFødselsnummer.Substring(0, 1));
         int d2 = int.Parse(readFødselsnummer.Substring(1, 1));
         int m1 = int.Parse(readFødselsnummer.Substring(2, 1));
         int m2 = int.Parse(readFødselsnummer.Substring(3, 1));
         int y1 = int.Parse(readFødselsnummer.Substring(4, 1));
         int y2 = int.Parse(readFødselsnummer.Substring(5, 1));
         int i1 = int.Parse(readFødselsnummer.Substring(6, 1));
         int i2 = int.Parse(readFødselsnummer.Substring(7, 1));
         int i3 = int.Parse(readFødselsnummer.Substring(8, 1));

         int cs1 = 11 - (((3 * d1) + (7 * d2) + (6 * m1) + (1 * m2) + (8 * y1) + (9 * y2) + (4 * i1) + (5 * i2) + (2 * i3)) % 11);
         int cs2 = 11 - (((5 * d1) + (4 * d2) + (3 * m1) + (2 * m2) + (7 * y1) + (6 * y2) + (5 * i1) + (4 * i2) + (3 * i3) + (2 * cs1)) % 11);

         if (cs1 == 11)
         {
            cs1 = 0;
         }

         if (cs2 == 11)
         {
            cs2 = 0;
         }

         $"{cs1}{cs2}".Should().Be(readCs);
      }

      private void IsLegalFødselsnummer(string readFødselsnummer, Person p)
      {
         readFødselsnummer.Should().HaveLength(11);

         int birthYear = int.Parse(readFødselsnummer.Substring(4, 2));
         int indNo = int.Parse(readFødselsnummer.Substring(6, 3));

         IsLegalIndividualNumber(indNo, birthYear, p);
         IsLegalChecksum(readFødselsnummer);
      }

      [Fact]
      public void can_create_norwegian_fødselsnummer()
      {
         var f = new Faker("nb_NO");
         var person = f.Person;

         string fødselsnummer = person.Fødselsnummer();

         IsLegalFødselsnummer(fødselsnummer, person);
      }

      [Fact]
      public void can_create_correct_checksum_1()
      {
         // Test fødselsnummer from DSF.
         IsLegalChecksum("31080700442");
      }

      [Fact]
      public void can_create_correct_checksum_2()
      {
         // Test fødselsnummer from DSF.
         IsLegalChecksum("10050050489");
      }
   }
}

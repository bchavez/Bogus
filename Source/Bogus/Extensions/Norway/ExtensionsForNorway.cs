using System;

namespace Bogus.Extensions.Norway
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForNorway
   {
      /// <summary>
      /// Norwegian national identity number
      /// </summary>
      public static string Fødselsnummer(this Person p)
      {
         const string Key = nameof(ExtensionsForNorway) + "Fødselsnummer";
         if (p.context.ContainsKey(Key))
         {
            return p.context[Key] as string;
         }

         /*
             DDMMYYXXXCC
             | | | |  |--> Checksum
             | | | |
             | | | |
             | | | |-----> Individual number
             | | |-------> Year (last two digits)
             | |---------> Month
             |-----------> Day

             The individual number has to be even for women and odd for men.

             The checksum is calculated with a modulo checksum algorithm.
             If either of the checksum numbers are 10, the fødselsnummer gets
             rejected, and a new individual number has to be generated.

            https://www.skatteetaten.no/en/person/national-registry/birth-and-name-selection/children-born-in-norway/national-id-number/

            https://nn.wikipedia.org/wiki/F%C3%B8dselsnummer

            https://github.com/deegane/NINTool/blob/master/backend/src/main/java/com/nin/validation/NorwegianNinValidator.kt

            https://github.com/magnuswatn/fodselsnummer/blob/master/fodselsnummer.py
         */

         var r = p.Random;
         string birthDate = $"{p.DateOfBirth:ddMMyy}";

         string individualNumber;
         string checksum;
         bool isOkChecksum;

         do
         {
            individualNumber = GenerateIndividualNumber(r, p.Gender, p.DateOfBirth.Year);
            isOkChecksum = GenerateChecksum(birthDate, individualNumber, out checksum);
         } while (!isOkChecksum);

         string final = $"{p.DateOfBirth:ddMMyy}{individualNumber}{checksum}";

         p.context[Key] = final;
         return final;
      }

      private static string GenerateIndividualNumber(Randomizer r, DataSets.Name.Gender gender, int year)
      {
         int from;
         int to;

         if (1854 <= year && year <= 1899)
         {
            from = 500;
            to = 749;
         }
         else if (1900 <= year && year <= 1999)
         {
            from = 0;
            to = 499;
         }
         else if (2000 <= year && year <= 2039)
         {
            from = 500;
            to = 999;
         }
         else
         {
            throw new ArgumentOutOfRangeException(nameof(year), $"{nameof(year)} must be between 1854 and 2039.");
         }

         int individualNumber = gender == DataSets.Name.Gender.Female ? r.Even(from, to) : r.Odd(from, to);

         return individualNumber.ToString("D3");
      }

      private static bool GenerateChecksum(string birthDate, string individualNumber, out string checksum)
      {
         int d1 = int.Parse(birthDate.Substring(0, 1));
         int d2 = int.Parse(birthDate.Substring(1, 1));
         int m1 = int.Parse(birthDate.Substring(2, 1));
         int m2 = int.Parse(birthDate.Substring(3, 1));
         int y1 = int.Parse(birthDate.Substring(4, 1));
         int y2 = int.Parse(birthDate.Substring(5, 1));
         int i1 = int.Parse(individualNumber.Substring(0, 1));
         int i2 = int.Parse(individualNumber.Substring(1, 1));
         int i3 = int.Parse(individualNumber.Substring(2, 1));

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

         checksum = $"{cs1}{cs2}";

         return cs1 < 10 && cs2 < 10;
      }
   }
}
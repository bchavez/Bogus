using System;
using System.Linq;
using Bogus.DataSets;
using static Bogus.DataSets.Name;

namespace Bogus.Extensions.Sweden
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForSweden
   {
      /// <summary>
      /// Swedish national identity number
      /// </summary>
      public static string Personnummer(this Person person)
      {
         const string key = nameof(ExtensionsForSweden) + nameof(Personnummer);
         if (person.context.ContainsKey(key))
         {
            return person.context[key] as string;
         }

         /*

             YYYYMMDDBBGC
             |   | | | ||---> (C)Checksum
             |   | | | | 
             |   | | | |----> (G)Gender
             |   | | |------> (B)Birthplace
             |   | |--------> (D)day 
             |   |----------> (M)Month
             |--------------> (Y)Year
            Source: https://syntaxwarriors.com/p/1021/Generate-a-Random-Swedish-Personnumber-with-control-digit
         
            info: https://skatteverket.se/privat/folkbokforing/personnummerochsamordningsnummer.4.3810a01c150939e893f18c29.html
         */
         var r = person.Random;
         var individualNumber = GenerateIndividualNumber(r, person.Gender, person.DateOfBirth);


         person.context[key] = individualNumber;
         return individualNumber;
      }


      private static string GenerateIndividualNumber(Randomizer r, Gender gender, DateTime dateOfBirth)
      {
         var genderNumber = GetGenderNumber(r, gender);
         var p = dateOfBirth.ToString("yyyyMMddff") + genderNumber;
         var checksum = GetLuhn(p.Substring(2));
         return p + checksum;
      }

      private static int GetGenderNumber(Randomizer r, Gender gender)
      {
         if( gender is Gender.Male )
            return r.Even(1, 9);
         if( gender is Gender.Female )
            return r.Odd(1, 9);
         throw new ArgumentOutOfRangeException(nameof(gender), gender, "Gender not handled.");
      }

      private static int GetLuhn(string value)
      {
         // Luhn algorithm doubles every other number in the value.
         // To get the correct checksum digit we aught to append a 0 on the sequence.
         // If the result becomes a two digit number, subtract 9 from the value.
         // If the total sum is not a 0, the last checksum value should be subtracted from 10.
         // The resulting value is the check value that we use as control number.

         // The value passed is a string, so we aught to get the actual integer value from each char (i.e., subtract '0' which is 48).
         var t = value.ToCharArray().Select(d => d - 48).ToArray();
         var sum = 0;
         int temp;
         for (var i = 0; i < t.Length; i++)
         {
            temp = t[i];
            temp *= 2 - (i % 2);
            if (temp > 9)
            {
               temp -= 9;
            }

            sum += temp;
         }

         return ((int) Math.Ceiling(sum / 10.0)) * 10 - sum;
      }
   }
}
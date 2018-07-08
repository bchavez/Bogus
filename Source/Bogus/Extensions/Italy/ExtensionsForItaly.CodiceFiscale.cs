using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bogus.Extensions.Italy
{
   /// <summary>
   /// This class implements the principal routines of the Italian fiscal code,
   /// used to unambiguously identify individuals residing in Italy.
   /// <see cref="https://en.wikipedia.org/wiki/Italian_fiscal_code_card"/>
   /// </summary>
   internal static class CodiceFiscaleGenerator
   {
      /// <summary>
      /// Map used by the algorithm for even characters
      /// </summary>
      private static readonly Dictionary<char, int> evenMap = new Dictionary<char, int>
      {
         { '0', 0 },
         { '1', 1 },
         { '2', 2 },
         { '3', 3 },
         { '4', 4 },
         { '5', 5 },
         { '6', 6 },
         { '7', 7 },
         { '8', 8 },
         { '9', 9 },
         { 'A', 0 },
         { 'B', 1 },
         { 'C', 2 },
         { 'D', 3 },
         { 'E', 4 },
         { 'F', 5 },
         { 'G', 6 },
         { 'H', 7 },
         { 'I', 8 },
         { 'J', 9 },
         { 'K', 10 },
         { 'L', 11 },
         { 'M', 12 },
         { 'N', 13 },
         { 'O', 14 },
         { 'P', 15 },
         { 'Q', 16 },
         { 'R', 17 },
         { 'S', 18 },
         { 'T', 19 },
         { 'U', 20 },
         { 'V', 21 },
         { 'W', 22 },
         { 'X', 23 },
         { 'Y', 24 },
         { 'Z', 25 }
      };

      /// <summary>
      /// Array that maps months onto letters
      /// </summary>
      private static readonly char[] monthChars = { 'A', 'B', 'C', 'D', 'E', 'H', 'L', 'M', 'P', 'R', 'S', 'T' };

      /// <summary>
      /// Map used by the algorithm for odd characters
      /// </summary>
      private static readonly Dictionary<char, int> oddMap = new Dictionary<char, int>
      {
         { '0', 1 },
         { '1', 0 },
         { '2', 5 },
         { '3', 7 },
         { '4', 9 },
         { '5', 13 },
         { '6', 15 },
         { '7', 17 },
         { '8', 19 },
         { '9', 21 },
         { 'A', 1 },
         { 'B', 0 },
         { 'C', 5 },
         { 'D', 7 },
         { 'E', 9 },
         { 'F', 13 },
         { 'G', 15 },
         { 'H', 17 },
         { 'I', 19 },
         { 'J', 21 },
         { 'K', 2 },
         { 'L', 4 },
         { 'M', 18 },
         { 'N', 20 },
         { 'O', 11 },
         { 'P', 3 },
         { 'Q', 6 },
         { 'R', 8 },
         { 'S', 12 },
         { 'T', 14 },
         { 'U', 16 },
         { 'V', 10 },
         { 'W', 22 },
         { 'X', 25 },
         { 'Y', 24 },
         { 'Z', 23 }
      };

      /// <summary>
      ///   Generates an Italian Fiscal Code
      /// </summary>
      /// <param name="lastName">Last name of the holder</param>
      /// <param name="firstName">First name of the holder</param>
      /// <param name="birthday">Birthday of the holder</param>
      /// <param name="male">Indicates whether the holder is male</param>
      /// <param name="validChecksum">
      ///   Indicates whether the generated Fiscal Code has a valid checksum or not
      /// </param>
      /// <returns>The generated Fiscal Code</returns>
      public static string Generate(
         string lastName,
         string firstName,
         DateTime birthday,
         bool male,
         bool validChecksum = true)
      {
         var sb = new StringBuilder();
         sb.Append(GetFiscalCodeSqueezedName(lastName, false));
         sb.Append(GetFiscalCodeSqueezedName(firstName, true));
         sb.Append((birthday.Year % 100).ToString("00"));
         sb.Append(monthChars[birthday.Month - 1]);
         sb.Append((birthday.Day + (male ? 0 : 40)).ToString("00"));

         //To guarantee code stability for a person, we generate
         //fake city-of-birth code through surname and birth date.
         //Using actual city code database would be too heavy.
         sb.Append(lastName[0].ToString().ToUpper());
         var birthDatePositiveHash = Math.Abs(birthday.GetHashCode());
         sb.Append((birthDatePositiveHash % 1000).ToString("000"));

         var checksum = ComputeChecksumCodiceFiscale(sb.ToString(), validChecksum);
         sb.Append(checksum);

         return sb.ToString();
      }

      /// <summary>
      /// Checksum computation algorithm
      /// </summary>
      /// <param name="prefix">The code</param>
      /// <param name="validChecksum">Indicates whether the computed checksum must be valid or not</param>
      private static char ComputeChecksumCodiceFiscale(string prefix, bool validChecksum)
      {
         int total = 0;
         for( int i = 0; i < 15; i += 2 )
         {
            total += oddMap[prefix[i]];
         }

         for( int i = 1; i < 15; i += 2 )
         {
            total += evenMap[prefix[i]];
         }

         if( !validChecksum )
         {
            total++; // set wrong checksum
         }

         return (char)('A' + (total % 26));
      }

      /// <summary>
      ///   This method applies the rule giving the consonants and vowels extracted by the name,
      ///   according to the algorithm.
      /// </summary>
      /// <param name="name">The name to process</param>
      /// <param name="isFirstName">true, in case of first names</param>
      /// <returns>The squeezed name</returns>
      private static string GetFiscalCodeSqueezedName(string name, bool isFirstName)
      {
         var sb = new StringBuilder();
         var normalizedName = name.ToUpperInvariant();
         var regex = new Regex("[^A-Z]");
         normalizedName = regex.Replace(normalizedName, string.Empty);

         // manages first name special case (first names having more than 3 consonants -> the 2nd
         // is skipped)
         var consonantToSkipIdx = -1;
         if( isFirstName )
         {
            var consonantCount = 0;
            for( int i = 0; i < normalizedName.Length; i++ )
            {
               if( !IsVowel(normalizedName[i]) )
               {
                  consonantCount++;
                  if( consonantCount == 2 )
                  {
                     consonantToSkipIdx = i;
                  }
               }
            }

            if( consonantCount <= 3 )
            {
               consonantToSkipIdx = -1;
            }
         }

         // add consonants
         for( int i = 0; i < normalizedName.Length; i++ )
         {
            if( !IsVowel(normalizedName[i]) && (i != consonantToSkipIdx) )
            {
               sb.Append(normalizedName[i]);
               if( sb.Length == 3 )
               {
                  return sb.ToString();
               }
            }
         }

         // add vowels
         for( int i = 0; i < normalizedName.Length; i++ )
         {
            if( IsVowel(normalizedName[i]) )
            {
               sb.Append(normalizedName[i]);
               if( sb.Length == 3 )
               {
                  return sb.ToString();
               }
            }
         }

         // add padding X
         while( sb.Length < 3 )
         {
            sb.Append("X");
         }

         return sb.ToString();
      }

      /// <summary>
      /// Indicates whether a char is a vowel
      /// </summary>
      /// <param name="c">The char to test</param>
      /// <returns>True if is is a vowel, false otherwise</returns>
      private static bool IsVowel(char c)
      {
         var vowels = new[] { 'A', 'E', 'I', 'O', 'U' };
         return vowels.Contains(c);
      }
   }
}
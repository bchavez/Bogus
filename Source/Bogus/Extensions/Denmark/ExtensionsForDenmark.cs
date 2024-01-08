using static Bogus.DataSets.Name;
using System;

namespace Bogus.Extensions.Denmark;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForDenmark
{
   /// <summary>
   /// Danish Personal Identification number
   /// </summary>
   /// <param name="p">The holder.</param>
   /// <param name="validChecksum">
   ///   Indicates whether the generated CPR number should have a valid checksum or not.
   /// </param>
   public static string Cpr(this Person p, bool validChecksum = true, bool includeDash = true)
   {
      const string Key = nameof(ExtensionsForDenmark) + "CPR";
      if (p.context.ContainsKey(Key))
      {
         return p.context[Key] as string;
      }

      /*
          DDMMYY-XXXX
          | | |  |  
          | | |  |
          | | |  |
          | | |  |----> (X)Individual number
          | | |-------> (Y)Year (last two digits)
          | |---------> (M)Month
          |-----------> (D)Day

          The individual number has to be even for women and odd for men.

          As of 2007 there is no longer a requirement for a checksum with a modulo algorithm.
      
         https://cpr.dk/cpr-systemet/opbygning-af-cpr-nummeret
            
         https://da.wikipedia.org/wiki/CPR-nummer
               
         https://www.cprgenerator.net/metode
      */

      var r = p.Random;
      string birthDate = $"{p.DateOfBirth:ddMMyy}";
      string individualNumber;
      string checksum;
      bool hasValidChecksum;

      if (validChecksum)
      {
         do
         {
            individualNumber = GenerateIndividualThreeDigitNumber(r, p.DateOfBirth.Year);
            hasValidChecksum = GenerateChecksum(birthDate, p.Gender, individualNumber, out checksum);
         } while (!hasValidChecksum);
      }
      else
      {
         checksum = string.Empty;
         individualNumber = GenerateIndividualFourDigitNumber(r, p.Gender, p.DateOfBirth.Year);
      }

      string final;
      if( includeDash ) {
         final = $"{birthDate}-{individualNumber}{checksum}";
      }
      else
      {
         final = $"{birthDate}{individualNumber}{checksum}";
      }

      p.context[Key] = final;
      return final;
   }

   private static string GenerateIndividualFourDigitNumber(Randomizer r, DataSets.Name.Gender gender, int year)
   {
      int from;
      int to;

      switch( year )
      {
         case >= 1858 and <= 1899:
            from = 5000;
            to = 8999;
            break;
         case >= 1900 and <= 1936:
            from = 0;
            to = 3999;
            break;
         case >= 1937 and <= 1999:
            from = 0;
            to = 4999;
            break;
         case >= 2000 and <= 2036:
            from = 4000;
            to = 9999;
            break;
         case >= 2037 and <= 2057:
            from = 5000;
            to = 9999;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(year), $"{nameof(year)} must be between 1858 and 2057.");
      }

      int individualNumber = gender == DataSets.Name.Gender.Female ? r.Even(from, to) : r.Odd(from, to);

      return individualNumber.ToString("D4");
   }

   private static string GenerateIndividualThreeDigitNumber(Randomizer r, int year)
   {
      int from;
      int to;

      switch( year )
      {
         case >= 1858 and <= 1899:
            from = 500;
            to = 899;
            break;
         case >= 1900 and <= 1936:
            from = 0;
            to = 399;
            break;
         case >= 1937 and <= 1999:
            from = 0;
            to = 499;
            break;
         case >= 2000 and <= 2036:
            from = 400;
            to = 999;
            break;
         case >= 2037 and <= 2057:
            from = 500;
            to = 999;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(year), $"{nameof(year)} must be between 1858 and 2057.");
      }

      int individualNumber = r.Int(from, to);

      return individualNumber.ToString("D3");
   }

   private static bool GenerateChecksum(string birthDate, DataSets.Name.Gender gender, string individualNumber, out string checksum)
   {
      var factors = new[] { 4, 3, 2, 7, 6, 5, 4, 3, 2 };
      var digits = (birthDate + individualNumber).ToCharArray();

      int cs = 0;
      for (int i = 0; i < 9; i++)
      {
         cs += (digits[i] - '0') * factors[i];
      }

      cs = 11 - (cs % 11);

      if (cs == 11)
      {
         cs = 0;
      }

      checksum = $"{cs}";

      if (gender == Gender.Female && cs % 2 != 0) return false;
      if (gender == Gender.Male && cs % 2 == 0) return false;

      return cs < 10;
   }
}
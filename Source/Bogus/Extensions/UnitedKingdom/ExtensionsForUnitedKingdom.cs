using System;

using Bogus.Bson;
using Bogus.DataSets;

namespace Bogus.Extensions.UnitedKingdom;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForUnitedKingdom
{
   /// <summary>
   /// Banking Sort Code
   /// </summary>
   public static string SortCode(this Finance finance, bool includeSeparator = true)
   {
      const string withSeparator = "##-##-##";
      const string withoutSeparator = "######";

      if( includeSeparator )
      {
         return finance.Random.ReplaceNumbers(withSeparator);
      }

      return finance.Random.ReplaceNumbers(withoutSeparator);
   }

   /// <summary>
   /// National Insurance Number
   /// </summary>
   public static string Nino(this Finance finance, bool includeSeparator = true)
   {
      const string valid1stPrefixChars =   "ABCEGHJKLMNOPRSTWXYZ";
      //const string valid2ndPrefixChars = "ABCEGHJKLMN PRSTWXYZ";
      const string validSuffixChars = "ABCD";

      var prefix = finance.Random.String2(2, chars: valid1stPrefixChars);

      if (prefix.EndsWith("O"))
      {  //second character in prefix can't end with an 'O'
         //Remap O to an X.
         prefix = prefix.Replace('O', 'X');
      }

      //check for invalid prefixes
      if (prefix == "GB" || prefix == "BG" || prefix == "NK" ||
          prefix == "KN" || prefix == "TN" || prefix == "NT" ||
          prefix == "ZZ")
      {
         //if the prefix is any of the invalid prefixes,
         //Remap an invalid prefix to a well known valid one.
         prefix = "CE";
      }

      var suffix = finance.Random.String2(1, validSuffixChars);

      if (includeSeparator)
      {
         return finance.Random.ReplaceNumbers($"{prefix} ## ## ## {suffix}");
      }
      return finance.Random.ReplaceNumbers($"{prefix}######{suffix}");
   }

   /// <summary>
   /// Country of the United Kingdom
   /// </summary>
   public static string CountryOfUnitedKingdom(this Address address)
   {
      var countries = Database.Get(nameof(address), "uk_country", "en_GB") as BArray;
      return address.Random.ArrayElement(countries);
   }

   /// <summary>
   /// Generates a UK compatible VAT registration number.
   /// </summary>
   /// <param name="finance">A reference to <see cref="Finance"/>.</param>
   /// <param name="registrationNumberType">The type of vat registration number to generate.</param>
   /// <param name="includeSeparator">Indicates the resultant string should be spaced according to VAT registration formatting defaults.</param>
   /// <returns>A string representation of a VAT registration number.</returns>
   /// <exception cref="NotImplementedException">Exception thrown if the requested registration number is not supported.</exception>
   public static string VatNumber(this Finance finance, VatRegistrationNumberType registrationNumberType, bool includeSeparator = true)
   {
      int min = 0;
      int max = 9_999_999;
      switch( registrationNumberType )
      {
         case VatRegistrationNumberType.HealthAuthority:
            min = 500;
            max = 999;
            break;
         case VatRegistrationNumberType.GovernmentDepartment:
            min = 0;
            max = 499;
            break;
      }

      int random = finance.Random.Int(min, max);

      switch( registrationNumberType )
      {
         case VatRegistrationNumberType.HealthAuthority:
            return includeSeparator
               ? $"GB HA {random:000}"
               : $"GBHA{random:000}";
         case VatRegistrationNumberType.GovernmentDepartment:
            return includeSeparator
               ? $"GB GD {random:000}"
               : $"GBGD{random:000}";
         case VatRegistrationNumberType.Standard:
            int checksum = CalculateChecksum(random);
            return includeSeparator
               ? $"GB {random:000 0000} {checksum:00}"
               : $"GB{random:0000000}{checksum:00}";
         case VatRegistrationNumberType.BranchTrader:
            checksum = CalculateChecksum(random);
            string suffix = finance.Random.ReplaceNumbers("###");
            return includeSeparator
               ? $"GB {random:000 0000} {checksum:00} {suffix}"
               : $"GB{random:0000000}{checksum:00}{suffix}";
         default:
            throw new NotImplementedException($"[{registrationNumberType}] is not a supported [{nameof(VatRegistrationNumberType)}].");
      }
   }

   private static int CalculateChecksum(int n)
   {
      int total = 0;
      int divisor = 1;

      while( n / divisor >= 10 )
      {
         divisor *= 10;
      }

      int index = 0;
      while( divisor > 0 )
      {
         int digit = n / divisor;
         total += digit * (8 - index);

         n %= divisor;
         divisor /= 10;
         index++;
      }

      while( total > 0 )
      {
         total -= 97;
      }

      return Math.Abs(total);
   }
}

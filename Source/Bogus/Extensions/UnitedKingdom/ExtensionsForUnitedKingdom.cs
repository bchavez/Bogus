using Bogus.DataSets;

namespace Bogus.Extensions.UnitedKingdom
{
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
         string[] invalidPrefixes = { "GB", "BG", "NK", "KN", "TN", "NT", "ZZ" };
         char[] valid1stPrefixChars = { 'A','B','C','E','G','H','J','K','L','M','N','O','P','R','S','T','W','X','Y','Z' };
         char[] valid2ndPrefixChars = { 'A','B','C','E','G','H','J','K','L','M','N',    'P','R','S','T','W','X','Y','Z' };
         char[] validSuffixChars = { 'A', 'B', 'C', 'D' };

         string prefix;
         do
         {
            prefix = "";
            prefix += finance.Random.ArrayElement(valid1stPrefixChars);
            prefix += finance.Random.ArrayElement(valid2ndPrefixChars);
         } while (invalidPrefixes.Any(x => x.Equals(prefix)));

         char suffix = finance.Random.ArrayElement(validSuffixChars);

         if (includeSeparator)
         {
            return finance.Random.ReplaceNumbers($"{prefix} ## ## ## {suffix}");
         }
         return finance.Random.ReplaceNumbers($"{prefix}######{suffix}");
      }
   }
}

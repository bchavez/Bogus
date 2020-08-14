using Bogus.Bson;
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
   }
}

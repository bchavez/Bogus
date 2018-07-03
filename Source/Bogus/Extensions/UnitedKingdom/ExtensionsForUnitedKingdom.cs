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
      public static string Nin(this Finance finance, bool includeSeparator = true)
      {
         const string withSeparator = "?? ## ## ## ?";
         const string withoutSeparator = "??######?";
         
         if( includeSeparator )
         {
            return finance.Random.Replace(withSeparator);
         }

         return finance.Random.Replace(withoutSeparator);
      }
   }
}

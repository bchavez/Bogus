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
      public static string SortCode(this Finance finance, bool includeSeperator = true)
      {
         const string withSeperators = "##-##-##";
         const string withoutSeperators = "######";

         if( includeSeperator )
         {
            return finance.Random.ReplaceNumbers(withSeperators);
         }

         return finance.Random.ReplaceNumbers(withoutSeperators);
      }
   }
}
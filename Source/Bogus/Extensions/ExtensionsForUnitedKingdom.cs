using Bogus.DataSets;

namespace Bogus.Extensions.UnitedKingdom
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForUnitedKingdom
   {
      private static Randomizer r = new Randomizer();
      /// <summary>
      /// Banking Short Code
      /// </summary>
      public static string ShortCode(this Finance finance, bool includeSeperator = true)
      {
         const string withSeperators = "##-##-##";
         const string withoutSeperators = "######";

         if( includeSeperator )
         {
            return r.ReplaceNumbers(withSeperators);
         }

         return r.ReplaceNumbers(withoutSeperators);
      }
   }
}
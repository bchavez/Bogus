namespace Bogus.Extensions.France
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForFrance
   {
      /// <summary>
      /// Numéro de sécurité sociale
      /// </summary>
      public static string Nss(this Person p)
      {
         const string Key = nameof(ExtensionsForFrance) + "NSS";

         if (p.context.ContainsKey(Key))
         {
            return p.context[Key] as string;
         }

         var randomizer = p.Random;
         var nss = randomizer.ReplaceNumbers("# ## ## ## ### ### ##");

         p.context[Key] = nss;

         return nss;
      }
   }
}

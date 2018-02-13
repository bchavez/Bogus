namespace Bogus.Extensions.Denmark
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForDenmark
   {
      /// <summary>
      /// Danish Personal Identification number
      /// </summary>
      public static string Cpr(this Person p)
      {
         const string Key = nameof(ExtensionsForDenmark) + "CPR";
         if( p.context.ContainsKey(Key) )
         {
            return p.context[Key] as string;
         }

         var r = p.Random;
         var final = $"{p.DateOfBirth:ddMMyy}-{r.Replace("####")}";

         p.context[Key] = final;
         return final;
      }
   }
}
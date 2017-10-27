namespace Bogus.Extensions.UnitedStates
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForUnitedStates
   {
      /// <summary>
      /// Social Security Number
      /// </summary>
      public static string Ssn(this Person p)
      {
         const string Key = nameof(ExtensionsForUnitedStates) + "SSN";

         if (p.context.ContainsKey(Key))
         {
            return p.context[Key] as string;
         }

         var randomizer = new Randomizer();
         var ssn = randomizer.ReplaceNumbers("###-##-####");

         p.context[Key] = ssn;

         return ssn;
      }
   }
}
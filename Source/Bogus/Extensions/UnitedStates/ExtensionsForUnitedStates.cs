using Bogus.DataSets;

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

         if( p.context.ContainsKey(Key) )
         {
            return p.context[Key] as string;
         }

         var randomizer = p.Random;

         //See Issue 260, SSN validity:
         // https://secure.ssa.gov/apps10/poms.nsf/lnx/0110201035

         var a = randomizer.Int(1, 898);
         if (a == 666) a++;

         var b = randomizer.Int(1, 99);
         var c = randomizer.Int(1, 9999);

         var ssn = $"{a:000}-{b:00}-{c:0000}";

         p.context[Key] = ssn;

         return ssn;
      }

      /// <summary>
      /// Employer Identification Number
      /// </summary>
      public static string Ein(this Company c)
      {
         return c.Random.ReplaceNumbers("##-#######");
      }
   }
}
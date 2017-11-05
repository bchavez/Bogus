using System.Linq;

namespace Bogus.Extensions.Canada
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForCanada
   {
      private static int[] Mask = {1, 2, 1, 2, 1, 2, 1, 2, 1};

      /// <summary>
      /// Social Insurance Number for Canada 
      /// </summary>
      public static string Sin(this Person p)
      {
         const string Key = nameof(ExtensionsForCanada) + "SIN";
         if( p.context.ContainsKey(Key) )
         {
            return p.context[Key] as string;
         }

         //bit verbose, but works. :)
         //could be mathematically simplified.
         //brute forced this one. yeah.
         //d
         //should pass basic validation, but only some 
         //numbers dont start with 8 etc.

         /*
         1 — Atlantic Provinces: Nova Scotia, New Brunswick, Prince Edward Island, and Newfoundland and Labrador (this may also cover overseas residents).
         2–3 — Quebec
         4–5 — Ontario (#4 includes overseas forces)
         6 — Prairie Provinces (Manitoba, Saskatchewan, and Alberta), Northwest Territories, and Nunavut
         7 — Pacific Region (British Columbia and Yukon)
         8 — Not used
         9 — Temporary resident
         0 — Not used (Canada Revenue may assign fictitious SIN numbers beginning with zero to taxpayers who do not have SINs)
         */

         var r = p.Random;
         //get 8 numbers
         var numbers = r.Digits(8);

         // the last number that makes it pass the checksum.
         var last = 10 - (numbers.Sum() % 10);
         if( last == 10 )
            last = 0;

         var digits = numbers.Concat(new[] {last}).ToArray();

         var comp = digits
            .Zip(Mask, (n, c) =>
               {
                  if( c == 2 && n % c == 1 )
                  {
                     // odd digit, it was multiplied, reverse the process
                     return (10 + (n - 1)) / 2;
                  }
                  if( c == 2 )
                  {
                     //simply divide an even number by two
                     return n / 2;
                  }
                  //else c == 1, and n was multiplied by 1
                  return n;
               }).ToArray();


         var sinstr = $"{comp[0]}{comp[1]}{comp[2]} {comp[3]}{comp[4]}{comp[5]} {comp[6]}{comp[7]}{comp[8]}";

         p.context[Key] = sinstr;

         return sinstr;
      }
   }
}
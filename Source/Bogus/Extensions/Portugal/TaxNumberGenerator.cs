using System.Linq;

namespace Bogus.Extensions.Portugal
{
   internal static class TaxNumberGenerator
   {
      public static readonly int[] NifIdentify = { 1, 2 };
      public static readonly int[] NipcIdentify = { 5, 6, 8, 9 };
      private static readonly int[] Weights = { 9, 8, 7, 6, 5, 4, 3, 2 };

      /// <summary>
      /// Rules for generate the last number for the combination
      /// </summary>
      /// <param name="arrNumber">The array number for calculate</param>
      public static string Create(int[] arrNumber)
      {
         var sum1 = arrNumber.Zip(Weights, (d, w) => d * w)
            .Sum();

         var sum1mod = sum1 % 11;

         var check1 = 0;
         if (sum1mod >= 2)
         {
            check1 = 11 - sum1mod;
         }

         var all = arrNumber.Concat(new[] { check1 }).ToArray();

         return $"{all[0]}{all[1]}{all[2]}{all[3]}{all[4]}{all[5]}{all[6]}{all[7]}{all[8]}";
      }
   }
}
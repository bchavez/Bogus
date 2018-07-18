using Bogus.DataSets;
using System.Linq;

namespace Bogus.Extensions.Portugal
{
   /// <summary>
   /// API extensions specific for Portugal.
   /// </summary>
   public static class ExtensionsForPortugal
   {
      private static readonly int[] NifIdentify = { 1, 2 };
      private static readonly int[] NipcIdentify = { 5, 6, 8, 9 };
      private static readonly int[] Weights = { 9, 8, 7, 6, 5, 4, 3, 2 };

      /// <summary>
      /// Rules for generate the last number for the combination
      /// </summary>
      /// <param name="arrNumber">The array number for calculate</param>
      /// <returns></returns>
      private static string TaxNumberGenerator(int[] arrNumber)
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

      /// <summary>
      /// Número de Identificação Fiscal (NIF)
      /// </summary>
      /// <remarks>
      /// Tax identification number. Also referred to as Taxpayer Number, identifies a tax entity that is a taxpayer in Portugal, whether a company or a natural person.
      /// </remarks>
      /// <param name="p">Object will receive the NIF value</param>
      public static string Nif(this Person p)
      {
         const string Key = nameof(ExtensionsForPortugal) + "NIF";
         if (p.context.ContainsKey(Key))
         {
            return p.context[Key] as string;
         }
         
         var id =  new[] { NifIdentify[p.Random.Int(0, NifIdentify.Length - 1)] };
         var digits = p.Random.Digits(7);

         var nifNumber = id.Concat(digits).ToArray();

         var final = TaxNumberGenerator(nifNumber);

         p.context[Key] = final;

         return final;
      }

      /// <summary>
      /// Número de Identificação de Pessoa Colectiva (NIPC)
      /// </summary>
      /// <remarks>
      /// Tax identification number for companies. A Collective Identification Number is the most correct term to refer to a company's NIF. The first digit can be 5, 6 public collective, 8, irregular legal person or provisional number.
      /// </remarks>
      /// <param name="c">Object will receive the NIPC value</param>
      public static string Nipc(this Company c)
      {
         var id = new[] { NipcIdentify[c.Random.Int(0, NipcIdentify.Length - 1)] };
         var digits = c.Random.Digits(7);

         var nipcNumber = id.Concat(digits).ToArray();

         return TaxNumberGenerator(nipcNumber);
      }
   }
}
using Bogus.DataSets;
using System.Linq;

namespace Bogus.Extensions.Portugal
{
   /// <summary>
   /// API extensions specific for Portugal.
   /// </summary>
   public static class ExtensionsForPortugal
   {
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

         var id = new[] {p.Random.ArrayElement(TaxNumberGenerator.NifIdentify)};
         var digits = p.Random.Digits(7);

         var nifNumber = id.Concat(digits).ToArray();

         var final = TaxNumberGenerator.Create(nifNumber);

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
         var id = new[] {c.Random.ArrayElement(TaxNumberGenerator.NipcIdentify)};
         var digits = c.Random.Digits(7);

         var nipcNumber = id.Concat(digits).ToArray();

         return TaxNumberGenerator.Create(nipcNumber);
      }
   }
}
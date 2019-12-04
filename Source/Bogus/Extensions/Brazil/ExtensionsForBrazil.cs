using System.Linq;
using Bogus.DataSets;

namespace Bogus.Extensions.Brazil
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForBrazil
   {
      private static readonly int[] CpfWeights = {10, 9, 8, 7, 6, 5, 4, 3, 2};
      private static readonly int[] CnpjWeights = {2, 3, 4, 5, 6, 7, 8, 9, 2, 3, 4, 5, 6};

      /// <summary>
      /// Cadastro de Pessoas Físicas
      /// </summary>
      /// <param name="includeFormatSymbols">Includes formatting symbols.</param>
      public static string Cpf(this Person p, bool includeFormatSymbols = true)
      {
         int[] finalDigits;

         const string Key = nameof(ExtensionsForBrazil) + "CPF";
         if( p.context.ContainsKey(Key) )
         {
            finalDigits = p.context[Key] as int[];
            return FormatCpf(finalDigits, includeFormatSymbols);
         }

         var digits = p.Random.Digits(9);
         var sum1 = digits.Zip(CpfWeights, (d, w) => d * w)
            .Sum();

         var sum1mod = sum1 % 11;

         int check1 = 0;
         if( sum1mod >= 2 )
         {
            check1 = 11 - sum1mod;
         }

         var finalWeights = new[] {11}.Concat(CpfWeights);

         var sum2 = digits.Concat(new[] {check1})
            .Zip(finalWeights, (d, w) => d * w)
            .Sum();

         var sum2mod = sum2 % 11;

         var check2 = 0;
         if( sum2mod >= 2 )
         {
            check2 = 11 - sum2mod;
         }

         finalDigits = digits.Concat(new[] {check1, check2}).ToArray();

         p.context[Key] = finalDigits;

         return FormatCpf(finalDigits, includeFormatSymbols);
      }

      public static string FormatCpf(int[] digits, bool includeFormatSymbols)
      {
         if (includeFormatSymbols)
         {
            return $"{digits[0]}{digits[1]}{digits[2]}.{digits[3]}{digits[4]}{digits[5]}.{digits[6]}{digits[7]}{digits[8]}-{digits[9]}{digits[10]}";
         }
         else
         {
            return $"{digits[0]}{digits[1]}{digits[2]}{digits[3]}{digits[4]}{digits[5]}{digits[6]}{digits[7]}{digits[8]}{digits[9]}{digits[10]}";
         }
      }


      /// <summary>
      /// Cadastro Nacional da Pessoa Jurídica
      /// </summary>
      /// <param name="includeFormatSymbols">Includes formatting symbols.</param>
      public static string Cnpj(this Company c, bool includeFormatSymbols = true)
      {
         var digits = c.Random.Digits(12);
         digits[8] = 0;
         digits[9] = 0;
         digits[10] = 0;
         digits[11] = 1;

         var firstDigit = digits.Reverse().Zip(CnpjWeights, (d, w) => d * w).Sum();
         firstDigit = 11 - firstDigit % 11;

         if( firstDigit >= 10 )
            firstDigit = 0;

         var secondDigit = firstDigit * 2 + digits.Reverse().Zip(CnpjWeights.Skip(1), (d, w) => d * w).Sum();
         secondDigit = 11 - (secondDigit % 11);
         if( secondDigit >= 10 )
            secondDigit = 0;

         var all = digits.Concat(new[] {firstDigit, secondDigit}).ToArray();

         string final;
         if ( includeFormatSymbols )
         {
            final = $"{all[0]}{all[1]}.{all[2]}{all[3]}{all[4]}.{all[5]}{all[6]}{all[7]}/{all[8]}{all[9]}{all[10]}{all[11]}-{all[12]}{all[13]}";
         }
         else
         {
            final = $"{all[0]}{all[1]}{all[2]}{all[3]}{all[4]}{all[5]}{all[6]}{all[7]}{all[8]}{all[9]}{all[10]}{all[11]}{all[12]}{all[13]}";
         }

         return final;
      }
   }
}
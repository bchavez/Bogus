using System.Linq;
using Bogus.DataSets;

namespace Bogus.Extensions.Russia;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForRussia
{
   private static readonly int[] InnIndividualFirstWeights = {7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
   private static readonly int[] InnIndividualSecondWeights = {3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
   private static readonly int[] InnLegalWeights = {2, 4, 10, 3, 5, 9, 4, 6, 8};

   /// <summary>
   /// Страховой номер индивидуального лицевого счёта (СНИЛС)
   /// </summary>
   /// <param name="includeFormatSymbols">Includes formatting symbols.</param>
   public static string Snils(this Person p, bool includeFormatSymbols = true)
   {
      // https://ru.wikipedia.org/wiki/Страховой_номер_индивидуального_лицевого_счёта
      const string Key = nameof(ExtensionsForRussia) + nameof(Snils);
      if( p.context.TryGetValue(Key, out var value) )
      {
         return FormatSnils((int[])value, includeFormatSymbols);
      }

      var digits = p.Random.Digits(9);

      var sum = 0;
      for( var i = 0; i < digits.Length; i++ )
         sum += digits[i] * (9 - i);

      var check = sum % 101;
      if( check == 100 )
         check = 0;

      var finalDigits = digits.Concat(new[] {check / 10, check % 10}).ToArray();
      p.context[Key] = finalDigits;

      return FormatSnils(finalDigits, includeFormatSymbols);
   }

   private static string FormatSnils(int[] digits, bool includeFormatSymbols)
   {
      if( includeFormatSymbols )
      {
         return $"{digits[0]}{digits[1]}{digits[2]}-{digits[3]}{digits[4]}{digits[5]}-{digits[6]}{digits[7]}{digits[8]} {digits[9]}{digits[10]}";
      }

      return string.Concat(digits);
   }

   /// <summary>
   /// Индивидуальный номер налогоплательщика (ИНН) физического лица
   /// </summary>
   public static string Inn(this Person p)
   {
      // https://ru.wikipedia.org/wiki/Идентификационный_номер_налогоплательщика
      const string Key = nameof(ExtensionsForRussia) + nameof(Inn);
      if( p.context.TryGetValue(Key, out var value) )
      {
         return (string)value;
      }

      var digits = p.Random.Digits(10);
      digits[0] = p.Random.Number(1, 9);

      var firstCheck = WeightedMod(digits, InnIndividualFirstWeights);
      var withFirstCheck = digits.Concat(new[] {firstCheck}).ToArray();
      var secondCheck = WeightedMod(withFirstCheck, InnIndividualSecondWeights);

      var inn = string.Concat(digits) + firstCheck + secondCheck;
      p.context[Key] = inn;

      return inn;
   }

   /// <summary>
   /// Индивидуальный номер налогоплательщика (ИНН) юридического лица
   /// </summary>
   public static string Inn(this Company c)
   {
      // https://ru.wikipedia.org/wiki/Идентификационный_номер_налогоплательщика
      var digits = c.Random.Digits(9);
      digits[0] = c.Random.Number(1, 9);

      var check = WeightedMod(digits, InnLegalWeights);

      return string.Concat(digits) + check;
   }

   private static int WeightedMod(int[] digits, int[] weights)
   {
      var sum = 0;
      for( var i = 0; i < weights.Length; i++ )
         sum += digits[i] * weights[i];

      return (sum % 11) % 10;
   }
}

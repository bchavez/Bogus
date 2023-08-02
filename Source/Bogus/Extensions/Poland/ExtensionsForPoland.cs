using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bogus.Extensions.Poland;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForPoland
{
   /// <summary>
   /// Number of Powszechny Elektroniczny System Ewidencji Ludności (PESEL)
   /// </summary>
   /// <param name="person">Person born between 1800 and 2300</param>
   public static string Pesel(this Person person)
   {
      // https://en.wikipedia.org/wiki/PESEL

      const string Key = nameof(ExtensionsForPoland) + nameof(Pesel);
      if (person.context.ContainsKey(Key))
      {
         return person.context[Key] as string;
      }

      return new StringBuilder()
         .AppendPeselDateOfBirth(person.DateOfBirth)
         .Append(person.Random.Number(9))
         .Append(person.Random.Number(9))
         .Append(person.Random.Number(9))
         .AppendPeselGender(person)
         .AppendPeselChecksum()
         .ToString();
   }

   private static StringBuilder AppendPeselDateOfBirth(this StringBuilder builder, DateTime dateOfBirth)
   {
      int monthShift = dateOfBirth.Year switch
      {
         < 1800 => throw new ArgumentOutOfRangeException("PESEL for the year below 1800 is invalid."),
         < 1900 => 80,
         < 2000 => 0,
         < 2100 => 20,
         < 2200 => 40,
         < 2300 => 60,
         _ => throw new ArgumentOutOfRangeException("PESEL for year above 2300 is invalid."),
      };

      return builder
         .Append((dateOfBirth.Year % 100).ToString("00"))
         .Append((dateOfBirth.Month+monthShift).ToString("00"))
         .Append(dateOfBirth.Day.ToString("00"));
   }

   private static StringBuilder AppendPeselGender(this StringBuilder builder, Person person)
   {
      return builder
         .Append(person.Gender == Name.Gender.Male
            ? person.Random.Odd(0, 9)
            : person.Random.Even(0, 9));
   }

   private static StringBuilder AppendPeselChecksum(this StringBuilder builder)
   {
      int m = 0;

      for (int i = 0; i < builder.Length; i++)
         m += (int)char.GetNumericValue(builder[i]) * PeselWeights[i];

      m %= 10;

      return builder.Append(m == 0 ? 0 : 10-m);
   }

   private static readonly int[] PeselWeights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3, 1 };

   /// <summary>
   /// Numer identyfikacji podatkowej (NIP)
   /// </summary>
   /// <remarks>
   /// Consists only of digits
   /// </remarks>
   public static string Nip(this Company company)
   {
      // https://pl.wikipedia.org/wiki/Numer_identyfikacji_podatkowej
      int[] digits = company.Random.Digits(9);
      int sum = digits.Zip(NipWeights, (d, w) => d * w).Sum();

      if (sum % 11 == 10)
      {
         sum -= digits[8] * NipWeights[8];
         digits[8]++;
         digits[8] %= 10;
         sum += digits[8] * NipWeights[8];
      }

      return string.Join("", digits)+(sum % 11);
   }

   private static readonly int[] NipWeights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

   /// <summary>
   /// Number of Rejestr Gospodarki Narodowej (REGON)
   /// </summary>
   /// <param name="type">9 or 14 digits REGON type</param>
   public static string Regon(this Company company, RegonType type = RegonType.Regon9)
   {
      // https://pl.wikipedia.org/wiki/REGON
      if (type == RegonType.Random)
         type = company.Random.ArrayElement(new[] { RegonType.Regon9, RegonType.Regon14 });

      int[] digits = company.Random.Digits((int)type - 1);
      int sum = digits.Zip(RegonWeights[type], (d, w) => d * w).Sum();
      int checksum = sum % 11;

      if (checksum == 10)
         checksum = 0;

      return string.Join("", digits)+checksum;
   }

   public enum RegonType
   {
      Regon9 = 9,
      Regon14 = 14,
      Random = 15
   }

   private static readonly Dictionary<RegonType, int[]> RegonWeights = new()
   {
      [RegonType.Regon9] = new int[] { 8, 9, 2, 3, 4, 5, 6, 7 },
      [RegonType.Regon14] = new int[] { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8 },
   };
}
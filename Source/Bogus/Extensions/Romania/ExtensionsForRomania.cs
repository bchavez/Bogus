
using System;
using Bogus.DataSets;

namespace Bogus.Extensions.Romania;

/// <summary>
///  API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForRomania
{
   /// <summary>
   /// Romanian Personal Identification number (CNP)
   /// </summary>
   /// <param name="p"></param>
   /// <returns></returns>
   /// <remarks>
   ///  https://ro.wikipedia.org/wiki/Cod_numeric_personal_(Rom%C3%A2nia)
   ///  '|S| |AA| |LL| |ZZ| |JJ| |ZZZ| |C|
   ///  '|_| |__| |__| |__| |__| |___| |_|
   ///  '  :     :      :      :     :       :    --> Cifra de control
   ///  '  :     :      :      :     :       --> Numarul de ordine atribuit persoanei
   ///  '  :     :      :      :     --> Codul judetului
   ///  '  :     :      :      --> Ziua nasterii
   ///  '  :     :      --> Luna nasterii
   ///  '  :     --> Anul nasterii
   ///  '  --> Cifra sexului (M/F) pentru:
   ///  '        1/2 - cetateni romani nascuti intre 1 ian 1900 si 31 dec 1999
   ///  '        3/4 - cetateni romani nascuti intre 1 ian 1800 si 31 dec 1899
   ///  '        5/6 - cetateni romani nascuti intre 1 ian 2000 si 31 dec 2099
   ///  '        7/8 - rezidenti
   ///  '       Persoanele de cetatenie straina se identifica cu cifra "9"
   /// </remarks>
   public static string Cnp(this Person p)
   {
      const string Key = nameof(ExtensionsForRomania) + "CNP";

      if (p.context.ContainsKey(Key))
      {
         return p.context[Key] as string;
      }

      var randomizer = p.Random;

      var judet = randomizer.Enum<RomanianBirthCounty>();
      int sex;
      //persoanele rezidente
      //sex = p.Gender == Gender.Male ? 7 : 8;
      switch (p.DateOfBirth.Year)
      {
         case >= 1900 and <= 1999:
            sex = p.Gender == Name.Gender.Male ? 1 : 2;
            break;
         case >= 1800 and <= 1899:
            sex = p.Gender == Name.Gender.Male ? 3 : 4;
            break;
         case >= 2000 and <= 2099:
            sex = p.Gender == Name.Gender.Male ? 5 : 6;
            break;
         default:
            throw new ArgumentOutOfRangeException(nameof(p.DateOfBirth.Year),
               $"{nameof(p.DateOfBirth.Year)} must be between 1900 and 2099.");
      }

      var seq = randomizer.Int(1, 999);
      var cnp = sex +
                p.DateOfBirth.ToString("yyMMdd") +
                ((int)judet).ToString("00") +
                seq.ToString("000");

      var checksum = GenerateChecksum(cnp);
      var final = cnp + checksum;
      p.context[Key] = final;

      return final;
   }

   private static string GenerateChecksum(string cnp)
   {
      var cifra = new[] { 2, 7, 9, 1, 4, 6, 3, 5, 8, 2, 7, 9 };

      var sum = 0;
      for (var i = 0; i < 12; i++)
      {
         sum += cifra[i] * int.Parse(cnp[i].ToString()); //calculate checksum
      }

      var rest = sum % 11;
      var checksum = rest == 10 ? "1" : rest.ToString("0");

      return checksum;
   }

}
namespace Bogus.Extensions.Belgium;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForBelgium
{

   /// <summary>
   /// The national number is a unique identification number (11 digits) assigned to persons registered in Belgium.
   /// Everyone with a Belgian identity document or a residence document has this number.
   /// </summary>
   /// <param name="includeFormatSymbols">Includes formatting symbols.</param>
   public static string NationalNumber(this Person p, bool includeFormatSymbols = true)
   {
      /*
         YY.MM.DD-SSS.CC
         |  |  |  |   |
         |  |  |  |   |
         |  |  |  |   |--> (C)Checksum digit (modulo 97)
         |  |  |  |------> (S)Sequential number for people born on the same date. Even for women (002-998), odd for men (001-997).
         |  |  |---------> (D)Day of birth date (may be 0 for international refugees where exact birth date is unknown)
         |  |------------> (M)Month of birth date (may be 0 for international refugees where exact birth date is unknown)
         |---------------> (Y)Year of birth date (last two digits)

         https://nl.wikipedia.org/wiki/Rijksregisternummer
     */

      var sequence = p.Gender == DataSets.Name.Gender.Male
         ? p.Random.Odd(1, 997)
         : p.Random.Even(2, 998);

      var baseNumber = $"{p.DateOfBirth:yyMMdd}{sequence:000}";

      var checkNumber = CalculateCheckNumber(baseNumber, p.DateOfBirth);

      var nationalNumber = $"{baseNumber}{checkNumber}";

      return includeFormatSymbols
         ? FormatNationalNumber(nationalNumber)
         : nationalNumber;
   }

   internal static string CalculateCheckNumber(string baseNumber, DateTime dateOfBirth)
   {
      var baseNumberLong = ulong.Parse(baseNumber);
      var bornAfter2000 = dateOfBirth.Year >= 2000;
      var checkNumber = bornAfter2000
         ? 97 - (int)((baseNumberLong + 2000000000L) % 97)
         : 97 - (int)(baseNumberLong % 97);

      return checkNumber.ToString("D2");
   }

   private static string FormatNationalNumber(string nationalNumber)
   {
      string year = nationalNumber.Substring(0, 2);
      string month = nationalNumber.Substring(2, 2);
      string day = nationalNumber.Substring(4, 2);
      string serial = nationalNumber.Substring(6, 3);
      string checkDigits = nationalNumber.Substring(9, 2);

      // Format as yy.MM.dd-sss.00
      return $"{year}.{month}.{day}-{serial}.{checkDigits}";
   }
}
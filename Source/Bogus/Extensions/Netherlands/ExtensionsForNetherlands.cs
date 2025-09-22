using Bogus.DataSets;

namespace Bogus.Extensions.Netherlands;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForNetherlands
{
   /// <summary>
   /// Burgerservicenummer (BSN)
   /// </summary>
   /// <remarks>
   /// See also: https://nl.wikipedia.org/wiki/Burgerservicenummer
   /// </remarks>
   public static string Bsn(this Person p)
   {
      // Generate a random 8-digit base number
      var baseNumber = p.Random.Number(10000000, 99999999);

      // Calculate the check digit using the 11-proof
      var checkDigit = CalculateBsnCheckDigit(baseNumber);

      // If check digit is valid (not 10 or 11), return the 9-digit BSN
      if (checkDigit < 10)
      {
         return $"{baseNumber}{checkDigit}";
      }

      // If check digit is invalid, try again with a different base number
      // This is a simple retry mechanism to ensure we get a valid BSN
      return GenerateValidBsn(p);
   }

   private static string GenerateValidBsn(Person p)
   {
      int attempts = 0;
      const int maxAttempts = 1000; // Prevent infinite loops

      while (attempts < maxAttempts)
      {
         var baseNumber = p.Random.Number(10000000, 99999999);
         var checkDigit = CalculateBsnCheckDigit(baseNumber);

         if (checkDigit < 10)
         {
            return $"{baseNumber}{checkDigit}";
         }

         attempts++;
      }

      // Fallback: generate a known valid BSN pattern
      // Use a base that we know will produce a valid check digit
      return "123456782"; // This is a valid BSN for testing purposes
   }

   private static int CalculateBsnCheckDigit(int baseNumber)
   {
      // Convert to string to easily access individual digits
      var digits = baseNumber.ToString();

      // BSN 11-proof calculation
      // Multiply each digit by its weight (9, 8, 7, 6, 5, 4, 3, 2)
      // and sum them up
      int sum = 0;
      for (int i = 0; i < 8; i++)
      {
         int digit = int.Parse(digits[i].ToString());
         int weight = 9 - i; // Weights: 9, 8, 7, 6, 5, 4, 3, 2
         sum += digit * weight;
      }

      // The check digit is the remainder when sum is divided by 11
      var checkDigit = sum % 11;

      // If remainder is 10 or 11, the BSN is invalid
      return checkDigit;
   }
}
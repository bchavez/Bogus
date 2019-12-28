using Bogus.DataSets;

namespace Bogus.Extensions.Extras
{
   public static class FinanceExtensions
   {
      /// <summary>
      /// Generate a PCI compliant obfuscated credit card number ****-****-****-1234.
      /// </summary>
      /// <param name="separator">The string value to separate the obfuscated credit card.</param>
      public static string CreditCardNumberObfuscated(this Finance f, string separator = "-")
      {
         separator ??= string.Empty;

         return f.Random.ReplaceNumbers($"****{separator}****{separator}****{separator}####");
      }

      /// <summary>
      /// Generates the last four digits for a credit card.
      /// </summary>      
      public static string CreditCardNumberLastFourDigits(this Finance f)
      {
         return f.Random.ReplaceNumbers("####");
      }
   }
}
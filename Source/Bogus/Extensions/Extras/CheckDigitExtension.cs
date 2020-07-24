using System.Collections.Generic;
using System.Linq;

namespace Bogus.Extensions.Extras
{
   /// <summary>
   /// Shamelessly copied (and modified) from here: 
   /// https://stackoverflow.com/questions/21249670/implementing-luhn-algorithm-using-c-sharp
   /// </summary>
   public static class CheckDigitExtension
   {
      static readonly int[] Results = {0, 2, 4, 6, 8, 1, 3, 5, 7, 9};

      /// <summary>
      /// For a list of digits, compute the ending checkdigit 
      /// </summary>
      /// <param name="digits">The list of digits for which to compute the check digit</param>
      /// <returns>the check digit</returns>
      public static int CheckDigit(this IList<int> digits)
      {
         var i = 0;
         var lengthMod = digits.Count % 2;
         return (digits.Sum(d => i++ % 2 == lengthMod ? d : Results[d]) * 9) % 10;
      }

      /// <summary>
      /// Return a list of digits including the checkdigit
      /// </summary>
      /// <param name="digits">The original list of digits</param>
      /// <returns>the new list of digits including checkdigit</returns>
      public static IList<int> AppendCheckDigit(this IList<int> digits)
      {
         var result = digits;
         result.Add(digits.CheckDigit());
         return result;
      }

      /// <summary>
      /// Returns true when a list of digits has a valid checkdigit
      /// </summary>
      /// <param name="digits">The list of digits to check</param>
      /// <returns>true/false depending on valid checkdigit</returns>
      public static bool HasValidCheckDigit(this IList<int> digits)
      {
         return digits.Last() == CheckDigit(digits.Take(digits.Count - 1).ToList());
      }

      /// <summary>
      /// Internal conversion function to convert string into a list of ints
      /// </summary>
      /// <param name="digits">the original string</param>
      /// <returns>the list of ints</returns>
      private static IList<int> ToDigitList(this string digits)
      {
         return digits.Select(d => d - 48).ToList();
      }

      /// <summary>
      /// For a string of digits, compute the ending checkdigit 
      /// </summary>
      /// <param name="digits">The string of digits for which to compute the check digit</param>
      /// <returns>the check digit</returns>
      public static string CheckDigit(this string digits)
      {
         return digits.ToDigitList().CheckDigit().ToString();
      }

      /// <summary>
      /// Return a string of digits including the checkdigit
      /// </summary>
      /// <param name="digits">The original string of digits</param>
      /// <returns>the new string of digits including checkdigit</returns>
      public static string AppendCheckDigit(this string digits)
      {
         return digits + digits.CheckDigit();
      }

      /// <summary>
      /// Returns true when a string of digits has a valid checkdigit
      /// </summary>
      /// <param name="digits">The string of digits to check</param>
      /// <returns>true/false depending on valid checkdigit</returns>
      public static bool HasValidCheckDigit(this string digits)
      {
         return digits.ToDigitList().HasValidCheckDigit();
      }
   }
}
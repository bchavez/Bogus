using System;
using System.Globalization;
using System.Text;

namespace Bogus.Extensions
{
   /// <summary>
   /// General helper string extensions for working with fake data.
   /// </summary>
   public static class ExtensionsForString
   {
      /// <summary>
      /// Clamps the length of a string filling between min and max characters.
      /// If the string is below the minimum, the string is appended with paddingChar up to the minimum length.
      /// If the string is over the maximum, the string is truncated at maximum characters; additionally, if the result string ends with
      /// whitespace, it is replaced with a paddingChar characters.
      /// </summary>
      public static string ClampLength(this string str, int? min = null, int? max = null, char paddingChar = 'A')
      {
         if( max != null && str.Length > max )
         {
            str = str.Substring(0, max.Value).Trim();
         }
         if( min != null && min > str.Length )
         {
            var missingChars = min - str.Length;
            var fillerChars = "".PadRight(missingChars.Value, paddingChar);
            return str + fillerChars;
         }
         return str;
      }

      /// <summary>
      /// A string extension method that removes the diacritics character from the strings.
      /// </summary>
      /// <param name="this">The @this to act on.</param>
      /// <returns>The string without diacritics character.</returns>
      public static string RemoveDiacritics(this string @this)
      {
         string normalizedString = @this.Normalize(NormalizationForm.FormD);
         var sb = new StringBuilder();

         foreach( char t in normalizedString )
         {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
            if( uc != UnicodeCategory.NonSpacingMark )
            {
               sb.Append(t);
            }
         }

         return sb.ToString().Normalize(NormalizationForm.FormC);
      }

      /// <summary>
      /// Transliterates Unicode characters to US-ASCII. For example, Russian cryllic "Анна Фомина" becomes "Anna Fomina".
      /// </summary>
      /// <param name="this">The @this string to act on.</param>
      /// <param name="lang">The language character set to use.</param>
      public static string Transliterate(this string @this, string lang = "en")
      {
         return Transliterater.Translate(@this, lang);
      }
   }
}
using System.Globalization;
using System.Text;

namespace Bogus.Extensions
{
    /// <summary>
    /// General helper string extensions for working with fake data.
    /// </summary>
    public static class ExtensionsForString
    {
        private static Randomizer r = new Randomizer();

        /// <summary>
        /// Clamps the length of a string filling between min and max characters.
        /// If the string is below the minimum, the string is appended with random characters up to the minimum length.
        /// If the string is over the maximum, the string is truncated at maximum characters; if the result string ends with
        /// whitespace, it is replaced with a random characters.
        /// </summary>
        public static string ClampLength(this string str, int? min = null, int? max = null)
        {
            if (max != null && str.Length > max)
            {
                str = str.Substring(0, max.Value).Trim();
            }
            if (min != null && min > str.Length)
            {
                var missingChars = min - str.Length;
                var fillerChars = r.Replace("".PadRight(missingChars.Value, '?'));
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
    }
}
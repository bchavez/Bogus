using System.Collections.Generic;
using System.Text.RegularExpressions;
using Bogus.Extensions;

namespace Bogus
{
   /// <summary>
   /// Some utility functions
   /// </summary>
   public static class Utils
   {
      /// <summary>
      /// Slugify's text so that it is URL compatible. IE: "Can make food" -> "Can-make-food".
      /// </summary>
      public static string Slugify(string txt)
      {
         var str = txt.Replace(" ", "-").RemoveDiacritics();
         return SlugifyRegex.Replace(str, "");
      }

      public static Regex SlugifyRegex = new Regex(@"[^a-zA-Z0-9\.\-_]+", RegexOptions.Compiled);

      /// <summary>
      /// Takes string parts and joins them together with a separator.
      /// </summary>
      public static string Slashify(IEnumerable<string> parts, string separator = "/")
      {
         return string.Join(separator, parts);
      }
   }
}
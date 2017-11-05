using System;
using System.Collections.Generic;
using System.Linq;
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
         return Regex.Replace(str, @"[^a-zA-Z0-9\.\-_]+", "");
      }

      /// <summary>
      /// Takes string parts and joins them together with a separator.
      /// </summary>
      public static string Slashify(IEnumerable<string> parts, string separator = "/")
      {
         return string.Join(separator, parts);
      }
   }
}
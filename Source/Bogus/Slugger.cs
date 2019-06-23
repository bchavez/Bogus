using System.Collections.Generic;

namespace Bogus
{
   /// <summary>
   /// An advanced utility class to slugify text. A port of https://github.com/pid/speakingurl
   /// </summary>
   public static class Slugger
   {
      public static Dictionary<char, char> charMap = new Dictionary<char, char>()
         {

         };

      public static string GetSlug(
         string input,
         string separator = "-",
         string lang = "en",
         bool symbols = true,
         bool maintainCase = false,
         bool titleCase = false,
         int? truncate = null,
         bool uric = false,
         bool uricNoSlash = false,
         bool mark = false
      )
      {

      }
   }
}
using System.Collections.Generic;
using System.Globalization;

namespace Bogus.Extensions
{
   /// <summary>
   /// Extension methods over <seealso cref="CultureInfo"/>.
   /// </summary>
   public static class ExtensionsForCultureInfo
   {
      public static Dictionary<string, string> Lookup = new Dictionary<string, string>
         {
            {"cs", "cz"},
            {"en-IN", "en_IND"},
            {"ka", "ge"},
            {"id", "id_ID"},
            {"nb", "nb_NO"},
            {"nn", "nb_NO"}
         };

      /// <summary>
      /// Helper extension that maps .NET <seealso cref="CultureInfo"/> to Bogus locale codes like 'en_US`.
      /// </summary>
      public static string ToBogusLocale(this CultureInfo ci)
      {
         /*
Re: https://github.com/bchavez/Bogus/issues/132
Looks like the following need to be remapped for Bogus

cs -> cz
en-IN -> en_IND
ka -> ge
id -> id_ID

nb,nn -> nb_NO
          */

         var locale = Normalize(ci);

         // try matching the full locale code with the _ character
         if( Database.LocaleResourceExists(locale) ) return locale;

         if( locale.Contains("_") )
         {
            //try matching the locale without _
            locale = locale.Substring(0, locale.IndexOf('_'));
         }

         if( Database.LocaleResourceExists(locale) ) return locale;

         //if we tried the locale code before the _ then we have no match
         //sorry :(
         return null;
      }

      private static string Normalize(CultureInfo ci)
      {
         if( Lookup.TryGetValue(ci.Name, out var bogusCode) ) return bogusCode;
         if( Lookup.TryGetValue(ci.TwoLetterISOLanguageName, out bogusCode) ) return bogusCode;

         return ci.Name.Replace('-', '_');
      }
   }
}
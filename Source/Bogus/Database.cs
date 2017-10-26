using System;
using System.Collections.Generic;
using System.IO;
using Bogus.Bson;

namespace Bogus
{
   /// <summary>
   /// The main database object that can access locale data.
   /// </summary>
   public static class Database
   {
      /// <summary>
      /// The root of all locales in a single BObject.
      /// </summary>
      public static Lazy<Dictionary<string, BObject>> Data = new Lazy<Dictionary<string, BObject>>(Initialize, isThreadSafe: true);

      /// <summary>
      /// Returns all locales available inside Bogus' assembly manifest.
      /// </summary>
      public static string[] GetAllLocales()
      {
         var asm = typeof(Database).Assembly;

         var localesFound = new List<string>();
         //Do a quick scan of all the resources in the assembly.
         foreach( var resourceName in asm.GetManifestResourceNames() )
         {
            if( resourceName.EndsWith(".locale.bson") )
            {
               var localeName = resourceName.Replace("Bogus.data.", "").Replace(".locale.bson", "");
               localesFound.Add(localeName);
            }
         }

         return localesFound.ToArray();
      }

      /// <summary>
      /// Initializes the default locale database.
      /// </summary>
      private static Dictionary<string, BObject> Initialize()
      {
         //Just lazy load English only.
         return new Dictionary<string, BObject>
            {
               {"en", InitLocale("en")}
            };
      }


      internal static BObject InitLocale(string locale)
      {
         var asm = typeof(Database).Assembly;
         var resourceName = $"Bogus.data.{locale}.locale.bson";

         using( var s = asm.GetManifestResourceStream(resourceName) )
         using( var ms = new MemoryStream() )
         {
            s.CopyTo(ms);

            return Bson.Bson.Load(ms.ToArray());
         }
      }

      public static BObject GetLocale(string locale)
      {
         if( !Data.Value.TryGetValue(locale, out BObject l) )
         {
            l = InitLocale(locale);
            Data.Value.Add(locale, l);
         }

         return l;
      }

      /// <summary>
      /// Determines if a key exists in the locale.
      /// </summary>
      public static bool HasKey(string category, string path, string locale, string fallbackLocale = "en")
      {
         var l = GetLocale(locale);
         var value = Select(category, path, l);
         if( value != null )
            return true;

         if( fallbackLocale == null ) return false;

         l = GetLocale(fallbackLocale);
         value = Select(category, path, l);

         if( value != null )
            return true;

         return false;
      }

      /// <summary>
      /// Returns the JToken of the locale category path. If the key does not exist, then the locale fallback is used.
      /// </summary>
      public static BValue Get(string category, string path, string locale = "en", string localeFallback = "en")
      {
         var l = GetLocale(locale);

         var val = Select(category, path, l);

         if( val != null )
         {
            return val;
         }

         //fallback path
         var fallback = GetLocale(localeFallback);

         return Select(category, path, fallback);
      }

      private static BValue Select(string category, string path, BValue localeRoot)
      {
         if( !localeRoot.ContainsKey(category) ) return null;

         var section = localeRoot[category];

         var current = 0;
         while( true )
         {
            var len = path.IndexOf('.', current);

            string key;

            if( len < 0 )
            {
               //dot in path not found, final key
               key = path.Substring(current);
               return section[key];
            }
            key = path.Substring(current, len);
            section = section[key];
            if( section is null ) return null;
            current = len + 1;
         }
      }
   }
}

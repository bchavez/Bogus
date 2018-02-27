using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using Bogus.Bson;
using Bogus.Platform;

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
      public static Lazy<ConcurrentDictionary<string, BObject>> Data =
         new Lazy<ConcurrentDictionary<string, BObject>>(Initialize, LazyThreadSafetyMode.ExecutionAndPublication);

      /// <summary>
      /// Returns all locales available inside Bogus' assembly manifest.
      /// </summary>
      public static string[] GetAllLocales()
      {
         var asm = typeof(Database).GetAssembly();

         return asm.GetManifestResourceNames()
            .Where(name => name.EndsWith(".locale.bson"))
            .Select(name => name.Replace("Bogus.data.", "").Replace(".locale.bson", ""))
            .ToArray();
      }

      /// <summary>
      /// Initializes the default locale database.
      /// </summary>
      private static ConcurrentDictionary<string, BObject> Initialize()
      {
         //Just lazy load English only.
         var d = new ConcurrentDictionary<string, BObject>();
         d.TryAdd("en", DeserializeLocale("en"));
         return d;
      }

      internal static BObject DeserializeLocale(string locale)
      {
         var asm = typeof(Database).GetAssembly();
         var resourceName = $"Bogus.data.{locale}.locale.bson";

         return ResourceHelper.ReadBObjectResource(asm, resourceName);
      }

      /// <summary>
      /// Gets a locale from the locale lookup cache, if the locale doesn't exist in the lookup cache,
      /// the locale is read from the assembly manifest and added to the locale lookup cache.
      /// </summary>
      public static BObject GetLocale(string locale)
      {
         if( !Data.Value.TryGetValue(locale, out BObject l) )
         {
            l = DeserializeLocale(locale);
            Data.Value.TryAdd(locale, l);
         }

         return l;
      }

      /// <summary>
      /// Reset, reload, and reinitialize the locale from Bogus' assembly resource.
      /// Any patches or modifications to the specified locale are destroyed.
      /// </summary>
      public static void ResetLocale(string locale)
      {
         Data.Value[locale] = DeserializeLocale(locale);
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

         //fall back path
         var fallback = GetLocale(localeFallback);

         return Select(category, path, fallback);
      }

      private static BValue Select(string category, string path, BValue localeRoot)
      {
         var section = localeRoot[category];
         if( section is null ) return null;

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

   /// <summary>
   /// Helper utility class to read resource manifest streams.
   /// </summary>
   public static class ResourceHelper
   {
      /// <summary>
      /// Reads a byte[] resource from an assembly.
      /// </summary>
      public static byte[] ReadResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using( var s = assembly.GetManifestResourceStream(resourceName) )
         using( var ms = new MemoryStream() )
         {
            s.CopyTo(ms);

            return ms.ToArray();
         }
      }

      /// <summary>
      /// Reads a BSON <see cref="BValue"/> resource from an assembly.
      /// </summary>
      public static BValue ReadBValueResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using( var s = assembly.GetManifestResourceStream(resourceName) )
         using( var ms = new MemoryStream() )
         {
            s.CopyTo(ms);

            return Bson.Bson.Load(ms.ToArray());
         }
      }

      /// <summary>
      /// Reads a BSON <see cref="BObject"/> resource from an assembly.
      /// </summary>
      public static BObject ReadBObjectResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using( var s = assembly.GetManifestResourceStream(resourceName) )
         using( var ms = new MemoryStream() )
         {
            s.CopyTo(ms);

            return Bson.Bson.Load(ms.ToArray());
         }
      }
   }
}
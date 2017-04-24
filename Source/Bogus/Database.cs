using System;
using System.IO;
using System.Reflection;
using Bogus.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bogus
{
    /// <summary>
    /// The main database object that can access locale data.
    /// </summary>
    public static class Database
    {
        /// <summary>
        /// The root of all locales in a single JObject. { de: { ... } ,  en: { ... } }
        /// </summary>
        public static Lazy<JObject> Data = new Lazy<JObject>(Initialize, isThreadSafe: true);


        /// <summary>
        /// Initializes the database by going though all the locales in the assembly manifests.
        /// and merges them into a single JObject like. IE: Root["en"] or Root["de"].
        /// </summary>
        private static JObject Initialize()
        {
            //var asm = typeof(Database).Assembly;
            var asm = typeof(Database).GetAssembly();
            var root = new JObject();

            var resourcesFound = false;

            foreach( var resourceName in asm.GetManifestResourceNames() )
            {
                resourcesFound = true;
                if( resourceName.EndsWith(".locale.json") )
                {
                    using( var s = typeof(Database).GetAssembly().GetManifestResourceStream(resourceName) )
                    using( var sr = new StreamReader(s) )
                    {
                        var serializer = new JsonSerializer();
                        var locale = serializer.Deserialize<JObject>(new JsonTextReader(sr));
                        var props = locale.Properties();
                        foreach( var prop in props )
                            root.Add(prop.Name, prop.First);
                    }
                }
            }

            if( !resourcesFound )
            {
                throw new NotImplementedException("No Locale Resources Could Be Found.");
            }

            return root;
        }

        /// <summary>
        /// Returns the JToken of the locale.category.key. If the key does not exist, then the locale fallback is used.
        /// </summary>
        public static JToken Get(string category, string key, string locale = "en", string localeFallback = "en" )
        {
            var path = $"{locale}.{category}.{key}";
            var jtoken = Data.Value.SelectToken(path);

            if( jtoken != null && jtoken.HasValues )
            {
                return jtoken;
            }

            //fallback path
            var fallbackPath = $"{localeFallback}.{category}.{key}";

            return Data.Value.SelectToken(fallbackPath);
        }
    }
}

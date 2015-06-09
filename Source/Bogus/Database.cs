using System;
using System.IO;
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
            var asm = typeof(Database).Assembly;
            var root = new JObject();

            foreach( var resourceName in asm.GetManifestResourceNames() )
            {
                if( resourceName.EndsWith(".locale.json") )
                {
                    using( var s = typeof(Database).Assembly.GetManifestResourceStream(resourceName) )
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
            return root;
        }

        /// <summary>
        /// Returns the JToken of the locale.category.key. If the key does not exist, then the locale fallback is used.
        /// </summary>
        public static JToken Get(string category, string key, string locale = "en", string localeFallback = "en" )
        {
            var path = string.Format("{0}.{1}.{2}", locale, category, key);
            var jtoken = Data.Value.SelectToken(path);

            if( jtoken != null )
            {
                return jtoken;
            }

            //fallback path
            var fallbackPath = string.Format("{0}.{1}.{2}", localeFallback, category, key);

            return Data.Value.SelectToken(fallbackPath, errorWhenNoMatch: true);
        }
    }
}

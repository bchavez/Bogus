using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentFaker
{
    public static class Faker
    {
        public static Lazy<JObject> Data = new Lazy<JObject>(Initialize);

        private static JObject Initialize()
        {
            var asm = typeof(Faker).Assembly;
            var root = new JObject();

            foreach( var resourceName in asm.GetManifestResourceNames() )
            {
                if( resourceName.EndsWith(".locale.json") )
                {
                    using( var s = typeof(Faker).Assembly.GetManifestResourceStream(resourceName) )
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

        public static JToken Get(string category, string subKind, string locale = "en", string localeFallback = "en" )
        {
            var path = string.Format("{0}.{1}.{2}", locale, category, subKind);
            var jtoken = Data.Value.SelectToken(path);

            if( jtoken != null )
            {
                return jtoken;
            }

            //fallback path
            var fallbackPath = string.Format("{0}.{1}.{2}", localeFallback, category, subKind);

            return Data.Value.SelectToken(fallbackPath, errorWhenNoMatch: true);
        }
    }
    
    public class Faker<T> where T : class
    {
        public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> prop, Func<TProperty> generator )
        {

            return this;
        }
    }
}
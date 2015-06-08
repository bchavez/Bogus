using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
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
    
    public class Faker<T> where T : class, new()
    {
        internal readonly string locale;

        public Faker(string locale = "en")
        {
            this.locale = locale;
        }

        public Rule<T,TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> prop )
        {
            return new Rule<T, TProperty>(this, prop);
        }

        internal Dictionary<string, Action<T>> setup;

        public T Generate()
        {
            return new T();
        }
    }

    public class Rule<T, TProperty> where T : class, new()
    {
        private readonly Faker<T> faker;
        private readonly Expression<Func<T, TProperty>> prop;

        public Rule(Faker<T> faker, Expression<Func<T, TProperty>> prop)
        {
            this.faker = faker;
            this.prop = prop;
        }

        public Faker<T> Use<TGenerator>(Func<TGenerator, TProperty> generateAction) where TGenerator : class, new()
        {
            var propName = PropertyName.For(this.prop);

            var gen = new TGenerator();

            var hasLocale = gen as ILocale;

            if( hasLocale != null )
                hasLocale.Locale = this.faker.locale;

            Action action = () =>
                {
                    generateAction(gen);
                };

            faker.setup.Add(propName, action);

            return faker;
        }

    }

    public static class PropertyName
    {
        public static string For<T,TProp>(Expression<Func<T, TProp>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }
        public static string For<T>(Expression<Func<T, object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }
        public static string For(Expression<Func<object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }
        public static string GetMemberName(Expression expression)
        {
            MemberExpression memberExpression;

            var unary = expression as UnaryExpression;
            if( unary != null )
                //In this case the return type of the property was not object,
                //so .Net wrapped the expression inside of a unary Convert()
                //expression that casts it to type object. In this case, the
                //Operand of the Convert expression has the original expression.
                memberExpression = unary.Operand as MemberExpression;
            else
                //when the property is of type object the body itself is the
                //correct expression
                memberExpression = expression as MemberExpression;

            if( memberExpression == null
                || !( memberExpression.Member is PropertyInfo ) )
                throw new ArgumentException(
                    "Expression was not of the form 'x =&gt; x.property'.");

            return memberExpression.Member.Name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bogus
{
    /// <summary>
    /// Generates fake objects of T.
    /// </summary>
    public class Faker<T> : ILocaleAware where T : class
    {
#pragma warning disable 1591
        protected internal Faker FakerHub;
        protected internal BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetField;
        protected internal Func<Faker, T> CustomActivator;
        protected internal readonly Dictionary<string, Func<Faker, T, object>> Actions = new Dictionary<string, Func<Faker, T, object>>();
        protected internal readonly Lazy<Dictionary<string, MemberInfo>> TypeProperties;
        protected internal bool? UseStrictMode;
        protected internal bool? IsValid;
        protected internal Action<Faker, T> FinalizeAction;
#pragma warning restore 1591

        /// <summary>
        /// Creates a Faker with a locale.
        /// </summary>
        /// <param name="locale"></param>
        public Faker(string locale = "en")
        {
            this.Locale = locale;
            FakerHub = new Faker(locale);
            TypeProperties = new Lazy<Dictionary<string, MemberInfo>>(() =>
                {
                    return typeof(T).GetMembers(BindingFlags)
                        .Where(m =>
                            {
                                if( m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any() )
                                {
                                    //no compiler generated stuff
                                    return false;
                                }
                                return m is PropertyInfo || m is FieldInfo;
                            })
                        .ToDictionary(pi => pi.Name);
                });
        }

        /// <summary>
        /// Forcibly makes a new person context.
        /// </summary>
        public virtual void MakeNewContext()
        {
            this.FakerHub.Person = new Person(this.Locale);
        }

        //TODO: Maybe we can have a way to opt-in or ignore like Json.net. But will need smarter validation
        /// <summary>
        /// Set the binding flags visibility when getting properties. IE: Only public or public+private properties.
        /// </summary>
        public Faker<T> UseBindingFlags(BindingFlags flags)
        {
            this.BindingFlags = flags;
            return this;
        }


        /// <summary>
        /// Uses the factory method to generate new instances.
        /// </summary>
        public Faker<T> CustomInstantiator(Func<Faker, T> factoryMethod)
        {
            this.CustomActivator = factoryMethod;
            return this;
        }

        /// <summary>
        /// Creates a rule for a compound property and providing access to the instance being generated.
        /// </summary>
        public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, TProperty> setter)
        {
            var propName = PropertyName.For(property);

            Func<Faker, T, object> invoker = (f, t) => setter(f, t);

            this.Actions.Add(propName, invoker);

            return this;
        }

        /// <summary>
        /// Creates a rule for a property.
        /// </summary>
        public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, TProperty> setter )
        {
            var propName = PropertyName.For(property);

            Func<Faker, T, object> invoker = (f, t) => setter(f);

            this.Actions.Add(propName, invoker);

            return this;
        }

        /// <summary>
        /// Ensures all properties of T have rules.
        /// </summary>
        /// <param name="ensureRulesForAllProperties">Overrides any global setting in Faker.DefaultStrictMode</param>
        /// <returns></returns>
        public Faker<T> StrictMode(bool ensureRulesForAllProperties)
        {
            UseStrictMode = ensureRulesForAllProperties;
            return this;
        }

        /// <summary>
        /// Action is invoked after all the rules are applied.
        /// </summary>
        public Faker<T> FinishWith(Action<Faker, T> action)
        {
            this.FinalizeAction = action;
            return this;
        }
        
        /// <summary>
        /// Generates a fake object of T.
        /// </summary>
        /// <returns></returns>
        public virtual T Generate()
        {
            var instance = CustomActivator == null ? Activator.CreateInstance<T>() : CustomActivator(this.FakerHub);

            Populate(instance);

            return instance;
        }

        /// <summary>
        /// Generates multiple fake objects of T.
        /// </summary>
        public virtual IEnumerable<T> Generate(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => Generate());
        }

        /// <summary>
        /// Only populates an instance of T.
        /// </summary>
        public virtual void Populate(T instance)
        {
            var useStrictMode = UseStrictMode ?? Faker.DefaultStrictMode;
            if( useStrictMode && !IsValid.HasValue ) 
            {
                //run validation
                this.IsValid = Validate();
            }
            if( useStrictMode && !IsValid.GetValueOrDefault())
            {
                throw new InvalidOperationException($"Cannot generate {typeof(T)} because strict mode is enabled on this type and some properties/fields have no rules.");
            }

            var typeProps = TypeProperties.Value;

            foreach( var kvp in Actions )
            {
                MemberInfo member;
                typeProps.TryGetValue(kvp.Key, out member);
                var valueFactory = kvp.Value;

                var prop = member as PropertyInfo;
                prop?.SetValue(instance, valueFactory(FakerHub, instance), null);

                var field = member as FieldInfo;
                field?.SetValue(instance, valueFactory(FakerHub, instance));
            }

            if( FinalizeAction != null )
            {
                FinalizeAction(this.FakerHub, instance);
            }

            MakeNewContext();
            
        }

        /// <summary>
        /// Checks if all properties have rules.
        /// </summary>
        /// <returns>True if validation pases, false otherwise.</returns>
        public virtual bool Validate()
        {
            return TypeProperties.Value.Count == Actions.Count;
        }

        /// <summary>
        /// The current locale.
        /// </summary>
        public string Locale { get; set; }
    }
}
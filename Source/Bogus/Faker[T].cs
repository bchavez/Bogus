using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bogus
{
    /// <summary>
    /// Generates fake objects of T.
    /// </summary>
    public class Faker<T> : ILocaleAware where T : class
    {
#pragma warning disable 1591
        protected internal Faker FakerHub;
        protected internal Func<Faker, T> CustomActivator;
        protected internal IBinder binder;
        protected internal readonly Dictionary<string, Func<Faker, T, object>> Actions = new Dictionary<string, Func<Faker, T, object>>();
        protected internal readonly Dictionary<string, MemberInfo> TypeProperties;
        protected internal bool? UseStrictMode;
        protected internal bool? IsValid;
        protected internal Action<Faker, T> FinalizeAction;
#pragma warning restore 1591

        /// <summary>
        /// Creates a Faker with a locale.
        /// </summary>
        /// <param name="locale">language</param>
        /// <param name="binder">A binder that discovers properties or fields on T that are candidates for faking. Null uses the default Binder.</param>
        public Faker(string locale = "en", IBinder binder = null)
        {
            this.binder = binder ?? new Binder();
            this.Locale = locale;
            FakerHub = new Faker(locale);
            TypeProperties = this.binder.GetMembers(typeof(T));
        }

        /// <summary>
        /// Set the binding flags visibility when getting properties. IE: Only public or public+private properties.
        /// </summary>
        [Obsolete("Use new Binder(BindingFlags) if you are using custom BindingFlags. Construct Faker<T> with a custom IBinder.", true)]
        public Faker<T> UseBindingFlags(BindingFlags flags)
        {
            throw new NotImplementedException("Use new Binder(BindingFlags) when constructing a Faker<T> class.");
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
        /// RuleFor helper for constant string values.
        /// </summary>
        /// <param name="constantValue">Constant string value used to set the property.</param>
        /// <returns></returns>
        public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, TProperty constantValue)
        {
            return RuleFor(property, (f) => constantValue);
        }

        /// <summary>
        /// Ignore a property or field when using StrictMode.
        /// </summary>
        /// <typeparam name="TPropertyOrField"></typeparam>
        /// <param name="propertyOrField"></param>
        /// <returns></returns>
        public Faker<T> Ignore<TPropertyOrField>(Expression<Func<T, TPropertyOrField>> propertyOrField)
        {
            var propNameOrField = PropertyName.For(propertyOrField);

            if( !this.TypeProperties.Remove(propNameOrField) )
            {
                throw new ArgumentException($"The property or field {propNameOrField} was not found on {typeof(T)} during the binding discovery of T. Can't ignore something that doesn't exist.");
            }

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
                throw new InvalidOperationException($"StrictMode validation failure on {typeof(T)}. The Binder found {TypeProperties.Count} properties/fields but have {Actions.Count} actions rules.");
            }

            var typeProps = TypeProperties;

            lock( Randomizer.Locker.Value )
            {
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

                FinalizeAction?.Invoke(this.FakerHub, instance);

                FakerHub.NewContext();
            }
        }

        /// <summary>
        /// Checks if all properties have rules.
        /// </summary>
        /// <returns>True if validation pases, false otherwise.</returns>
        public virtual bool Validate()
        {
            return TypeProperties.Count == Actions.Count;
        }

        /// <summary>
        /// The current locale.
        /// </summary>
        public string Locale { get; set; }
    }
}
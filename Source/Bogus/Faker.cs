using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Bogus.DataSets;

namespace Bogus
{
    /// <summary>
    /// A hub of all the categories merged into a single class to ease fluent syntax API.
    /// </summary>
    public class Faker
    {
        /// <summary>
        /// The default mode to use when generating objects. Strict mode ensures that all properties have rules.
        /// </summary>
        public static bool DefaultStrictMode = false;

        /// <summary>
        /// Create a Faker with a specific locale.
        /// </summary>
        public Faker(string locale = "en")
        {
            this.Internet = new Internet(locale);
            this.Date = new Date {Locale = locale};
            this.Address = new Address(locale);
            this.Finance = new Finance() {Locale = locale};
            this.Image = new Images(locale);
            this.Lorem = new Lorem(locale);
            this.Name = new Name(locale);
            this.Phone = new PhoneNumbers(locale);
            this.Person = new Person(locale);
            this.Random = new Randomizer();
        }
        
        /// <summary>
        /// A contextually relevant fields of a person.
        /// </summary>
        public Person Person { get; set; }     
        /// <summary>
        /// Generate Phone Numbers
        /// </summary>
        public PhoneNumbers Phone { get; set; }
        /// <summary>
        /// Generate Names
        /// </summary>
        public Name Name { get; set; }
        /// <summary>
        /// Generate Words
        /// </summary>
        public Lorem Lorem { get; set; }
        /// <summary>
        /// Generate Image URL Links
        /// </summary>
        public Images Image { get; set; }
        /// <summary>
        /// Generate Finance Items
        /// </summary>
        public Finance Finance { get; set; }
        /// <summary>
        /// Generate Addresses
        /// </summary>
        public Address Address { get; set; }
        /// <summary>
        /// Generate Dates
        /// </summary>
        public Date Date { get; set; }
        /// <summary>
        /// Generate Internet stuff like Emails and UserNames.
        /// </summary>
        public Internet Internet { get; set; }
        /// <summary>
        /// Generate numbers, booleans, and decimals.
        /// </summary>
        public Randomizer Random { get; set; }

        /// <summary>
        /// Helper method to pick a random element.
        /// </summary>
        public T PickRandom<T>(IEnumerable<T> items)
        {
            return this.Random.ArrayElement(items.ToArray());
        }

        /// <summary>
        /// Picks a random Enum of T. Works only with Enums.
        /// </summary>
        /// <typeparam name="T">Must be an Enum</typeparam>
        public T PickRandom<T>() where T : struct
        {
            var e = typeof(T);
            if( !e.IsEnum )
                throw new ArgumentException("When calling PickRandom<T>() with no parameters T must be an enum.");

            var val = this.Random.ArrayElement(Enum.GetNames(e));
            
            T picked;
            Enum.TryParse(val, out picked );
            return picked;
        }
    }
    
    /// <summary>
    /// Generates fake objects of T.
    /// </summary>
    public class Faker<T> where T : class
    {
#pragma warning disable 1591
        protected internal Faker FakerHub;
        protected internal BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public;
        protected internal Func<Faker, T> CustomActivator;
        protected internal readonly Dictionary<string, Func<Faker, T, object>> Actions = new Dictionary<string, Func<Faker, T, object>>();
        protected internal readonly Lazy<Dictionary<string, PropertyInfo>> TypeProperties;
        protected internal bool? UseStrictMode;
        protected internal bool? IsValid;
#pragma warning restore 1591

        /// <summary>
        /// Creates a Faker with a locale.
        /// </summary>
        /// <param name="locale"></param>
        public Faker(string locale = "en")
        {
            FakerHub = new Faker(locale);
            TypeProperties = new Lazy<Dictionary<string, PropertyInfo>>(() =>
                {
                    return typeof(T).GetProperties(BindingFlags)
                        .ToDictionary(pi => pi.Name);
                });
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
                throw new InvalidOperationException(string.Format("Cannot generate {0} because strict mode is enabled on this type and some properties have no rules.",
                    typeof(T)));
            }

            var typeProps = TypeProperties.Value;

            foreach( var kvp in Actions )
            {
                PropertyInfo prop;
                typeProps.TryGetValue(kvp.Key, out prop);
                var valueFactory = kvp.Value;
                prop.SetValue(instance, valueFactory(FakerHub, instance), null);
            }
        }

        /// <summary>
        /// Checks if all properties have rules.
        /// </summary>
        /// <returns>True if validation pases, false otherwise.</returns>
        public virtual bool Validate()
        {
            return TypeProperties.Value.Count == Actions.Count;
        }
    }
}

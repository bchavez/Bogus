using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.AccessControl;
using Bogus.Generators;

namespace Bogus
{
    public class Faker
    {
        public static bool DefaultStrictMode = false;

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
        
        public Person Person { get; set; }
        public PhoneNumbers Phone { get; set; }
        public Name Name { get; set; }
        public Lorem Lorem { get; set; }
        public Images Image { get; set; }
        public Finance Finance { get; set; }
        public Address Address { get; set; }
        public Date Date { get; set; }
        public Internet Internet { get; set; }
        
        public Randomizer Random { get; set; }

        public T PickRandom<T>(IEnumerable<T> items)
        {
            return this.Random.ArrayElement(items.ToArray());
        }

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
    
    public class Faker<T> where T : class
    {
        protected internal Faker faker;

        public Faker(string locale = "en")
        {
            faker = new Faker(locale);
            typeProperties = new Lazy<Dictionary<string, PropertyInfo>>(() =>
                {
                    return typeof(T).GetProperties(bindingFlags)
                        .ToDictionary(pi => pi.Name);
                });
        }

        protected BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.NonPublic | BindingFlags.Public;

        public Faker<T> UseBindingFlags(BindingFlags flags)
        {
            this.bindingFlags = flags;
            return this;
        }

        protected internal Func<Faker, T> customInstantiator;

        public Faker<T> CustomInstantiator(Func<Faker, T> factoryMethod)
        {
            this.customInstantiator = factoryMethod;
            return this;
        }

        public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, TProperty> setter)
        {
            var propName = PropertyName.For(property);

            Func<Faker, T, object> invoker = (f, t) => setter(f, t);

            this.actions.Add(propName, invoker);

            return this;
        }
        public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, TProperty> setter )
        {
            var propName = PropertyName.For(property);

            Func<Faker, T, object> invoker = (f, t) => setter(f);

            this.actions.Add(propName, invoker);

            return this;
        }

        public Faker<T> StrictMode(bool ensureRulesForAllProperties)
        {
            strictMode = ensureRulesForAllProperties;
            return this;
        }

        protected Dictionary<string, Func<Faker, T, object>> actions = new Dictionary<string, Func<Faker, T, object>>();
        protected Lazy<Dictionary<string, PropertyInfo>> typeProperties;



        protected bool? strictMode;

        protected bool? isValid;

        public virtual T Generate()
        {
            var instance = customInstantiator == null ? Activator.CreateInstance<T>() : customInstantiator(this.faker);

            Populate(instance);

            return instance;
        }

        public virtual IEnumerable<T> Generate(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => Generate());
        }

        public virtual void Populate(T instance)
        {
            var useStrictMode = strictMode ?? Faker.DefaultStrictMode;
            if( useStrictMode && !isValid.HasValue ) 
            {
                //run validation
                this.isValid = Validate();
            }
            if( useStrictMode && !isValid.GetValueOrDefault())
            {
                throw new InvalidOperationException(string.Format("Cannot generate {0} because strict mode is enabled on this type and some properties have no rules.",
                    typeof(T)));
            }

            var typeProps = typeProperties.Value;

            foreach( var kvp in actions )
            {
                PropertyInfo prop;
                typeProps.TryGetValue(kvp.Key, out prop);
                var valueFactory = kvp.Value;
                prop.SetValue(instance, valueFactory(faker, instance), null);
            }
        }

        public virtual bool Validate()
        {
            return typeProperties.Value.Count == actions.Count;
        }
    }
}

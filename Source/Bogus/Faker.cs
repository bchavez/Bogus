using System;
using System.Collections.Generic;
using System.Linq;
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
}

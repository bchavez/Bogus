using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bogus.DataSets;

namespace Bogus
{
    /// <summary>
    /// A hub of all the categories merged into a single class to ease fluent syntax API.
    /// </summary>
    public class Faker : ILocaleAware
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
            Locale = locale;

            this.Address = new Address(locale);
            this.Company = new Company(locale);
            this.Date = new Date {Locale = locale};
            this.Finance = new Finance {Locale = locale};
            this.Hacker = new Hacker();
            this.Image = new Images(locale);
            this.Internet = new Internet(locale);
            this.Lorem = new Lorem(locale);
            this.Name = new Name(locale);
            this.Phone = new PhoneNumbers(locale);

            this.Random = new Randomizer();
        }

        /// <summary>
        /// Can parse a handle bar expression like "{{name.lastName}}, {{name.firstName}} {{name.suffix}}".
        /// </summary>
        public string Parse(string str)
        {
            return Tokenizer.Parse(str,
                this.Address,
                this.Company,
                this.Date,
                this.Finance,
                this.Hacker,
                this.Image,
                this.Internet,
                this.Lorem,
                this.Name,
                this.Phone);
        }


        private Person person;

        /// <summary>
        /// A contextually relevant fields of a person.
        /// </summary>
        public Person Person => person ?? (person = new Person(this.Locale));

        /// <summary>
        /// Creates hacker gibberish.
        /// </summary>
        public Hacker Hacker { get; set; }

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
        /// Generates company names, titles and BS.
        /// </summary>
        public Company Company { get; set; }

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
            return this.Random.Enum<T>();
        }

        /// <summary>
        /// The current locale for the dataset.
        /// </summary>
        /// <value>The locale.</value>
        public string Locale { get; set; }

        /// <summary>
        /// Resets the person context.
        /// </summary>
        internal void ResetPersonContext()
        {
            person = null;
        }
    }


}

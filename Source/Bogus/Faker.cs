using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Bogus.DataSets;
using System;

namespace Bogus
{
   /// <summary>
   /// A hub of all the categories merged into a single class to ease fluent syntax API.
   /// </summary>
   public class Faker : ILocaleAware, IHasRandomizer, IHasContext
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

         this.Address = this.Notifier.Flow(new Address(locale));
         this.Company = this.Notifier.Flow(new Company(locale));
         this.Date = this.Notifier.Flow(new Date (locale));
         this.Finance = this.Notifier.Flow(new Finance {Locale = locale});
         this.Hacker = this.Notifier.Flow(new Hacker(locale));
         this.Image = this.Notifier.Flow(new Images(locale));
         this.Internet = this.Notifier.Flow(new Internet(locale));
         this.Lorem = this.Notifier.Flow(new Lorem(locale));
         this.Name = this.Notifier.Flow(new Name(locale));

         this.Phone = this.Notifier.Flow(new PhoneNumbers(locale));
         this.System = this.Notifier.Flow(new DataSets.System(locale));
         this.Commerce = this.Notifier.Flow(new Commerce(locale));
         this.Database = this.Notifier.Flow(new DataSets.Database());
         this.Rant = this.Notifier.Flow(new Rant());
         this.Vehicle = this.Notifier.Flow(new Vehicle());

         this.Music = this.Notifier.Flow(new Music());

         this.Hashids = new Hashids();
      }

      Dictionary<string, object> IHasContext.Context { get; } = new Dictionary<string, object>();

      /// <summary>
      /// See <see cref="SeedNotifier"/>
      /// </summary>
      protected SeedNotifier Notifier = new SeedNotifier();

      SeedNotifier IHasRandomizer.GetNotifier()
      {
         return this.Notifier;
      }

      private Randomizer randomizer;

      /// <summary>
      /// Generate numbers, booleans, and decimals.
      /// </summary>
      [RegisterMustasheMethods]
      public Randomizer Random
      {
         get => this.randomizer ?? (this.Random = new Randomizer());
         set
         {
            this.randomizer = value;
            this.Notifier.Notify(value);
         }
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
            this.Phone,
            this.System,
            this.Commerce,
            this.Database,
            this.Random);
      }


      private Person person;

      /// <summary>
      /// A contextually relevant fields of a person.
      /// </summary>
      public Person Person => person ??= new Person(this.Random, this.Locale);

      /// <summary>
      /// Creates hacker gibberish.
      /// </summary>
      [RegisterMustasheMethods]
      public Hacker Hacker { get; set; }

      /// <summary>
      /// Generate Phone Numbers
      /// </summary>
      [RegisterMustasheMethods]
      public PhoneNumbers Phone { get; set; }

      /// <summary>
      /// Generate Names
      /// </summary>
      [RegisterMustasheMethods]
      public Name Name { get; set; }

      /// <summary>
      /// Generate Words
      /// </summary>
      [RegisterMustasheMethods]
      public Lorem Lorem { get; set; }

      /// <summary>
      /// Generate Image URL Links
      /// </summary>
      [RegisterMustasheMethods]
      public Images Image { get; set; }

      /// <summary>
      /// Generate Finance Items
      /// </summary>
      [RegisterMustasheMethods]
      public Finance Finance { get; set; }

      /// <summary>
      /// Generate Addresses
      /// </summary>
      [RegisterMustasheMethods]
      public Address Address { get; set; }

      /// <summary>
      /// Generate Dates
      /// </summary>
      [RegisterMustasheMethods]
      public Date Date { get; set; }

      /// <summary>
      /// Generates company names, titles and BS.
      /// </summary>
      [RegisterMustasheMethods]
      public Company Company { get; set; }

      /// <summary>
      /// Generate Internet stuff like Emails and UserNames.
      /// </summary>
      [RegisterMustasheMethods]
      public Internet Internet { get; set; }

      /// <summary>
      /// Generates data related to commerce
      /// </summary>
      [RegisterMustasheMethods]
      public Commerce Commerce { get; set; }

      /// <summary>
      /// Generates fake data for many computer systems properties
      /// </summary>
      [RegisterMustasheMethods]
      public DataSets.System System { get; set; }

      /// <summary>
      /// Generates fake database things.
      /// </summary>
      [RegisterMustasheMethods]
      public DataSets.Database Database { get; set; }

      /// <summary>
      /// Generates random user content.
      /// </summary>
      [RegisterMustasheMethods]
      public Rant Rant { get; set; }

      /// <summary>
      /// Generates data related to vehicles.
      /// </summary>
      [RegisterMustasheMethods]
      public Vehicle Vehicle { get; set; }

      /// <summary>
      /// Generates data related to music.
      /// </summary>
      [RegisterMustasheMethods]
      public Music Music { get; set; }

      /// <summary>
      /// Helper method to pick a random element.
      /// </summary>
      public T PickRandom<T>(IEnumerable<T> items)
      {
         return this.Random.ArrayElement(items.ToArray());
      }

      /// <summary>
      /// Helper method to pick a random element.
      /// </summary>
      public T PickRandom<T>(IList<T> items)
      {
         return this.Random.ListItem(items);
      }

      /// <summary>
      /// Helper method to pick a random element.
      /// </summary>
      public T PickRandom<T>(ICollection<T> items)
      {
         return this.Random.CollectionItem(items);
      }

      /// <summary>
      /// Helper method to pick a random element.
      /// </summary>
      public T PickRandom<T>(List<T> items)
      {
         return this.Random.ListItem(items);
      }

      /// <summary>
      /// Picks a random item of T specified in the parameter list.
      /// </summary>
      public T PickRandom<T>(params T[] items)
      {
         return this.Random.ArrayElement(items);
      }

      /// <summary>
      /// Picks a random item of T specified in the parameter list.
      /// </summary>
      public T PickRandomParam<T>(params T[] items)
      {
         return this.Random.ArrayElement(items);
      }

      /// <summary>
      /// Helper to pick random subset of elements out of the list.
      /// </summary>
      /// <param name="amountToPick">amount of elements to pick of the list.</param>
      /// <exception cref="ArgumentException">if amountToPick is lower than zero.</exception>
      public IEnumerable<T> PickRandom<T>(IEnumerable<T> items, int amountToPick)
      {
         if( amountToPick < 0 )
         {
            throw new ArgumentOutOfRangeException($"{nameof(amountToPick)} needs to be a positive integer.");
         }
         var size = items.Count();
         if( amountToPick > size )
         {
            throw new ArgumentOutOfRangeException($"{nameof(amountToPick)} is greater than the number of items.");
         }
         return this.Random.Shuffle(items).Take(amountToPick);
      }

      /// <summary>
      /// Helper method to call faker actions multiple times and return the result as IList of T
      /// </summary>
      public IList<T> Make<T>(int count, Func<T> action)
      {
         return Enumerable.Range(1, count).Select(n => action()).ToList();
      }

      /// <summary>
      /// Helper method to call faker actions multiple times and return the result as IList of T.
      /// This method passes in the current index of the generation.
      /// </summary>
      public IList<T> Make<T>(int count, Func<int, T> action)
      {
         return Enumerable.Range(1, count).Select(action).ToList();
      }

      /// <summary>
      /// Returns an IEnumerable[T] with LINQ deferred execution. Generated values
      /// are not guaranteed to be repeatable until .ToList() is called.
      /// </summary>
      public IEnumerable<T> MakeLazy<T>(int count, Func<T> action)
      {
         return Enumerable.Range(1, count).Select(n => action());
      }

      /// <summary>
      /// Same as Make() except this method passes in the current index of the generation. Also,
      /// returns an IEnumerable[T] with LINQ deferred execution. Generated values are not
      /// guaranteed to be repeatable until .ToList() is called.
      /// </summary>
      public IEnumerable<T> MakeLazy<T>(int count, Func<int, T> action)
      {
         return Enumerable.Range(1, count).Select(action);
      }

      /// <summary>
      /// Picks a random Enum of T. Works only with Enums.
      /// </summary>
      /// <typeparam name="T">Must be an Enum</typeparam>
      public T PickRandom<T>() where T : struct, Enum
      {
         return this.Random.Enum<T>();
      }

      /// <summary>
      /// Picks a random Enum of T, excluding those passed as parameters.
      /// </summary>
      /// <param name="exclude">The items in the Enum of T to exclude from selection.</param>
      public T PickRandomWithout<T>(params T[] exclude) where T : struct, Enum
      {
         return this.Random.Enum(exclude);
      }

      /// <summary>
      /// The current locale for the dataset.
      /// </summary>
      /// <value>The locale.</value>
      public string Locale { get; set; }

      /// <summary>
      /// Triggers a new generation context
      /// </summary>
      internal void NewContext()
      {
         person = null;
         this.capturedGlobalIndex = Interlocked.Increment(ref GlobalUniqueIndex);
         Interlocked.Increment(ref IndexFaker);
      }

      /// <summary>
      /// Checks if the internal state is ready to be used by <seealso cref="Faker{T}"/>.
      /// In other words, has NewContext ever been called since this object was created?
      /// See Issue 143. https://github.com/bchavez/Bogus/issues/143
      /// </summary>
      internal bool HasContext => this.IndexFaker != -1;

      /// <summary>
      /// A global variable that is automatically incremented on every
      /// new object created by Bogus. Useful for composing property values that require
      /// uniqueness.
      /// </summary>
      public static int GlobalUniqueIndex = -1;

      private int capturedGlobalIndex;

      /// <summary>
      /// Alias for IndexGlobal.
      /// </summary>
      //[Obsolete("Please use IndexGlobal instead.")]
      public int UniqueIndex => capturedGlobalIndex;

      /// <summary>
      /// A global static variable that is automatically incremented on every
      /// new object created by Bogus across all Faker[T]s in the entire application.
      /// Useful for composing property values that require uniqueness across
      /// the entire application.
      /// </summary>
      public int IndexGlobal => capturedGlobalIndex;

      /// <summary>
      /// A local variable that is automatically incremented on every
      /// new object generated by the Faker[T] instance for lifetime of Faker[T].
      /// </summary>
      public int IndexFaker = -1;

      /// <summary>
      /// A local index variable that can be controlled inside rules with ++ and --.
      /// This variable's lifetime exists for the lifetime of Faker[T].
      /// </summary>
      public int IndexVariable = 0;

      /// <summary>
      /// HashID generator with default (string.Empty) salt. See: https://github.com/ullmark/hashids.net
      /// </summary>
      public Hashids Hashids { get; set; }
   }
}
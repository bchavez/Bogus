using Bogus.Bson;
using Bogus.Platform;
using System;
using System.Text.RegularExpressions;

namespace Bogus
{
   /// <summary>
   /// Data set methods that access the BSON database of locales.
   /// </summary>
   public class DataSet : ILocaleAware, IHasRandomizer
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="DataSet"/> class.
      /// </summary>
      /// <param name="locale">The locale wanting to be set. Default is "en" for English.</param>
      /// <exception cref="BogusException">
      /// When the given <paramref name="locale"/> isn't found.
      /// </exception>
      public DataSet(string locale = "en")
      {
         if (!Database.LocaleResourceExists(locale))
         {
            throw new BogusException(
               $"The locale '{locale}' does not exist. To see all available locales visit {AssemblyVersionInformation.AssemblyDescription}.");
         }

         this.Locale = locale;

         this.Category = ResolveCategory(this.GetType());
      }

      /// <summary>
      /// Gets or sets the category name inside the locale.
      /// </summary>
      protected string Category { get; set; }

      /// <summary>
      /// Gets or sets the current locale of the data set.
      /// </summary>
      public string Locale { get; set; }

      /// <summary>
      /// See <see cref="SeedNotifier"/>.
      /// </summary>
      protected SeedNotifier Notifier = new SeedNotifier();

      private Randomizer randomizer;

      /// <summary>
      /// Gets or sets the <see cref="Randomizer"/> used to generate values.
      /// </summary>
      public Randomizer Random
      {
         get => this.randomizer ?? (this.Random = new Randomizer());
         set
         {
            this.randomizer = value;
            this.Notifier.Notify(value);
         }
      }

      SeedNotifier IHasRandomizer.GetNotifier()
      {
         return this.Notifier;
      }

      /// <summary>
      /// Resolves the 'category' type of a dataset type; respects the 'DataCategory' attribute.
      /// </summary>
      /// <param name="type">The type wanting to get the category from.</param>
      /// <returns>The name of the category.</returns>
      public static string ResolveCategory(Type type)
      {
         var categoryAttribute = type.GetCustomAttributeX<DataCategoryAttribute>();
         return categoryAttribute != null ? categoryAttribute.Name : type.Name.ToLowerInvariant();
      }

      /// <summary>
      /// Returns a BSON value given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">path/key in the category</param>
      /// <returns>A BSON value for the given JSON path.</returns>
      protected internal virtual BValue Get(string path)
      {
         return Database.Get(this.Category, path, this.Locale);
      }

      /// <summary>
      /// Returns a BSON value given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="category">Overrides the category name on the dataset.</param>
      /// <param name="path">path/key in the category.</param>
      /// <returns>A BSON value for the given JSON path.</returns>
      protected internal virtual BValue Get(string category, string path)
      {
         return Database.Get(category, path, this.Locale);
      }

      /// <summary>
      /// Determines if a key exists in the locale.
      /// </summary>
      /// <returns>A boolean to indicate if the locale exists.</returns>
      protected internal virtual bool HasKey(string path, bool includeFallback = true)
      {
         if (includeFallback)
         {
            return Database.HasKey(this.Category, path, this.Locale);
         }

         return Database.HasKey(this.Category, path, this.Locale, null);
      }

      /// <summary>
      /// Returns a BSON array given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category.</param>
      /// <returns>A BSON value for the given JSON path.</returns>
      protected internal virtual BArray GetArray(string path)
      {
         return (BArray)Get(path);
      }

      protected internal virtual BArray GetArray(string category, string path)
      {
         return (BArray)Get(category, path);
      }

      /// <summary>
      /// Returns a BSON object given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">path/key in the category</param>
      /// <returns>A BSON value for the given JSON path.</returns>
      protected internal virtual BObject GetObject(string path)
      {
         return (BObject)Get(path);
      }

      /// <summary>
      /// Picks a random string inside a BSON array. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
      /// <param name="min">The minimum value.</param>
      /// <param name="max">The maximum value.</param>
      /// <returns>A random item from the BSON array.</returns>
      protected internal virtual string GetRandomArrayItem(string path, int? min = null, int? max = null)
      {
         return this.GetRandomArrayItem(this.Category, path, min, max);
      }

      protected internal virtual string GetRandomArrayItem(string category, string path, int? min = null, int? max = null)
      {
         var arr = GetArray(category, path);
         if (!arr.HasValues)
         {
            return string.Empty;
         }

         return Random.ArrayElement(arr, min, max);
      }

      /// <summary>
      /// Picks a random BObject inside an array. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
      /// <returns>A random BObject based on the given path.</returns>
      protected internal virtual BObject GetRandomBObject(string path)
      {
         var arr = GetArray(path);
         if (!arr.HasValues)
         {
            return null;
         }

         return Random.ArrayElement(arr) as BObject;
      }

      /// <summary>
      /// Picks a random string inside a BSON array, then formats it. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
      /// <returns>A random formatted value.</returns>
      protected internal virtual string GetFormattedValue(string path)
      {
         var value = GetRandomArrayItem(path);

         var tokenResult = ParseTokens(value);

         return Random.Replace(tokenResult);
      }

      private static readonly Regex parseTokensRegex = new Regex("\\#{(.*?)\\}", RegexOptions.Compiled);

      /// <summary>
      /// Recursive parse the tokens in the string.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <returns>The parsed token.</returns>
      private string ParseTokens(string value)
      {
         var parseResult = parseTokensRegex.Replace(
            value,
            x =>
               {
                  BArray result;
                  var groupValue = x.Groups[1].Value.ToLowerInvariant().Split('.');
                  if (groupValue.Length == 1)
                  {
                     result = (BArray)Database.Get(this.Category, groupValue[0], this.Locale);
                  }
                  else
                  {
                     result = (BArray)Database.Get(groupValue[0], groupValue[1], this.Locale);
                  }

                  var randomElement = this.Random.ArrayElement(result);

                  // replace values
                  return ParseTokens(randomElement);
               });

         return parseResult;
      }
   }
}
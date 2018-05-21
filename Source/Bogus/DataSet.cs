using System;
using System.Text.RegularExpressions;
using Bogus.Bson;
using Bogus.Platform;

namespace Bogus
{
   /// <summary>
   /// Data set methods that access the BSON database of locales.
   /// </summary>
   public class DataSet : ILocaleAware, IHasRandomizer
   {
      /// <summary>
      /// Resolves the 'category' type of a dataset type; respects the 'DataCategory' attribute.
      /// </summary>
      public static string ResolveCategory(Type type)
      {
         var categoryAttribute = type.GetCustomAttributeX<DataCategoryAttribute>();
         return categoryAttribute != null ? categoryAttribute.Name : type.Name.ToLower();
      }

      /// <summary>
      /// Default constructor
      /// </summary>
      /// <param name="locale"></param>
      public DataSet(string locale = "en")
      {
         if( !Database.LocaleResourceExists(locale) )
            throw new BogusException(
               $"The locale '{locale}' does not exist. To see all available locales visit {AssemblyVersionInformation.AssemblyDescription}."
               );

         this.Locale = locale;

         this.Category = ResolveCategory(this.GetType());
      }

      /// <summary>
      /// See <see cref="SeedNotifier"/>
      /// </summary>
      protected SeedNotifier Notifier = new SeedNotifier();

      private Randomizer randomizer;

      /// <summary>
      /// The Randomizer
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
      /// The category name inside the locale
      /// </summary>
      protected string Category { get; set; }

      /// <summary>
      /// Current locale of the data set.
      /// </summary>
      public string Locale { get; set; }

      /// <summary>
      /// Returns a BSON value given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">path/key in the category</param>
      protected internal virtual BValue Get(string path)
      {
         return Database.Get(this.Category, path, this.Locale);
      }

      /// <summary>
      /// Returns a BSON value given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="category">Overrides the category name on the dataset</param>
      /// <param name="path">path/key in the category</param>
      protected internal virtual BValue Get(string category, string path)
      {
         return Database.Get(category, path, this.Locale);
      }

      /// <summary>
      /// Determines if a key exists in the locale.
      /// </summary>
      protected internal virtual bool HasKey(string path, bool includeFallback = true)
      {
         if( includeFallback )
            return Database.HasKey(this.Category, path, this.Locale);

         return Database.HasKey(this.Category, path, this.Locale, null);
      }

      /// <summary>
      /// Returns a BSON array given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
      /// <returns></returns>
      protected internal virtual BArray GetArray(string path)
      {
         return (BArray)Get(path);
      }

      /// <summary>
      /// Returns a BSON object given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">path/key in the category</param>
      protected internal virtual BObject GetObject(string path)
      {
         return (BObject)Get(path);
      }

      /// <summary>
      /// Picks a random string inside a BSON array. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
      protected internal virtual string GetRandomArrayItem(string path, int? min = null, int? max = null)
      {
         var arr = GetArray(path);
         if( !arr.HasValues ) return string.Empty;
         return Random.ArrayElement(arr, min, max);
      }

      /// <summary>
      /// Picks a random BObject inside an array.
      /// </summary>
      protected internal virtual BObject GetRandomBObject(string path)
      {
         var arr = GetArray(path);
         if( !arr.HasValues ) return null;
         return Random.ArrayElement(arr) as BObject;
      }

      /// <summary>
      /// Picks a random string inside a BSON array, then formats it. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
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
      private string ParseTokens(string value)
      {
         var cityResult = parseTokensRegex.Replace(value,
            x =>
               {
                  BArray result;
                  var groupValue = x.Groups[1].Value.ToLower().Split('.');
                  if( groupValue.Length == 1 )
                  {
                     result = (BArray)Database.Get(this.Category, groupValue[0], this.Locale);
                  }
                  else
                  {
                     result = (BArray)Database.Get(groupValue[0], groupValue[1], this.Locale);
                  }

                  var randomElement = this.Random.ArrayElement(result);

                  //replace values
                  return ParseTokens(randomElement);
               }
         );
         return cityResult;
      }
   }
}
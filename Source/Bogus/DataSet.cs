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
         this.Locale = locale;

         this.Category = ResolveCategory(this.GetType());
      }

      protected SeedNotifier<DataSet> Notifier = new SeedNotifier<DataSet>();

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
      public BValue Get(string path)
      {
         return Database.Get(this.Category, path, this.Locale);
      }

      /// <summary>
      /// Determines if a key exists in the locale.
      /// </summary>
      protected bool HasKey(string path, bool includeFallback = true)
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
      public BArray GetArray(string path)
      {
         return (BArray)Get(path);
      }

      /// <summary>
      /// Returns a BSON object given a JSON path into the data set. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">path/key in the category</param>
      public BObject GetObject(string path)
      {
         return (BObject)Get(path);
      }

      /// <summary>
      /// Picks a random string inside a BSON array. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
      public string GetRandomArrayItem(string path, int? min = null, int? max = null)
      {
         var arr = GetArray(path);
         if( !arr.HasValues ) return string.Empty;
         return Random.ArrayElement(arr, min, max);
      }

      /// <summary>
      /// Picks a random string inside a BSON array, then formats it. Only simple "." dotted JSON paths are supported.
      /// </summary>
      /// <param name="path">key in the category</param>
      protected string GetFormattedValue(string path)
      {
         var value = GetRandomArrayItem(path);

         var tokenResult = ParseTokens(value);

         return Random.Replace(tokenResult);
      }

      /// <summary>
      /// Recursive parse the tokens in the string.
      /// </summary>
      /// <param name="value">The value.</param>
      private string ParseTokens(string value)
      {
         var regex = new Regex("\\#{(.*?)\\}");
         var cityResult = regex.Replace(value,
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
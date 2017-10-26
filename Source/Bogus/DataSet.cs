using System;
using System.Text.RegularExpressions;
using Bogus.Bson;
using Bogus.Platform;

namespace Bogus
{
    /// <summary>
    /// Data set methods that access the JSON database of locales.
    /// </summary>
    public class DataSet : ILocaleAware
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

            this.Random = new Randomizer();
        }

        /// <summary>
        /// The Randomizer
        /// </summary>
        public Randomizer Random { get; set; }

        /// <summary>
        /// The category name inside the locale
        /// </summary>
        protected string Category { get; set; }

        /// <summary>
        /// Current locale of the data set.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// This method accesses the JSON path of a locale dataset LOCALE.CATEGORY.KEY and returns the JToken.
        /// </summary>
        /// <param name="keyOrSubPath">key in the category</param>
        /// <returns></returns>
        public BValue Get(string path)
        {
            return Database.Get(this.Category, path, Locale);
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
      /// Helper method to access LOCALE.CATEGORY.KEY of a locale data set and returns it as a JArray.
      /// </summary>
      /// <param name="keyOrSubPath">key in the category</param>
      /// <returns></returns>
      public BArray GetArray(string path)
        {
            return (BArray)Get(path);
        }

        /// <summary>
        /// Helper method to access LOCALE.CATEGORY.KEY of a locale data set and returns it as a JObject.
        /// </summary>
        /// <param name="keyOrSubPath">key in the category</param>
        /// <returns></returns>
        public BObject GetObject(string path)
        {
           return (BObject)Get(path);
        }

        /// <summary>
        /// Helper method to access LOCALE.CATEGORY.KEY of a locale data set and returns a random element.
        /// It assumes LOCALE.CATEGORY.KEY is a JArray.
        /// </summary>
        /// <param name="keyOrSubPath">key in the category</param>
        /// <returns></returns>
        public string GetRandomArrayItem(string path)
        {
            var arr = GetArray(path);
            if( !arr.HasValues ) return string.Empty;
            return Random.ArrayElement(GetArray(path));
        }


        /// <summary>
        /// Retrieves a random value from the locale info.
        /// </summary>
        /// <param name="keyOrSubPath">key in the category</param>
        /// <returns>System.String.</returns>
        protected string GetFormattedValue(string path )
        {
            var value = GetRandomArrayItem( path );

            var tokenResult = ParseTokens( value );

            return Random.Replace( tokenResult );
        }

        /// <summary>
        /// Recursive parse the tokens in the string .
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        private string ParseTokens( string value )
        {
            var regex = new Regex( "\\#{(.*?)\\}" );
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

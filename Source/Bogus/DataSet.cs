using System;
using Newtonsoft.Json.Linq;

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
            var categoryAttribute = Attribute.GetCustomAttribute(type, typeof(DataCategoryAttribute)) as DataCategoryAttribute;
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
        public JToken Get(string keyOrSubPath)
        {
            return Database.Get(this.Category, keyOrSubPath, Locale);
        }

        /// <summary>
        /// Helper method to access LOCALE.CATEGORY.KEY of a locale data set and returns it as a JArray.
        /// </summary>
        /// <param name="keyOrSubPath">key int the category</param>
        /// <returns></returns>
        public JArray GetArray(string keyOrSubPath)
        {
            return (JArray)Get(keyOrSubPath);
        }

        /// <summary>
        /// Helper method to access LOCALE.CATEGORY.KEY of a locale data set and returns it as a JObject.
        /// </summary>
        /// <param name="keyOrSubPath">key int the category</param>
        /// <returns></returns>
        public JObject GetObject(string keyOrSubPath)
        {
            return (JObject)Get(keyOrSubPath);
        }

        /// <summary>
        /// Helper method to access LOCALE.CATEGORY.KEY of a locale data set and returns a random element.
        /// It assumes LOCALE.CATEGORY.KEY is a JArray.
        /// </summary>
        /// <param name="keyOrSubPath">key int the category</param>
        /// <returns></returns>
        public string GetRandomArrayItem(string keyOrSubPath)
        {
            return Random.ArrayElement(GetArray(keyOrSubPath));
        }
    }
}

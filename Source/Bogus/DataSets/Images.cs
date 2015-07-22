namespace Bogus.DataSets
{
    /// <summary>
    /// Generates images URLs from lorempixel.com
    /// </summary>
    [DataCategory("image")]
    public class Images : DataSet
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
        public Images(string locale = "en") : base(locale)
        {
        }

        /// <summary>
        /// Gets a random image.
        /// </summary>
        /// <returns></returns>
        public string Image()
        {
            var categories = new[]
                {"abstract", "animals", "business", "cats", "city", "food", "nightlife", "fashion", "people", "nature", "sports", "technics", "transport"};

            var picked = Random.ArrayElement(categories);
            return ImageUrl(picked);
        }

        
        //TODO: Add support for greyscale
        /// <summary>
        /// Creates an image URL with http://lorempixel.com.
        /// </summary>
        protected virtual string ImageUrl( string category, int width = 640, int height = 480)
        {
            var path = string.Format("http://lorempixel.com/{0}/{1}/{2}", width, height, category);
            return string.IsNullOrWhiteSpace(category) ? path.Trim('/') : path;
        }

        /// <summary>
        /// Gets an abstract looking image.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Abstract(int width = 640, int height = 480)
        {
            return ImageUrl("abstract", width, height);
        }

        /// <summary>
        /// Gets an image of an animal.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Animals(int width = 640, int height = 480)
        {
            return ImageUrl("animals", width, height);
        }

        /// <summary>
        /// Gets a business looking image.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Business(int width = 640, int height = 480)
        {
            return ImageUrl("business", width, height);
        }

        /// <summary>
        /// Gets a picture of a cat.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Cats(int width = 640, int height = 480)
        {
            return ImageUrl("cats", width, height);
        }

        /// <summary>
        /// Gets a city looking image.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string City(int width = 640, int height = 480)
        {
            return ImageUrl("city", width, height);
        }

        /// <summary>
        /// Gets an image of food.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Food(int width = 640, int height = 480)
        {
            return ImageUrl("food", width, height);

        }

        /// <summary>
        /// Gets an image with city looking nightlife.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Nightlife(int width = 640, int height = 480)
        {
            return ImageUrl("nightlife", width, height);

        }

        /// <summary>
        /// Gets an image in the fashion category.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Fashion(int width = 640, int height = 480)
        {
            return ImageUrl("fashion", width, height);

        }

        /// <summary>
        /// Gets an image of humans.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string People(int width = 640, int height = 480)
        {
            return ImageUrl("people", width, height);

        }

        /// <summary>
        /// Gets an image of nature.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Nature(int width = 640, int height = 480)
        {
            return ImageUrl("nature", width, height);

        }

        /// <summary>
        /// Gets an image related to sports.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Sports(int width = 640, int height = 480)
        {
            return ImageUrl("sports", width, height);

        }

        /// <summary>
        /// Get a technology related image.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Technics(int width = 640, int height = 480)
        {
            return ImageUrl("technics", width, height);

        }

        /// <summary>
        /// Get a transportation related image.
        /// </summary>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        /// <returns></returns>
        public string Transport(int width = 640, int height = 480)
        {
            return ImageUrl("transport", width, height);

        }
    }
}
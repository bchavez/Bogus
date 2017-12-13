using System;

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
      public string Image(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         var categories = new[]
               {"abstract", "animals", "business", "cats", "city", "food", "nightlife", "fashion", "people", "nature", "sports", "technics", "transport"};

         var picked = Random.ArrayElement(categories);
         return ImageUrl(picked, width, height, randomize, https);
      }


      //TODO: Add support for greyscale
      /// <summary>
      /// Creates an image URL with http://lorempixel.com.
      /// </summary>
      protected virtual string ImageUrl(string category, int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         var proto = "http://";
         if( https )
         {
            proto = "https://";
         }
         var url = $"{proto}lorempixel.com/{width}/{height}";
         if( !string.IsNullOrWhiteSpace(category) )
         {
            url += $"/{category}";
            if( randomize )
            {
               url += $"/{this.Random.Number(1, 10)}";
            }
         }

         return url;
      }

      /// <summary>
      /// Gets an abstract looking image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Abstract(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("abstract", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of an animal.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Animals(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("animals", width, height, randomize, https);
      }

      /// <summary>
      /// Gets a business looking image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Business(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("business", width, height, randomize, https);
      }

      /// <summary>
      /// Gets a picture of a cat.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Cats(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("cats", width, height, randomize, https);
      }

      /// <summary>
      /// Gets a city looking image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string City(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("city", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of food.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Food(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("food", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image with city looking nightlife.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Nightlife(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("nightlife", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image in the fashion category.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Fashion(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("fashion", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of humans.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string People(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("people", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of nature.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Nature(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("nature", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image related to sports.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Sports(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("sports", width, height, randomize, https);
      }

      /// <summary>
      /// Get a technology related image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Technics(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("technics", width, height, randomize, https);
      }

      /// <summary>
      /// Get a transportation related image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      public string Transport(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return ImageUrl("transport", width, height, randomize, https);
      }

      /// <summary>
      /// Get a SVG data URI image with a specific width and height.
      /// </summary>
      /// <param name="width">Width of the image.</param>
      /// <param name="height">Height of the image.</param>
      public string DataUri(int width, int height)
      {
         var rawPrefix = "data:image/svg+xml;charset=UTF-8,";
         var svgString =
            $@"<svg xmlns=""http://www.w3.org/2000/svg"" version=""1.1"" baseProfile=""full"" width=""{width}"" height=""{
                  height
               }""> <rect width=""100%"" height=""100%"" fill=""grey""/>  <text x=""0"" y=""20"" font-size=""20"" text-anchor=""start"" fill=""white"">{width}x{
                  height
               }</text> </svg>";

         return rawPrefix + Uri.EscapeDataString(svgString);
      }
   }
}
using System;

namespace Bogus.DataSets
{
   public static class LoremPixelCategory
   {
      public const string Abstract = "abstract";
      public const string Animals = "animals";
      public const string Business = "business";
      public const string Cats = "cats";
      public const string City = "city";
      public const string Food = "food";
      public const string Nightlife = "nightlife";
      public const string Fashion = "fashion";
      public const string People = "people";
      public const string Nature = "nature";
      public const string Sports = "sports";
      public const string Technics = "technics";
      public const string Transport = "transport";
      public const string Random = "random";
   }

   public partial class Images
   {
      /// <summary>
      /// Gets a random LoremPixel.com image.
      /// </summary>
      [Obsolete("Please use Images.LoremPixelUrl(). Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Image(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl(LoremPixelCategory.Random, width, height, randomize, https);
      }
  
      /// <summary>
      /// Gets an abstract looking image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Abstract. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Abstract(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("abstract", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of an animal.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Animals. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Animals(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("animals", width, height, randomize, https);
      }

      /// <summary>
      /// Gets a business looking image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Business. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Business(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("business", width, height, randomize, https);
      }

      /// <summary>
      /// Gets a picture of a cat.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Cats. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Cats(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("cats", width, height, randomize, https);
      }

      /// <summary>
      /// Gets a city looking image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.City. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string City(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("city", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of food.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Food. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Food(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("food", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image with city looking nightlife.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Nightlife. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Nightlife(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("nightlife", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image in the fashion category.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Fashion. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Fashion(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("fashion", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of humans.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.People. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string People(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("people", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image of nature.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Nature. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Nature(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("nature", width, height, randomize, https);
      }

      /// <summary>
      /// Gets an image related to sports.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Sports. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Sports(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("sports", width, height, randomize, https);
      }

      /// <summary>
      /// Get a technology related image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Technics. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Technics(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("technics", width, height, randomize, https);
      }

      /// <summary>
      /// Get a transportation related image.
      /// </summary>
      /// <param name="width">Width</param>
      /// <param name="height">Height</param>
      /// <param name="randomize">Adds a random cache busting number to the URL</param>
      /// <param name="https">Uses https:// protocol</param>
      [Obsolete("Please use Images.LoremPixelUrl() method with category:LoremPixelCategory.Transport. Consider using PicsumUrl() method as a more reliable/faster service.")]
      public string Transport(int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         return LoremPixelUrl("transport", width, height, randomize, https);
      }
   }
}
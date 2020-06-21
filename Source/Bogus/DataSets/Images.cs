using System;
using System.Text;

namespace Bogus.DataSets
{
   /// <summary>
   /// Generates images URLs.
   /// </summary>
   [DataCategory("image")]
   public partial class Images : DataSet
   {
      /// <summary>
      /// Default constructor
      /// </summary>
      public Images(string locale = "en") : base(locale)
      {
      }

      /// <summary>
      /// Get a SVG data URI image with a specific width and height.
      /// </summary>
      /// <param name="width">Width of the image.</param>
      /// <param name="height">Height of the image.</param>
      /// <param name="htmlColor">An html color in named format 'grey', RGB format 'rgb(r,g,b)', or hex format '#888888'.</param>
      public string DataUri(int width, int height, string htmlColor = "grey")
      {
         var rawPrefix = "data:image/svg+xml;charset=UTF-8,";
         var svgString =
            $@"<svg xmlns=""http://www.w3.org/2000/svg"" version=""1.1"" baseProfile=""full"" width=""{width}"" height=""{height}""><rect width=""100%"" height=""100%"" fill=""{htmlColor}""/><text x=""{width / 2}"" y=""{height / 2}"" font-size=""20"" alignment-baseline=""middle"" text-anchor=""middle"" fill=""white"">{width}x{height}</text></svg>";

         return rawPrefix + Uri.EscapeDataString(svgString);
      }

      /// <summary>
      /// Get an image from the https://placeimg.com service.
      /// </summary>
      /// <param name="width">Width of the image.</param>
      /// <param name="height">Height of the image.</param>
      /// <param name="category">Category of the image. See <seealso cref="PlaceImgCategory"/> for string categories.</param>
      /// <param name="filter">The filter to apply to the image. See <seealso cref="PlaceImgFilter"/>.</param>
      /// <returns></returns>
      public string PlaceImgUrl(
         int width = 640, int height = 480,
         string category = PlaceImgCategory.Any,
         PlaceImgFilter? filter = null)
      {
         //https://placeimg.com/640/480/nature/grayscale
         //https://placeimg.com/640/480/nature
         //https://placeimg.com/640/480/any/sepia
         //https://placeimg.com/640/480/any

         const string Url = "https://placeimg.com";

         var sb = new StringBuilder(Url);
         sb.Append($"/{width}/{height}/{category}");

         if (filter is PlaceImgFilter.Grayscale)
         {
            sb.Append("/grayscale");
         }
         else if ( filter is PlaceImgFilter.Sepia )
         {
            sb.Append("/sepia");
         }

         return sb.ToString();

      }

      /// <summary>
      /// Get an image from the https://picsum.photos service.
      /// </summary>
      /// <param name="width">Width of the image.</param>
      /// <param name="height">Height of the image.</param>
      /// <param name="grayscale">Grayscale (no color) image.</param>
      /// <param name="blur">Blurry image.</param>
      /// <param name="imageId">Optional Image ID found here https://picsum.photos/images</param>
      public string PicsumUrl(int width = 640, int height = 480, bool grayscale = false, bool blur = false, int? imageId = null )
      {
         const string Url = "https://picsum.photos";

         var sb = new StringBuilder(Url);

         if (grayscale)
         {
            sb.Append("/g");
         }

         sb.Append($"/{width}/{height}");
         
         var n = imageId ?? this.Random.Number(0, 1084);
         sb.Append($"/?image={n}");

         if (blur)
         {
            sb.Append("&blur");
         }

         return sb.ToString();
      }

      /// <summary>
      /// Get an image from https://placeholder.com service.
      /// </summary>
      /// <param name="width">Width of the image.</param>
      /// <param name="height">Height of the image.</param>
      /// <param name="text"></param>
      /// <param name="format">Image format. Supported values: 'jpg', 'jpeg', 'png', 'gif', 'webp'.</param>
      /// <param name="backColor">HTML color code for the background color.</param>
      /// <param name="textColor">HTML color code for the foreground (text) color.</param>
      public string PlaceholderUrl(int width, int height, string text = null, string backColor = "cccccc", string textColor = "9c9c9c", string format = "png")
      {
         const string Url = "https://via.placeholder.com/";

         var sb = new StringBuilder(Url);

         sb.Append(width)
            .Append("x")
            .Append(height)
            .Append("/")
            .Append(backColor)
            .Append("/")
            .Append(textColor)
            .Append(".")
            .Append(format);

         if( text != null )
         {
            sb.Append("?text=")
               .Append(Uri.EscapeUriString(text));
         }

         return sb.ToString();
      }

      /// <summary>
      /// Get an image from https://loremflickr.com service.
      /// </summary>
      /// <param name="keywords">Space or comma delimited list of keywords you want the picture to contain. IE: "cat, dog" for images with cats and dogs.</param>
      /// <param name="width">The image width.</param>
      /// <param name="height">The image height.</param>
      /// <param name="grascale">Grayscale the image.</param>
      /// <param name="matchAllKeywords">True tries to match an image with all specified keywords. False tries to match an image with any specified keyword.</param>
      /// <param name="lockId">Deterministic image id. By default, this method generates URLs with image lock ids.
      /// So, if a random seed is set, repeat runs of this method will generate the same lock id sequence
      /// for images. If you want explicit control over the lock id, you can pass it as a parameter here.
      /// Additionally, if you don't want any lock ids, pass -1 for this parameter this method will generate
      /// a URL that will result in a new random image every time the HTTP URL is hit.
      /// </param>
      public string LoremFlickrUrl(
         int width = 320, int height = 240,
         string keywords = null,
         bool grascale = false,
         bool matchAllKeywords = false, int? lockId = null)
      {
         const string Url = "https://loremflickr.com";

         var sb = new StringBuilder();
         if (grascale)
         {
            sb.Append("/g");
         }

         sb.Append($"/{width}/{height}");

         if (keywords != null)
         {
            var tags = keywords.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
            var cleanTags = string.Join(",", tags);
            var match = matchAllKeywords ? "all" : "any";
            sb.Append($"/{cleanTags}/{match}");
         }

         if( lockId is null )
         {
            lockId = this.Random.Number(int.MaxValue);
         }
         if( lockId >= 0 )
         {
            sb.Append($"?lock={lockId}");
         }

         return Url + sb;
      }

      /// <summary>
      /// Creates an image URL with http://lorempixel.com. Note: This service is slow. Consider using PicsumUrl() as a faster alternative.
      /// </summary>
      public string LoremPixelUrl(string category = LoremPixelCategory.Random, int width = 640, int height = 480, bool randomize = false, bool https = false)
      {
         if( category == LoremPixelCategory.Random )
         {
            var categories = new[]
               {
                  LoremPixelCategory.Abstract,
                  LoremPixelCategory.Animals,
                  LoremPixelCategory.Business,
                  LoremPixelCategory.Cats,
                  LoremPixelCategory.City,
                  LoremPixelCategory.Food,
                  LoremPixelCategory.Nightlife,
                  LoremPixelCategory.Fashion,
                  LoremPixelCategory.People,
                  LoremPixelCategory.Nature,
                  LoremPixelCategory.Sports,
                  LoremPixelCategory.Technics,
                  LoremPixelCategory.Transport
               };

            category = this.Random.ArrayElement(categories);
         }

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
   }
}
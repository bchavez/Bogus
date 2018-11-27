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

         var sb = new StringBuilder();

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

         return Url + sb;
      }
   }
}
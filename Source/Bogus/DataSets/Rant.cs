using System.Linq;

namespace Bogus.DataSets
{
   /// <summary>
   /// Generates random user content.
   /// </summary>
   public class Rant : DataSet
   {
      /// <summary>
      /// Generates a random user review.
      /// </summary>
      public string Review(string product = "product")
      {
         return this.GetRandomArrayItem("review")
            .Replace("$product", product);
      }

      /// <summary>
      /// Generate an array of random reviews.
      /// </summary>
      public string[] Reviews(string product = "product", int lines = 2)
      {
         return Enumerable.Range(1, lines)
            .Select(_ => this.Review(product))
            .ToArray();
      }
   }
}
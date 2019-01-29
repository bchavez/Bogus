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
      /// <param name="product">The name of the product.</param>
      /// <returns>A user review as a string.</returns>
      public string Review(string product = "product")
      {
         return this.GetRandomArrayItem("review")
            .Replace("$product", product);
      }

      /// <summary>
      /// Generate an array of random reviews.
      /// </summary>
      /// <param name="product">The name of the product.</param>
      /// <param name="lines">The number of reviews to be generated.</param>
      /// <returns>A string array of user reviews.</returns>
      public string[] Reviews(string product = "product", int lines = 2)
      {
         return Enumerable.Range(1, lines)
            .Select(_ => this.Review(product))
            .ToArray();
      }
   }
}
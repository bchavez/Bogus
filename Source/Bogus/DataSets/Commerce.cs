using System;
using System.Linq;

namespace Bogus.DataSets
{
   /// <summary>
   /// Methods relating to commerce.
   /// </summary>
   public class Commerce : DataSet
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Commerce"/> class.
      /// </summary>
      /// <param name="locale">The locale used to generate the values.</param>
      public Commerce(string locale = "en") : base(locale)
      {
      }

      /// <summary>
      /// Get a random commerce department.
      /// </summary>
      /// <param name="max">The maximum amount of departments</param>
      /// <param name="returnMax">If true the method returns the max amount of values, otherwise the number of categories returned is between 1 and max.</param>
      /// <returns>A random commerce department.</returns>
      public string Department(int max = 3, bool returnMax = false)
      {
         var num = max;

         if( !returnMax )
         {
            num = this.Random.Number(1, max);
         }

         var cats = Categories(num);
         if( num > 1 )
         {
            var catJoin = string.Join(", ", cats.Take(cats.Length - 1));
            var catLast = cats.Last();
            return $"{catJoin} & {catLast}";
         }

         return cats[0];
      }

      // there is an easier way to do this.
      // check finance.amount
      /// <summary>
      /// Get a random product price.
      /// </summary>
      /// <param name="min">The minimum price.</param>
      /// <param name="max">The maximum price.</param>
      /// <param name="decimals">How many decimals the number may include.</param>
      /// <param name="symbol">The symbol in front of the price.</param>
      /// <returns>A randomly generated price.</returns>
      public string Price(decimal min = 1, decimal max = 1000, int decimals = 2, string symbol = "")
      {
         var amount = max - min;
         var part = (decimal)Random.Double() * amount;
         return symbol + Math.Round(min + part, decimals);
      }

      /// <summary>
      /// Get random product categories.
      /// </summary>
      /// <param name="num">The amount of categories to be generated.</param>
      /// <returns>A collection of random product categories.</returns>
      public string[] Categories(int num)
      {
         var result = new string[num];

         for( var i = 0; i < num; i++ )
         {
            result[i] = GetRandomArrayItem("department");
         }

         return result;
      }

      /// <summary>
      /// Get a random product name.
      /// </summary>
      /// <returns>A random product name.</returns>
      public string ProductName()
      {
         return $"{ProductAdjective()} {ProductMaterial()} {Product()}";
      }

      /// <summary>
      /// Get a random color.
      /// </summary>
      /// <returns>A random color.</returns>
      public string Color()
      {
         return GetRandomArrayItem("color");
      }

      /// <summary>
      /// Get a random product.
      /// </summary>
      /// <returns>A random product.</returns>
      public string Product()
      {
         return GetRandomArrayItem("product_name.product");
      }

      /// <summary>
      /// Random product adjective.
      /// </summary>
      /// <returns>A random product adjective.</returns>
      public string ProductAdjective()
      {
         return GetRandomArrayItem("product_name.adjective");
      }

      /// <summary>
      /// Random product material.
      /// </summary>
      /// <returns>A random product material.</returns>
      public string ProductMaterial()
      {
         return GetRandomArrayItem("product_name.material");
      }

      /// <summary>
      /// Random product description.
      /// </summary>
      /// <returns>A random product description.</returns>
      public string ProductDescription()
      {
         return GetRandomArrayItem("product_description");
      }

      /// <summary>
      /// EAN-8 checksum weights.
      /// </summary>
      protected static int[] Ean8Weights = {3, 1, 3, 1, 3, 1, 3};

      /// <summary>
      /// Get a random EAN-8 barcode number.
      /// </summary>
      /// <returns>A random EAN-8 barcode number.</returns>
      public string Ean8()
      {
         // [3, 1, 3, 1, 3, 1, 3]
         return this.Ean(8, Ean8Weights);
      }

      /// <summary>
      /// EAN-18 checksum weights.
      /// </summary>
      protected static int[] Ean13Weights = { 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3 };

      /// <summary>
      /// Get a random EAN-13 barcode number.
      /// </summary>
      /// <returns>A random EAN-13 barcode number.</returns>
      public string Ean13()
      {
         // [1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3]
         return this.Ean(13, Ean13Weights);
      }

      private string Ean(int length, int[] weights)
      {
         var digits = this.Random.Digits(length - 1);

         var weightedSum =
            digits.Zip(weights,
                  (d, w) => d * w)
               .Sum();

         var checkDigit = (10 - weightedSum % 10) % 10;

         return string.Join("", digits) + checkDigit;
      }
   }
}
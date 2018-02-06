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
      /// Default constructor
      /// </summary>
      public Commerce(string locale = "en") : base(locale)
      {
      }

      /// <summary>
      /// Get a random commerce department.
      /// </summary>
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
            return string.Format("{0} & {1}", string.Join(", ", cats.Take(cats.Length - 1)),
               cats.Last());
         }

         return cats[0];
      }

      // there is an easier way to do this.
      // check finance.amount
      /// <summary>
      /// Get a random product price.
      /// </summary>
      public string Price(decimal min = 1, decimal max = 1000, int decimals = 2, string symbol = "")
      {
         var amount = (max - min);
         var part = (decimal)Random.Double() * amount;
         return symbol + Math.Round(min + part, decimals);
      }

      /// <summary>
      /// Get random product categories.
      /// </summary>
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
      public string ProductName()
      {
         return $"{ProductAdjective()} {ProductMaterial()} {Product()}";
      }

      /// <summary>
      /// Get a random color.
      /// </summary>
      public string Color()
      {
         return GetRandomArrayItem("color");
      }

      /// <summary>
      /// Get a random product.
      /// </summary>
      public string Product()
      {
         return GetRandomArrayItem("product_name.product");
      }

      /// <summary>
      /// Random product adjective.
      /// </summary>
      public string ProductAdjective()
      {
         return GetRandomArrayItem("product_name.adjective");
      }

      /// <summary>
      /// Random product material.
      /// </summary>
      public string ProductMaterial()
      {
         return GetRandomArrayItem("product_name.material");
      }
   }
}
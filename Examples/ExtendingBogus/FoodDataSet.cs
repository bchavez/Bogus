using Bogus;
using Bogus.Premium;

namespace ExtendingBogus
{
   /// <summary>
   /// The following shows how to create a dedicated DataSet accessible via C# extension method.
   /// </summary>
   public static class ExtensionsForFood
   {
      public static Food Food(this Faker faker)
      {
         return ContextHelper.GetOrSet(faker, () => new Food());
      }
   }

   /// <summary>
   /// This DatSet can be created manually using `new Candy()`, or by fluent extension method via <seealso cref="ExtensionsForFood"/>.
   /// </summary>
   public class Food : DataSet
   {
      private static readonly string[] Candies =
         {
            "Hard candy", "Taffy", "Chocolate bar", "Stick candy",
            "Jelly bean", "Mint", "Cotton candy", "Lollipop"
         };

      /// <summary>
      /// Returns some type of candy.
      /// </summary>
      public string Candy()
      {
         return this.Random.ArrayElement(Candies);
      }

      private static readonly string[] Drinks = { "Soda", "Water", "Beer", "Wine", "Coffee", "Lemonade", "Milk" };
      public string Drink()
      {
         return this.Random.ArrayElement(Drinks);
      }
   }

}

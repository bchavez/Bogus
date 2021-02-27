using Bogus;

namespace ExtendingBogus
{
   class Program
   {
      static void Main(string[] args)
      {
         var userFaker = new Faker<User>()
            //Extend Bogus with a 'new' Food data set; see FoodDataSet.cs
            .RuleFor(p => p.FaveCandy, f => f.Food().Candy()) 
            .RuleFor(p => p.FaveDrink, f => f.Food().Drink())
            //Extend the existing Address data set with a custom C# extension method; see ExtensionsForAddress.cs
            .RuleFor(p => p.PostCode, f => f.Address.DowntownTorontoPostalCode()); 

         var user = userFaker.Generate();
         user.Dump();
      }
   }

   public class User
   {
      public string FaveDrink;
      public string FaveCandy;
      public string PostCode;
   }
}

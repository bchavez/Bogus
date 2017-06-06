namespace Bogus.Tests.AutoFakerTests.Models.Complex
{
   public sealed class Product
   {
      public Product(int id)
      {
         this.Id = id;
      }

      public int Id { get; }
      public string Description { get; set; }
      public Price Price { get; set; }
   }
}
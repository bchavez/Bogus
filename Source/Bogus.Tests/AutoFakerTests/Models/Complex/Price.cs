namespace Bogus.Tests.AutoFakerTests.Models.Complex
{
   public struct Price
   {
      public Price(decimal amount, string units)
      {
         this.Amount = amount;
         this.Units = units;
      }

      public decimal Amount { get; }
      public string Units { get; }
   }
}
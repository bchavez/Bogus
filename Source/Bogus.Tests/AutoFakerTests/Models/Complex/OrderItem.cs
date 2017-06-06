using System.Collections.Generic;

namespace Bogus.Tests.AutoFakerTests.Models.Complex
{
   public sealed class OrderItem
   {
      public OrderItem(Product product)
      {
         this.Product = product;
      }

      public Product Product { get; }
      public Quantity Quantity { get; set; }
      public IDictionary<int, decimal> Discounts { get; set; }
   }
}
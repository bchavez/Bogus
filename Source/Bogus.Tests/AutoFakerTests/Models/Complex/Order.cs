using System;
using System.Collections.Generic;

namespace Bogus.Tests.AutoFakerTests.Models.Complex
{
   public sealed class Order
   {
      public DateTime Timestamp;

      public Order(int id, ICalculator calculator)
      {
         this.Id = id;
         this.Calculator = calculator;
      }

      public int Id { get; }
      public ICalculator Calculator { get; }
      public Guid? Code { get; set; }
      public Status Status { get; set; }
      public DiscountBase[] Discounts { get; set; }
      public IEnumerable<OrderItem> Items { get; set; }
   }
}
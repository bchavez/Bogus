using System;

namespace Bogus
{
   /// <summary>
   /// A wrapper around System.Random that supports the IRandom interface.
   /// </summary>
   internal class RandomWrapper : IRandom
   {
      private readonly Random random;

      public RandomWrapper(Random random)
      {
         this.random = random ?? throw new ArgumentNullException(nameof(random));
      }

      public int Next()
      {
         return random.Next();
      }

      public int Next(int minValue, int maxValue)
      {
         return random.Next(minValue, maxValue);
      }

      public void NextBytes(byte[] buffer)
      {
         random.NextBytes(buffer);
      }

      public double NextDouble()
      {
         return random.NextDouble();
      }
   }
}

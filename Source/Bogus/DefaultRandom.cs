using System;

namespace Bogus
{
   /// <summary>
   /// A wrapper around <see cref="Random"/> that supports the <see cref="IRandom"/> interface.
   /// </summary>
   internal class DefaultRandom : IRandom
   {
      private readonly Random random;

      public DefaultRandom(Random random)
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

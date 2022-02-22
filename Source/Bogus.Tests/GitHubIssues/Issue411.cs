using System;
using System.Collections.Generic;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue411
   {
      [Fact]
      public void randomizer_uses_custom_PRNG()
      {
         var customRandomizer = new CustomRandomizer();

         for (int i = 0; i < 10; i++)
            Assert.Equal(CustomRandom.randomInts[i % CustomRandom.randomInts.Count], customRandomizer.Number(int.MaxValue));

      }

      private class CustomRandom : Random
      {
         public static readonly List<int> randomInts = new List<int> { 4, 3, 9, 2001, 42, 7, 13 };
         private int randomIntIndex = 0;

         public override int Next()
         {
            if (randomIntIndex >= randomInts.Count)
               randomIntIndex = 0;

            return randomInts[randomIntIndex++];
         }

         public override int Next(int minValue, int maxValue) => Next() % (maxValue - minValue) + minValue;
         public override void NextBytes(byte[] buffer) => throw new NotImplementedException();
         public override double NextDouble() => throw new NotImplementedException();
      }

      private class CustomRandomizer : Randomizer
      {
         public CustomRandomizer()
         {
            localSeed = new CustomRandom();
         }
      }
   }
}

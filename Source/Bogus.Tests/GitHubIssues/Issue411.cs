using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue411
   {
      [Fact]
      public void randomizer_uses_custom_PRNG()
      {
         var customRandomizer = new CustomRandomizer();

         for (int i = 0; i < 10; i++)
         {          
            var index = i % CustomRandom.Data.Count;
            var knownValue = CustomRandom.Data[index];

            customRandomizer.Number(int.MaxValue).Should().Be(knownValue);
         }
      }

      private class CustomRandom : Random
      {
         public static readonly List<int> Data = new List<int> { 4, 3, 9, 2001, 42, 7, 13 };
         private int index = 0;

         public override int Next()
         {
            if (index >= Data.Count)
            {
               index = 0;
            }

            return Data[index++];
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

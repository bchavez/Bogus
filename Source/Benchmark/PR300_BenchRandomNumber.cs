using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Bogus;

namespace Benchmark
{
   [SimpleJob(RuntimeMoniker.NetCoreApp31), SimpleJob(RuntimeMoniker.Net471)]
   [MarkdownExporter, MemoryDiagnoser, RPlotExporter]
   public class PR300_BenchRandomNumber
   {
      private NumberTests n;

      [GlobalSetup]
      public void Setup()
      {
         n = new NumberTests();
      }

      [Benchmark]
      public int OldMethod()
      {
         return n.Number(int.MinValue, int.MaxValue);
      }

      [Benchmark]
      public int BitShift()
      {
         return n.NumberBitShift(int.MinValue, int.MaxValue);
      }

      [Benchmark]
      public int JDGMethod()
      {
         return n.NumberJDG(int.MinValue, int.MaxValue);
      }

      [Benchmark]
      public int JDGMethod2()
      {
         return n.NumberJDG2(int.MinValue, int.MaxValue);
      }
   }

   public class NumberTests 
   {
      internal static Lazy<object> Locker = new Lazy<object>(() => new object(), LazyThreadSafetyMode.ExecutionAndPublication);

      private readonly Random localSeed = new System.Random();

      private static byte[] temp = new byte[4];

      public int Number(int min = 0, int max = 1)
      {
         //lock any seed access, for thread safety.
         lock (Locker.Value)
         {
            //Clamp max value, Issue #30.
            max = max == int.MaxValue ? max : max + 1;
            return localSeed.Next(min, max);
         }
      }

      public int NumberBitShift(int min = 0, int max = 1)
      {
         //lock any seed access, for thread safety.
         lock (Locker.Value)
         {
            // Adjust the range as needed to make max inclusive. The Random.Next function uses exclusive upper bounds.

            // If max can be extended by 1, just do that.
            if (max < int.MaxValue) return localSeed.Next(min, max + 1);

            // If max is exactly int.MaxValue, then check if min can be used to push the range out by one the other way.
            // If so, then we can simply add one to the result to put it back in the correct range.
            if (min > int.MinValue) return 1 + localSeed.Next(min - 1, max);

            localSeed.NextBytes(temp);
            return temp[0] << 24 | temp[1] << 16 | temp[2] << 8 | temp[3];
         }
      }

      public int NumberJDG(int min = 0, int max = 1)
      {
         //lock any seed access, for thread safety.
         lock (Locker.Value)
         {
            // Adjust the range as needed to make max inclusive. The Random.Next function uses exclusive upper bounds.

            // If max can be extended by 1, just do that.
            if (max < int.MaxValue) return localSeed.Next(min, max + 1);

            // If max is exactly int.MaxValue, then check if min can be used to push the range out by one the other way.
            // If so, then we can simply add one to the result to put it back in the correct range.
            if (min > int.MinValue) return 1 + localSeed.Next(min - 1, max);

            // If we hit this line, then min is int.MinValue and max is int.MaxValue, which mean the caller wants a
            // number from a range spanning all possible values of int. The Random class only supports exclusive
            // upper bounds, period, and the upper bound must be specified as an int, so the best we can get in a
            // single call is a value in the range (int.MinValue, int.MaxValue - 1). Instead, what we do is get two
            // samples, one in the range (int.MinValue, -1) and the other as unbiased as possible, and using the
            // second one to decide, 50% of the time we invert all the bits in the sample, shifting its range to
            // (0, int.MaxValue).
            var result = localSeed.Next(int.MinValue, 0);

            if ((localSeed.Next() & 0x10000000) == 0)
               result = ~result;

            return result;
         }
      }

      public int NumberJDG2(int min = 0, int max = 1)
      {
         lock( Locker.Value )
         {
            if (max < int.MaxValue) return localSeed.Next(min, max + 1);
            if (min > int.MinValue) return 1 + localSeed.Next(min - 1, max);

            int sample1 = localSeed.Next();
            int sample2 = localSeed.Next();

            int topHalf = (sample1 >> 8) & 0xFFFF;
            int bottomHalf = (sample2 >> 8) & 0xFFFF;

            return (topHalf << 16) | bottomHalf;
         }
      }
   }
}
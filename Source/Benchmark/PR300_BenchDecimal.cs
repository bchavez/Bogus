using System;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Bogus;

namespace Benchmark
{
   [SimpleJob(RuntimeMoniker.NetCoreApp31), SimpleJob(RuntimeMoniker.Net471)]
   [MarkdownExporter, MemoryDiagnoser, RPlotExporter]
   public class PR300_BenchDecimal
   {
      private CustomRandomizer r;

      [GlobalSetup]
      public void Setup()
      {
         r = new CustomRandomizer();
      }

      [Benchmark]
      public decimal OldMethod()
      {
         return r.Decimal();
      }

      [Benchmark]
      public decimal JDGMethod()
      {
         return r.DecimalJDG();
      }

      [Benchmark]
      public decimal JDGMethodNoAlloc()
      {
         return r.DecimalJDGNoAlloc();
      }

      [Benchmark]
      public decimal JDGMethodNoAllocMult()
      {
         return r.DecimalJDGNoAllocMult();
      }
   }

   public class CustomRandomizer : Randomizer
   {
      internal static Lazy<object> Locker = new Lazy<object>(() => new object(), LazyThreadSafetyMode.ExecutionAndPublication);
      
      private readonly Random localSeed = new Random();

      public int NumberJDG(int min = 0, int max = 1)
      {
         //lock any seed access, for thread safety.
         lock (Locker.Value)
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

      public decimal DecimalJDG(decimal min = 0.0m, decimal max = 1.0m)
      {
         // Decimal: 128 bits wide
         //   bit 0: sign bit
         //   bit 1-10: not used
         //   bit 11-15: scale (values 29, 30, 31 not used)
         //   bit 16-31: not used
         //   bit 32-127: mantissa (96 bits)

         // Max value: 00000000 FFFFFFFF FFFFFFFF FFFFFFFF
         //          = 79228162514264337593543950335

         // Max value with max scaling: 001C0000  FFFFFFFF  FFFFFFFF  FFFFFFFF
         //                           = 7.9228162514264337593543950335

         // Step 1: Generate a value with uniform distribution between 0 and this value.
         // This ensures the greatest level of precision in the distribution of bits;
         // the resulting value, after it is adjusted into the caller's desired range,
         // should not skip any possible values at the least significant end of the
         // mantissa.

         int[] bits = new int[4];

         bits[0] = NumberJDG(int.MinValue, int.MaxValue);
         bits[1] = NumberJDG(int.MinValue, int.MaxValue);
         bits[2] = NumberJDG(int.MinValue, int.MaxValue);
         bits[3] = 0x1C0000;

         decimal result = new decimal(bits);

         // Step 2: Scale the value and adjust it to the desired range. This may decrease
         // the accuracy by adjusting the scale as necessary, but we get the best possible
         // outcome by starting with the most precise scale.
         return result * (max - min) / 7.9228162514264337593543950335m + min;
      }

      public decimal DecimalJDGNoAlloc(decimal min = 0.0m, decimal max = 1.0m)
      {
         int lo = NumberJDG(int.MinValue, int.MaxValue);
         int mid = NumberJDG(int.MinValue, int.MaxValue);
         int hi = NumberJDG(int.MinValue, int.MaxValue);
         byte scale = 0x1C;

         decimal result = new decimal(lo, mid, hi, false, scale);

         return result * (max - min) / 7.9228162514264337593543950335m + min;
      }

      public decimal DecimalJDGNoAllocMult(decimal min = 0.0m, decimal max = 1.0m)
      {
         int lo = NumberJDG(int.MinValue, int.MaxValue);
         int mid = NumberJDG(int.MinValue, int.MaxValue);
         int hi = NumberJDG(int.MinValue, int.MaxValue);
         byte scale = 0x1C;

         decimal result = new decimal(lo, mid, hi, false, scale);

         return result * (max - min) * 0.1262177448353618888658765704m + min;
      }
   }
}
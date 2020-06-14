using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmark
{
   [RPlotExporter]
   public class BenchSsn
   {
      private Randomizer r;

      [GlobalSetup]
      public void Setup()
      {
         r = new Randomizer(1337);
      }

      [Benchmark]
      public void SsnAlgo1()
      {
         var a = r.Int(1, 898);
         if (a == 666) a++;

         var b = r.Int(1, 99);
         var c = r.Int(1, 9999);

         var result = $"{a:000}-{b:00}-{c:0000}";
      }

      [Benchmark]
      public void SsnAlgo2()
      {
         var a = r.Int(1, 898);
         if (a == 666) a++;

         var b = r.Int(1, 99);
         var c = r.Int(1, 9999);

         var result = string.Format("{0:000}-{1:00}-{2:0000}", a, b, c);
      }

      [Benchmark]
      public void SsnAlgo3()
      {
         var x = r.Int();

         // right shift all bits except fir the first 10 bits = 2^10 = 1024.
         var a = (x >> (32 - 10)) % 898;
         if (a == 0 || a == 666) a++;

         // use the first 7 bits = 2^7 = 128
         var b = (x & 0x7F);
         if (b == 0) b++;

         // last 2^14 = 16384, for last 4 digits of SSN
         var c = (x >> 7) & 0x3FFF;
         if (c == 0) c++;

         var result = $"{a:000}-{b:00}-{c:0000}";
      }
   }
}
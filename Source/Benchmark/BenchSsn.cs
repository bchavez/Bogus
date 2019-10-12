using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Exporters;
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
         r = new Randomizer();
      }

      [Benchmark]
      public void SsnAlgo1()
      {
         var a = r.Int(1, 898);
         var b = r.Int(1, 99);
         var c = r.Int(1, 9999);

         var result = $"{a:000}-{b:00}-{c:0000}";
      }

      [Benchmark]
      public void SsnAlgo2()
      {
         var a = r.Int(1, 898);
         var b = r.Int(1, 99);
         var c = r.Int(1, 9999);

         var result = string.Format("{0:000}-{1:00}-{2:0000}", a, b, c);
      }

      [Benchmark]
      public void SsnAlgo3()
      {
         // 898 = 0b1110000010

         var x = r.Int();

         var a = x & 0xFFC00000 >> 22;

         var b = x & 0x003F ;

         var c = (x << 10 + 7 + 10);


      }
   }
}
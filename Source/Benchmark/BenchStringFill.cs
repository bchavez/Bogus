using System.Text;
using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmark
{
   [RPlotExporter]
   public class BenchStringFill
   {
      [Params(2, 10, 100, 500, 1000, 5000, 20000)]
      public int TargetLength { get; set; }

      //[Params("abcd","abcdefghijklmnopqrstuvwxyz",
      //   "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ")]
      public string Pool { get; set; } = "abcdefghijklmnopqrstuvwxyz";

      [GlobalSetup]
      public void Setup()
      {
         this.r = new Randomizer();
      }

      private Randomizer r;

      private string result;

      [Benchmark]
      public void FillWithStringBuilder()
      {
         var sb = new StringBuilder(this.TargetLength);

         for( int i = 0; i < this.TargetLength; i++ )
         {
            var idx = r.Number(0, this.Pool.Length - 1);
            sb.Append(this.Pool[idx]);
         }

         this.result = sb.ToString();
      }

      [Benchmark]
      public void FillWithFixedCharArray()
      {
         var target = new char[this.TargetLength];

         for (int i = 0; i < this.TargetLength; i++)
         {
            var idx = r.Number(0, this.Pool.Length - 1);
            target[i] = this.Pool[idx];
         }

         this.result = new string(target);
      }
   }
}
using Bogus.Distributions.Gaussian;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.DistributionTests
{
   public class GaussianTest : SeededTest
   {

      private readonly ITestOutputHelper console;
      private Randomizer r;

      public GaussianTest(ITestOutputHelper console)
      {
         this.console = console;
         r = new Randomizer();
      }


      /// <summary>
      /// Given a decent size population, the datapoints should evenutally start exhibiting a mean value that is close
      /// to the requested mean.
      /// </summary>
      [Fact]
      public void sample_should_match_specified_mean()
      {
         double desiredMean = 105.5d;
         double desiredStandardDeviation = 11.2d;
         int desiredSampleSize = 1000;
         double sum = 0d;

         for (int i = 0; i < desiredSampleSize; i++)
         {
            sum += r.DoubleGaussian(desiredMean, desiredStandardDeviation);
         }

         double mean = sum / desiredSampleSize;
         console.WriteLine($"Desired Mean: {desiredMean}  Actual Mean: {mean}");

         // Must be within tollerance of 10%
         mean.Should().BeInRange(desiredMean * 0.9, desiredMean * 1.1);

         
      }


   }
}

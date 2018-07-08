using System;
using Bogus.Distributions.Gaussian;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.DistributionTests
{
   public class GaussianTests : SeededTest
   {

      private readonly ITestOutputHelper console;
      private Randomizer r;

      public GaussianTests(ITestOutputHelper console)
      {
         this.console = console;
         r = new Randomizer();
      }

      /// <summary>
      /// Given a reasonable number of generated random numbers using the Gaussian methods, the mean and
      /// standard deviation of those generated numbers should be very close the mean and standard deviation
      /// that was specified when generating those numbers.
      /// </summary>
      /// <remarks>
      /// Because we can't expect the mean and standard deviation to match exactly, we allow a 10% tolerance.
      /// </remarks>
      [Fact]
      public void generated_doubles_should_match_specified_parameters()
      {
         double desiredMean = 105.5d;
         double desiredStandardDeviation = 11.2d;
         int desiredSampleSize = 1000;
         double sum = 0d;
         double[] dataPoints = new Double[desiredSampleSize];

         for (int i = 0; i < desiredSampleSize; i++)
         {
            double value = r.GaussianDouble(desiredMean, desiredStandardDeviation);
            sum += value;
            dataPoints[i] = value;
         }

         double mean = sum / desiredSampleSize;
         console.WriteLine($"Desired Mean: {desiredMean}; Actual Mean: {mean}");

         // Must be within tolerance of 10%
         mean.Should().BeInRange(desiredMean * 0.9, desiredMean * 1.1);

         // Calculate the Standard Deviation, now that we have the mean
         double sumSquares = 0;
         for (int i = 0; i < desiredSampleSize; i++)
         {
            sumSquares += (dataPoints[i] - mean) * (dataPoints[i] - mean);
         }

         double standardDeviation = Math.Sqrt(sumSquares / desiredSampleSize);
         console.WriteLine(
            $"Desired Standard Deviation: {desiredStandardDeviation}; Actual Standard Deviation {standardDeviation}");

         // Must be withing a tolerance of 10%
         standardDeviation.Should().BeInRange(desiredStandardDeviation * 0.9, desiredStandardDeviation * 1.1);
      }


      /// <summary>
      /// Given a reasonable number of generated random numbers using the Gaussian methods, the mean and
      /// standard deviation of those generated numbers should be very close the mean and standard deviation
      /// that was specified when generating those numbers.
      /// </summary>
      /// <remarks>
      /// Because we can't expect the mean and standard deviation to match exactly, we allow a 10% tolerance.
      /// </remarks>
      [Fact]
      public void generated_decimals_should_match_specified_parameters()
      {
         double desiredMean = 105.5d;
         double desiredStandardDeviation = 11.2d;
         int desiredSampleSize = 1000;
         decimal sum = 0M;
         decimal[] dataPoints = new decimal[desiredSampleSize];

         for (int i = 0; i < desiredSampleSize; i++)
         {
            decimal value = r.GaussianDecimal(desiredMean, desiredStandardDeviation);
            sum += value;
            dataPoints[i] = value;
         }

         double mean = (double) (sum / desiredSampleSize);
         console.WriteLine($"Desired Mean: {desiredMean}; Actual Mean: {mean}");

         // Must be within tolerance of 10%
         mean.Should().BeInRange(desiredMean * 0.9d, desiredMean * 1.1d);

         // Calculate the Standard Deviation, now that we have the mean
         double sumSquares = 0;
         for (int i = 0; i < desiredSampleSize; i++)
         {
            sumSquares += ((double) dataPoints[i] - mean) * ((double) dataPoints[i] - mean);
         }

         double standardDeviation = Math.Sqrt(sumSquares / desiredSampleSize);
         console.WriteLine(
            $"Desired Standard Deviation: {desiredStandardDeviation}; Actual Standard Deviation {standardDeviation}");

         // Must be withing a tolerance of 10%
         standardDeviation.Should().BeInRange(desiredStandardDeviation * 0.9, desiredStandardDeviation * 1.1);
      }


      /// <summary>
      /// Given a reasonable number of generated random numbers using the Gaussian methods, the mean and
      /// standard deviation of those generated numbers should be very close the mean and standard deviation
      /// that was specified when generating those numbers.
      /// </summary>
      /// <remarks>
      /// Because we can't expect the mean and standard deviation to match exactly, we allow a 10% tolerance.
      /// </remarks>
      [Fact]
      public void generated_ints_should_match_specified_parameters()
      {
         double desiredMean = 105.5d;
         double desiredStandardDeviation = 11.2d;
         int desiredSampleSize = 1000;
         int sum = 0;
         int[] dataPoints = new int[desiredSampleSize];

         for (int i = 0; i < desiredSampleSize; i++)
         {
            int value = r.GaussianInt(desiredMean, desiredStandardDeviation);
            sum += value;
            dataPoints[i] = value;
         }

         double mean = (double) sum / desiredSampleSize;
         console.WriteLine($"Desired Mean: {desiredMean}; Actual Mean: {mean}");

         // Must be within tolerance of 10%
         mean.Should().BeInRange(desiredMean * 0.9, desiredMean * 1.1);

         // Calculate the Standard Deviation, now that we have the mean
         double sumSquares = 0;
         for (int i = 0; i < desiredSampleSize; i++)
         {
            sumSquares += (dataPoints[i] - mean) * (dataPoints[i] - mean);
         }

         double standardDeviation = Math.Sqrt(sumSquares / desiredSampleSize);
         console.WriteLine(
            $"Desired Standard Deviation: {desiredStandardDeviation}; Actual Standard Deviation {standardDeviation}");

         // Must be withing a tolerance of 10%
         standardDeviation.Should().BeInRange(desiredStandardDeviation * 0.9, desiredStandardDeviation * 1.1);

      }


      /// <summary>
      /// Given a reasonable number of generated random numbers using the Gaussian methods, the mean and
      /// standard deviation of those generated numbers should be very close the mean and standard deviation
      /// that was specified when generating those numbers.
      /// </summary>
      /// <remarks>
      /// Because we can't expect the mean and standard deviation to match exactly, we allow a 10% tolerance.
      /// </remarks>
      [Fact]
      public void generated_floats_should_match_specified_parameters()
      {
         double desiredMean = 105.5d;
         double desiredStandardDeviation = 11.2d;
         int desiredSampleSize = 1000;
         float sum = 0;
         float[] dataPoints = new float[desiredSampleSize];

         for (int i = 0; i < desiredSampleSize; i++)
         {
            float value = r.GaussianFloat(desiredMean, desiredStandardDeviation);
            sum += value;
            dataPoints[i] = value;
         }

         double mean = sum / desiredSampleSize;
         console.WriteLine($"Desired Mean: {desiredMean}; Actual Mean: {mean}");

         // Must be within tolerance of 10%
         mean.Should().BeInRange(desiredMean * 0.9d, desiredMean * 1.1d);

         // Calculate the Standard Deviation, now that we have the mean
         double sumSquares = 0;
         for (int i = 0; i < desiredSampleSize; i++)
         {
            sumSquares += ((double)dataPoints[i] - mean) * ((double)dataPoints[i] - mean);
         }

         double standardDeviation = Math.Sqrt(sumSquares / desiredSampleSize);
         console.WriteLine(
            $"Desired Standard Deviation: {desiredStandardDeviation}; Actual Standard Deviation {standardDeviation}");

         // Must be withing a tolerance of 10%
         standardDeviation.Should().BeInRange(desiredStandardDeviation * 0.9, desiredStandardDeviation * 1.1);
      }
      
   }
}

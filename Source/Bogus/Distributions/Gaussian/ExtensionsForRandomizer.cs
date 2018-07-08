using System;

namespace Bogus.Distributions.Gaussian
{

   public static class ExtensionsForRandomizer
    {

       // Coefficients used in Acklam's Inverse Normal Cumulative Distribution function.
       private static readonly double[] AcklamsCoefficientA =
          {-39.696830d, 220.946098d, -275.928510d, 138.357751d, -30.664798d, 2.506628d};

       private static readonly double[] AcklamsCoefficientB =
          {-54.476098d, 161.585836d, -155.698979d, 66.801311d, -13.280681d};

       private static readonly double[] AcklamsCoefficientC =
          {-0.007784894002d, -0.32239645d, -2.400758d, -2.549732d, 4.374664d, 2.938163d};

       private static readonly double[] AcklamsCoefficientD = { 0.007784695709d, 0.32246712d, 2.445134d, 3.754408d };

       // Break-Points used in Acklam's Inverse Normal Cumulative Distribution function.
       private const double AcklamsLowBreakPoint = 0.02425d;
       private const double AcklamsHighBreakPoint = 1.0d - AcklamsLowBreakPoint;


      /// <summary>
      /// This algorithm follows Peter J Acklam's Inverse Normal Cumulative Distribution function.
      /// Reference: P.J. Acklam, "An algorithm for computing the inverse normal cumulative distribution function," 2010
      /// </summary>
      /// <returns>
      /// A double between 0.0 and 1.0
      /// </returns>
      private static double InverseNCD(double probability)
      {
         // Rational approximation for lower region of distribution
         if (probability < AcklamsLowBreakPoint)
         {
            double q = Math.Sqrt(-2 * Math.Log(probability));
            return (((((AcklamsCoefficientC[0] * q + AcklamsCoefficientC[1]) * q + AcklamsCoefficientC[2]) * q + AcklamsCoefficientC[3]) * q +
                     AcklamsCoefficientC[4]) * q + AcklamsCoefficientC[5]) /
                   ((((AcklamsCoefficientD[0] * q + AcklamsCoefficientD[1]) * q + AcklamsCoefficientD[2]) * q + AcklamsCoefficientD[3]) * q + 1);
         }

         // Rational approximation for upper region of distribution
         if (AcklamsHighBreakPoint < probability)
         {
            double q = Math.Sqrt(-2 * Math.Log(1 - probability));
            return -(((((AcklamsCoefficientC[0] * q + AcklamsCoefficientC[1]) * q + AcklamsCoefficientC[2]) * q + AcklamsCoefficientC[3]) * q +
                      AcklamsCoefficientC[4]) * q + AcklamsCoefficientC[5]) /
                   ((((AcklamsCoefficientD[0] * q + AcklamsCoefficientD[1]) * q + AcklamsCoefficientD[2]) * q + AcklamsCoefficientD[3]) * q + 1);
         }

         // Rational approximation for central region of distribution
         {
            double q = probability - 0.5d;
            double r = q * q;
            return (((((AcklamsCoefficientA[0] * r + AcklamsCoefficientA[1]) * r + AcklamsCoefficientA[2]) * r + AcklamsCoefficientA[3]) * r +
                     AcklamsCoefficientA[4]) * r + AcklamsCoefficientA[5]) * q /
                   (((((AcklamsCoefficientB[0] * r + AcklamsCoefficientB[1]) * r + AcklamsCoefficientB[2]) * r + AcklamsCoefficientB[3]) * r +
                     AcklamsCoefficientB[4]) * r + 1);
         }
      }


      /// <summary>
      /// Generate a random double, based on the specified normal distribution.
      /// <example>
      /// To create random values around an average height of 69.1
      /// inches with a standard deviation of 2.9 inches away from the mean
      /// <code>
      /// GaussianDouble(69.1, 2.9)
      /// </code>
      /// </example>
      /// </summary>
      /// <param name="mean">Mean value of the normal distribution</param>
      /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
      public static double GaussianDouble(this Randomizer rnd, double mean, double standardDeviation)
       {
             double p = InverseNCD(rnd.Double(0D, 1D));
             return (p * standardDeviation) + mean;
       }

      /// <summary>
      /// Generate a random int, based on the specified normal distribution.
      /// <example>
      /// To create random int values around an average age of 35 years, with
      /// a standard deviation of 4 years away from the mean.
      /// </example>
      /// <code>
      /// call GaussianInt(35, 4)
      /// </code>
      /// </summary>
      /// <param name="mean">Mean average of the normal distribution</param>
      /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
      public static int GaussianInt(this Randomizer rnd, double mean, double standardDeviation)
      {
         return Convert.ToInt32(GaussianDouble(rnd, mean, standardDeviation));
      }

      /// <summary>
      /// Generate a float decimal, based on the specified normal distribution.
      /// <example>
      /// To create random float values around an average height of 69.1
      /// inches with a standard deviation of 2.9 inches away from the mean
      /// <code>
      /// GaussianFloat(69.1, 2.9)
      /// </code>
      /// </example>
      /// </summary>
      /// <param name="mean">Mean average of the normal distribution</param>
      /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
      public static float GaussianFloat(this Randomizer rnd, double mean, double standardDeviation)
      {
          return Convert.ToSingle(GaussianDouble(rnd, mean, standardDeviation));
      }

      /// <summary>
      /// Generate a random decimal, based on the specified normal distribution.
      /// <example>
      /// To create random values around an average height of 69.1
      /// inches with a standard deviation of 2.9 inches away from the mean
      /// <code>
      /// GaussianDecimal(69.1, 2.9)
      /// </code>
      /// </example>
      /// </summary>
      /// <param name="mean">Mean average of the normal distribution</param>
      /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
      public static decimal GaussianDecimal(this Randomizer rnd, double mean, double standardDeviation)
      {
          return Convert.ToDecimal(GaussianDouble(rnd, mean, standardDeviation));
      }

   }
}

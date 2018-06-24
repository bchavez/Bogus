using System;

namespace Bogus.Distributions.Gussian
{
   /// <summary>
   /// 
   /// </summary>
   public static class ExtensionsForRandomizer
    {

       // Coefficients used in Acklam's Inverse Normal Cumulative Distribution function.
       private static readonly double[] AklmasCoefficientA =
          {-39.696830d, 220.946098d, -275.928510d, 138.357751d, -30.664798d, 2.506628d};

       private static readonly double[] AklamsCoefficientB =
          {-54.476098d, 161.585836d, -155.698979d, 66.801311d, -13.280681d};

       private static readonly double[] AklamsCoefficientC =
          {-0.007784894002d, -0.32239645d, -2.400758d, -2.549732d, 4.374664d, 2.938163d};

       private static readonly double[] AklamsCoefficientD = { 0.007784695709d, 0.32246712d, 2.445134d, 3.754408d };

       // Break-Points used in Aklam's Inverse Normal Cumulative Distribution function.
       private const double AklamsLowBreakPoint = 0.02425d;
       private const double AklamsHighBreakPoint = 1.0d - AklamsLowBreakPoint;


       /// <summary>
      /// This algorithm follows Peter J Acklam's Inverse Normal Cumulative Distribution function.
      /// Reference: P.J. Acklam, "An algorithm for computing the inverse normal cumulative distribution function," 2010
      /// </summary>
      /// <param name="probability"></param>
      /// <returns>
      /// A double between 0.0 and 1.0
      /// </returns>
      private static double InverseCumulativeStandardNormalDistribution(double probability)
      {

         // Rational approximation for lower region of distirbution
         if (probability < AklamsLowBreakPoint)
         {
            double q = Math.Sqrt(-2 * Math.Log(probability));
            return (((((AklamsCoefficientC[0] * q + AklamsCoefficientC[1]) * q + AklamsCoefficientC[2]) * q + AklamsCoefficientC[3]) * q +
                     AklamsCoefficientC[4]) * q + AklamsCoefficientC[5]) /
                   ((((AklamsCoefficientD[0] * q + AklamsCoefficientD[1]) * q + AklamsCoefficientD[2]) * q + AklamsCoefficientD[3]) * q + 1);
         }

         // Rational approximation for upper region of distribution
         if (AklamsHighBreakPoint < probability)
         {
            double q = Math.Sqrt(-2 * Math.Log(1 - probability));
            return -(((((AklamsCoefficientC[0] * q + AklamsCoefficientC[1]) * q + AklamsCoefficientC[2]) * q + AklamsCoefficientC[3]) * q +
                      AklamsCoefficientC[4]) * q + AklamsCoefficientC[5]) /
                   ((((AklamsCoefficientD[0] * q + AklamsCoefficientD[1]) * q + AklamsCoefficientD[2]) * q + AklamsCoefficientD[3]) * q + 1);
         }

         // Rational approximation for central region of disribution
         {
            double q = probability - 0.5d;
            double r = q * q;
            return (((((AklmasCoefficientA[0] * r + AklmasCoefficientA[1]) * r + AklmasCoefficientA[2]) * r + AklmasCoefficientA[3]) * r +
                     AklmasCoefficientA[4]) * r + AklmasCoefficientA[5]) * q /
                   (((((AklamsCoefficientB[0] * r + AklamsCoefficientB[1]) * r + AklamsCoefficientB[2]) * r + AklamsCoefficientB[3]) * r +
                     AklamsCoefficientB[4]) * r + 1);
         }
      }


       /// <summary>
       /// Generate a random double, based on the specified normal distribution.
       /// </summary>
       /// <param name="rnd"></param>
       /// <param name="mean">Mean value of the normal distribution</param>
       /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
       public static double Double(this Randomizer rnd, double mean, double standardDeviation)
       {
             double p = InverseCumulativeStandardNormalDistribution(rnd.Double(0D, 1D));
             return (p * standardDeviation) + mean;
       }

       /// <summary>
       /// Generate a random int, based on the specified normal distribution.
       /// </summary>
       /// <param name="rnd"></param>
       /// <param name="mean">Mean average of the normal distribution</param>
       /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
       public static int Int(this Randomizer rnd, double mean, double standardDeviation)
       {
          return Convert.ToInt32(Double(rnd, mean, standardDeviation));
       }

       /// <summary>
       /// Generate a random float, based on the specified normal distribution.
       /// </summary>
       /// <param name="rnd"></param>
       /// <param name="mean">Mean average of the normal distribution</param>
       /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
       public static float Float(this Randomizer rnd, double mean, double standardDeviation)
       {
          return Convert.ToSingle(Double(rnd, mean, standardDeviation));
       }

       /// <summary>
       /// Generate a random decimal, based on the specified normal distribution.
       /// </summary>
       /// <param name="rnd"></param>
       /// <param name="mean">Mean average of the normal distribution</param>
       /// <param name="standardDeviation">Standard deviation of the normal distribution</param>
       public static decimal Decimal(this Randomizer rnd, double mean, double standardDeviation)
       {
          return Convert.ToDecimal(Double(rnd, mean, standardDeviation));
       }


   }
}

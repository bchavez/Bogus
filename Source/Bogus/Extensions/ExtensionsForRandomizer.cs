using System;

namespace Bogus.Extensions
{
   public static class ExtensionsForRandomizer
   {
      /// <summary>
      /// Get a random decimal, between 0.0 and 1.0.
      /// </summary>
      /// <param name="min">Minimum, default 0.0</param>
      /// <param name="max">Maximum, default 1.0</param>
      public static decimal Decimal2(this Randomizer r, decimal min = 0.0m, decimal max = 1.0m)
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

         int lowBits = r.Number(int.MinValue, int.MaxValue);
         int middleBits = r.Number(int.MinValue, int.MaxValue);
         int highBits = r.Number(int.MinValue, int.MaxValue);

         const int Scale = 28;

         decimal result = new decimal(lowBits, middleBits, highBits, isNegative: false, Scale);

         // Step 2: Scale the value and adjust it to the desired range. This may decrease
         // the accuracy by adjusting the scale as necessary, but we get the best possible
         // outcome by starting with the most precise scale.
         return result * (max - min) / 7.9228162514264337593543950335m + min;
      }


      public static decimal Decimal3(this Randomizer r, decimal min = 0.0m, decimal max = 1.0m)
      {
         if (min > max)
         {
            decimal tmp = min;

            min = max;
            max = tmp;
         }

         var unscaledSample = GenerateRandomMaximumPrecisionDecimal(r);

         return ScaleMaximumPrecisionDecimalToRange(unscaledSample, min, max);
      }


      /// <summary>
      /// Generates a decimal with maximum precision with uniform distribution across the range of
      /// decimal values with maximum scaling: (decimal.MinValue .. decimal.MaxValue) divided by 10^28.
      /// </summary>
      /// <returns></returns>
      internal static decimal GenerateRandomMaximumPrecisionDecimal(Randomizer r)
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

         int lowBits = r.Number(int.MinValue, int.MaxValue);
         int middleBits = r.Number(int.MinValue, int.MaxValue);
         int highBits = r.Number(int.MinValue, int.MaxValue);

         bool isNegative = r.Bool();

         const int Scale = 28;

         return new decimal(lowBits, middleBits, highBits, isNegative, Scale);
      }

      /// <summary>
      /// Takes a decimal in the range in which decimal has maximum precision ((decimal.MinValue .. decimal.MaxValue) scaled by the maximum
      /// scaling factor of 10^28) and transforms it into the caller's desired range, reducing precision only as much as needed.
      /// </summary>
      /// <param name="input">A decimal in the range (decimal.MinValue / SCALE .. decimal.MaxValue / SCALE), where SCALE is the maximum scaling factor (10^28).</param>
      /// <param name="min">The lower bound of the target range.</param>
      /// <param name="max">The upper bound of the target range.</param>
      /// <returns></returns>
      internal static decimal ScaleMaximumPrecisionDecimalToRange(decimal input, decimal min, decimal max)
      {
         const decimal SmallestMaxPrecision = -7.9228162514264337593543950335m;
         const decimal LargestMaxPrecision = 7.9228162514264337593543950335m;

         if (input <= SmallestMaxPrecision)
            return min;
         if (input >= LargestMaxPrecision)
            return max;

         // Step 1: Figure out how much of the scale we can keep without causing an overflow.
         // Note that the range can actually exceed decimal.MaxValue, e.g. if max is itself
         // decimal.MaxValue and min is negative. So, we work with half the range, using this
         // scale factor that is as close to 0.5 as possible without causing the result of
         // decimal.MaxValue * ScaleFactor to round up. If it rounds up, then the result of
         // decimal.MaxValue * ScaleFactor - decimal.MinValue * ScaleFactor will still be
         // larger than decimal.MaxValue.
         const decimal OneHalfScaleFactor = 0.4999999999999999999999999999m;

         decimal halfRange = max * OneHalfScaleFactor - min * OneHalfScaleFactor;

         // Two reasons we're forced to use a scaled multiplier:
         //
         // 1. The range (max - min) is itself too large to store in decimal.MaxValue.
         // 2. The result of result * (max - min) is too large to store in decimal.MaxValue.
         //
         // Check condition 1:
         bool useScaledMultiplier = (halfRange > decimal.MaxValue * OneHalfScaleFactor);

         decimal multiplier = halfRange;
         decimal divisor = 7.922816251426433759354395032m; // This value is the maximum value at maximum precision times (OneHalfScaleFactor / 0.5).

         // Check condition 2:
         decimal inputMagnitude = Math.Abs(input);

         if (inputMagnitude >= 1.0m)
         {
            decimal maximumMultiplier = decimal.MaxValue / inputMagnitude;

            while (multiplier >= maximumMultiplier)
            {
               // Drop one digit of precision and try again.
               multiplier *= 0.1m;
               divisor *= 0.1m;

               useScaledMultiplier = true;
            }
         }

         // Step 2: Scale the value and adjust it to the desired range. This may decrease
         // the accuracy by adjusting the scale as necessary, but we get the best possible
         // outcome by starting with the most precise scale.
         if (useScaledMultiplier)
         {
            decimal rangeMiddle = max * OneHalfScaleFactor + min * OneHalfScaleFactor;

            return rangeMiddle + input * multiplier / divisor;
         }
         else
         {
            // If we're dealing with values at the upper extreme ends (decimal.MinValue, decimal.MaxValue),
            // then half their value can't be represented in a decimal and will be subject to rounding. The
            // result of that rounding is that adding the values together will produce an out-of-range value.
            // Therefore, we can't use a straightforward (min + max) * 0.5, or even (min * 0.5 + max * 0.5).
            decimal rangeSize = max - min;
            decimal rangeMiddle;

            if (min == max)
               rangeMiddle = min;
            else if (-min == max)
               rangeMiddle = 0m;
            else if (Math.Abs(min) < Math.Abs(max))
               rangeMiddle = max - rangeSize * 0.5m;
            else
               rangeMiddle = min + rangeSize * 0.5m;

            return rangeMiddle + input * rangeSize / 15.845632502852867518708790067m;
         }
      }

   }
}
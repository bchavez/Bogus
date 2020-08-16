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
   }
}
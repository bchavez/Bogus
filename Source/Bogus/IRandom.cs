
namespace Bogus
{
   /// <summary>
   /// An interface that abstracts the subset of System.Random that is used by Bogus.
   /// </summary>
   public interface IRandom
   {
      //
      // Summary:
      //     Returns a non-negative random integer.
      //
      // Returns:
      //     A 32-bit signed integer that is greater than or equal to 0 and less than System.Int32.MaxValue.
      int Next();

      //
      // Summary:
      //     Returns a random integer that is within a specified range.
      //
      // Parameters:
      //   minValue:
      //     The inclusive lower bound of the random number returned.
      //
      //   maxValue:
      //     The exclusive upper bound of the random number returned. maxValue must be greater
      //     than or equal to minValue.
      //
      // Returns:
      //     A 32-bit signed integer greater than or equal to minValue and less than maxValue;
      //     that is, the range of return values includes minValue but not maxValue. If minValue
      //     equals maxValue, minValue is returned.
      //
      // Exceptions:
      //   T:System.ArgumentOutOfRangeException:
      //     minValue is greater than maxValue.
      int Next(int minValue, int maxValue);

      //
      // Summary:
      //     Fills the elements of a specified array of bytes with random numbers.
      //
      // Parameters:
      //   buffer:
      //     An array of bytes to contain random numbers.
      //
      // Exceptions:
      //   T:System.ArgumentNullException:
      //     buffer is null.
      void NextBytes(byte[] buffer);

      //
      // Summary:
      //     Returns a random floating-point number that is greater than or equal to 0.0,
      //     and less than 1.0.
      //
      // Returns:
      //     A double-precision floating point number that is greater than or equal to 0.0,
      //     and less than 1.0.
      double NextDouble();
   }
}

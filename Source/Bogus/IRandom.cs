using System;

namespace Bogus
{
   /// <summary>
   /// An interface that abstracts the subset of System.Random that is used by Bogus.
   /// </summary>
   public interface IRandom
   {
      /// <summary>
      /// Returns a non-negative random integer.
      /// </summary>
      /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <seealso cref="System.Int32.MaxValue"/>.
      /// </returns>
      int Next();

      /// <summary>
      /// Returns a random integer that is within a specified range.
      /// </summary>
      /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
      /// <param name="maxValue">
      /// The exclusive upper bound of the random number returned. <paramref name="maxValue"/> 
      /// must be greater than or equal to <paramref name="minValue"/>.
      /// </param>
      /// <returns>
      /// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than 
      /// <paramref name="maxValue"/>; that is, the range of return values includes 
      /// <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/>
      /// equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
      /// </returns>
      /// <exception cref="ArgumentOutOfRangeException">
      /// When <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
      /// </exception>
      int Next(int minValue, int maxValue);

      /// <summary>
      /// Fills the elements of a specified array of bytes with random numbers.
      /// </summary>
      /// <param name="buffer">An array of bytes to contain random numbers.</param>
      /// <exception cref="ArgumentNullException">When buffer is null.</exception>
      void NextBytes(byte[] buffer);

      /// <summary>
      /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
      /// </summary>
      /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
      double NextDouble();
   }
}

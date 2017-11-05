using System.Collections.Generic;

namespace Bogus
{
   /// <summary>
   /// Describes a Hashids provider
   /// </summary>
   public interface IHashids
   {
      /// <summary>
      /// Decodes the provided hashed string.
      /// </summary>
      /// <param name="hash">the hashed string</param>
      /// <exception cref="T:System.OverflowException">if one or many of the numbers in the hash overflowing the integer storage</exception>
      /// <returns>the numbers</returns>
      int[] Decode(string hash);

      /// <summary>
      /// Decodes the provided hashed string into longs
      /// </summary>
      /// <param name="hash">the hashed string</param>
      /// <returns>the numbers</returns>
      long[] DecodeLong(string hash);

      /// <summary>
      /// Decodes the provided hashed string into a hex string
      /// </summary>
      /// <param name="hash">the hashed string</param>
      /// <returns>the hex string</returns>
      string DecodeHex(string hash);

      /// <summary>
      /// Encodes the provided numbers into a hashed string
      /// </summary>
      /// <param name="numbers">the numbers</param>
      /// <returns>the hashed string</returns>
      string Encode(params int[] numbers);

      /// <summary>
      /// Encodes the provided numbers into a hashed string
      /// </summary>
      /// <param name="numbers">the numbers</param>
      /// <returns>the hashed string</returns>
      string Encode(IEnumerable<int> numbers);

      /// <summary>
      /// Encodes the provided numbers into a hashed string
      /// </summary>
      /// <param name="numbers">the numbers</param>
      /// <returns>the hashed string</returns>
      string EncodeLong(params long[] numbers);

      /// <summary>
      /// Encodes the provided numbers into a hashed string
      /// </summary>
      /// <param name="numbers">the numbers</param>
      /// <returns>the hashed string</returns>
      string EncodeLong(IEnumerable<long> numbers);

      /// <summary>
      /// Encodes the provided hex string
      /// </summary>
      /// <param name="hex">the hex string</param>
      /// <returns>the hashed string</returns>
      string EncodeHex(string hex);
   }
}
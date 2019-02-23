namespace Bogus
{
   /// <summary>
   /// Static class for holding character string constants.
   /// </summary>
   public static class Chars
   {
      /// <summary>
      /// Lower case, a-z.
      /// </summary>
      public const string LowerCase = "abcdefghijklmnopqrstuvwxyz";

      /// <summary>
      /// Upper case, A-Z.
      /// </summary>
      public const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

      /// <summary>
      /// Numbers, 0-9.
      /// </summary>
      public const string Numbers = "0123456789";

      /// <summary>
      /// Hexadecimal, 0-9 and a-f.
      /// </summary>
      public const string HexLowerCase = Numbers + "abcdef";

      /// <summary>
      /// Hexadecimal, 0-9 and A-F.
      /// </summary>
      public const string HexUpperCase = Numbers + "ABCDEF";


      /// <summary>
      /// Alphanumeric upper case 0-9 and A-Z.
      /// </summary>
      public const string AlphaNumericUpperCase = Numbers + UpperCase;
      
      /// <summary>
      /// Alphanumeric lower case 0-9 and a-z.
      /// </summary>
      public const string AlphaNumericLowerCase = Numbers + LowerCase;
   }
}
namespace Bogus.DataSets
{
   /// <summary>
   /// Type of color format
   /// </summary>
   public enum ColorFormat
   {
      /// <summary>
      /// Hexadecimal format: #4d0e68
      /// </summary>
      Hex = 0x1,
      /// <summary>
      /// CSS format: rgb(77,14,104)
      /// </summary>
      Rgb,
      /// <summary>
      /// Delimited R,G,B: 77,14,104
      /// </summary>
      Delimited
   }
}
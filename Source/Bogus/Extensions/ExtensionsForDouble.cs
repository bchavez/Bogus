using static System.Math;

namespace Bogus.Extensions
{
   /// <summary>
   /// Extensions for <seealso cref="double"/>.
   /// </summary>
   public static class ExtensionsForDouble
   {
      /// <summary>
      /// Convert radians to numeric (signed) degrees.
      /// </summary>
      public static double ToDegrees(this double radians)
      {
         return radians * 180 / PI;
      }

      /// <summary>
      /// Convert numeric degrees to radians.
      /// </summary>
      public static double ToRadians(this double degrees)
      {
         return degrees * PI / 180;
      }

      /// <summary>
      /// Normalize longitude to −180°...+180°
      /// </summary>
      public static double NomralizeLongitude(this double degrees)
      {
         return ((degrees + 540) % 360) - 180;
      }
   }
}
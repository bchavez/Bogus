namespace Bogus.Models
{
   /// <summary>
   /// Creates a LatLon point on the earth's surface at the specified latitude / longitude.
   /// </summary>
   public struct LatLon
   {
      /// <summary>
      /// Latitude in degrees. The geographic coordinate that specifies the north–south position of a point on the Earth's surface.
      /// </summary>
      public double Latitude { get; set; }
      /// <summary>
      /// Longitude in degrees. The geographic coordinate that specifies the east-west position of a point on the Earth's surface.
      /// </summary>
      public double Longitude { get; set; }
   }
}
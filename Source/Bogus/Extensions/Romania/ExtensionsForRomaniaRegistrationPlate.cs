using Bogus.DataSets;

namespace Bogus.Extensions.Romania;

/// <summary>
/// https://en.wikipedia.org/wiki/Vehicle_registration_plates_of_Romania
/// </summary>
public static class ExtensionsForRomaniaRegistrationPlate
{
   private static readonly string[] LicensePlateSuffix = ["###???", "##???"];

   /// <summary>
   ///  Generate a Romania Vehicle Registration Plate.
   /// </summary>
   /// <param name="vehicle">Object to extend.</param>
   /// <param name="countyCode">Romanian County Code. Default is <see cref="RomanianCountyCode.X"/> thats means random from all codes.</param>
   /// <returns>A string containing a Romania registration plate.</returns>
   /// <remarks>
   /// This is based on the information in the Wikipedia article on
   /// Vehicle registration plates of the Romania.<br />
   /// https://en.wikipedia.org/wiki/Vehicle_registration_plates_of_Romania
   /// </remarks>
   public static string RoRegistrationPlate(this Vehicle vehicle, RomanianCountyCode countyCode = RomanianCountyCode.X)
   {
      var sufix = vehicle.Random.Replace(vehicle.Random.ArrayElement(LicensePlateSuffix));
      if (countyCode == RomanianCountyCode.X)
      {
         countyCode = vehicle.Random.Enum(RomanianCountyCode.X);
      }
      return $"{countyCode}{sufix}";
   }
}
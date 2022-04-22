using System.Text;

namespace Bogus.DataSets;

/// <summary>
/// Methods for generating vehicle information
/// </summary>
public class Vehicle : DataSet
{
   private const string StrictUpperCase = "ABCDEFGHJKLMNPRSTUVWXYZ";
   private const string StrictAlphaNumericUpperCase = Chars.Numbers + StrictUpperCase;

   /// <summary>
   /// Generate a vehicle identification number (VIN).
   /// </summary>
   /// <param name="strict">Limits the acceptable characters to alpha numeric uppercase except I, O and Q.</param>
   public string Vin(bool strict = false)
   {
      var sb = new StringBuilder();

      var allowedUpperCase = Chars.UpperCase;
      var allowedAlphaNumericChars = Chars.AlphaNumericUpperCase;
      if (strict)
      {
         allowedUpperCase = StrictUpperCase;
         allowedAlphaNumericChars = StrictAlphaNumericUpperCase;
      }

      sb.Append(this.Random.String2(10, allowedAlphaNumericChars));
      sb.Append(this.Random.String2(1, allowedUpperCase));
      sb.Append(this.Random.String2(1, allowedAlphaNumericChars));
      sb.Append(this.Random.Number(min: 10000, max: 99999));

      return sb.ToString();
   }

   /// <summary>
   /// Get a vehicle manufacture name. IE: Toyota, Ford, Porsche.
   /// </summary>
   public string Manufacturer()
   {
      return GetRandomArrayItem("manufacturer");
   }

   /// <summary>
   /// Get a vehicle model. IE: Camry, Civic, Accord.
   /// </summary>
   public string Model()
   {
      return GetRandomArrayItem("model");
   }

   /// <summary>
   /// Get a vehicle type. IE: Minivan, SUV, Sedan.
   /// </summary>
   public string Type()
   {
      return GetRandomArrayItem("type");
   }

   /// <summary>
   /// Get a vehicle fuel type. IE: Electric, Gasoline, Diesel.
   /// </summary>
   public string Fuel()
   {
      return GetRandomArrayItem("fuel");
   }
}
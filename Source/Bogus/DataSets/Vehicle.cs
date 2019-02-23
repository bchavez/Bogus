using System.Text;

namespace Bogus.DataSets
{
   /// <summary>
   /// Methods for generating vehicle information
   /// </summary>
   public class Vehicle : DataSet
   {
      /// <summary>
      /// Generate a vehicle identification number (VIN).
      /// </summary>
      public string Vin()
      {
         var sb = new StringBuilder();

         sb.Append(this.Random.String2(10, Chars.AlphaNumericUpperCase));
         sb.Append(this.Random.String2(1, Chars.UpperCase));
         sb.Append(this.Random.String2(1, Chars.AlphaNumericUpperCase));
         sb.Append(this.Random.Number(min: 10000, max: 100000));

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
}
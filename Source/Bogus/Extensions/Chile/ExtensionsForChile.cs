using Bogus.DataSets;

namespace Bogus.Extensions.Chile
{
    /// <summary>
    /// API extensions specific for a geographical location.
    /// </summary>
    public static class ExtensionsForChile
    {
      /// <summary>
      /// Generates a valid Chilean RUT for a Person (Rol Unico Tributario)
      /// </summary>
      /// <param name="dotFormat">Use the thousands separator for the Chilean RUT (11.111.111-1)</param>
      /// <returns>A string representation for a valid Chilean RUT (Rol Unico Tributario)</returns>
      public static string Rut(this Person person, bool dotFormat = true)
      {
         GenerateChileanId(dotFormat);
      }

      /// <summary>
      /// Generates a valid Chilean RUT for a Company (Rol Unico Tributario)
      /// </summary>
      /// <param name="dotFormat">Use the thousands separator for the Chilean RUT (11.111.111-1)</param>
      /// <returns>A string representation for a valid Chilean RUT (Rol Unico Tributario)</returns>

      public static string Rut(this Company company, bool dotFormat = true)
      {
         GenerateChileanId(dotFormat);
      }

      /// <summary>
      /// A general Chilean ID generator
      /// </summary>
      /// <param name="dotFormat">Use the thousands separator for the Chilean RUT (11.111.111-1)</param>
      /// <returns></returns>
      private static string GenerateChileanId(bool dotFormat = true)
      {
         Random rnd = new();
         int num = rnd.Next(1000000, 99999999);

         string dig = Digito(num);

         if (dotFormat)
         {
               var rut = string.Format("{0:0,0}", num);
               return $"{rut}-{dig}";
         }

         return $"{num}-{dig}";
      }

      /// <summary>
      /// Algorithm to generate a verification digit based on an integer between 1000000 and 99999999
      /// </summary>
      /// <param name="rut">Represents a full valid RUT number</param>
      /// <returns>A string representing a number that validates the provided number</returns>
      private static string Digito(int rut)
      {
         if (rut < 1000000 || rut > 99999999)
               throw new ArgumentOutOfRangeException("The provided integer is outside of the range between 1000000 and 99999999");

         int suma = 0;
         int multiplicador = 1;

         while (rut != 0)
         {
               multiplicador++;
               if (multiplicador == 8)
                  multiplicador = 2;
               suma += (rut % 10) * multiplicador;
               rut /= 10;
         }

         suma = 11 - (suma % 11);

         if (suma == 11)
         {
               return "0";
         }
         else if (suma == 10)
         {
               return "K";
         }
         else
         {
               return suma.ToString();
         }
      }
    }
}


namespace ExtendingBogus
{
   /// <summary>
   /// Augment the existing <seealso cref="Bogus.DataSets.Address"/> DataSet via C# extension method.
   /// </summary>
   public static class ExtensionsForAddress
   {
      private static readonly string[] CanadaDowntownTorontoPostalCodes =
         {
            "M5S", "M5B", "M5X", "M5V", "M4W", "M4X", "M4Y",
            "M5A", "M5C", "M5T", "M5E", "M5G", "M5H", "M5J",
            "M5K", "M5L", "M6G"
         };

      public static string DowntownTorontoPostalCode(this Bogus.DataSets.Address address)
      {
         return address.Random.ArrayElement(CanadaDowntownTorontoPostalCodes);
      }
   }
}

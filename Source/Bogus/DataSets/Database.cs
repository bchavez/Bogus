namespace Bogus.DataSets
{
   /// <summary>
   /// Generates some random database stuff.
   /// </summary>
   public class Database : DataSet
   {
      /// <summary>
      /// Generates a column name.
      /// </summary>
      /// <returns>A random column name.</returns>
      public string Column()
      {
         return this.GetRandomArrayItem("column");
      }

      /// <summary>
      /// Generates a column type.
      /// </summary>
      /// <returns>A random column type.</returns>
      public string Type()
      {
         return this.GetRandomArrayItem("type");
      }

      /// <summary>
      /// Generates a collation.
      /// </summary>
      /// <returns>A random collation.</returns>
      public string Collation()
      {
         return this.GetRandomArrayItem("collation");
      }

      /// <summary>
      /// Generates a storage engine.
      /// </summary>
      /// <returns>A random storage engine.</returns>
      public string Engine()
      {
         return this.GetRandomArrayItem("engine");
      }
   }
}
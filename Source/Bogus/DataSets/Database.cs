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
      public string Column()
      {
         return this.GetRandomArrayItem("column");
      }

      /// <summary>
      /// Generates a column type.
      /// </summary>
      public string Type()
      {
         return this.GetRandomArrayItem("type");
      }

      /// <summary>
      /// Generates a collation.
      /// </summary>
      public string Collation()
      {
         return this.GetRandomArrayItem("collation");
      }

      /// <summary>
      /// Generates a storage engine.
      /// </summary>
      public string Engine()
      {
         return this.GetRandomArrayItem("engine");
      }
   }
}
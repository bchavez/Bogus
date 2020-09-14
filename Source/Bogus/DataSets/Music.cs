namespace Bogus.DataSets
{
   public class Music : DataSet
   {
      /// <summary>
      /// Get a music genre
      /// </summary>
      public string Genre()
      {
         return GetRandomArrayItem("genre");
      }
   }
}
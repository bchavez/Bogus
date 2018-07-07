namespace Bogus.DataSets
{
   /// <summary>
   /// Hackerish words
   /// </summary>
   public class Hacker : DataSet
   {
      /// <summary>
      /// Create a hacker lingo dataset.
      /// </summary>
      public Hacker(string locale = "en") : base(locale)
      {
      }

      /// <summary>
      /// Returns an abbreviation.
      /// </summary>
      public string Abbreviation()
      {
         return GetRandomArrayItem("abbreviation");
      }

      /// <summary>
      /// Returns a adjective.
      /// </summary>
      public string Adjective()
      {
         return GetRandomArrayItem("adjective");
      }

      /// <summary>
      /// Returns a noun.
      /// </summary>
      public string Noun()
      {
         return GetRandomArrayItem("noun");
      }

      /// <summary>
      /// Returns a verb.
      /// </summary>
      public string Verb()
      {
         return GetRandomArrayItem("verb");
      }

      /// <summary>
      /// Returns an -ing verb.
      /// </summary>
      public string IngVerb()
      {
         return GetRandomArrayItem("ingverb");
      }

      /// <summary>
      /// Returns a phrase.
      /// </summary>
      public string Phrase()
      {
         var phrase = GetRandomArrayItem("phrase");

         return phrase.Replace("{{abbreviation}}", Abbreviation())
            .Replace("{{adjective}}", Adjective())
            .Replace("{{ingverb}}", IngVerb())
            .Replace("{{noun}}", Noun())
            .Replace("{{verb}}", Verb());
      }
   }
}
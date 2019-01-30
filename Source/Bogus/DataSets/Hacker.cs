namespace Bogus.DataSets
{
   /// <summary>
   /// Hackerish words
   /// </summary>
   public class Hacker : DataSet
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Hacker"/> class.
      /// </summary>
      /// <param name="locale">The locale that will be used to generate values.</param>
      public Hacker(string locale = "en") : base(locale)
      {
      }

      /// <summary>
      /// Returns an abbreviation.
      /// </summary>
      /// <returns>A random abbreviation.</returns>
      public string Abbreviation()
      {
         return GetRandomArrayItem("abbreviation");
      }

      /// <summary>
      /// Returns a adjective.
      /// </summary>
      /// <returns>A random adjective.</returns>
      public string Adjective()
      {
         return GetRandomArrayItem("adjective");
      }

      /// <summary>
      /// Returns a noun.
      /// </summary>
      /// <returns>A random noun.</returns>
      public string Noun()
      {
         return GetRandomArrayItem("noun");
      }

      /// <summary>
      /// Returns a verb.
      /// </summary>
      /// <returns>A random verb.</returns>
      public string Verb()
      {
         return GetRandomArrayItem("verb");
      }

      /// <summary>
      /// Returns a verb ending with -ing.
      /// </summary>
      /// <returns>A random -ing verb.</returns>
      public string IngVerb()
      {
         return GetRandomArrayItem("ingverb");
      }

      /// <summary>
      /// Returns a phrase.
      /// </summary>
      /// <returns>A random phrase.</returns>
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
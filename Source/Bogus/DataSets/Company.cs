using System.Linq;
using Bogus.Bson;

namespace Bogus.DataSets
{
   /// <summary>
   /// Generates a random company name and phrases
   /// </summary>
   public class Company : DataSet
   {
      /// <summary>
      /// The source to pull names from.
      /// </summary>
      protected Name Name = null;

      /// <summary>
      /// Initializes a new instance of the <see cref="Company"/> class.
      /// </summary>
      /// <param name="locale">The locale used to generate the values.</param>
      public Company(string locale = "en") : base(locale)
      {
         this.Name = this.Notifier.Flow(new Name(locale));
      }

      /// <summary>
      /// Get a company suffix. "Inc" and "LLC" etc.
      /// </summary>
      /// <returns>A random company suffix.</returns>
      public string CompanySuffix()
      {
         return Random.ArrayElement(Suffixes());
      }

      /// <summary>
      /// Get a company name.
      /// </summary>
      /// <param name="formatIndex">0: name + suffix, 1: name-name, 2: name, name and name."</param>
      /// <returns>A random company name.</returns>
      public string CompanyName(int? formatIndex = null)
      {
         var formats = new[]
            {
               "{{name.lastName}} {{company.companySuffix}}",
               "{{name.lastName}} - {{name.lastName}}",
               "{{name.lastName}}, {{name.lastName}} and {{name.lastName}}"
            };

         var index = formatIndex ?? Random.Number(formats.Length - 1);
         return CompanyName(formats[index]);
      }

      /// <summary>
      /// Get a company name. The format can use any name.* and company.* methods.
      /// </summary>
      /// <param name="format">Example: "{{name.lastName}} {{company.companySuffix}}"</param>
      /// <returns>A random company name in the given format.</returns>
      public string CompanyName(string format)
      {
         return Tokenizer.Parse(format, this, this.Name);
      }


      /// <summary>
      /// Get a company catch phrase.
      /// </summary>
      /// <returns>A random company catch phrase.</returns>
      public string CatchPhrase()
      {
         return $"{CatchPhraseAdjective()} {CatchPhraseDescriptor()} {CatchPhraseNoun()}";
      }

      /// <summary>
      /// Get a company BS phrase.
      /// </summary>
      /// <returns>A random company BS phrase.</returns>
      public string Bs()
      {
         return $"{BsBuzz()} {BsAdjective()} {BsNoun()}";
      }

#pragma warning disable 1591
      internal protected virtual string[] Suffixes()
      {
         return GetArray("suffix").OfType<BValue>().Select(s => s.StringValue).ToArray();
      }

      internal protected virtual string CatchPhraseAdjective()
      {
         return GetRandomArrayItem("adjective");
      }


      internal protected virtual string CatchPhraseDescriptor()
      {
         return GetRandomArrayItem("descriptor");
      }

      internal protected virtual string CatchPhraseNoun()
      {
         return GetRandomArrayItem("noun");
      }

      internal protected virtual string BsAdjective()
      {
         return GetRandomArrayItem("bs_adjective");
      }

      internal protected virtual string BsBuzz()
      {
         return GetRandomArrayItem("bs_verb");
      }

      internal protected virtual string BsNoun()
      {
         return GetRandomArrayItem("bs_noun");
      }
#pragma warning restore 1591
   }
}
using System;
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
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
        public Company(string locale = "en") : base(locale)
        {
           this.Name = this.Notifier.Flow(new Name(locale));
        }
        
        /// <summary>
        /// Get a company suffix. "Inc" and "LLC" etc.
        /// </summary>
        /// <returns></returns>
        public string CompanySuffix()
        {
            return Random.ArrayElement(Suffexes());
        }

        /// <summary>
        /// Get a company name.
        /// </summary>
        /// <param name="formatIndex">0: name + suffix, 1: name-name, 2: name, name and name."</param>
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
        public string CompanyName(string format)
        {
            return Tokenizer.Parse(format, this, this.Name);
        }


        /// <summary>
        /// Get a company catch phrase.
        /// </summary>
        /// <returns></returns>
        public string CatchPhrase()
        {
            return $"{CatchPhraseAdjective()} {CatchPhraseDescriptor()} {CatchPhraseNoun()}";
        }

        /// <summary>
        /// Get a company BS phrase.
        /// </summary>
        /// <returns></returns>
        public string Bs()
        {
            return $"{BsBuzz()} {BsAdjective()} {BsNoun()}";
        }

#pragma warning disable 1591
        internal protected virtual string[] Suffexes()
        {
            return GetArray("suffix").OfType<BValue>().Select( s => s.StringValue).ToArray();
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

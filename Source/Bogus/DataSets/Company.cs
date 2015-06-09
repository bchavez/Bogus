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
            this.Name = new Name(locale);
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
        /// Get a company name
        /// </summary>
        /// <param name="formatIndex">0: name + suffix, 1: name-name, 2: name, name and name."</param>
        /// <returns></returns>
        public string CompanyName(int? formatIndex = null)
        {
            formatIndex = formatIndex ?? Random.Number(2);

            if( formatIndex == 0 )
                return string.Format("{0} {1}", Name.LastName(), CompanySuffix());
            if( formatIndex == 1 )
                return string.Format("{0}-{1}", Name.LastName(), Name.LastName());

            return string.Format("{0}, {1} and {2}", Name.LastName(), Name.LastName(), Name.LastName());
        }


        /// <summary>
        /// Get a company catch phrase.
        /// </summary>
        /// <returns></returns>
        public string CatchPhrase()
        {
            return string.Format("{0} {1} {2}",
                CatchPhraseAdjective(),
                CatchPhraseDescriptor(),
                CatchPhraseNoun());
        }

        /// <summary>
        /// Get a company BS phrase.
        /// </summary>
        /// <returns></returns>
        public string Bs()
        {
            return string.Format("{0} {1} {2}",
                BsAdjective(),
                BsBuzz(),
                BsNoun());
        }

#pragma warning disable 1591
        protected virtual string[] Suffexes()
        {
            return new[] { "Inc", "and Sons", "LLC", "Group", "and Daughters" };
        }

        protected virtual string CatchPhraseAdjective()
        {
            return GetRandomArrayItem("adjective");
        }


        protected virtual string CatchPhraseDescriptor()
        {
            return GetRandomArrayItem("descriptor");
        }

        protected virtual string CatchPhraseNoun()
        {
            return GetRandomArrayItem("noun");
        }

        protected virtual string BsAdjective()
        {
            return GetRandomArrayItem("bs_adjective");
        }

        protected virtual string BsBuzz()
        {
            return GetRandomArrayItem("bs_verb");
        }

        protected virtual string BsNoun()
        {
            return GetRandomArrayItem("bs_noun");
        }
#pragma warning restore 1591

    }
}

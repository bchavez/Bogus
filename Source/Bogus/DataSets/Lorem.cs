using System.Linq;

namespace Bogus.DataSets
{
    /// <summary>
    /// Generates plain old boring text.
    /// </summary>
    public class Lorem : DataSet
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
        public Lorem(string locale = "en") : base(locale)
        {
        }

        /// <summary>
        /// Get some lorem words
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string[] Words(int num = 3)
        {
            return Random.Shuffle(GetArray("words")).Take(num)
                .Select(s => (string)s)
                .ToArray();
        }

        /// <summary>
        /// Get a random sentence. Default minimum of 3 words but at most 10 words (range = 7).
        /// If you want a sustenance with 5 words always call Sentence(5, range: 0);
        /// </summary>
        /// <param name="minWordCount">Minimum word count</param>
        /// <param name="range">Plus, add extra number of words ranging from 0 to range</param>
        /// <returns></returns>
        public string Sentance(int minWordCount = 3, int range = 7)
        {
            return string.Join(" ", Words(minWordCount + Random.Number(range)));
        }

        /// <summary>
        /// Get some sentences.
        /// </summary>
        /// <param name="count">The number of sentences</param>
        /// <returns></returns>
        public string Sentances(int count = 3)
        {
            var sentances = Enumerable.Range(1, count)
                .Select(s => Sentance());

            return string.Join("\n", sentances);
        }

        /// <summary>
        /// Get a paragraph.
        /// </summary>
        /// <param name="count">The number of paragraphs</param>
        /// <returns></returns>
        public string Paragraph(int count = 3)
        {
            return Sentances(count + Random.Number(3));
        }

        /// <summary>
        /// Get some paragraphs with tabs n all.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string Paragraphs(int count = 3, string separator = "\r\n")
        {
            var paras = Enumerable.Range(1, count)
                .Select(i => Paragraph());

            return string.Join(separator, paras);
        }
    }
}

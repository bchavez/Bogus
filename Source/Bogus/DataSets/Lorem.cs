using System;
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
        /// Get a random lorem word.
        /// </summary>
        public string Word()
        {
            return this.GetRandomArrayItem("words");
        }

        /// <summary>
        /// Get some lorem words
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string[] Words(int num = 3)
        {
            return Enumerable.Range(1, num).Select(f => Word()).ToArray(); // lol
        }

        /// <summary>
        /// Get a character letter.
        /// </summary>
        /// <param name="num">Number of characters to return.</param>
        /// <returns></returns>
        public string Letter(int num = 1)
        {
            if( num <= 0 )
                return string.Empty;

            var w = Words(1)[0];
            var c = Random.ArrayElement(w.ToArray());
            return c + Letter(num - 1);
        }

        /// <summary>
        /// Get a random sentence. Default minimum of 3 words but at most 10 words (range = 7).
        /// If you want a sustenance with 5 words always call Sentence(5, range: 0);
        /// </summary>
        /// <param name="wordCountunt">Minimum word count</param>
        /// <param name="range">Plus, add extra number of words ranging from 0 to range</param>
        /// <returns></returns>
        public string Sentence(int? wordCount = null)
        {
            var wc = wordCount ?? this.Random.Number(3, 10);

            var sentence = string.Join(" ", Words(wc));
            return sentence.Substring(0, 1).ToUpper() + sentence.Substring(1) + ".";
        }

        /// <summary>
        /// Slugify lorem words.
        /// </summary>
        /// <param name="wordcount"></param>
        public string Slug(int wordcount = 3)
        {
            var words = Words(wordcount);
            return Utils.Slugify(string.Join(" ", words));
        }

        /// <summary>
        /// Get some sentences.
        /// </summary>
        /// <param name="sentanceCount">The number of sentences</param>
        /// <returns></returns>
        public string Sentences(int? sentanceCount = null, string separator = "\n")
        {
            var sc = sentanceCount ?? this.Random.Number(2, 6);
            var sentences = Enumerable.Range(1, sc)
                .Select(s => Sentence());

            return string.Join(separator, sentences);
        }

        /// <summary>
        /// Get a paragraph.
        /// </summary>
        /// <param name="count">The number of paragraphs</param>
        /// <returns></returns>
        public string Paragraph(int count = 3)
        {
            return Sentences(count + Random.Number(3), " ");
        }

        /// <summary>
        /// Get some paragraphs with tabs n all.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string Paragraphs(int count = 3, string separator = "\n\n")
        {
            var paras = Enumerable.Range(1, count)
                .Select(i => Paragraph());

            return string.Join(separator, paras);
        }

        /// <summary>
        /// Get random text on a random lorem methods.
        /// </summary>
        public string Text()
        {
            var methods = new Func<string>[] {() => Word(), () => Sentence(), () => Sentences(), () => Paragraph()};

            var randomLoremMethod = this.Random.ArrayElement(methods);
            return randomLoremMethod();
        }

        /// <summary>
        /// Get lines of lorem
        /// </summary>
        /// <returns></returns>
        public string Lines(int? lineCount = null, string seperator = "\n")
        {
            var lc = lineCount ?? this.Random.Number(1, 5);

            return Sentences(lc, seperator);
        }

    }
}

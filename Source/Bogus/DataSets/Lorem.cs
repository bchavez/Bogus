﻿using System;
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
      public string[] Words(int num = 3)
      {
         return Enumerable.Range(1, num).Select(f => Word()).ToArray(); // lol
      }

      /// <summary>
      /// Get a character letter.
      /// </summary>
      /// <param name="num">Number of characters to return.</param>
      public string Letter(int num = 1)
      {
         if( num <= 0 )
            return string.Empty;

         var w = Words(1)[0];
         var c = Random.ArrayElement(w.ToArray());
         return c + Letter(num - 1);
      }

      /// <summary>
      /// Get a random sentence of specific number of words. 
      /// </summary>
      /// <param name="wordCount">Get a sentence with wordCount words. Defaults between 3 and 10</param>
      /// <param name="range">Add anywhere between 0 to 'range' additional words to wordCount. Default is 0.</param>
      public string Sentence(int? wordCount = null, int? range = 0)
      {
         var wc = wordCount ?? this.Random.Number(3, 10);
         if( range > 0 )
         {
            wc += this.Random.Number(range.Value);
         }

         var sentence = string.Join(" ", Words(wc));
         return sentence.Substring(0, 1).ToUpper() + sentence.Substring(1) + ".";
      }

      /// <summary>
      /// Get some sentences.
      /// </summary>
      /// <param name="sentenceCount">The number of sentences</param>
      public string Sentences(int? sentenceCount = null, string separator = "\n")
      {
         var sc = sentenceCount ?? this.Random.Number(2, 6);
         var sentences = Enumerable.Range(1, sc)
            .Select(s => Sentence());

         return string.Join(separator, sentences);
      }

      /// <summary>
      /// Get a paragraph.
      /// </summary>
      /// <param name="min">The minimum number of sentences in the paragraph.
      /// The final number of sentences returned in the paragraph is bound between [min, min + 3], inclusive.
      /// If you want an exact number of sentences, use the <seealso cref="Sentences"/> method.</param>
      public string Paragraph(int min = 3)
      {
         return Sentences(min + Random.Number(3), " ");
      }

      /// <summary>
      /// Get a specified number of paragraphs.
      /// </summary>
      /// <param name="count">Number of paragraphs</param>
      public string Paragraphs(int count = 3, string separator = "\n\n")
      {
         var paras = Enumerable.Range(1, count)
            .Select(i => Paragraph());

         return string.Join(separator, paras);
      }

      /// <summary>
      /// Get a random number of paragraphs between <paramref name="min"/> and <paramref name="max"/>.
      /// </summary>
      /// <param name="min">Minimum number of paragraphs</param>
      /// <param name="max">Maximum number of paragraphs</param>
      public string Paragraphs(int min, int max, string separator = "\n\n")
      {
         var n = this.Random.Number(min, max);
         return Paragraphs(n, separator);
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
      /// Get lines of lorem.
      /// </summary>
      /// <returns></returns>
      public string Lines(int? lineCount = null, string separator = "\n")
      {
         var lc = lineCount ?? this.Random.Number(1, 5);

         return Sentences(lc, separator);
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
   }
}
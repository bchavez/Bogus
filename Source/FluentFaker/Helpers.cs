using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentFaker
{
    public static class Helpers
    {
        public static string Slugify(string txt)
        {
            var str = txt.Replace(" ", "");
            return Regex.Replace(str, @"[^\w\.\-]+", "");
        }
        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
        {
            return Shuffle(source, Random.Generator);
        }

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> source, System.Random rng)
        {
            if( source == null ) throw new ArgumentNullException("source");
            if( rng == null ) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, System.Random rng)
        {
            List<T> buffer = source.ToList();
            for( int i = 0; i < buffer.Count; i++ )
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

        public static string ReplaceSymbolsWithNumbers(string format, char symbol = '#')
        {
            var chars = format.Select(c => c == symbol ? Convert.ToChar('0' + Random.Number(9)) : c)
                .ToArray();

            return new string(chars);
        }
    }
    
}
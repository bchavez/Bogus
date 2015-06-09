using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Bogus
{
    /// <summary>
    /// The randomizer. It randoms things.
    /// </summary>
    public class Randomizer
    {
        /// <summary>
        /// Set the random number generator manually with a seed to get reproducible results.
        /// </summary>
        public static Random Seed = new Random();

        /// <summary>
        /// Get an int from 0 to max.
        /// </summary>
        /// <param name="max">Upper bound, inclusive</param>
        /// <returns></returns>
        public int Number(int max)
        {
            return Number(0, max);
        }

        /// <summary>
        /// Get an int from min to max.
        /// </summary>
        /// <param name="min">Lower bound, inclusive</param>
        /// <param name="max">Upper bound, inclusive</param>
        /// <returns></returns>
        public int Number(int min = 0, int max = 1)
        {
            return Seed.Next(min, max + 1);
        }

        /// <summary>
        /// Get a random double.
        /// </summary>
        /// <returns></returns>
        public double Double()
        {
            return Seed.NextDouble();
        }

        /// <summary>
        /// Get a random boolean
        /// </summary>
        /// <returns></returns>
        public  bool Bool()
        {
            return Number() == 0;
        }

        /// <summary>
        /// Get a random array element.
        /// </summary>
        public T ArrayElement<T>(T[] array)
        {
            var r = Number(max: array.Length - 1);
            return array[r];
        }

        /// <summary>
        /// Helper method to get a random JProperty.
        /// </summary>
        public JToken ArrayElement(JProperty[] props)
        {
            var r = Number(max: props.Length - 1);
            return props[r];
        }
        /// <summary>
        /// Get a random array element.
        /// </summary>
        public string ArrayElement(Array array)
        {
            array = array ?? new[] {"a", "b", "c"};

            var r = Number(max: array.Length - 1);

            return array.GetValue(r).ToString();
        }

        /// <summary>
        /// Helper method to get a random element inside a JArray
        /// </summary>
        public string ArrayElement(JArray array)
        {
            var r = Number(max: array.Count - 1);

            return array[r].ToString();
        }

        /// <summary>
        /// Replaces symbols with numbers. IE: ### -> 283
        /// </summary>
        /// <param name="format"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public string Replace(string format, char symbol = '#')
        {
            var chars = format.Select(c => c == symbol ? Convert.ToChar('0' + Number(9)) : c)
                .ToArray();

            return new string(chars);
        }

        /// <summary>
        /// Shuffles an IEnumerable source.
        /// </summary>
        public IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
        {
            List<T> buffer = source.ToList();
            for( int i = 0; i < buffer.Count; i++ )
            {
                int j = Seed.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}

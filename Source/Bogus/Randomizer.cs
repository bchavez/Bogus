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
        /// Get a random sequence of digits
        /// </summary>
        /// <param name="count">How many</param>
        /// <param name="minDigit">minimum digit, inclusive</param>
        /// <param name="maxDigit">maximum digit, inclusive</param>
        /// <returns></returns>
        public int[] Digits(int count, int minDigit = 0, int maxDigit = 9)
        {
            if( maxDigit > 9 || maxDigit < 0 ) throw new ArgumentException(nameof(maxDigit), "max digit can't be lager than 9 or smaller than 0");
            if( minDigit > 9 || minDigit < 0 ) throw new ArgumentException(nameof(minDigit), "min digit can't be lager than 9 or smaller than 0");

            var digits = new int[count];
            for( var i = 0; i < count; i++)
            {
                digits[i] = Number(min: minDigit, max: maxDigit);
            }
            return digits;
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
        /// Returns a random even number
        /// </summary>
        /// <param name="min">Lower bound, inclusive</param>
        /// <param name="max">Upper bound, inclusive</param>
        public int Even(int min = 0, int max = 1)
        {
            var result = 0;
            do
            {
                result = Number(min, max);
            } while( result % 2 == 1 );
            return result;
        }

        /// <summary>
        /// Returns a random even number
        /// </summary>
        /// <param name="min">Lower bound, inclusive</param>
        /// <param name="max">Upper bound, inclusive</param>
        public int Odd(int min = 0, int max = 1)
        {
            int result = 0;
            do
            {
                result = Number(min, max);
            } while (result % 2 == 0);
            return result;
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
        public bool Bool()
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
        public string ReplaceNumbers(string format, char symbol = '#')
        {
            var chars = format.Select(c => c == symbol ? Convert.ToChar('0' + Number(9)) : c)
                .ToArray();

            return new string(chars);
        }
        /// <summary>
        /// Replaces symbols with numbers and letters. # = number, ? = letter, * = number or letter. IE: ###???* -> 283QED4
        /// </summary>
        /// <param name="format"></param>
        public string Replace(string format)
        {
            var chars = format.Select(c =>
                {
                    if (c == '*')
                    {
                        c = Bool() ? '#' : '?';
                    }
                    if ( c == '#' )
                    {
                        return Convert.ToChar('0' + Number(9));
                    }
                    if( c == '?' )
                    {
                        return Convert.ToChar('A' + Number(25));
                    }
                    
                    return c;
                })
                .ToArray();

            return new string(chars);
        }

        /// <summary>
        /// Picks a random Enum of T. Works only with Enums.
        /// </summary>
        /// <typeparam name="T">Must be an Enum</typeparam>
        /// <param name="exclude">Exclude enum values from being returned</param>
        public T Enum<T>(params T[] exclude) where T : struct 
        {
            var e = typeof(T);
            if (!e.IsEnum)
                throw new ArgumentException("When calling Enum<T>() with no parameters T must be an enum.");

            var selection = System.Enum.GetNames(e);
            
            if( exclude.Any() )
            {
                var excluded = exclude.Select(ex => System.Enum.GetName(e, ex));
                selection = selection.Except(excluded).ToArray();
            }

            if ( !selection.Any() )
            {
                throw new ArgumentException("There are no values after exclusion to choose from.");
            }

            var val = this.ArrayElement(selection);

            T picked;
            System.Enum.TryParse(val, out picked);
            return picked;
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
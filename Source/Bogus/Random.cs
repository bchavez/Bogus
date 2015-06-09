using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Bogus
{
    public class Randomizer
    {
        public static Random Seed = new Random();

        public int Number(int max)
        {
            return Number(0, max);
        }
        public int Number(int min = 0, int max = 1)
        {
            return Seed.Next(min, max + 1);
        }

        public double Double()
        {
            return Seed.NextDouble();
        }
        public  bool Bool()
        {
            return Number() == 0;
        }

        public T ArrayElement<T>(T[] array)
        {
            var r = Number(max: array.Length - 1);
            return array[r];
        }

        public JToken ArrayElement(JProperty[] props)
        {
            var r = Number(max: props.Length - 1);
            return props[r];
        }
        public string ArrayElement(Array array)
        {
            array = array ?? new[] {"a", "b", "c"};

            var r = Number(max: array.Length - 1);

            return array.GetValue(r).ToString();
        }
        public string ArrayElement(JArray array)
        {
            var r = Number(max: array.Count - 1);

            return array[r].ToString();
        }

        public string Replace(string format, char symbol = '#')
        {
            var chars = format.Select(c => c == symbol ? Convert.ToChar('0' + Number(9)) : c)
                .ToArray();

            return new string(chars);
        }

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

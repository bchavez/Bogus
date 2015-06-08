using System;
using Newtonsoft.Json.Linq;

namespace FluentFaker
{
    public static class Random
    {
        public static System.Random Generator = new System.Random();

        public static int Number(int max)
        {
            return Number(0, max);
        }
        public static int Number(int min = 0, int max = 1)
        {
            return Generator.Next(min, max + 1);
        }

        public static bool Bool()
        {
            return Number() == 0;
        }

        public static JToken ArrayElement(JProperty[] props)
        {
            var r = Number(max: props.Length - 1);
            return props[r];
        }

        public static string ArrayElement(Array array)
        {
            array = array ?? new[] {"a", "b", "c"};

            var r = Number(max: array.Length - 1);

            return array.GetValue(r).ToString();
        }
        public static string ArrayElement(JArray array)
        {

            var r = Number(max: array.Count - 1);

            return array[r].ToString();
        }
    }
}
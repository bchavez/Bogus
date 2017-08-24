using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Bogus.DataSets;
using Bogus.Extensions;
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

        internal static Lazy<object> Locker = new Lazy<object>(() => new object(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// Get an int from 0 to max.
        /// </summary>
        /// <param name="max">Upper bound, inclusive. Only int.MaxValue is exclusive.</param>
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
            if(maxDigit > 9 || maxDigit < 0) throw new ArgumentException(nameof(maxDigit), "max digit can't be lager than 9 or smaller than 0");
            if(minDigit > 9 || minDigit < 0) throw new ArgumentException(nameof(minDigit), "min digit can't be lager than 9 or smaller than 0");

            var digits = new int[count];
            for(var i = 0; i < count; i++)
            {
                digits[i] = Number(min: minDigit, max: maxDigit);
            }
            return digits;
        }

        /// <summary>
        /// Get an int from min to max.
        /// </summary>
        /// <param name="min">Lower bound, inclusive</param>
        /// <param name="max">Upper bound, inclusive. Only int.MaxValue is exclusive.</param>
        /// <returns></returns>
        public int Number(int min = 0, int max = 1)
        {
            //lock any seed access, for thread safety.
            lock(Locker.Value)
            {
                //Clamp max value, Issue #30.
                max = max == int.MaxValue ? max : max + 1;
                return Seed.Next(min, max);
            }
        }

        /// <summary>
        /// Returns a random even number
        /// </summary>
        /// <param name="min">Lower bound, inclusive</param>
        /// <param name="max">Upper bound, inclusive</param>
        public int Even(int min = 0, int max = 1)
        {
            var result = 0;
            do //could do this better by just +1 or -1 if it's not an even/odd number
            {
                result = Number(min, max);
            } while(result % 2 == 1);
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
            do //could do this better by just +1 or -1 if it's not an even/odd number
            {
                result = Number(min, max);
            } while(result % 2 == 0);
            return result;
        }


        /// <summary>
        /// Get a random double, between 0.0 and 1.0.
        /// </summary>
        /// <param name="min">Minimum, default 0.0</param>
        /// <param name="max">Maximum, default 1.0</param>
        public double Double(double min = 0.0d, double max = 1.0d)
        {
            //lock any seed access, for thread safety.
            lock(Locker.Value)
            {
                if( min == 0.0d && max == 1.0d )
                {
                    //use default implementation
                    return Seed.NextDouble();
                }

                return Seed.NextDouble() * (max - min) + min;
            }
        }

        /// <summary>
        /// Get a random decimal, between 0.0 and 1.0
        /// </summary>
        /// <param name="min">Minimum, default 0.0</param>
        /// <param name="max">Maximum, default 1.0</param>
        public decimal Decimal(decimal min = 0.0m, decimal max = 1.0m)
        {
            return Convert.ToDecimal(Double()) * (max - min) + min;
        }

        /// <summary>
        /// Get a random float, between 0.0 and 1.0
        /// </summary>
        /// <param name="min">Minimum, default 0.0</param>
        /// <param name="max">Maximum, default 1.0</param>
        public float Float(float min = 0.0f, float max = 1.0f)
        {
            return Convert.ToSingle(Double()) * (max - min) + min;
        }

        /// <summary>
        /// Generate a random byte between 0 and 255.
        /// </summary>
        /// <param name="min">Min value, default 0</param>
        /// <param name="max">Max value, default 255</param>
        public byte Byte(byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            return Convert.ToByte(Number(min, max));
        }

        /// <summary>
        /// Get a random sequence of bytes.
        /// </summary>
        /// <param name="count">The size of the byte array</param>
        public byte[] Bytes(int count)
        {
            var arr = new byte[count];
            lock( Locker.Value )
            {
                Seed.NextBytes(arr);
            }
            return arr;
        }

        /// <summary>
        /// Generate a random sbyte between -128 and 127.
        /// </summary>
        /// <param name="min">Min value, default -128</param>
        /// <param name="max">Max value, default 127</param>
        public sbyte SByte(sbyte min = sbyte.MinValue, sbyte max = sbyte.MaxValue)
        {
            return Convert.ToSByte(Number(min, max));
        }
        
        /// <summary>
        /// Generate a random int between MinValue and MaxValue.
        /// </summary>
        /// <param name="min">Min value, default MinValue</param>
        /// <param name="max">Max value, default MaxValue</param>
        public int Int(int min = int.MinValue, int max = int.MaxValue)
        {
            return this.Number(min, max);
        }

        /// <summary>
        /// Generate a random uint between MinValue and MaxValue.
        /// </summary>
        /// <param name="min">Min value, default MinValue</param>
        /// <param name="max">Max value, default MaxValue</param>
        public uint UInt(uint min = uint.MinValue, uint max = uint.MaxValue)
        {
            return Convert.ToUInt32(Double() * (max - min) + min);
        }

        /// <summary>
        /// Generate a random ulong between -128 and 127.
        /// </summary>
        /// <param name="min">Min value, default -128</param>
        /// <param name="max">Max value, default 127</param>
        public ulong ULong(ulong min = ulong.MinValue, ulong max = ulong.MaxValue)
        {
            return Convert.ToUInt64(Double() * (max - min) + min);
        }
        
        /// <summary>
        /// Generate a random long between MinValue and MaxValue.
        /// </summary>
        /// <param name="min">Min value, default MinValue</param>
        /// <param name="max">Max value, default MaxValue</param>
        public long Long(long min = long.MinValue, long max = long.MaxValue)
        {
            var range = (decimal)max - min; //use more bits?
            return Convert.ToInt64((decimal)Double() * range + min);
        }

        /// <summary>
        /// Generate a random short between MinValue and MaxValue.
        /// </summary>
        /// <param name="min">Min value, default MinValue</param>
        /// <param name="max">Max value, default MaxValue</param>
        public short Short(short min = short.MinValue, short max = short.MaxValue)
        {
            return Convert.ToInt16(Double() * (max - min) + min);
        }

        /// <summary>
        /// Generate a random ushort between MinValue and MaxValue.
        /// </summary>
        /// <param name="min">Min value, default MinValue</param>
        /// <param name="max">Max value, default MaxValue</param>
        public ushort UShort(ushort min = ushort.MinValue, ushort max = ushort.MaxValue)
        {
            return Convert.ToUInt16(Double() * (max - min) + min);
        }

        /// <summary>
        /// Generate a random char between MinValue and MaxValue.
        /// </summary>
        /// <param name="min">Min value, default MinValue</param>
        /// <param name="max">Max value, default MaxValue</param>
        public char Char(char min = char.MinValue, char max = char.MaxValue)
        {
            return Convert.ToChar(Number(min, max));
        }

        /// <summary>
        /// Generate a random chars between MinValue and MaxValue.
        /// </summary>
        /// <param name="min">Min value, default MinValue</param>
        /// <param name="max">Max value, default MaxValue</param>
        /// <param name="count">The length of chars to return</param>
        public char[] Chars(char min = char.MinValue, char max = char.MaxValue, int count = 5)
        {
            var arr = new char[count];
            for(var i = 0; i < count; i++)
                arr[i] = Char(min, max);
            return arr;
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
        /// Get a random list item.
        /// </summary>
        public T ListItem<T>(List<T> list)
        {
           return ListItem(list as IList<T>);
        }

        /// <summary>
        /// Get a random list item.
        /// </summary>
        public T ListItem<T>(IList<T> list)
        {
            var r = Number(max: list.Count - 1);
            return list[r];
        }

        /// <summary>
        /// Get a random collection item.
        /// </summary>
        public T CollectionItem<T>(ICollection<T> collection)
        {
           var r = Number(max: collection.Count - 1);
           return collection.Skip(r).First();
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

        internal T ArrayElement<T>(JArray array)
        {
            var r = Number(max: array.Count - 1);

            return array[r].ToObject<T>();
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
        /// Replaces symbols with numbers and letters. # = number, ? = letter, * = number or letter. IE: ###???* -> 283QED4. Letters are uppercase.
        /// </summary>
        /// <param name="format"></param>
        public string Replace(string format)
        {
            var chars = format.Select(c =>
                {
                    if(c == '*')
                    {
                        c = Bool() ? '#' : '?';
                    }
                    if(c == '#')
                    {
                        return Convert.ToChar('0' + Number(9));
                    }
                    if(c == '?')
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
            if(!e.IsEnum())
                throw new ArgumentException("When calling Enum<T>() with no parameters T must be an enum.");

            var selection = System.Enum.GetNames(e);

            if(exclude.Any())
            {
                var excluded = exclude.Select(ex => System.Enum.GetName(e, ex));
                selection = selection.Except(excluded).ToArray();
            }

            if(!selection.Any())
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
            for(var i = 0; i < buffer.Count; i++)
            {
                int j;
                //lock any seed access, for thread safety.
                lock(Locker.Value)
                {
                    j = Seed.Next(i, buffer.Count);
                }
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }

        /// <summary>
        /// Returns a single word or phrase in English.
        /// </summary>
        public string Word()
        {
            var randomWordMethod = ListItem(WordFunctions.Functions);
            return randomWordMethod();
        }

        /// <summary>
        /// Gets some random words and phrases in English.
        /// </summary>
        /// <param name="count">Number of times to call Word()</param>
        public string Words(int? count = null)
        {
            if(count == null)
                count = Number(1, 3);

            var words = WordsArray(count.Value);

            return string.Join(" ", words);
        }

        /// <summary>
        /// Get a range of words in an array (English)
        /// </summary>
        /// <param name="min">Minimum word count.</param>
        /// <param name="max">Maximum word count.</param>
        public string[] WordsArray(int min, int max)
        {
            var count = Number(min, max);
            return WordsArray(count);
        }

        /// <summary>
        /// Get a specific number of words in an array (English).
        /// </summary>
        public string[] WordsArray(int count)
        {
            return Enumerable.Range(1, count)
                .Select(f => Word())
                .ToArray(); // lol.
        }

        /// <summary>
        /// Get a random unique GUID.
        /// </summary>
        public Guid Uuid()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// Returns a random locale.
        /// </summary>
        public string RandomLocale()
        {
            var ele = ArrayElement(Database.Data.Value.Properties().ToArray()) as JProperty;
            return ele.Name;
        }


        private static char[] AlphaChars =
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
                'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
                'u', 'v', 'w', 'x', 'y', 'z'
            };
        /// <summary>
        /// Returns a random set of alpha numeric characters 0-9, a-z
        /// </summary>
        public string AlphaNumeric(int count)
        {
            var sb = new StringBuilder();
            return Enumerable.Range(1, count).Aggregate(sb, (b, i) => b.Append(ArrayElement(AlphaChars)), b => b.ToString());
        }

        //items are weighted by the decimal probability in their value
        /// <summary>
        /// Returns a selection of T[] based on a weighted distribution of probability
        /// </summary>
        /// <param name="items">Items to draw the selection from.</param>
        /// <param name="weights">Weights in decimal form: ie:[.25, .50, .25] for total of 3 items. Should add up to 1.</param>
        public T WeightedRandom<T>(T[] items, float[] weights)
        {
            if (weights.Length != items.Length) throw new ArgumentOutOfRangeException($"{nameof(items)}.Length and {nameof(weights)}.Length must be the same.");

            var rand = this.Float();
            float max;
            float min = 0f;

            var item = default(T);

            for (int i = 0; i < weights.Length; i++)
            {
                max = min + weights[i];
                item = items[i];
                if (rand >= min && rand <= max)
                {
                    break;
                }
                min = min + weights[i];
            }

            return item;
        }

    }

    public static class WordFunctions
    {
        public static List<Func<string>> Functions = new List<Func<string>>();

        static WordFunctions()
        {
            var commerce = new Commerce();
            var company = new Company();
            var address = new Address();
            var finance = new Finance();
            var hacker = new Hacker();
            var name = new Name();

            Functions.Add(() => commerce.Department());
            Functions.Add(() => commerce.ProductName());
            Functions.Add(() => commerce.ProductAdjective());
            Functions.Add(() => commerce.ProductMaterial());
            Functions.Add(() => commerce.ProductName());
            Functions.Add(() => commerce.Color());

            Functions.Add(() => company.CatchPhraseAdjective());
            Functions.Add(() => company.CatchPhraseDescriptor());
            Functions.Add(() => company.CatchPhraseNoun());
            Functions.Add(() => company.BsAdjective());
            Functions.Add(() => company.BsBuzz());
            Functions.Add(() => company.BsNoun());

            Functions.Add(() => address.StreetSuffix());
            Functions.Add(() => address.County());
            Functions.Add(() => address.Country());
            Functions.Add(() => address.State());
            
            Functions.Add(() => address.StreetSuffix());

            Functions.Add(() => finance.AccountName());
            Functions.Add(() => finance.TransactionType());
            Functions.Add(() => finance.Currency().Description);

            Functions.Add(() => hacker.Noun());
            Functions.Add(() => hacker.Verb());
            Functions.Add(() => hacker.Adjective());
            Functions.Add(() => hacker.IngVerb());
            Functions.Add(() => hacker.Abbreviation());

            Functions.Add(() => name.JobDescriptor());
            Functions.Add(() => name.JobArea());
            Functions.Add(() => name.JobType());
        }
    }

}
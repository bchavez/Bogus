using System.Linq;
using System.Text;

namespace Bogus.Extensions.Canada
{
    public static class ExtensionsForCanada
    {
        private static int[] Mask = {1, 2, 1,  2, 1, 2,  1, 2, 1};

        /// <summary>
        /// Social Insurance Number for Canada 
        /// </summary>
        public static string Sin(this Person p)
        {
            const string Key = nameof(ExtensionsForCanada) + "SIN";
            if( p.context.ContainsKey(Key) )
            {
                return p.context[Key] as string;
            }

            //bit verbose, but works. :)
            //could be mathematically simplified.
            //brute forced this one. yeah.

            var r = new Randomizer();
            //get 8 numbers
            var numbers = Enumerable.Range(0, 8)
                .Select(s => r.Number(9)).ToList();

            // the last number that makes it pass the checksum.
            var last = 10 - (numbers.Sum() % 10);
            if (last == 10)
                last = 0;

            var digits = numbers.Concat(new[] {last}).ToArray();

            var comp = digits
                .Zip(Mask, (n, c) =>
                    {
                        if( c == 2 && n % c == 1 )
                        {
                            // odd digit, it was multiplied, reverse the process
                            return (10 + (n - 1)) / 2;
                        }
                        if( c == 2 )
                        {
                            //simply divide an even number by two
                            return n / 2;
                        }
                        //else c == 1, and n was multiplied by 1
                        return n;
                    }).ToArray();

            var chars = comp
                .Select(n => (byte)(n + '0')); //boost all digits to ASCII number range

            var sinstr = Encoding.ASCII.GetString(chars.ToArray());

            p.context[Key] = sinstr;

            return sinstr;
        }

     
    }
}
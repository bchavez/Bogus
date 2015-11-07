using System;
using System.Linq;

namespace Bogus.Extensions.Brazil
{
    public static class ExtensionsForBrazil 
    {
        private static Randomizer r = new Randomizer();
        private static int[] Weights = {10, 9, 8, 7, 6, 5, 4, 3, 2};
        public static string Cpf(this Person p)
        {
            const string Key = nameof(ExtensionsForBrazil) + "CPF";
            if (p.context.ContainsKey(Key))
            {
                return p.context[Key] as string;
            }

            var digits = r.Digits(9);
            var sum1 = digits.Zip(Weights, (d, w) => d * w)
                .Sum();

            var sum1mod = sum1 % 11;

            int check1 = 0;
            if( sum1mod >= 2 )
            {
                check1 = 11 - sum1mod;
            }

            var finalWeights = new[] {11}.Concat(Weights);

            var sum2 = digits.Concat(new[] {check1})
                .Zip(finalWeights, (d, w) => d * w)
                .Sum();

            var sum2mod = sum2 % 11;

            var check2 = 0;
            if( sum2mod >= 2 )
            {
                check2 = 11 - sum2mod;
            }

            var all = digits.Concat(new[] {check1, check2}).ToArray();

            var final = $"{all[0]}{all[1]}{all[2]}.{all[3]}{all[4]}{all[5]}.{all[6]}{all[7]}{all[8]}-{all[9]}{all[10]}";

            p.context[Key] = final;

            return final;
        }
    }
}
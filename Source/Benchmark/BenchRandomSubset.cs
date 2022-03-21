using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmark
{
    [RPlotExporter]
    public class BenchRandomSubset
    {
        private Randomizer r;
        private List<int> items;

        [Params(2, 10, 100, 500, 1000, 1999)]
        public int Selections { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            r = new Randomizer();
            items = Enumerable.Range(1, 2000).ToList();
        }

        [Benchmark]
        public void PickRandom()
        {
            PickRandom(items, this.Selections).ToList();
        }

        [Benchmark]
        public void ShuffleTake()
        {
            r.Shuffle(items).Take(this.Selections).ToList();
        }


        /// <summary>
        /// Helper to pick random subset of elements out of the list.
        /// </summary>
        /// <param name="amountToPick">amount of elements to pick of the list.</param>
        /// <exception cref="ArgumentException">if amountToPick is lower than zero.</example>
        public IEnumerable<T> PickRandom<T>(IEnumerable<T> items, int amountToPick)
        {
            if (amountToPick < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(amountToPick)} needs to be a positive integer.");
            }
            var size = items.Count();
            if (amountToPick > size)
            {
                throw new ArgumentOutOfRangeException($"{nameof(amountToPick)} is greater than the number of items.");
            }

            foreach (var item in items)
            {
                if (amountToPick <= 0)
                {
                    yield break;
                }
                if (r.Int(1, size) <= amountToPick)
                {
                    amountToPick--;
                    yield return item;
                }
                size--;
            }
        }

    }
}
using System;
using System.Linq;

namespace Bogus.DataSets
{
    public class Commerce : DataSet
    {
        public Commerce( string locale = "en" ) : base( locale )
        {
        }

        public string Department(int max = 3, bool returnMax = false)
        {
            var num = max;
            if( !returnMax )
            {
                do
                {
                    num = this.Random.Number(max);
                } while (num == 0);
            }

            var cats = Categories(num);
            if (num > 1)
            {
                return string.Format("{0} & {1}", string.Join(", ", cats.Take(cats.Length - 1)),
                    cats.Last());
            }

            return cats[0];
        }

        /// there is an easier way to do this.
        /// check finance.amount
        public string Price( decimal min = 0, decimal max = 1000, int decimals = 2, string symbol = "" )
        {
            var amount = ( max - min );
            var part = (decimal)Random.Double() * amount;
            return symbol + Math.Round(min + part, decimals);
        }

        public string[] Categories(int num)
        {
            var result = new string[num];

            for ( var i = 0; i < num; i++ )
            {
                result[i] = GetRandomArrayItem( "department" );
            }
            return result;
        }

        public string ProductName()
        {
            return string.Format( "{0} {1} {2}",
                ProductAdjective(),
                ProductMaterial(),
                Product()
                );
        }

        public string Color()
        {
            return GetRandomArrayItem( "color" );
        }

        public string Product()
        {
            return GetRandomArrayItem( "product_name.product" );
        }

        public string ProductAdjective()
        {
            return GetRandomArrayItem( "product_name.adjective" );
        }

        public string ProductMaterial()
        {
            return GetRandomArrayItem( "product_name.material" );
        }

    }
}
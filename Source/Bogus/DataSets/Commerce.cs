using System;

namespace Bogus.DataSets
{
    public class Commerce : DataSet
    {
        public Commerce( string locale = "en" ) : base( locale )
        {
        }


        public string Department(int max = 3)
        {

            return null;
        }

        /// there is an easier way to do this.
        /// check finance.amount
        public string Price( double min = 0, double max = 1000, int dec = 2, string symbol = "" )
        {
            if ( min < 0 | max < 0 )
                return symbol + 0.00;
            a
            return symbol + Math.Round(
                this.Random.Double() * (max - min) + min
                * Math.Pow( 10, dec )
                / Math.Pow( 10, dec ), dec );
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
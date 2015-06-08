using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace FluentFaker
{
    public class Currency
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
    }

    public class Finance : Category
    {
        /// <summary>
        /// Get an account number. Default length is 8 digits.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Account(int length = 8)
        {
            var template = new string('#', length);
            return Utils.ReplaceSymbolsWithNumbers(template);
        }

        /// <summary>
        /// Get an account name. Like "savings", "checking", "Home Loan" etc..
        /// </summary>
        /// <returns></returns>
        public string AccountName()
        {
            var type = GetRandomArrayItem("account_type");
            return string.Format("{0} Account", type);
        }

        /// <summary>
        /// Get a random amount. Default 0 - 1000.
        /// </summary>
        /// <param name="min">Min value. Default 0.</param>
        /// <param name="max">Max value. Default 1000.</param>
        /// <param name="decimals">Decimal places. Default 2.</param>
        /// <returns></returns>
        public decimal Amount(decimal min = 0, decimal max = 1000, int decimals = 2)
        {
            var amount = (max - min);
            var part = (decimal)Random.Generator.NextDouble() * amount;
            return Math.Round( min + part, decimals);
        }

        /// <summary>
        /// Get a transaction type: "deposit", "withdrawal", "payment", or "invoice".
        /// </summary>
        /// <returns></returns>
        public string TransactionType()
        {
            return GetRandomArrayItem("transaction_type");
        }

        /// <summary>
        /// Get a random currency.
        /// </summary>
        /// <returns></returns>
        public Currency Currency()
        {
            var obj = GetObject("currency");
            var keys = obj.Properties().ToArray();
            var prop = Random.ArrayElement(keys) as JProperty;
            
            var cur = prop.First.ToObject<Currency>();
            cur.Description = prop.Name;

            return cur;
        }
    }
}
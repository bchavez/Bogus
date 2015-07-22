using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Bogus.DataSets
{
    /// <summary>
    /// Represents a currency
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// The long for description of the currency. IE: "US Dollar"
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The currency code. iE: USD.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// The currency symbol. IE: $
        /// </summary>
        public string Symbol { get; set; }
    }


    /// <summary>
    /// Provides financial randomness.
    /// </summary>
    public class Finance : DataSet
    {
        /// <summary>
        /// Get an account number. Default length is 8 digits.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string Account(int length = 8)
        {
            var template = new string('#', length);
            return Random.Replace(template);
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
            var part = (decimal)Random.Double() * amount;
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

        //We could do better at generating these I suppose.
        /// <summary>
        /// Returns a credit card number that should pass validation. See [here](https://developers.braintreepayments.com/ios+ruby/reference/general/testing).
        /// </summary>
        /// <returns></returns>
        public string CreditCardNumber()
        {
            var cards = new[]
                {
                    "378282246310005",
                    "371449635398431",
                    "6011111111111117",
                    "3530111333300000",
                    "6304000000000000",
                    "5555555555554444",
                    "4111111111111111",
                    "4005519200000004",
                    "4009348888881881",
                    "4012000033330026",
                    "4012000077777777",
                    "4012888888881881",
                    "4217651111111119",
                    "4500600000000061"
                };

            return Random.ArrayElement(cards);
        }
    }

}

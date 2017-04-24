using System;
using System.Linq;
using System.Text.RegularExpressions;
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
            return $"{type} Account";
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

        /// <summary>
        /// Generates a random bitcoin address
        /// </summary>
        public string BitcoinAddress()
        {
            var addressLength = Math.Floor(this.Random.Double()*(36 - 27 + 1)) + 27;
            var address = this.Random.ArrayElement(new[] {"1", "3"});
            for( var i = 0; i < addressLength - 1; i++ )
            {
                address += "*";
            }
            return Random.Replace(address);
        }

        private static readonly string[] BicVowels = { "A", "E", "I", "O", "U" };
        /// <summary>
        /// Generates Bank Identifier Code (BIC) code.
        /// </summary>
        public string Bic()
        {
            var prob = this.Random.Number(100);
            return this.Random.Replace("???") +
                this.Random.ArrayElement(BicVowels) +
                this.Random.ArrayElement(IbanLib["iso3166"] as JArray) +
                this.Random.Replace("?") + "1" +
                (prob < 10 ?
                    this.Random.Replace("?" + this.Random.ArrayElement(BicVowels) + "?") :
                prob < 40 ?
                    this.Random.Replace("###") : "");
        }

        /// <summary>
        /// Generates an International Bank Account Number (IBAN).
        /// </summary>
        /// <param name="formatted"></param>
        /// <returns></returns>
        public string Iban(bool formatted = false)
        {
            var ibanFormat = this.Random.ArrayElement<IBanFormat>(IbanLib["formats"] as JArray);
            var s = "";
            var count = 0;
            for( var b = 0; b < ibanFormat.Bban.Length; b++ )
            {
                var bban = ibanFormat.Bban[b];
                var c = bban.Count;
                count += bban.Count;
                while( c > 0 )
                {
                    if( bban.Type == "a" )
                    {
                        s += this.Random.ArrayElement(IbanLib["alpha"] as JArray);
                    }
                    else if( bban.Type == "c" )
                    {
                        if( this.Random.Number(100) < 80 )
                        {
                            s += this.Random.Number(9);
                        }
                        else
                        {
                            s += this.Random.ArrayElement(IbanLib["alpha"] as JArray);
                        }
                    }
                    else
                    {
                        if( c >= 3 && this.Random.Number(100) < 30 )
                        {
                            if( this.Random.Bool() )
                            {
                                s += this.Random.ArrayElement(IbanLib["pattern100"] as JArray);
                                c -= 2;
                            }
                            else
                            {
                                s += this.Random.ArrayElement(IbanLib["pattern10"] as JArray);
                                c--;
                            }
                        }
                        else
                        {
                            s += this.Random.Number(9);
                        }
                    }
                    c--;
                }
                s = s.Substring(0, count);
            }
            var checksum = 98 - IbanMod97(IbanToDigitString(s + ibanFormat.Country + "00"));
            var iban = ibanFormat.Country + checksum.ToString("00") + s;

            if( formatted )
            {
                var matches = Regex.Matches(iban, ".{1,4}");
                var array = matches.OfType<Match>()
                    .Select(m => m.Value)
                    .ToArray();
                return string.Join(" ", array);
            }
            return iban;
        }

        private int IbanMod97(string digitStr)
        {
            var m = 0;
            for( int i = 0; i < digitStr.Length; i++ )
            {
                m = ((m * 10) + (digitStr[i]-'0')) % 97;
            }
            return m;
        }

        private string IbanToDigitString(string str)
        {
            return Regex.Replace(str, "[A-Z]", (m) => (Convert.ToChar(m.Value) - 55).ToString());
        }

        private class IBanFormat
        {
            public class BbanItem
            {
                public string Type { get; set; }
                public int Count { get; set; }
            }
            public string Country { get; set; }
            public int Total { get; set; }
            public BbanItem[] Bban { get; set; }
            public string Format { get; set; }
        }

        private static JObject IbanLib = JObject.Parse(IbanLibJson);

        private const string IbanLibJson = @"
{
  alpha: [
    'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
  ],
  pattern10: [
    ""01"", ""02"", ""03"", ""04"", ""05"", ""06"", ""07"", ""08"", ""09""
  ],
  pattern100: [
    ""001"", ""002"", ""003"", ""004"", ""005"", ""006"", ""007"", ""008"", ""009""
  ],
  formats: [
    {
      country: ""AL"",
      total: 28,
      bban: [
        {
          type: ""n"",
          count: 8
        },
        {
          type: ""c"",
          count: 16
        }
      ],
      format: ""ALkk bbbs sssx cccc cccc cccc cccc""
    },
    {
      country: ""AD"",
      total: 24,
      bban: [
        {
          type: ""n"",
          count: 8
        },
        {
          type: ""c"",
          count: 12
        }
      ],
      format: ""ADkk bbbb ssss cccc cccc cccc""
    },
    {
      country: ""AT"",
      total: 20,
      bban: [
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""n"",
          count: 11
        }
      ],
      format: ""ATkk bbbb bccc cccc cccc""
    },
    {
      country: ""AZ"",
      total: 28,
      bban: [
        {
          type: ""c"",
          count: 4
        },
        {
          type: ""n"",
          count: 20
        }
      ],
      format: ""AZkk bbbb cccc cccc cccc cccc cccc""
    },
    {
      country: ""BH"",
      total: 22,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""c"",
          count: 14
        }
      ],
      format: ""BHkk bbbb cccc cccc cccc cc""
    },
    {
      country: ""BE"",
      total: 16,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""n"",
          count: 9
        }
      ],
      format: ""BEkk bbbc cccc ccxx""
    },
    {
      country: ""BA"",
      total: 20,
      bban: [
        {
          type: ""n"",
          count: 6
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""BAkk bbbs sscc cccc ccxx""
    },
    {
      country: ""BR"",
      total: 29,
      bban: [
        {
          type: ""n"",
          count: 13
        },
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""a"",
          count: 1
        },
        {
          type: ""c"",
          count: 1
        }
      ],
      format: ""BRkk bbbb bbbb ssss sccc cccc ccct n""
    },
    {
      country: ""BG"",
      total: 22,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""n"",
          count: 6
        },
        {
          type: ""c"",
          count: 8
        }
      ],
      format: ""BGkk bbbb ssss ddcc cccc cc""
    },
    {
      country: ""CR"",
      total: 21,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""n"",
          count: 14
        }
      ],
      format: ""CRkk bbbc cccc cccc cccc c""
    },
    {
      country: ""HR"",
      total: 21,
      bban: [
        {
          type: ""n"",
          count: 7
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""HRkk bbbb bbbc cccc cccc c""
    },
    {
      country: ""CY"",
      total: 28,
      bban: [
        {
          type: ""n"",
          count: 8
        },
        {
          type: ""c"",
          count: 16
        }
      ],
      format: ""CYkk bbbs ssss cccc cccc cccc cccc""
    },
    {
      country: ""CZ"",
      total: 24,
      bban: [
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""CZkk bbbb ssss sscc cccc cccc""
    },
    {
      country: ""DK"",
      total: 18,
      bban: [
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""DKkk bbbb cccc cccc cc""
    },
    {
      country: ""DO"",
      total: 28,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""n"",
          count: 20
        }
      ],
      format: ""DOkk bbbb cccc cccc cccc cccc cccc""
    },
    {
      country: ""TL"",
      total: 23,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""TLkk bbbc cccc cccc cccc cxx""
    },
    {
      country: ""EE"",
      total: 20,
      bban: [
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 12
        }
      ],
      format: ""EEkk bbss cccc cccc cccx""
    },
    {
      country: ""FO"",
      total: 18,
      bban: [
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""FOkk bbbb cccc cccc cx""
    },
    {
      country: ""FI"",
      total: 18,
      bban: [
        {
          type: ""n"",
          count: 6
        },
        {
          type: ""n"",
          count: 8
        }
      ],
      format: ""FIkk bbbb bbcc cccc cx""
    },
    {
      country: ""FR"",
      total: 27,
      bban: [
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""c"",
          count: 11
        },
        {
          type: ""n"",
          count: 2
        }
      ],
      format: ""FRkk bbbb bggg ggcc cccc cccc cxx""
    },
    {
      country: ""GE"",
      total: 22,
      bban: [
        {
          type: ""c"",
          count: 2
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""GEkk bbcc cccc cccc cccc cc""
    },
    {
      country: ""DE"",
      total: 22,
      bban: [
        {
          type: ""n"",
          count: 8
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""DEkk bbbb bbbb cccc cccc cc""
    },
    {
      country: ""GI"",
      total: 23,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""c"",
          count: 15
        }
      ],
      format: ""GIkk bbbb cccc cccc cccc ccc""
    },
    {
      country: ""GR"",
      total: 27,
      bban: [
        {
          type: ""n"",
          count: 7
        },
        {
          type: ""c"",
          count: 16
        }
      ],
      format: ""GRkk bbbs sssc cccc cccc cccc ccc""
    },
    {
      country: ""GL"",
      total: 18,
      bban: [
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""GLkk bbbb cccc cccc cc""
    },
    {
      country: ""GT"",
      total: 28,
      bban: [
        {
          type: ""c"",
          count: 4
        },
        {
          type: ""c"",
          count: 4
        },
        {
          type: ""c"",
          count: 16
        }
      ],
      format: ""GTkk bbbb mmtt cccc cccc cccc cccc""
    },
    {
      country: ""HU"",
      total: 28,
      bban: [
        {
          type: ""n"",
          count: 8
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""HUkk bbbs sssk cccc cccc cccc cccx""
    },
    {
      country: ""IS"",
      total: 26,
      bban: [
        {
          type: ""n"",
          count: 6
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""ISkk bbbb sscc cccc iiii iiii ii""
    },
    {
      country: ""IE"",
      total: 22,
      bban: [
        {
          type: ""c"",
          count: 4
        },
        {
          type: ""n"",
          count: 6
        },
        {
          type: ""n"",
          count: 8
        }
      ],
      format: ""IEkk aaaa bbbb bbcc cccc cc""
    },
    {
      country: ""IL"",
      total: 23,
      bban: [
        {
          type: ""n"",
          count: 6
        },
        {
          type: ""n"",
          count: 13
        }
      ],
      format: ""ILkk bbbn nncc cccc cccc ccc""
    },
    {
      country: ""IT"",
      total: 27,
      bban: [
        {
          type: ""a"",
          count: 1
        },
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""c"",
          count: 12
        }
      ],
      format: ""ITkk xaaa aabb bbbc cccc cccc ccc""
    },
    {
      country: ""JO"",
      total: 30,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 18
        }
      ],
      format: ""JOkk bbbb nnnn cccc cccc cccc cccc cc""
    },
    {
      country: ""KZ"",
      total: 20,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""c"",
          count: 13
        }
      ],
      format: ""KZkk bbbc cccc cccc cccc""
    },
    {
      country: ""XK"",
      total: 20,
      bban: [
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 12
        }
      ],
      format: ""XKkk bbbb cccc cccc cccc""
    },
    {
      country: ""KW"",
      total: 30,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""c"",
          count: 22
        }
      ],
      format: ""KWkk bbbb cccc cccc cccc cccc cccc cc""
    },
    {
      country: ""LV"",
      total: 21,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""c"",
          count: 13
        }
      ],
      format: ""LVkk bbbb cccc cccc cccc c""
    },
    {
      country: ""LB"",
      total: 28,
      bban: [
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""c"",
          count: 20
        }
      ],
      format: ""LBkk bbbb cccc cccc cccc cccc cccc""
    },
    {
      country: ""LI"",
      total: 21,
      bban: [
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""c"",
          count: 12
        }
      ],
      format: ""LIkk bbbb bccc cccc cccc c""
    },
    {
      country: ""LT"",
      total: 20,
      bban: [
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""n"",
          count: 11
        }
      ],
      format: ""LTkk bbbb bccc cccc cccc""
    },
    {
      country: ""LU"",
      total: 20,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""c"",
          count: 13
        }
      ],
      format: ""LUkk bbbc cccc cccc cccc""
    },
    {
      country: ""MK"",
      total: 19,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""c"",
          count: 10
        },
        {
          type: ""n"",
          count: 2
        }
      ],
      format: ""MKkk bbbc cccc cccc cxx""
    },
    {
      country: ""MT"",
      total: 31,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""c"",
          count: 18
        }
      ],
      format: ""MTkk bbbb ssss sccc cccc cccc cccc ccc""
    },
    {
      country: ""MR"",
      total: 27,
      bban: [
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""n"",
          count: 13
        }
      ],
      format: ""MRkk bbbb bsss sscc cccc cccc cxx""
    },
    {
      country: ""MU"",
      total: 30,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 15
        },
        {
          type: ""a"",
          count: 3
        }
      ],
      format: ""MUkk bbbb bbss cccc cccc cccc 000d dd""
    },
    {
      country: ""MC"",
      total: 27,
      bban: [
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""c"",
          count: 11
        },
        {
          type: ""n"",
          count: 2
        }
      ],
      format: ""MCkk bbbb bsss sscc cccc cccc cxx""
    },
    {
      country: ""MD"",
      total: 24,
      bban: [
        {
          type: ""c"",
          count: 2
        },
        {
          type: ""c"",
          count: 18
        }
      ],
      format: ""MDkk bbcc cccc cccc cccc cccc""
    },
    {
      country: ""ME"",
      total: 22,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""n"",
          count: 15
        }
      ],
      format: ""MEkk bbbc cccc cccc cccc xx""
    },
    {
      country: ""NL"",
      total: 18,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""NLkk bbbb cccc cccc cc""
    },
    {
      country: ""NO"",
      total: 15,
      bban: [
        {
          type: ""n"",
          count: 4
        },
        {
          type: ""n"",
          count: 7
        }
      ],
      format: ""NOkk bbbb cccc ccx""
    },
    {
      country: ""PK"",
      total: 24,
      bban: [
        {
          type: ""c"",
          count: 4
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""PKkk bbbb cccc cccc cccc cccc""
    },
    {
      country: ""PS"",
      total: 29,
      bban: [
        {
          type: ""c"",
          count: 4
        },
        {
          type: ""n"",
          count: 9
        },
        {
          type: ""n"",
          count: 12
        }
      ],
      format: ""PSkk bbbb xxxx xxxx xccc cccc cccc c""
    },
    {
      country: ""PL"",
      total: 28,
      bban: [
        {
          type: ""n"",
          count: 8
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""PLkk bbbs sssx cccc cccc cccc cccc""
    },
    {
      country: ""PT"",
      total: 25,
      bban: [
        {
          type: ""n"",
          count: 8
        },
        {
          type: ""n"",
          count: 13
        }
      ],
      format: ""PTkk bbbb ssss cccc cccc cccx x""
    },
    {
      country: ""QA"",
      total: 29,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""c"",
          count: 21
        }
      ],
      format: ""QAkk bbbb cccc cccc cccc cccc cccc c""
    },
    {
      country: ""RO"",
      total: 24,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""c"",
          count: 16
        }
      ],
      format: ""ROkk bbbb cccc cccc cccc cccc""
    },
    {
      country: ""SM"",
      total: 27,
      bban: [
        {
          type: ""a"",
          count: 1
        },
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""c"",
          count: 12
        }
      ],
      format: ""SMkk xaaa aabb bbbc cccc cccc ccc""
    },
    {
      country: ""SA"",
      total: 24,
      bban: [
        {
          type: ""n"",
          count: 2
        },
        {
          type: ""c"",
          count: 18
        }
      ],
      format: ""SAkk bbcc cccc cccc cccc cccc""
    },
    {
      country: ""RS"",
      total: 22,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""n"",
          count: 15
        }
      ],
      format: ""RSkk bbbc cccc cccc cccc xx""
    },
    {
      country: ""SK"",
      total: 24,
      bban: [
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""SKkk bbbb ssss sscc cccc cccc""
    },
    {
      country: ""SI"",
      total: 19,
      bban: [
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""SIkk bbss sccc cccc cxx""
    },
    {
      country: ""ES"",
      total: 24,
      bban: [
        {
          type: ""n"",
          count: 10
        },
        {
          type: ""n"",
          count: 10
        }
      ],
      format: ""ESkk bbbb gggg xxcc cccc cccc""
    },
    {
      country: ""SE"",
      total: 24,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""n"",
          count: 17
        }
      ],
      format: ""SEkk bbbc cccc cccc cccc cccc""
    },
    {
      country: ""CH"",
      total: 21,
      bban: [
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""c"",
          count: 12
        }
      ],
      format: ""CHkk bbbb bccc cccc cccc c""
    },
    {
      country: ""TN"",
      total: 24,
      bban: [
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""n"",
          count: 15
        }
      ],
      format: ""TNkk bbss sccc cccc cccc cccc""
    },
    {
      country: ""TR"",
      total: 26,
      bban: [
        {
          type: ""n"",
          count: 5
        },
        {
          type: ""c"",
          count: 1
        },
        {
          type: ""c"",
          count: 16
        }
      ],
      format: ""TRkk bbbb bxcc cccc cccc cccc cc""
    },
    {
      country: ""AE"",
      total: 23,
      bban: [
        {
          type: ""n"",
          count: 3
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""AEkk bbbc cccc cccc cccc ccc""
    },
    {
      country: ""GB"",
      total: 22,
      bban: [
        {
          type: ""a"",
          count: 4
        },
        {
          type: ""n"",
          count: 6
        },
        {
          type: ""n"",
          count: 8
        }
      ],
      format: ""GBkk bbbb ssss sscc cccc cc""
    },
    {
      country: ""VG"",
      total: 24,
      bban: [
        {
          type: ""c"",
          count: 4
        },
        {
          type: ""n"",
          count: 16
        }
      ],
      format: ""VGkk bbbb cccc cccc cccc cccc""
    }
  ],
  iso3166: [
    ""AC"", ""AD"", ""AE"", ""AF"", ""AG"", ""AI"", ""AL"", ""AM"", ""AN"", ""AO"", ""AQ"", ""AR"", ""AS"",
    ""AT"", ""AU"", ""AW"", ""AX"", ""AZ"", ""BA"", ""BB"", ""BD"", ""BE"", ""BF"", ""BG"", ""BH"", ""BI"",
    ""BJ"", ""BL"", ""BM"", ""BN"", ""BO"", ""BQ"", ""BR"", ""BS"", ""BT"", ""BU"", ""BV"", ""BW"", ""BY"",
    ""BZ"", ""CA"", ""CC"", ""CD"", ""CE"", ""CF"", ""CG"", ""CH"", ""CI"", ""CK"", ""CL"", ""CM"", ""CN"",
    ""CO"", ""CP"", ""CR"", ""CS"", ""CS"", ""CU"", ""CV"", ""CW"", ""CX"", ""CY"", ""CZ"", ""DD"", ""DE"",
    ""DG"", ""DJ"", ""DK"", ""DM"", ""DO"", ""DZ"", ""EA"", ""EC"", ""EE"", ""EG"", ""EH"", ""ER"", ""ES"",
    ""ET"", ""EU"", ""FI"", ""FJ"", ""FK"", ""FM"", ""FO"", ""FR"", ""FX"", ""GA"", ""GB"", ""GD"", ""GE"",
    ""GF"", ""GG"", ""GH"", ""GI"", ""GL"", ""GM"", ""GN"", ""GP"", ""GQ"", ""GR"", ""GS"", ""GT"", ""GU"",
    ""GW"", ""GY"", ""HK"", ""HM"", ""HN"", ""HR"", ""HT"", ""HU"", ""IC"", ""ID"", ""IE"", ""IL"", ""IM"",
    ""IN"", ""IO"", ""IQ"", ""IR"", ""IS"", ""IT"", ""JE"", ""JM"", ""JO"", ""JP"", ""KE"", ""KG"", ""KH"",
    ""KI"", ""KM"", ""KN"", ""KP"", ""KR"", ""KW"", ""KY"", ""KZ"", ""LA"", ""LB"", ""LC"", ""LI"", ""LK"",
    ""LR"", ""LS"", ""LT"", ""LU"", ""LV"", ""LY"", ""MA"", ""MC"", ""MD"", ""ME"", ""MF"", ""MG"", ""MH"",
    ""MK"", ""ML"", ""MM"", ""MN"", ""MO"", ""MP"", ""MQ"", ""MR"", ""MS"", ""MT"", ""MU"", ""MV"", ""MW"",
    ""MX"", ""MY"", ""MZ"", ""NA"", ""NC"", ""NE"", ""NF"", ""NG"", ""NI"", ""NL"", ""NO"", ""NP"", ""NR"",
    ""NT"", ""NU"", ""NZ"", ""OM"", ""PA"", ""PE"", ""PF"", ""PG"", ""PH"", ""PK"", ""PL"", ""PM"", ""PN"",
    ""PR"", ""PS"", ""PT"", ""PW"", ""PY"", ""QA"", ""RE"", ""RO"", ""RS"", ""RU"", ""RW"", ""SA"", ""SB"",
    ""SC"", ""SD"", ""SE"", ""SG"", ""SH"", ""SI"", ""SJ"", ""SK"", ""SL"", ""SM"", ""SN"", ""SO"", ""SR"",
    ""SS"", ""ST"", ""SU"", ""SV"", ""SX"", ""SY"", ""SZ"", ""TA"", ""TC"", ""TD"", ""TF"", ""TG"", ""TH"",
    ""TJ"", ""TK"", ""TL"", ""TM"", ""TN"", ""TO"", ""TR"", ""TT"", ""TV"", ""TW"", ""TZ"", ""UA"", ""UG"",
    ""UM"", ""US"", ""UY"", ""UZ"", ""VA"", ""VC"", ""VE"", ""VG"", ""VI"", ""VN"", ""VU"", ""WF"", ""WS"",
    ""YE"", ""YT"", ""YU"", ""ZA"", ""ZM"", ""ZR"", ""ZW""
  ]
}";
    }

}

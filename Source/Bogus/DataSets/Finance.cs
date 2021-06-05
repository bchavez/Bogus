using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bogus.Bson;
using Bogus.Extensions.Extras;

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
      /// The currency code. IE: USD.
      /// </summary>
      public string Code { get; set; }

      /// <summary>
      /// The currency symbol. IE: $
      /// </summary>
      public string Symbol { get; set; }

      public static Currency Default = new Currency { Description = "US Dollar", Code = "USD", Symbol = "$" };
   }

   /// <summary>
   /// Represents an enumeration of all the supported card types.
   /// </summary>
   public class CardType
   {
      internal string Value { get; }

      private CardType(string value)
      {
         this.Value = value;
         All.Add(this);
      }

      /// <summary>
      /// List of all card types.
      /// </summary>
      public static readonly List<CardType> All = new List<CardType>();

      /// <summary>
      /// Visa card number
      /// </summary>
      public static readonly CardType Visa = new CardType("visa");

      /// <summary>
      /// Mastercard card number
      /// </summary>
      public static readonly CardType Mastercard = new CardType("mastercard");

      /// <summary>
      /// Discover card number
      /// </summary>
      public static readonly CardType Discover = new CardType("discover");

      /// <summary>
      /// American Express card number
      /// </summary>
      public static readonly CardType AmericanExpress = new CardType("american_express");

      /// <summary>
      /// Diners Club card number
      /// </summary>
      public static readonly CardType DinersClub = new CardType("diners_club");

      /// <summary>
      /// JCB card number
      /// </summary>
      public static readonly CardType Jcb = new CardType("jcb");

      /// <summary>
      /// Switch card number
      /// </summary>
      public static readonly CardType Switch = new CardType("switch");

      /// <summary>
      /// Solo card number
      /// </summary>
      public static readonly CardType Solo = new CardType("solo");

      /// <summary>
      /// Maestro card number
      /// </summary>
      public static readonly CardType Maestro = new CardType("maestro");

      /// <summary>
      /// Laser card number
      /// </summary>
      public static readonly CardType Laser = new CardType("laser");

      /// <summary>
      /// Instapayment card number
      /// </summary>
      public static readonly CardType Instapayment = new CardType("instapayment");
   }


   /// <summary>
   /// Provides financial randomness.
   /// </summary>
   public class Finance : DataSet
   {
      /// <summary>
      /// Get an account number. Default length is 8 digits.
      /// </summary>
      /// <param name="length">The length of the account number.</param>
      public string Account(int length = 8)
      {
         var template = new string('#', length);
         return Random.Replace(template);
      }

      /// <summary>
      /// Get an account name. Like "savings", "checking", "Home Loan" etc..
      /// </summary>
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
      public decimal Amount(decimal min = 0, decimal max = 1000, int decimals = 2)
      {
         var amount = (max - min);
         var part = (decimal)Random.Double() * amount;
         return Math.Round(min + part, decimals);
      }


      /// <summary>
      /// Get a transaction type: "deposit", "withdrawal", "payment", or "invoice".
      /// </summary>
      public string TransactionType()
      {
         return GetRandomArrayItem("transaction_type");
      }

      /// <summary>
      /// Get a random currency.
      /// </summary>
      public Currency Currency(bool includeFundCodes = false)
      {
         var obj = this.GetRandomBObject("currency");

         var cur = new Currency
         {
            Description = obj["name"],
            Code = obj["code"],
            Symbol = obj["symbol"],
         };

         // GitHub Issue #80:
         // Make sure we exclude currency fund codes by default unless
         // the user wants them. See:
         //https://github.com/bchavez/Bogus/issues/80

         if (cur.Code.Contains(" "))
         {
            // We selected a currency fund code. Check if the user wants it.
            if (includeFundCodes)
            {
               cur.Code = cur.Code.Split(' ')[1];
               return cur;
            }
            //If they don't want fund codes, send back a default USD.
            //instead of selecting again (and possibly looping over and over).
            return DataSets.Currency.Default;
         }

         return cur;
      }

      /// <summary>
      /// Generate a random credit card number with valid Luhn checksum.
      /// </summary>
      /// <param name="provider">The type of credit card to generate (ie: American Express, Discover, etc.). Passing null, a random card provider will be chosen.</param>
      public string CreditCardNumber(CardType provider = null)
      {
         if (provider is null)
         {
            provider = this.Random.ListItem(CardType.All);
         }

         var format = GetRandomArrayItem($"credit_card.{provider.Value}");

         var symbol = '#';
         var expandedFormat = RegexStyleStringParse(format); // replace [4-9] with a random number in range etc...
         var cardNumber = this.Random.ReplaceNumbers(expandedFormat, symbol); // replace ### with random numbers

         var numberList = cardNumber.Where(char.IsDigit)
            .Select(c => int.Parse(c.ToString())).ToList();

         var checkNum = numberList.CheckDigit();
         return cardNumber.Replace("L", checkNum.ToString());

         string RegexStyleStringParse(string str = "")
         {
            // Deal with range repeat `{min,max}`
            var RANGE_REP_REG = new Regex(@"(.)\{(\d+)\,(\d+)\}");
            var REP_REG = new Regex(@"(.)\{(\d+)\}");
            var RANGE_REG = new Regex(@"\[(\d+)\-(\d+)\]");
            int min, max, tmp, repetitions;
            var token = RANGE_REP_REG.Match(str);
            while (token.Success)
            {
               min = Int32.Parse(token.Groups[2].Value);
               max = Int32.Parse(token.Groups[3].Value);

               if (min > max)
               {
                  tmp = max;
                  max = min;
                  min = tmp;
               }

               repetitions = this.Random.Number(min, max);

               str = str.Substring(0, token.Index) +
                     new string(token.Groups[1].Value[0], repetitions) +
                     str.Substring(token.Index + token.Groups[0].Length);

               token = RANGE_REP_REG.Match(str);
            }
            // Deal with repeat `{num}`
            token = REP_REG.Match(str);
            while (token.Success)
            {
               repetitions = Int32.Parse(token.Groups[2].Value);

               str = str.Substring(0, token.Index) +
                     new string(token.Groups[1].Value[0], repetitions) +
                     str.Substring(token.Index + token.Groups[0].Length);

               token = REP_REG.Match(str);
            }
            // Deal with range `[min-max]` (only works with numbers for now)
            //TODO: implement for letters e.g. [0-9a-zA-Z] etc.

            token = RANGE_REG.Match(str);
            while (token.Success)
            {
               min = Int32.Parse(token.Groups[1].Value); // This time we are not capturing the char before `[]`
               max = Int32.Parse(token.Groups[2].Value);
               // switch min and max
               if (min > max)
               {
                  tmp = max;
                  max = min;
                  min = tmp;
               }
               str = str.Substring(0, token.Index) +
                     this.Random.Number(min, max) +
                     str.Substring(token.Index + token.Groups[0].Length);
               token = RANGE_REG.Match(str);
            }
            return str;
         }
      }

      /// <summary>
      /// Generate a credit card CVV.
      /// </summary>
      public string CreditCardCvv()
      {
         return this.Random.Replace("###");
      }

      private static readonly char[] BtcCharset =
         {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k',
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F',
            'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
         };

      /// <summary>
      /// Generates a random Bitcoin address.
      /// </summary>
      public string BitcoinAddress()
      {
         var addressLength = this.Random.Number(25, 34);
         var lastBits = new string(this.Random.ArrayElements(BtcCharset, addressLength));
         if (this.Random.Bool())
         {
            return $"1{lastBits}";
         }
         return $"3{lastBits}";
      }

      /// <summary>
      /// Generate a random Ethereum address.
      /// </summary>
      public string EthereumAddress()
      {
         return Random.Hexadecimal(40);
      }

      /// <summary>
      /// Generate a random Litecoin address.
      /// </summary>
      public string LitecoinAddress()
      {
         var addressLength = this.Random.Number(26, 33);
         var lastBits = new string(this.Random.ArrayElements(BtcCharset, addressLength));
         var prefix = this.Random.Number(0, 2);
         
         if( prefix == 0 )
         {
            return $"L{lastBits}";
         }
         if( prefix == 1 )
         {
            return $"M{lastBits}";
         }
         return $"3{lastBits}";
      }

      /// <summary>
      /// Generates an ABA routing number with valid check digit.
      /// </summary>
      public string RoutingNumber()
      {
         var digits = this.Random.Digits(8);

         var sum = 0;
         for (var i = 0; i < digits.Length; i += 3)
         {
            sum += 3 * digits.ElementAt(i);
            sum += 7 * digits.ElementAt(i + 1);
            sum += digits.ElementAtOrDefault(i + 2);
         }

         var checkDigit = Math.Ceiling(sum / 10d) * 10 - sum;

         return digits.Aggregate("", (str, digit) => str + digit, str => str + checkDigit);
      }

      protected static readonly string[] BicVowels = { "A", "E", "I", "O", "U" };

      /// <summary>
      /// Generates Bank Identifier Code (BIC) code.
      /// </summary>
      public string Bic()
      {
         var prob = this.Random.Number(100);
         return this.Random.Replace("???") +
                this.Random.ArrayElement(BicVowels) +
                this.Random.ArrayElement(IbanIso3166) +
                this.Random.Replace("?") + "1" +
                (prob < 10 ? this.Random.Replace("?" + this.Random.ArrayElement(BicVowels) + "?") : prob < 40 ? this.Random.Replace("###") : "");
      }

      /// <summary>
      /// Generates an International Bank Account Number (IBAN).
      /// </summary>
      /// <param name="formatted">Formatted IBAN containing spaces.</param>
      /// <param name="countryCode">A two letter ISO3166 country code. Throws an exception if the country code is not found or is an invalid length.</param>
      /// <exception cref="KeyNotFoundException">An exception is thrown if the ISO3166 country code is not found.</exception>
      public string Iban(bool formatted = false, string countryCode = null)
      {
         var arr = this.GetArray("iban_formats");

         IBanFormat ibanFormat;
         if( countryCode is null )
         {
            var formatEntry = this.Random.ArrayElement(arr) as BObject;
            ibanFormat = this.GetIbanFormat(formatEntry);
         }
         else
         {
            if( countryCode.Length != 2 )
            {
               throw new ArgumentOutOfRangeException(nameof(countryCode), countryCode.Length, "The country code must be an ISO3166 two-letter country code.");
            }

            var formatEntry = arr.OfType<BObject>()
               .Where(b => countryCode.Equals(b["country"].StringValue, StringComparison.OrdinalIgnoreCase))
               .FirstOrDefault();

            if (formatEntry is null)
            {
               throw new KeyNotFoundException($"The ISO3166 IBAN country code '{countryCode}' was not found.");
            }

            ibanFormat = this.GetIbanFormat(formatEntry);
         }

         return Iban(ibanFormat, formatted);
      }

      protected string Iban(IBanFormat ibanFormat, bool formatted)
      {
         var stringBuilder = new StringBuilder();
         var count = 0;
         for (var b = 0; b < ibanFormat.Bban.Length; b++)
         {
            var bban = ibanFormat.Bban[b];
            var c = bban.Count;
            count += bban.Count;
            while (c > 0)
            {
               if (bban.Type == "a")
               {
                  stringBuilder.Append(this.Random.ArrayElement(IbanAlpha));
               }
               else if (bban.Type == "c")
               {
                  if (this.Random.Number(100) < 80)
                  {
                     stringBuilder.Append(this.Random.Number(9));
                  }
                  else
                  {
                     stringBuilder.Append(this.Random.ArrayElement(IbanAlpha));
                  }
               }
               else
               {
                  if (c >= 3 && this.Random.Number(100) < 30)
                  {
                     if (this.Random.Bool())
                     {
                        stringBuilder.Append(this.Random.ArrayElement(IbanPattern100));
                        c -= 2;
                     }
                     else
                     {
                        stringBuilder.Append(this.Random.ArrayElement(IbanPattern10));
                        c--;
                     }
                  }
                  else
                  {
                     stringBuilder.Append(this.Random.Number(9));
                  }
               }
               c--;
            }

            stringBuilder = stringBuilder.Remove(count, stringBuilder.Length - count);
         }
         var checksum = 98 - IbanMod97(IbanToDigitString(stringBuilder + ibanFormat.Country + "00"));
         var iban = ibanFormat.Country + checksum.ToString("00") + stringBuilder;

         if (formatted)
         {
            var matches = Regex.Matches(iban, ".{1,4}");
            var array = matches.OfType<Match>()
               .Select(m => m.Value)
               .ToArray();
            return string.Join(" ", array);
         }
         return iban;
      }

      protected int IbanMod97(string digitStr)
      {
         var m = 0;
         for (int i = 0; i < digitStr.Length; i++)
         {
            m = ((m * 10) + (digitStr[i] - '0')) % 97;
         }
         return m;
      }

      protected string IbanToDigitString(string str)
      {
         return Regex.Replace(str, "[A-Z]", (m) => (Convert.ToChar(m.Value) - 55).ToString());
      }

      protected class IBanFormat
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

      protected IBanFormat GetIbanFormat(BObject obj)
      {
         var bbitems = GetBbanItems(obj);

         return new IBanFormat
            {
               Country = obj["country"].StringValue,
               Total = obj["total"].Int32Value,
               Format = obj["format"].StringValue,
               Bban = bbitems
            };
      }

      protected IBanFormat.BbanItem[] GetBbanItems(BObject obj)
      {
         var arr = obj["bban"] as BArray;
         return arr.OfType<BObject>()
            .Select(o => new IBanFormat.BbanItem
            {
               Count = o["count"].Int32Value,
               Type = o["type"].StringValue
            })
            .ToArray();
      }

      protected static readonly string[] IbanAlpha =
            {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};

      protected static readonly string[] IbanPattern10 = { "01", "02", "03", "04", "05", "06", "07", "08", "09" };

      protected static readonly string[] IbanPattern100 = { "001", "002", "003", "004", "005", "006", "007", "008", "009" };

      protected static readonly string[] IbanIso3166 =
         {
            "AC", "AD", "AE", "AF", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR", "AS", "AT", "AU", "AW", "AX", "AZ",
            "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BL", "BM", "BN", "BO", "BQ", "BR", "BS", "BT", "BU",
            "BV", "BW", "BY", "BZ", "CA", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "CP",
            "CR", "CS", "CS", "CU", "CV", "CW", "CX", "CY", "CZ", "DD", "DE", "DG", "DJ", "DK", "DM", "DO", "DZ", "EA",
            "EC", "EE", "EG", "EH", "ER", "ES", "ET", "EU", "FI", "FJ", "FK", "FM", "FO", "FR", "FX", "GA", "GB", "GD",
            "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM",
            "HN", "HR", "HT", "HU", "IC", "ID", "IE", "IL", "IM", "IN", "IO", "IQ", "IR", "IS", "IT", "JE", "JM", "JO",
            "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR",
            "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "ME", "MF", "MG", "MH", "MK", "ML", "MM", "MN", "MO", "MP",
            "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ", "NA", "NC", "NE", "NF", "NG", "NI", "NL", "NO",
            "NP", "NR", "NT", "NU", "NZ", "OM", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "PR", "PS", "PT",
            "PW", "PY", "QA", "RE", "RO", "RS", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK",
            "SL", "SM", "SN", "SO", "SR", "SS", "ST", "SU", "SV", "SX", "SY", "SZ", "TA", "TC", "TD", "TF", "TG", "TH",
            "TJ", "TK", "TL", "TM", "TN", "TO", "TR", "TT", "TV", "TW", "TZ", "UA", "UG", "UM", "US", "UY", "UZ", "VA",
            "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "YE", "YT", "YU", "ZA", "ZM", "ZR", "ZW"
         };
   }
}
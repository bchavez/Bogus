using System;
using System.Linq;

namespace Bogus.DataSets
{
   /// <summary>
   /// Generates phone numbers
   /// </summary>
   [DataCategory("phone_number")]
   public class PhoneNumbers : DataSet
   {
      /// <summary>
      /// Default constructor
      /// </summary>
      /// <param name="locale"></param>
      public PhoneNumbers(string locale = "en") : base(locale)
      {
      }

      /// <summary>
      /// Get a phone number. 
      /// </summary>
      /// <param name="format">Format of phone number in any format. Replaces # characters with numbers. IE: '###-###-####' or '(###) ###-####'</param>
      /// <returns></returns>
      public string PhoneNumber(string format = null)
      {
         format = !string.IsNullOrWhiteSpace(format) ? format : PhoneFormat();
         return Random.Replace(ReplaceExclamChar(format));
      }

      /// <summary>
      /// Gets a phone number via format array index as defined in a locale's phone_number.formats[] array.
      /// </summary>
      /// <param name="phoneFormatsArrayIndex"></param>
      /// <returns></returns>
      public string PhoneNumberFormat(int phoneFormatsArrayIndex = 0)
      {
         var formatArray = GetArray("formats");
         var format = (string)formatArray[phoneFormatsArrayIndex];

         return PhoneNumber(format);
      }

      /// <summary>
      /// Gets the format of a phone number.
      /// </summary>
      protected virtual string PhoneFormat()
      {
         return GetRandomArrayItem("formats");
      }

      /// <summary>
      /// Replaces special ! characters in phone number formats. 
      /// </summary>
      protected virtual string ReplaceExclamChar(string s)
      {
         return this.Random.ReplaceSymbols(s, '!', () => Convert.ToChar('0' + this.Random.Number(2, 9)));
      }
   }
}
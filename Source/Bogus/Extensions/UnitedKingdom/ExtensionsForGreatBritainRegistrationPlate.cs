using System;
using System.Collections.Generic;
using System.Text;
using Bogus.DataSets;

namespace Bogus.Extensions.UnitedKingdom
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForGreatBritainRegistrationPlate
   {
      private static readonly DateTime StartOfCurrentStyle = new DateTime(2001, 9, 1);

      private static readonly DateTime EarliestRegistration = StartOfCurrentStyle;
      private static readonly DateTime LatestRegistration = new DateTime(2051, 2, 28);

      private static readonly char[] SequenceLetters = "ABCDEFGHJKLMNOPRSTUVWXYZ".ToCharArray();
      private static readonly char[] PrimaryLocations = "ABCDEFGHKLMNOPRSVWXY".ToCharArray();
      private static readonly Dictionary<char,char[]> SecondaryLocations = new Dictionary<char, char[]>
      {
         { 'A', "ABCDEFGJKMNOPRSTUVWXY".ToCharArray() },    // Anglia
         { 'B', "ABCDEFGHJKLMNOPRSTUVWX".ToCharArray() },   // Birmingham
         { 'C', "ABCDEFGHJKLMNOPRSTUVWX".ToCharArray() },   // Cymru (Wales)
         { 'D', "ABCDEFGHJKLMNOPSTUVWXY".ToCharArray() },   // Deeside
         { 'E', "ABCEFGJKLMNOPRSTUVWXY".ToCharArray() },    // Essex
         { 'F', "ABCDEFGHJKLMNPRSTVWXY".ToCharArray() },    // Forest and Fens
         { 'G', "ABCDEFGHJKLMNPRSTUVWXY".ToCharArray() },   // Garden of England
         { 'H', "ABCDEFGHJKLMNPRSTUVWY".ToCharArray() },    // Hampshire and Dorset
         { 'K', "ABCDEFGHJKLMNOPRSTUVWXY".ToCharArray() },  // Milton Keynes (not official mnemonic)
         { 'L', "ABCDEFGHJKLMNOPRSTUVWXY".ToCharArray() },  // London
         { 'M', "ABCDEFGHJKLMPTUVWX".ToCharArray() },       // Manchester and Merseyside
         { 'N', "ABCDEFGHJKLMNOPRSTUVWXY".ToCharArray() },  // North
         { 'O', "ABCDEFGHJLMOPTUVWXY".ToCharArray() },      // Oxford
         { 'P', "ABCDEFGHJKLMNOPRSTUVWXYZ".ToCharArray() }, // Preston
         { 'R', "ABCDEFGHJKLMNOPRSTVWXY".ToCharArray() },   // Reading
         { 'S', "ABCDEFGHJKLMNOPRSTVWXY".ToCharArray() },   // Scotland
         { 'V', "ABCEFGHJKLMNOPRSTUVXY".ToCharArray() },    // Severn Valley
         { 'W', "ABDEFGHJKLMNOPRSTUVWXY".ToCharArray() },   // West of England
         { 'Y', "ABCDEFGHJKLMNOPRSTUVWXY".ToCharArray() },  // Yorkshire
      };

      /// <summary>
      /// GB Vehicle Registration Plate
      /// </summary>
      /// <param name="vehicle">Object to extend.</param>
      /// <param name="dateFrom">The start of the range of registration dates.</param>
      /// <param name="dateTo">The end of the range of registration dates.</param>
      /// <returns>A string containing a GB registration plate.</returns>
      /// <remarks>
      /// This is based on the information in the Wikipedia article on
      /// Vehicle registration plates of the United Kingdom.
      /// https://en.wikipedia.org/wiki/Vehicle_registration_plates_of_the_United_Kingdom
      /// At present this only handles registration plates in the current
      /// scheme (September 2001 to February 2051).
      /// </remarks>
      public static string GbRegistrationPlate(this Vehicle vehicle, DateTime dateFrom, DateTime dateTo)
      {
         DateTime registrationDate = GenerateRegistrationDate(vehicle, dateFrom, dateTo);
         return GenerateCurrentStylePlates(vehicle, registrationDate);
      }

      private static string GenerateCurrentStylePlates(Vehicle vehicle, DateTime registrationDate)
      {
         StringBuilder sb = new StringBuilder();
         char primaryLocation = vehicle.Random.ArrayElement(PrimaryLocations);
         sb.Append(primaryLocation);

         char secondaryLocation = GetSecondaryLocation(vehicle, primaryLocation, registrationDate);
         sb.Append(secondaryLocation);
         int year = registrationDate.Year - 2000;
         if (registrationDate.Month < 3)
            year += 49;
         else if (registrationDate.Month >= 9)
            year += 50;
         sb.Append($"{year:D2}");

         ApplySN07Exception(sb);

         char[] sequence = vehicle.Random.ArrayElements(SequenceLetters, 3);
         sb.Append(sequence);
         return sb.ToString();
      }

      private static void ApplySN07Exception(StringBuilder sb)
      {
         // The DVLA don't permit plates starting SN07 (i.e. Registered in
         // Edinburgh between 1/Mar/2007 and 31/Aug/2007) because it looks
         // too much like the word "snot".

         sb.Replace("SN07", "TN07");
      }

      private static char GetSecondaryLocation(Vehicle vehicle, char primaryLocation, DateTime registrationDate)
      {
         if (primaryLocation == 'X')
         {
            // Export vehicles have their secondary location marker based on
            // the registration date rather than a specific DVLA office.
            switch (registrationDate.Month)
            {
               case 1:
               case 7:
                  return 'E';
               case 2:
               case 8:
                  return 'F';
               case 3: 
               case 9:
                  return 'A';
               case 4:
               case 10:
                  return 'B';
               case 5:
               case 11:
                  return 'C';
               case 6:
               case 12:
                  return 'D';
            }

            throw new InvalidOperationException(
               $"This code path should never be accessible. {nameof(primaryLocation)}={primaryLocation}; {nameof(registrationDate)}={registrationDate:O}");
         }
         
         char[] secondaryLocationChoices = SecondaryLocations[primaryLocation];
         char secondaryLocation = vehicle.Random.ArrayElement(secondaryLocationChoices);
         return secondaryLocation;
      }

      private static DateTime GenerateRegistrationDate(Vehicle vehicle, DateTime dateFrom, DateTime dateTo)
      {
         if (dateFrom < EarliestRegistration || dateFrom > LatestRegistration)
             throw new ArgumentOutOfRangeException(nameof(dateFrom), $"Can only accept registration dates between {EarliestRegistration:yyyy-MM-dd} and {LatestRegistration:yyyy-MM-dd}.");
         if (dateTo < EarliestRegistration || dateTo > LatestRegistration)
             throw new ArgumentOutOfRangeException(nameof(dateTo),$"Can only accept registration dates between {EarliestRegistration:yyyy-MM-dd} and {LatestRegistration:yyyy-MM-dd}.");
         
         // Swap the values of the to and from dates if they're the wrong way around.
         if (dateFrom > dateTo)
         {
            // Syntax not supported: (dateFrom, dateTo) = (dateTo, dateFrom);
            DateTime valueHolder = dateFrom;
            dateFrom = dateTo;
            dateTo = valueHolder;
         }
         
         dateFrom = dateFrom.Date;
         dateTo = dateTo.Date;
         int duration = (int) (dateTo - dateFrom).TotalDays;
         int offset = vehicle.Random.Int(0, duration);
         DateTime registrationDate = dateFrom.AddDays(offset);
         return registrationDate;
      }
   }
}
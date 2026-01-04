using System;
using System.Collections.Generic;
using System.Text;
using Bogus.DataSets;

namespace Bogus.Extensions.Netherlands;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class VehicleExtensionsForNetherlands
{
   // Dutch kenteken series with their date ranges and formats
   private static readonly DateTime Serie1Start = new(1951, 1, 1);
   private static readonly DateTime Serie2Start = new(1965, 1, 1);
   private static readonly DateTime Serie3Start = new(1973, 1, 1);
   private static readonly DateTime Serie4Start = new(1978, 7, 1);
   private static readonly DateTime Serie5Start = new(1991, 1, 1);
   private static readonly DateTime Serie6Start = new(1999, 1, 1);
   private static readonly DateTime Serie7Start = new(2004, 10, 29);
   private static readonly DateTime Serie8Start = new(2013, 3, 5);
   private static readonly DateTime Serie9Start = new(2015, 3, 30);
   private static readonly DateTime Serie10Start = new(2019, 8, 19);
   private static readonly DateTime Serie11Start = new(2024, 6, 4);
   private static readonly DateTime Serie12Start = new(2024, 1, 8);

   private static readonly DateTime EarliestRegistration = Serie1Start;
   private static readonly DateTime LatestRegistration = new(2030, 12, 31);

   // Letters used in Dutch kentekens (excluding forbidden combinations)
   private static readonly char[] ConsonantLetters = "BCDFGHJKLMNPRSTVWXZ".ToCharArray();
   private static readonly char[] AllLettersExceptKY = "BCDFGHJLMNPRSTVWXZ".ToCharArray(); // For serie 5
   private static readonly char[] AllLettersWithK = "BCDFGHJKLMNPRSTVWXZ".ToCharArray(); // For serie 6+
   
   // Forbidden combinations that should be avoided
   private static readonly HashSet<string> ForbiddenCombinations = new()
   {
      "SA", "SD", "SS", "KL", "GVD", "KKK", "LPF", "NSB", "PKK", "PSV", "PVV", "TBS", "BBB"
   };

   /// <summary>
   /// Dutch Vehicle Registration Plate (Kenteken)
   /// </summary>
   /// <param name="vehicle">Object to extend.</param>
   /// <param name="dateFrom">The start of the range of registration dates.</param>
   /// <param name="dateTo">The end of the range of registration dates.</param>
   /// <returns>A string containing a Dutch registration plate.</returns>
   /// <remarks>
   /// This is based on the information in the Wikipedia article on
   /// Dutch license plates (Nederlands kenteken).
   /// https://nl.wikipedia.org/wiki/Nederlands_kenteken
   /// Supports multiple kenteken series from 1951 to present.
   /// </remarks>
   public static string NlRegistrationPlate(this Vehicle vehicle, DateTime dateFrom, DateTime dateTo)
   {
      DateTime registrationDate = GenerateRegistrationDate(vehicle, dateFrom, dateTo);
      return GenerateKenteken(vehicle, registrationDate);
   }

   private static string GenerateKenteken(Vehicle vehicle, DateTime registrationDate)
   {
      // Determine which serie to use based on registration date
      // Note: Serie 11 starts later than Serie 12, so check it first
      if (registrationDate >= Serie11Start)
         return GenerateSerie11(vehicle); // XXX-99-X
      else if (registrationDate >= Serie12Start)
         return GenerateSerie12(vehicle); // X-99-XXX
      else if (registrationDate >= Serie10Start)
         return GenerateSerie10(vehicle); // X-999-XX
      else if (registrationDate >= Serie9Start)
         return GenerateSerie9(vehicle); // XX-999-X
      else if (registrationDate >= Serie8Start)
         return GenerateSerie8(vehicle); // 9-XXX-99
      else if (registrationDate >= Serie7Start)
         return GenerateSerie7(vehicle); // 99-XXX-9
      else if (registrationDate >= Serie6Start)
         return GenerateSerie6(vehicle); // 99-XX-XX
      else if (registrationDate >= Serie5Start)
         return GenerateSerie5(vehicle); // XX-XX-99
      else if (registrationDate >= Serie4Start)
         return GenerateSerie4(vehicle); // XX-99-XX
      else if (registrationDate >= Serie3Start)
         return GenerateSerie3(vehicle); // 99-XX-99
      else if (registrationDate >= Serie2Start)
         return GenerateSerie2(vehicle); // 99-99-XX
      else
         return GenerateSerie1(vehicle); // XX-99-99
   }

   private static string GenerateSerie1(Vehicle vehicle)
   {
      // XX-99-99 format, started with ND-00-01
      string firstLetter = vehicle.Random.ArrayElement(new[] { "N", "P", "R", "S", "T", "V", "X", "Z" });
      string secondLetter = vehicle.Random.ArrayElement(new[] { "D", "G", "K", "P", "T", "X" }); // Personenauto letters
      string numbers1 = vehicle.Random.Int(0, 99).ToString("D2");
      string numbers2 = vehicle.Random.Int(1, 99).ToString("D2");
      return $"{firstLetter}{secondLetter}-{numbers1}-{numbers2}";
   }

   private static string GenerateSerie2(Vehicle vehicle)
   {
      // 99-99-XX format
      string numbers1 = vehicle.Random.Int(10, 99).ToString("D2");
      string numbers2 = vehicle.Random.Int(1, 99).ToString("D2");
      string letters = GenerateRandomLetterPair(vehicle, ConsonantLetters);
      return $"{numbers1}-{numbers2}-{letters}";
   }

   private static string GenerateSerie3(Vehicle vehicle)
   {
      // 99-XX-99 format
      string numbers1 = vehicle.Random.Int(10, 99).ToString("D2");
      string letters = GenerateRandomLetterPair(vehicle, ConsonantLetters);
      string numbers2 = vehicle.Random.Int(1, 99).ToString("D2");
      return $"{numbers1}-{letters}-{numbers2}";
   }

   private static string GenerateSerie4(Vehicle vehicle)
   {
      // XX-99-XX format
      string letters1 = GenerateRandomLetterPair(vehicle, ConsonantLetters);
      string numbers = vehicle.Random.Int(1, 99).ToString("D2");
      string letters2 = GenerateRandomLetterPair(vehicle, ConsonantLetters);
      return $"{letters1}-{numbers}-{letters2}";
   }

   private static string GenerateSerie5(Vehicle vehicle)
   {
      // XX-XX-99 format (no K or Y)
      string letters1 = GenerateRandomLetterPair(vehicle, AllLettersExceptKY);
      string letters2 = GenerateRandomLetterPair(vehicle, AllLettersExceptKY);
      string numbers = vehicle.Random.Int(1, 99).ToString("D2");
      return $"{letters1}-{letters2}-{numbers}";
   }

   private static string GenerateSerie6(Vehicle vehicle)
   {
      // 99-XX-XX format (K is back except as first letter)
      string numbers = vehicle.Random.Int(1, 99).ToString("D2");
      string letters1 = GenerateRandomLetterPair(vehicle, AllLettersWithK);
      string letters2 = GenerateRandomLetterPair(vehicle, AllLettersWithK);
      return $"{numbers}-{letters1}-{letters2}";
   }

   private static string GenerateSerie7(Vehicle vehicle)
   {
      // 99-XXX-9 format
      string numbers1 = vehicle.Random.Int(0, 99).ToString("D2");
      string letters = GenerateRandomLetterTriple(vehicle, ConsonantLetters);
      string numbers2 = vehicle.Random.Int(1, 9).ToString();
      return $"{numbers1}-{letters}-{numbers2}";
   }

   private static string GenerateSerie8(Vehicle vehicle)
   {
      // 9-XXX-99 format (personenauto's: K, S, T, X, Z)
      string[] validFirstLetters = { "K", "S", "T", "X", "Z" };
      string firstLetter = vehicle.Random.ArrayElement(validFirstLetters);
      string remainingLetters = GenerateRandomLetterPair(vehicle, ConsonantLetters);
      string numbers1 = vehicle.Random.Int(1, 9).ToString();
      string numbers2 = vehicle.Random.Int(0, 99).ToString("D2");
      return $"{numbers1}-{firstLetter}{remainingLetters}-{numbers2}";
   }

   private static string GenerateSerie9(Vehicle vehicle)
   {
      // XX-999-X format
      string letters1 = GenerateRandomLetterPair(vehicle, ConsonantLetters);
      string numbers = vehicle.Random.Int(1, 999).ToString("D3");
      string letter2 = vehicle.Random.ArrayElement(ConsonantLetters).ToString();
      return $"{letters1}-{numbers}-{letter2}";
   }

   private static string GenerateSerie10(Vehicle vehicle)
   {
      // X-999-XX format
      string letter1 = vehicle.Random.ArrayElement(ConsonantLetters).ToString();
      string numbers = vehicle.Random.Int(1, 999).ToString("D3");
      string letters2 = GenerateRandomLetterPair(vehicle, ConsonantLetters);
      return $"{letter1}-{numbers}-{letters2}";
   }

   private static string GenerateSerie11(Vehicle vehicle)
   {
      // XXX-99-X format
      string letters1 = GenerateRandomLetterTriple(vehicle, ConsonantLetters);
      string numbers = vehicle.Random.Int(1, 99).ToString("D2");
      string letter2 = vehicle.Random.ArrayElement(ConsonantLetters).ToString();
      return $"{letters1}-{numbers}-{letter2}";
   }

   private static string GenerateSerie12(Vehicle vehicle)
   {
      // X-99-XXX format
      string letter1 = vehicle.Random.ArrayElement(ConsonantLetters).ToString();
      string numbers = vehicle.Random.Int(1, 99).ToString("D2");
      string letters2 = GenerateRandomLetterTriple(vehicle, ConsonantLetters);
      return $"{letter1}-{numbers}-{letters2}";
   }

   private static string GenerateRandomLetterPair(Vehicle vehicle, char[] availableLetters)
   {
      string result;
      int attempts = 0;
      do
      {
         char letter1 = vehicle.Random.ArrayElement(availableLetters);
         char letter2 = vehicle.Random.ArrayElement(availableLetters);
         result = $"{letter1}{letter2}";
         attempts++;
      } while (ForbiddenCombinations.Contains(result) && attempts < 100);
      
      return result;
   }

   private static string GenerateRandomLetterTriple(Vehicle vehicle, char[] availableLetters)
   {
      string result;
      int attempts = 0;
      do
      {
         char letter1 = vehicle.Random.ArrayElement(availableLetters);
         char letter2 = vehicle.Random.ArrayElement(availableLetters);
         char letter3 = vehicle.Random.ArrayElement(availableLetters);
         result = $"{letter1}{letter2}{letter3}";
         attempts++;
      } while (ContainsForbiddenSubstring(result) && attempts < 100);
      
      return result;
   }

   private static bool ContainsForbiddenSubstring(string letters)
   {
      foreach (string forbidden in ForbiddenCombinations)
      {
         if (letters.Contains(forbidden))
            return true;
      }
      return false;
   }

   private static DateTime GenerateRegistrationDate(Vehicle vehicle, DateTime dateFrom, DateTime dateTo)
   {
      if (dateFrom < EarliestRegistration || dateFrom > LatestRegistration)
          throw new ArgumentOutOfRangeException(nameof(dateFrom), $"Can only accept registration dates between {EarliestRegistration:yyyy-MM-dd} and {LatestRegistration:yyyy-MM-dd}.");
      if (dateTo < EarliestRegistration || dateTo > LatestRegistration)
          throw new ArgumentOutOfRangeException(nameof(dateTo), $"Can only accept registration dates between {EarliestRegistration:yyyy-MM-dd} and {LatestRegistration:yyyy-MM-dd}.");
      
      // Swap the values if they're the wrong way around
      if (dateFrom > dateTo)
      {
         DateTime valueHolder = dateFrom;
         dateFrom = dateTo;
         dateTo = valueHolder;
      }
      
      dateFrom = dateFrom.Date;
      dateTo = dateTo.Date;
      int duration = (int)(dateTo - dateFrom).TotalDays;
      int offset = vehicle.Random.Int(0, duration);
      DateTime registrationDate = dateFrom.AddDays(offset);
      return registrationDate;
   }
}
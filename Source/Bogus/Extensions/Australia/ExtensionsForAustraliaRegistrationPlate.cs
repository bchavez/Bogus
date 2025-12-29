using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bogus.DataSets;

namespace Bogus.Extensions.Australia;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForAustraliaRegistrationPlate
{

   /// <summary>
   /// HashSet of supported Australian states.
   /// </summary>
   private static readonly HashSet<string> SupportedStates = ["NSW", "VIC", "QLD", "WA", "SA", "TAS", "NT", "ACT"];

   /// <summary>
   /// Australian Vehicle Registration Plates
   /// </summary>
   /// <param name="vehicle"></param>
   /// <param name="from">The start of the range of registration dates.</param>
   /// <param name="to">The end of the range of registration dates.</param>
   /// <param name="state">The state abbreviation for which the registration plate is to be generated.</param>
   /// <returns></returns>
   /// <remarks>
   /// This is based on the information in the Wikipedia article on
   /// Vehicle registration plates of Australia.
   /// https://en.wikipedia.org/wiki/Vehicle_registration_plates_of_Australia
   /// Due to each state having its own registration plate format, we also need to provide a State Property on the Vehicle object.
   /// Different vehicle types also have different registration plate formats.
   /// Currently, this method only supports the "Car" or universal type of vehicle registration plates. 
   /// </remarks>
   public static string AusCarRegistrationPlate(this Vehicle vehicle, DateTime from, DateTime to, string state = null)
   {
      state ??= GenerateRandomState(vehicle);
      if (!SupportedStates.Contains(state))
         throw new ArgumentException($"Unsupported Australian state: {state}. Supported states are: {string.Join(", ", SupportedStates)}", nameof(state));
      DateTime registrationDate = GenerateRegistrationDate(vehicle, from, to);
      return GenerateRegistrationPlate(vehicle, registrationDate, state);
   }

   /// <summary>
   /// Represents a rule for formatting vehicle license plates based on a specified year range.
   /// </summary>
   /// <remarks>This class defines the format and logic for generating license plate numbers for vehicles
   /// registered within a specific range of years. The <see cref="Generator"/> property can be used to provide custom
   /// logic for generating the plate format.</remarks>
   private class PlateFormatRule
   {
      public int StartYear { get; set; }
      public int EndYear { get; set; }
      public string Format { get; set; }
      public Func<Vehicle, DateTime, string> Generator { get; set; } // Optional for custom logic
   }

   private static readonly Dictionary<string, List<PlateFormatRule>> StatePlateRules = new()
   {
      ["ACT"] =
      [
         new PlateFormatRule { StartYear = 1911, EndYear = 1968, Format = "NN-NNN" },
         new PlateFormatRule { StartYear = 1969, EndYear = 1998, Format = "YLL-NNN" },
         new PlateFormatRule { StartYear = 1999, EndYear = 9999, Format = "YLL-NNN" }
      ],
      ["NSW"] =
      [
         new PlateFormatRule { StartYear = 1910, EndYear = 1937, Format = "NNN-NNN" },
         new PlateFormatRule { StartYear = 1938, EndYear = 1951, Format = "LL-NNN" },
         new PlateFormatRule { StartYear = 1951, EndYear = 2004, Format = "LLL-NNN" },
         new PlateFormatRule { StartYear = 2005, EndYear = 9999, Format = "LL-NN-LL" }
      ],
      ["NT"] =
      [
         new PlateFormatRule { StartYear = 1931, EndYear = 2011, Format = "NNN-NNN" },
         new PlateFormatRule { StartYear = 2012, EndYear = 9999, Format = "LL-NN-AA" }
      ],
      ["QLD"] =
      [
         new PlateFormatRule { StartYear = 1955, EndYear = 1977, Format = "NNN-LLL" },
         new PlateFormatRule { StartYear = 1978, EndYear = 2019, Format = "LLL-NNN" },
         new PlateFormatRule { StartYear = 2020, EndYear = 9999, Format = "000-NLN" }
      ],
      ["SA"] =
      [
         new PlateFormatRule { StartYear = 1967, EndYear = 2008, Format = "SNNN-LLL" },
         new PlateFormatRule { StartYear = 2009, EndYear = 9999, Format = "SNNN-LLL" }
      ],
      ["TAS"] =
      [
         new PlateFormatRule { StartYear = 1954, EndYear = 1970, Format = "LLL-NNN" },
         new PlateFormatRule { StartYear = 1971, EndYear = 1971, Format = "LL-NNNN" },
         new PlateFormatRule { StartYear = 1972, EndYear = 9999, Format = "M NN LL" }
      ],
      ["VIC"] =
      [
         new PlateFormatRule { StartYear = 1910, EndYear = 1939, Format = "NNN-NNN" },
         new PlateFormatRule { StartYear = 1940, EndYear = 1953, Format = "LL-NNN" },
         new PlateFormatRule { StartYear = 1954, EndYear = 2013, Format = "LLL-NNN" },
         new PlateFormatRule { StartYear = 2014, EndYear = 9999, Format = "NLL-NLL" }
      ],
      ["WA"] =
      [
         new PlateFormatRule { StartYear = 1956, EndYear = 1978, Format = "LLL-NNN" },
         new PlateFormatRule { StartYear = 1979, EndYear = 9999, Format = "NLLL-NNN" }
      ]
   };


   /// <summary>
   /// This method generates a random registration date for a vehicle within a specified date range.
   /// </summary>
   /// <param name="vehicle"></param>
   /// <param name="from"></param>
   /// <param name="to"></param>
   /// <returns></returns>
   private static DateTime GenerateRegistrationDate(Vehicle vehicle, DateTime from, DateTime to)
   {
      // Swap if needed
      if (from > to)
      {
         var temp = from;
         from = to;
         to = temp;
      }
      from = from.Date;
      to = to.Date;
      int duration = (int)(to - from).TotalDays;
      int offset = vehicle.Random.Int(0, duration);
      return from.AddDays(offset);
   }

   private static string GenerateRegistrationPlate(Vehicle vehicle, DateTime registrationDate, string state)
   {
      state = state.ToUpperInvariant();
      if (!StatePlateRules.ContainsKey(state))
         throw new ArgumentException($"Unsupported Australian state: {state}", nameof(state));
      int year = registrationDate.Year;
      var rules = StatePlateRules[state];
      var rule = rules.FirstOrDefault(r => year >= r.StartYear && year <= r.EndYear);
      if (rule == null)
         throw new ArgumentException($"No plate format for {state} in year {year}");
      return rule.Generator != null ? rule.Generator(vehicle, registrationDate) : GeneratePlateFromFormat(vehicle, rule.Format);
   }

   private static readonly char[] Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
   private static readonly char[] Digits = "0123456789".ToCharArray();

   private static string GeneratePlateFromFormat(Vehicle vehicle, string format)
   {
      var sb = new StringBuilder();
      foreach (var c in format)
      {
         switch (c)
         {
            case 'L': sb.Append(vehicle.Random.ArrayElement(Letters)); break;
            case 'N': sb.Append(vehicle.Random.ArrayElement(Digits)); break;
            case '0': sb.Append(vehicle.Random.Number(1)); break;
            case 'M': sb.Append('M'); break;
            case 'S': sb.Append('S'); break;
            case 'Y': sb.Append('Y'); break;
            case '-': sb.Append('-'); break;
            default: sb.Append(c); break;
         }
      }
      return sb.ToString();
   }

   private static string GenerateRandomState(Vehicle vehicle)
   {
      return vehicle.Random.ListItem(SupportedStates.ToList());
   }
}
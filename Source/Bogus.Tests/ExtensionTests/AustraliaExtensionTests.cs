using System;
using System.Collections.Generic;
using Bogus.DataSets;
using Bogus.Extensions.Australia;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.ExtensionTests
{
   public class AustraliaExtensionTests(ITestOutputHelper console) : SeededTest
   {
      public static IEnumerable<object[]> PlateFormatTestData()
      {
         // Each tuple: state, year, expected format regex
         yield return ["ACT", 1970, @"^Y[A-Z]{2}-\d{3}$"];        
         yield return ["ACT", 2000, @"^Y[A-Z]{2}-\d{3}$"];         
         yield return ["NSW", 1955, @"^[A-Z]{3}-\d{3}$"];          
         yield return ["NSW", 2010, @"^[A-Z]{2}-\d{2}-[A-Z]{2}$"]; 
         yield return ["NT", 2000, @"^\d{3}-\d{3}$"];              
         yield return ["NT", 2015, @"^[A-Z]{2}-\d{2}-[A-Z]{2}$"];  
         yield return ["QLD", 1960, @"^\d{3}-[A-Z]{3}$"];          
         yield return ["QLD", 2010, @"^[A-Z]{3}-\d{3}$"];          
         yield return ["QLD", 2022, "^[01]{3}-[0-9][A-Z][0-9]$"];          
         yield return ["SA", 1970, @"^S\d{3}-[A-Z]{3}$"];          
         yield return ["SA", 2015, @"^S\d{3}-[A-Z]{3}$"];          
         yield return ["TAS", 1960, @"^[A-Z]{3}-\d{3}$"];          
         yield return ["TAS", 1970, @"^[A-Z]{3}-[0-9]{3}$"];          
         yield return ["TAS", 2015, @"^M \d{2} [A-Z]{2}$"];        
         yield return ["VIC", 1960, @"^[A-Z]{3}-\d{3}$"];          
         yield return ["VIC", 2015, @"^\d[A-Z]{2}-\d[A-Z]{2}$"];   
         yield return ["WA", 1960, @"^[A-Z]{3}-\d{3}$"];           
         yield return ["WA", 2015, @"^\d[A-Z]{3}-\d{3}$"];       
      }

      [Theory]
      [MemberData(nameof(PlateFormatTestData))]
      public void Generates_Correct_Plate_Format_For_State_And_Year(string state, int year, string expectedRegex)
      {
         var vehicle = new Vehicle();
         var from = new DateTime(year, 1, 1);
         var to = new DateTime(year, 12, 31);
         var plate = vehicle.AusCarRegistrationPlate(from, to, state);
         console.WriteLine($"{state} {year}: {plate}");
         Assert.Matches(expectedRegex, plate);
      }
   }
}

using System;

namespace Bogus.Extensions.Vietnam;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForVietnam
{
   /// <summary>
   /// The Citizen Identity Card Number (Căn Cước Công Dân - CCCD)
   /// is a unique 12-digit number registered to each Vietnamese citizen.
   /// </summary>
   /// <param name="p">The citizen owning the Citizen Identity Card Number</param>
   /// <returns>The generated Citizen Identity Card Number</returns>
   /// <exception cref="ArgumentException">Thrown when the city or year of birth is not supported.</exception>
   /// <seealso href="https://vi.wikipedia.org/wiki/C%C4%83n_c%C6%B0%E1%BB%9Bc_c%C3%B4ng_d%C3%A2n_(Vi%E1%BB%87t_Nam)"/>
   public static string Cccd(this Person p)
   {
      /*
         https://vi.wikipedia.org/wiki/C%C4%83n_c%C6%B0%E1%BB%9Bc_c%C3%B4ng_d%C3%A2n_(Vi%E1%BB%87t_Nam)
         CCC.G.BB.RRRRRR
         |   | |  |
         |   | |  |
         |   | |  |------> (R) Random number from 000001 to 999999.
         |   | |---------> (B) Birth year code of the citizen (last two digits of the birth year).
         |   |-----------> (G) Gender code based on the century of birth (20th century: Male 0, Female 1; 21st century: Male 2, Female 3; ...).
         |---------------> (C) Code of the province, city where the citizen is registered at birth.
      */

      const string key = nameof(ExtensionsForVietnam) + nameof(Cccd);
      if (p.context.TryGetValue(key, out var cccdNumber) )
      {
         return cccdNumber as string;
      }

      var randomNumber = p.Random.Number(1, 999999).ToString("D6");

      var birthYearCode = p.DateOfBirth.Year.ToString().Substring(2);

      var genderCode = p.DateOfBirth.Year switch
      {
         (>= 1900 and <= 1999) => (int)p.Gender,          // 20th Century: Male = 0, Female = 1
         (>= 2000 and <= 2099) => (int)p.Gender + 2,      // 21st Century: Male = 2, Female = 3
         (>= 2100 and <= 2199) => (int)p.Gender + 4,      // 22nd Century: Male = 4, Female = 5
         _ => throw new ArgumentException($"Year of birth {p.DateOfBirth.Year} is not supported.")
      };

      if (!VietnameseCityCodes.CityCodes.TryGetValue(p.Address.City, out var provinceCityCode))
      { 
         throw new ArgumentException($"City {p.Address.City} is not supported.");
      }

      var generatedNumber = $"{provinceCityCode}{genderCode}{birthYearCode}{randomNumber}";
      p.context[key] = generatedNumber;
      return generatedNumber;
   }
}
using Bogus.DataSets;

namespace Bogus.Extensions.Finland;

/// <summary>
/// API extensions specific for a geographical location.
/// </summary>
public static class ExtensionsForFinland
{
   /// <summary>
   /// Finnish Henkilötunnus
   /// </summary>
   public static string Henkilotunnus(this Person p)
   {
      const string Key = nameof(ExtensionsForFinland) + "Henkilötunnus";
      if( p.context.ContainsKey(Key) )
      {
         return p.context[Key] as string;
      }

      // DDMMYYCZZZQ
      //
      // DD
      // MM
      // YY - DOB
      // C - Century
      // ZZZ odd for males, even for females
      // Numbers 900-999 are only for temporary use, ie. in hospitals.
      // Q = The control character is calculated as the remainder of DDMMYYZZZ
      // divided by 31, i.e. drop the century sign and divide the resulting nine
      // digit number by 31. For remainders below ten, the remainder itself is
      // the control character, otherwise pick the corresponding character from
      // string "0123456789ABCDEFHJKLMNPRSTUVWXY". For example, 311280888 divided by 31
      // gives the remainder as 30, and since A=10, B=11, etc. ending up with Y=30.
      // Note that letters G, I, O, and Q are skipped.

      const string controlCharTable = "0123456789ABCDEFHJKLMNPRSTUVWXY";
      var r = p.Random;

      var year = p.DateOfBirth.Year;

      var c = "A";
      if( year >= 1800 && year <= 1899 )
         c = "+";
      else if( year >= 1900 && year <= 1999 )
         c = "-";

      var z = r.Int(2, 449) * 2;
      if( p.Gender == Name.Gender.Male )
         z++;

      var zzz = z.ToString("D3");

      var ddMMyy = $"{p.DateOfBirth:ddMMyy}";

      var n = int.Parse($"{ddMMyy}{zzz}") % 31;

      var q = controlCharTable[n].ToString();

      var final = $"{ddMMyy}{c}{zzz}{q}";

      p.context[Key] = final;
      return final;
   }
}
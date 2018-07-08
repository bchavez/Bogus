namespace Bogus.Extensions.Finland
{
   /// <summary>
   /// API extensions specific for a geographical location.
   /// </summary>
   public static class ExtensionsForFinland
   {
      /// <summary>
      /// Finnish Henkilötunnus
      /// </summary>
      public static string Henkilötunnus(this Person p)
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
         // Q = The control character is calculated as the remainder of DDMMYYZZZ
         // divided by 31, i.e. drop the century sign and divide the resulting nine
         // digit number by 31. For remainders below ten, the remainder itself is
         // the control character, otherwise pick the corresponding character from
         // string "0123456789ABCDEFHJKLMNPRSTUVWXY". For example, 311280888 divided by 31
         // gives the remainder as 30, and since A=10, B=11, etc. ending up with Y=30.

         var r = p.Random;

         var year = p.DateOfBirth.Year;

         var c = "A";
         if( year >= 1800 && year <= 1899 )
            c = "+";
         else if( year >= 1900 && year <= 1999 )
            c = "-";

         var ddMMyy = $"{p.DateOfBirth:ddMMyy}";

         var n = int.Parse(ddMMyy) % 31;

         var q = n.ToString();
         if( n >= 10 )
            q = ((char)('A' + (n - 10))).ToString();

         //no idea if its female or male.
         var zzz = r.Replace("###");

         var final = $"{ddMMyy}{c}{zzz}{q}";

         p.context[Key] = final;
         return final;
      }
   }
}
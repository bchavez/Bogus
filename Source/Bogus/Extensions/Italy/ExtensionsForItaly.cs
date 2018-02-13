using Bogus.DataSets;
using System;

namespace Bogus.Extensions.Italy
{
   /// <summary>
   /// Italian class extensions
   /// </summary>
   public static class ExtensionsForItaly
   {
      /// <summary>
      ///   Codice Fiscale
      /// </summary>
      /// <param name="p">The holder</param>
      /// <param name="validChecksum">
      ///   Indicates whether the generated Fiscal Code has a valid checksum or not
      /// </param>
      /// <returns>The generated Fiscal Code</returns>
      public static string CodiceFiscale(this Person p, bool validChecksum = true)
      {
         return CodiceFiscaleGenerator.Generate(
            p.LastName,
            p.FirstName,
            p.DateOfBirth,
            p.Gender == Name.Gender.Male,
            validChecksum);
      }

      /// <summary>
      ///   Codice Fiscale
      /// </summary>
      /// <param name="finance">An instance of the extended Finance class</param>
      /// <param name="lastName">Last name of the holder</param>
      /// <param name="firstName">First name of the holder</param>
      /// <param name="birthday">Birthday of the holder</param>
      /// <param name="isMale">Indicates whether the holder is male</param>
      /// <param name="validChecksum">
      ///   Indicates whether the generated Fiscal Code has a valid checksum or not
      /// </param>
      /// <returns>The generated Fiscal Code</returns>
      public static string CodiceFiscale(
         this Finance finance,
         string lastName,
         string firstName,
         DateTime birthday,
         bool isMale,
         bool validChecksum = true)
      {
         return CodiceFiscaleGenerator.Generate(
            lastName,
            firstName,
            birthday,
            isMale,
            validChecksum);
      }
   }
}
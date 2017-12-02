using Bogus.DataSets;
using System;

namespace Bogus.Extensions
{
   /// <summary>
   /// Italian class extensions
   /// </summary>
   public static class ExtensionsForItaly
   {
      /// <summary>
      ///   Extends the Person class with Italian Fiscal Code
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
      ///   Extends the Finance data set with Italian Fiscal Code and more
      ///   fine grained parameters
      /// </summary>
      /// <param name="finance">An instance of the extended Finance class</param>
      /// <param name="lastName">Lastname of the holder</param>
      /// <param name="firstName">Firstname of the holder</param>
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
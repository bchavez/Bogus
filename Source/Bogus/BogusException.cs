using System;

namespace Bogus
{
   /// <summary>
   /// General exception for Bogus.
   /// </summary>
   public class BogusException : Exception
   {
      public BogusException()
      {
      }

      public BogusException(string message) : base(message)
      {
      }

      public BogusException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}
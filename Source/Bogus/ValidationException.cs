using System;

namespace Bogus
{
   /// <summary>
   /// Represents a validation exception.
   /// </summary>
   public class ValidationException : Exception
   {
      public ValidationException(string message) : base(message)
      {
      }
   }
}
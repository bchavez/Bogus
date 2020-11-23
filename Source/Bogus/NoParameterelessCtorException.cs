using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bogus
{
   /// <summary>
   /// Used when 
   /// </summary>
   public class NoParameterlessCtorException<T> : Exception where T : class
   {
      public NoParameterlessCtorException(bool isHiddenConstructor,MissingMethodException innerException = null) 
         : base($"Could not find {(isHiddenConstructor ? "hidden" : "public")} parameterless constructor in {typeof(T).Name}. Consider using {nameof(Faker<T>.SkipConstructor)}(), {nameof(Faker<T>.UseConstructor)}() or provide {(isHiddenConstructor ? "hidden" : "public")} parameterless contructor in {typeof(T).Name}", innerException)
      {

      }

   }
}

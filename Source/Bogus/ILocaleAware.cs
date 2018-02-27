using System.Collections.Generic;

namespace Bogus
{
   /// <summary>
   /// Marker interface for datasets that are locale aware.
   /// </summary>
   public interface ILocaleAware
   {
      /// <summary>
      /// The current locale for the dataset.
      /// </summary>
      string Locale { get; set; }
   }

   /// <summary>
   /// Marker interface for objects that have a context storage property.
   /// </summary>
   public interface IHasContext
   {
      Dictionary<string, object> Context { get; }
   }
}
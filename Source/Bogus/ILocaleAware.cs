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
}
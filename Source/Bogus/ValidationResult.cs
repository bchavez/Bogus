using System.Collections.Generic;

namespace Bogus
{
   /// <summary>
   /// Contains validation results after validation
   /// </summary>
   public class ValidationResult
   {
      /// <summary>
      /// True if is valid
      /// </summary>
      internal bool IsValid { get; set; }

      /// <summary>
      /// A complete list of missing rules
      /// </summary>
      internal List<string> MissingRules { get; } = new List<string>();

      /// <summary>
      /// Extra validation messages to display
      /// </summary>
      internal List<string> ExtraMessages { get; } = new List<string>();
   }
}
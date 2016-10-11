using System.Collections.Generic;

namespace Bogus
{
  /// <summary>
  /// Contains valdation results after validation
  /// </summary>
  internal class ValidationResult
  {
    /// <summary>
    /// True if is valid
    /// </summary>
    internal bool IsValid { get; set; }
    /// <summary>
    /// A complete list of missing rules
    /// </summary>
    internal List<string> MissingRules { get; } = new List<string>();
  }
}
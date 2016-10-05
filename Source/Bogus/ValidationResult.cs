using System.Collections.Generic;

namespace Bogus
{
    internal class ValidationResult
    {
        internal bool IsValid { get; set; }
        internal List<string> MissingRules { get; } = new List<string>();
    }
}
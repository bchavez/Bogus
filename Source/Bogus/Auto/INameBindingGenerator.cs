using System;
using System.Collections.Generic;

namespace Bogus.Auto
{
    public interface INameBindingGenerator
        : IConditionalConvention
    {
        Type Type { get; }
        IEnumerable<string> Names { get; }
    }
}

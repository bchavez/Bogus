using System;

namespace Bogus.Auto
{
    public interface ITypeBindingGenerator
        : IConditionalConvention
    {
        Type Type { get; }
    }
}

using System.Linq;

namespace Bogus.Generators
{
    internal sealed class ArrayGenerator<TType>
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            var items = context.GenerateMany<TType>(context);
            return items.ToArray();
        }
    }
}
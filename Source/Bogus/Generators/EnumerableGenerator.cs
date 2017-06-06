namespace Bogus.Generators
{
    internal sealed class EnumerableGenerator<TType>
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.GenerateMany<TType>(context);
        }
    }
}
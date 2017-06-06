namespace Bogus.Generators
{
    internal sealed class NullableGenerator<TType>
        : IAutoGenerator
        where TType : struct
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Generate<TType>(context);
        }
    }
}
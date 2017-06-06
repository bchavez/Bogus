namespace Bogus.Generators
{
    internal sealed class EnumGenerator<TType>
        : IAutoGenerator
        where TType : struct
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Enum<TType>();
        }
    }
}
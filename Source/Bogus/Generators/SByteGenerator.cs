namespace Bogus.Generators
{
    internal sealed class SByteGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.SByte();
        }
    }
}
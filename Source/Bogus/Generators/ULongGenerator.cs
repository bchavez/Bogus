namespace Bogus.Generators
{
    internal sealed class ULongGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.ULong();
        }
    }
}
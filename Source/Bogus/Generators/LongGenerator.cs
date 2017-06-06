namespace Bogus.Generators
{
    internal sealed class LongGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Long();
        }
    }
}
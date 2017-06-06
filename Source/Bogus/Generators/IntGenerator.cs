namespace Bogus.Generators
{
    internal sealed class IntGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Int();
        }
    }
}
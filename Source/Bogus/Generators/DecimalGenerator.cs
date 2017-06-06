namespace Bogus.Generators
{
    internal sealed class DecimalGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Decimal();
        }
    }
}
namespace Bogus.Generators
{
    internal sealed class ShortGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Short();
        }
    }
}
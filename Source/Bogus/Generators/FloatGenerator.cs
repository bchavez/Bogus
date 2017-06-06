namespace Bogus.Generators
{
    internal sealed class FloatGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Float();
        }
    }
}
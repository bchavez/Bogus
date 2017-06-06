namespace Bogus.Generators
{
    internal sealed class BoolGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Bool();
        }
    }
}
namespace Bogus.Generators
{
    internal sealed class CharGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Char();
        }
    }
}
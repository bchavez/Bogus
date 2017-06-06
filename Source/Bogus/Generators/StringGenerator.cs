namespace Bogus.Generators
{
    internal sealed class StringGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.Word();
        }
    }
}
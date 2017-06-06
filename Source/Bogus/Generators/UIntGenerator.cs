namespace Bogus.Generators
{
    internal sealed class UIntGenerator
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            return context.Faker.Random.UInt();
        }
    }
}
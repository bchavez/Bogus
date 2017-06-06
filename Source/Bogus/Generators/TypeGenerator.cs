namespace Bogus.Generators
{
    internal sealed class TypeGenerator<TType>
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            // Note that all instances are converted to object to cater for boxing and struct population
            // When setting a value via reflection on a struct a copy is made
            // This means the changes are applied to a different instance to the one created here
            object instance = context.Binder.CreateInstance<TType>(context);

            // Populate the generated instance
            context.Binder.PopulateInstance<TType>(instance, context);

            return instance;
        }
    }
}
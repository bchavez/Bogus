namespace Bogus.Auto
{
    internal sealed class RecursionGuard
        : IConvention
    {
        void IConvention.Invoke(GenerateContext context)
        {
            var parent = context.Binding.Parent;

            // Drill through the binding tree and check nothing matches the current binding type
            while (parent != null)
            {
                if (parent.Type == context.Binding.Type)
                {
                    throw new GenerateException($"Unable to create type '{context.Binding.Type.FullName}' due to a circular reference. A rule should be configured for '{context.Binding.Name}'.", context.Binding);
                }

                parent = parent.Parent;
            }
        }
    }
}

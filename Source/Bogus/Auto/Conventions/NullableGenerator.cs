using System;

namespace Bogus.Auto
{
    internal sealed class NullableGenerator
        : IConditionalConvention
    {
        private static readonly Type NullableDefinition = typeof(Nullable<>);

        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.Value == null && binding.HasGenericDefinition(NullableDefinition);
        }

        void IConvention.Invoke(GenerateContext context)
        {
            // Get the inner generic type and generate a value for it
            var type = context.Binding.GetGenericArgument(0);
            var binding = new BindingInfo(type, context.Binding.Name, context.Binding.Parent);

            context.Binding.Value = context.Generate(binding);
        }
    }
}

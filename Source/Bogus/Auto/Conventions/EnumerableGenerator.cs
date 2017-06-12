namespace Bogus.Auto
{
    internal sealed class EnumerableGenerator
        : IConditionalConvention
    {
        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.Value == null && binding.HasGenericDefinition(BindingInfo.EnumerableDefinition);
        }

        void IConvention.Invoke(GenerateContext context)
        {
            var type = context.Binding.GetGenericArgument(0);
            var binding = new BindingInfo(type, context.Binding.Name, context.Binding.Parent);

            context.Binding.Value = context.GenerateMany(binding);
        }
    }
}

using System.Linq;

namespace Bogus.Auto
{
    internal sealed class ArrayGenerator
        : IConditionalConvention
    {
        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.Value == null && binding.Type.IsArray;
        }

        void IConvention.Invoke(GenerateContext context)
        {
            var type = context.Binding.Type.GetElementType();
            var binding = new BindingInfo(type, context.Binding.Name, context.Binding.Parent);
            
            context.Binding.Value = context.GenerateMany(binding);
        }
    }
}

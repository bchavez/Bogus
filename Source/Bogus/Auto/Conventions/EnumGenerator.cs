using Bogus.Extensions;

namespace Bogus.Auto
{
    internal sealed class EnumGenerator
        : GenericValueTypeConvention
    {
        protected override bool CanInvoke<TType>(BindingInfo binding)
        {
            return binding.IsNotBound(binding.Type) && binding.Type.IsEnum();
        }

        protected override void Invoke<TType>(GenerateContext context)
        {
            context.Binding.Value = context.FakerHub.Random.Enum<TType>();
        }
    }
}

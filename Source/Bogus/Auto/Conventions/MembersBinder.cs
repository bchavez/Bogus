using Bogus.Extensions;
using System.Linq;

namespace Bogus.Auto
{
    internal sealed class MembersBinder
        : IConditionalConvention
    {
        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.Value != null &&
                  !binding.Type.IsArray &&
                  !binding.Type.IsEnum() &&
                  !binding.HasGenericDefinition(BindingInfo.DictionaryDefinition) &&
                  !binding.HasGenericDefinition(BindingInfo.EnumerableDefinition) &&
                  !ConventionsRegistry.TypeBindingGenerators.Any(g => g.Type == binding.Type);
        }

        void IConvention.Invoke(GenerateContext context)
        {
            var members = context.Binder.GetMembers(context.Binding.Type);

            foreach (var member in members.Values)
            {
                // Generate the member value and bind it
                var binding = new BindingInfo(member, context.Binding);
                var value = context.Generate(binding);

                binding.Bind(context.Binding.Value, value);
            }
        }
    }
}

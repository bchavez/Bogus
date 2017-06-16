using Bogus.Extensions;
using System.Collections.Generic;
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
            members = members ?? new Dictionary<string, System.Reflection.MemberInfo>();

            foreach (var member in members.Values)
            {
                // Generate the member value and bind it
                var binding = new BindingInfo(context.Binding, member);
                var value = context.Generate(binding);

                binding.Bind(value);
            }
        }
    }
}

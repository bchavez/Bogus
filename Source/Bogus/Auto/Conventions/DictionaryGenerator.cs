using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bogus.Auto
{
    internal sealed class DictionaryGenerator
        : IConditionalConvention
    {
        private static readonly Type DictionaryDefinition = typeof(Dictionary<,>);

        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.Value == null && binding.HasGenericDefinition(BindingInfo.DictionaryDefinition);
        }

        void IConvention.Invoke(GenerateContext context)
        {
            var keyType = context.Binding.GetGenericArgument(0);
            var valueType = context.Binding.GetGenericArgument(1);
            var dictionaryType = DictionaryDefinition.MakeGenericType(keyType, valueType);
            var dictionary = Activator.CreateInstance(dictionaryType);
            var add = dictionaryType.GetMethod("Add");

            // Generate a set of keys and then generate a matching value for each key
            var binding = new BindingInfo(keyType, context.Binding.Name, context.Binding.Parent);
            var keys = context.GenerateMany(binding);

            foreach (var key in keys)
            {
                binding = new BindingInfo(valueType, context.Binding.Name, context.Binding.Parent);

                // Generate a value and create the dictionary item
                var value = context.Generate(binding);
                add.Invoke(dictionary, new[] { key, value });
            }

            context.Binding.Value = dictionary;
        }
    }
}

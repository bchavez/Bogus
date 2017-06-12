using Bogus.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Bogus.Auto
{
    internal static class BindingInfoExtensions
    {
        internal static object Default(this BindingInfo binding)
        {
            // We can create an instance if it is a value type, otherwise the default is null
            if (binding.Type.IsValueType())
            {
                return Activator.CreateInstance(binding.Type);
            }

            return null;
        }

        internal static bool IsNotBound(this BindingInfo binding, Type type)
        {
            var defaultValue = binding.Default();

            return binding.Type == type &&
                  (binding.Value == null || binding.Value.Equals(defaultValue));
        }

        internal static Type GetGenericArgument(this BindingInfo binding, int index)
        {
            if (binding.Type.IsGenericType())
            {
                var types = binding.Type.GetGenericArguments();
                return types.ElementAt(index);
            }

            return null;
        }

        internal static bool HasGenericDefinition(this BindingInfo binding, Type definition)
        {
            return binding.Type.IsGenericType() &&
                   binding.Type.GetGenericTypeDefinition() == definition;
        }
    }
}

using Bogus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bogus.Auto
{
    internal sealed class TypeActivator
        : IConditionalConvention
    {
        private static readonly Type EnumerableType = typeof(IEnumerable);
        private static readonly Type DictionaryType = typeof(IDictionary);

        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.Value == null &&
                  !binding.Type.IsInterface() &&
                  !binding.Type.IsAbstract();
        }

        void IConvention.Invoke(GenerateContext context)
        {
            var constructor = ResolveConstructor(context.Binding);

            if (constructor != null)
            {
                // If a constructor is found generate values for each of the parameters
                var parameters = new List<object>();

                foreach (var parameter in constructor.GetParameters())
                {
                    var binding = new BindingInfo(parameter.ParameterType, parameter.Name, context.Binding);
                    var value = context.Generate(binding);

                    parameters.Add(value);
                }

                context.Binding.Value = constructor.Invoke(parameters.ToArray());
            }
            else
            {
                context.Binding.Value = context.Binding.Default();
            }
        }

        private ConstructorInfo ResolveConstructor(BindingInfo binding)
        {
            var constructors = binding.Type.GetConstructors();

            // Locate a constructor that is used for populating as well
            if (DictionaryType.IsAssignableFrom(binding.Type))
            {
                return ResolveTypedConstructor(BindingInfo.DictionaryDefinition, constructors);
            }

            if (EnumerableType.IsAssignableFrom(binding.Type))
            {
                return ResolveTypedConstructor(BindingInfo.EnumerableDefinition, constructors);
            }

            // Attempt to find a default constructor
            // If one is not found, simply use the first in the list
            var defaultConstructor = (from c in constructors
                                      let p = c.GetParameters()
                                      where p.Count() == 0
                                      select c).SingleOrDefault();

            return defaultConstructor ?? constructors.FirstOrDefault();
        }

        private ConstructorInfo ResolveTypedConstructor(Type type, IEnumerable<ConstructorInfo> constructors)
        {
            // Find the first constructor that matches the passed generic definition
            return (from c in constructors
                    let p = c.GetParameters()
                    where p.Count() == 1
                    let m = p.Single()
                    where m.ParameterType.IsGenericType()
                    let d = m.ParameterType.GetGenericTypeDefinition()
                    where d == type
                    select c).SingleOrDefault();
        }
    }
}

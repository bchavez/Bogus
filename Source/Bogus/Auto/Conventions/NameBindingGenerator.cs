using System;
using System.Collections.Generic;
using System.Linq;

namespace Bogus.Auto
{
    internal sealed class NameBindingGenerator<TType>
        : INameBindingGenerator
    {
        internal NameBindingGenerator(Func<GenerateContext, TType> factory, params string[] names)
        {
            Factory = factory;
            Names = names;
        }

        public Type Type => typeof(TType);
        public IEnumerable<string> Names { get; }

        private Func<GenerateContext, TType> Factory { get; }

        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.IsNotBound(Type) &&
                   Names.Any(n => binding.Name.Equals(n, StringComparison.OrdinalIgnoreCase));
        }

        void IConvention.Invoke(GenerateContext context)
        {
            context.Binding.Value = Factory.Invoke(context);
        }

        public override string ToString()
        {
            var names = string.Join(", ", Names);
            return string.Concat("NameBindingGenerator: Type=", Type.Name, " Names=", names);
        }
    }
}

using System;

namespace Bogus.Auto
{
    internal sealed class TypeBindingGenerator<TType>
        : ITypeBindingGenerator
    {
        internal TypeBindingGenerator(Func<GenerateContext, TType> factory)
        {
            Factory = factory;
        }

        public Type Type => typeof(TType);

        private Func<GenerateContext, TType> Factory { get; }

        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return binding.IsNotBound(Type);
        }

        void IConvention.Invoke(GenerateContext context)
        {
            context.Binding.Value = Factory.Invoke(context);
        }

        public override string ToString()
        {
            return string.Concat("TypeBindingGenerator: Type=", Type.Name);
        }
    }
}

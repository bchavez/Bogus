using Bogus.Extensions;
using System.Reflection;

namespace Bogus.Auto
{
    public abstract class GenericValueTypeConvention
        : GenericConventionBase
    {
        private static readonly MethodInfo CanInvokeMethod;
        private static readonly MethodInfo InvokeMethod;

        static GenericValueTypeConvention()
        {
            CanInvokeMethod = GetMethod<GenericValueTypeConvention>("CanInvoke");
            InvokeMethod = GetMethod<GenericValueTypeConvention>("Invoke");
        }

        protected GenericValueTypeConvention()
            : base(CanInvokeMethod, InvokeMethod)
        { }

        protected abstract bool CanInvoke<TType>(BindingInfo binding)
            where TType : struct;

        protected abstract void Invoke<TType>(GenerateContext context)
            where TType : struct;

        internal override bool IsValidGenericBinding(BindingInfo binding)
        {
            return binding.Type.IsValueType();
        }
    }
}

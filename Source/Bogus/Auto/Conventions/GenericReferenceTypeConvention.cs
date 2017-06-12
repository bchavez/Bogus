using Bogus.Extensions;
using System.Reflection;

namespace Bogus.Auto
{
    public abstract class GenericReferenceTypeConvention
        : GenericConventionBase
    {
        private static readonly MethodInfo CanInvokeMethod;
        private static readonly MethodInfo InvokeMethod;

        static GenericReferenceTypeConvention()
        {
            CanInvokeMethod = GetMethod<GenericReferenceTypeConvention>("CanInvoke");
            InvokeMethod = GetMethod<GenericReferenceTypeConvention>("Invoke");
        }

        protected GenericReferenceTypeConvention()
            : base(CanInvokeMethod, InvokeMethod)
        { }

        protected abstract bool CanInvoke<TType>(BindingInfo binding)
            where TType : class;

        protected abstract void Invoke<TType>(GenerateContext context)
            where TType : class;

        internal override bool IsValidGenericBinding(BindingInfo binding)
        {
            return binding.Type.IsClass();
        }
    }
}

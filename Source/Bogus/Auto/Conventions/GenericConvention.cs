using System.Reflection;

namespace Bogus.Auto
{
    public abstract class GenericConvention
        : GenericConventionBase
    {
        private static readonly MethodInfo CanInvokeMethod;
        private static readonly MethodInfo InvokeMethod;

        static GenericConvention()
        {
            CanInvokeMethod = GetMethod<GenericConvention>("CanInvoke");
            InvokeMethod = GetMethod<GenericConvention>("Invoke");
        }

        protected GenericConvention()
            : base(CanInvokeMethod, InvokeMethod)
        { }

        protected abstract bool CanInvoke<TType>(BindingInfo binding);
        protected abstract void Invoke<TType>(GenerateContext context);
    }
}

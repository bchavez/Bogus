using System;
using System.Linq;
using System.Reflection;

namespace Bogus.Auto
{
    public abstract class GenericConventionBase
        : IConditionalConvention
    {
        internal GenericConventionBase(MethodInfo canInvoke, MethodInfo invoke)
        {
            CanInvokeMethod = canInvoke;
            InvokeMethod = invoke;
        }

        private MethodInfo CanInvokeMethod { get; }
        private MethodInfo InvokeMethod { get; }

        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            if (IsValidGenericBinding(binding))
            {
                var invoke = InvokeGenericMethod(binding.Type, CanInvokeMethod, binding);
                return Convert.ToBoolean(invoke);
            }

            return false;
        }

        void IConvention.Invoke(GenerateContext context)
        {
            InvokeGenericMethod(context.Binding.Type, InvokeMethod, context);
        }

        internal virtual bool IsValidGenericBinding(BindingInfo binding)
        {
            return true;
        }

        internal static MethodInfo GetMethod<TType>(string name)
        {
            return (from m in typeof(TType).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                    where m.Name == name
                    where m.IsAbstract
                    select m).Single();
        }

        private object InvokeGenericMethod(Type type, MethodInfo method, params object[] parameters)
        {
            method = method.MakeGenericMethod(type);
            return method.Invoke(this, parameters);
        }
    }
}

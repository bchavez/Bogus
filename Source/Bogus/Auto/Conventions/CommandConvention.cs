using System;

namespace Bogus.Auto
{
    internal sealed class CommandConvention
        : IConditionalConvention
    {
        internal CommandConvention(string name, Func<BindingInfo, bool> predicate, Action<GenerateContext> action)
        {
            if (name != null && string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be white space.", nameof(name));
            }

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        internal string Name { get; }

        private Func<BindingInfo, bool> Predicate { get; }
        private Action<GenerateContext> Action { get; }

        bool IConditionalConvention.CanInvoke(BindingInfo binding)
        {
            return Predicate.Invoke(binding);
        }

        void IConvention.Invoke(GenerateContext context)
        {
            Action.Invoke(context);
        }

        public override string ToString()
        {
            return string.Concat("CommandConvention: Name=", Name);
        }
    }
}

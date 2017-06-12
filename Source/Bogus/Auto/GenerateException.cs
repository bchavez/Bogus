using System;

namespace Bogus.Auto
{
    public sealed class GenerateException
        : Exception
    {
        internal GenerateException(string message, BindingInfo binding)
            : base(message)
        {
            Binding = binding;
        }

        public BindingInfo Binding { get; }
    }
}

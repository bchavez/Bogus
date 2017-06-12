using System;
using System.Collections.Generic;

namespace Bogus.Auto
{
    internal sealed class Generator
        : IGenerator
    {
        internal Generator(int count, IBinder binder, Faker fakerHub, IEnumerable<IConvention> conventions)
        {
            Count = count;
            Binder = binder;
            FakerHub = fakerHub;
            Conventions = conventions;
        }

        private int Count { get; }
        private IBinder Binder { get; }
        private Faker FakerHub { get; }
        private IEnumerable<IConvention> Conventions { get; }

        void IGenerator.Generate(BindingInfo binding)
        {
            if (binding == null)
            {
                throw new ArgumentNullException(nameof(binding));
            }

            // Simply apply the registered conventions to generate the binding value
            foreach (var convention in Conventions)
            {
                // Check if the convention is conditional and can be invoked
                if (convention is IConditionalConvention conditional && !conditional.CanInvoke(binding))
                {
                    continue;
                }

                // Invoke the convention and set the binding value
                var context = new GenerateContext(Count, this, Binder, binding, FakerHub);
                convention.Invoke(context);
            }
        }
    }
}

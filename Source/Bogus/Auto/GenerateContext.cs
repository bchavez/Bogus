using System.Collections.Generic;

namespace Bogus.Auto
{
    public sealed class GenerateContext
    {
        internal GenerateContext(int count, IGenerator generator, IEnumerable<IConvention> conventions, IBinder binder, BindingInfo binding, Faker fakerHub)
        {
            Count = count;
            Generator = generator;
            Conventions = conventions;
            Binder = binder;
            Binding = binding;
            FakerHub = fakerHub;
            Continuation = GenerateContinuation.Continue;
        }

        internal int Count { get; }
        internal IGenerator Generator { get; }
        internal IEnumerable<IConvention> Conventions { get; }

        public IBinder Binder { get; }
        public BindingInfo Binding { get; }
        public Faker FakerHub { get; }

        public GenerateContinuation Continuation { get; set; }

        public override string ToString()
        {
            return Binding.ToString();
        }
    }
}

namespace Bogus.Auto
{
    public sealed class GenerateContext
    {
        internal GenerateContext(int count, IGenerator generator, IBinder binder, BindingInfo binding, Faker fakerHub)
        {
            Count = count;
            Generator = generator;
            Binder = binder;
            Binding = binding;
            FakerHub = fakerHub;
        }

        internal int Count { get; }
        internal IGenerator Generator { get; }

        public IBinder Binder { get; }
        public BindingInfo Binding { get; }
        public Faker FakerHub { get; }

        public override string ToString()
        {
            return Binding.ToString();
        }
    }
}

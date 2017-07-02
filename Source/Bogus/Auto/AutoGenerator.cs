using System;
using System.Collections.Generic;
using System.Linq;

namespace Bogus.Auto
{
    public sealed class AutoGenerator
        : IGenerator
    {
        private const int DefaultCount = 3;

        public AutoGenerator(string locale) 
            : this(null, locale)
        { }

        public AutoGenerator(IBinder binder)
            : this(binder, null)
        { }

        public AutoGenerator(IBinder binder = null, string locale = "en")
        {
            Binder = binder ?? new Binder();
            FakerHub = new Faker(locale ?? "en");
            Conventions = new Conventions();
        }

        private IBinder Binder { get; }
        private Faker FakerHub { get; }

        public Conventions Conventions { get; }

        public TType Generate<TType>()
        {
            var binding = CreateBinding<TType>();

            // Run the binding through the generate pipeline to create an instance
            var context = CreateContext(Binder, binding, FakerHub);
            Generate(context);

            return (TType)binding.Value;
        }

        public IEnumerable<TType> Generate<TType>(int count)
        {
            var binding = CreateBinding<TType>();

            // Run the binding through the generate pipeline to create the requested number of instances
            var context = CreateContext(Binder, binding, FakerHub);
            Generate(count, context);

            return binding.Value as IEnumerable<TType> ?? Enumerable.Empty<TType>();
        }

        public TType Generate<TType>(Faker<TType> faker)
            where TType : class
        {
            // Create the binding and initialize it with a potential faker instance
            var binding = CreateBinding<TType>();
            InitBinding(binding, faker);

            // Run the binding through the generate pipeline to create an instance
            var context = CreateContext(faker.binder, binding, faker.FakerHub);
            Generate(context);

            return binding.Value as TType;
        }

        public IEnumerable<TType> Generate<TType>(int count, Faker<TType> faker)
            where TType : class
        {
            // Create the binding and initialize it with a potential faker instance
            var binding = CreateBinding<TType>();
            InitBinding(binding, faker);

            // Run the binding through the generate pipeline to create the requested number of instances
            var context = CreateContext(faker.binder, binding, faker.FakerHub);
            Generate(count, context);

            return binding.Value as IEnumerable<TType> ?? Enumerable.Empty<TType>();
        }

        void IGenerator.Generate(GenerateContext context)
        {
            Generate(context);
        }

        private BindingInfo CreateBinding<TType>()
        {
            var type = typeof(TType);
            return new BindingInfo(type, type.Name);
        }

        private void InitBinding<TType>(BindingInfo binding, Faker<TType> faker)
            where TType : class
        {
            if (faker == null)
            {
                throw new ArgumentNullException(nameof(faker));
            }

            // Check if the faker has specified a create action
            if (faker.CreateActions.ContainsKey(Faker.DefaultRuleSetName))
            {
                var action = faker.CreateActions[Faker.DefaultRuleSetName];
                binding.Value = action.Invoke(faker.FakerHub);
            }
        }

        private GenerateContext CreateContext(IBinder binder, BindingInfo binding, Faker fakerHub)
        {
            var builder = new ConventionsBuilder();
            var conventions = builder.Build(Conventions);

            return new GenerateContext(DefaultCount, this, conventions, binder, binding, fakerHub);
        }

        private void Generate(GenerateContext context)
        {
            foreach (var convention in context.Conventions)
            {
                // Check if the convention is conditional and can be invoked
                if (convention is IConditionalConvention conditional && !conditional.CanInvoke(context.Binding))
                {
                    continue;
                }

                // Invoke the convention so the binding value can be set
                convention.Invoke(context);

                // If the continuation is flagged as break, don't process any more conventions for this cycle
                if (context.Continuation != GenerateContinuation.Break)
                {
                    break;
                }
            }
        }

        private void Generate(int count, GenerateContext context)
        {
            // Construct the requested number of items and assign to the binding value
            context.Binding.Value = context.GenerateMany(count, context.Binding);
        }
    }
}

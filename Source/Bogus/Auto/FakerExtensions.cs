using System;
using System.Collections.Generic;
using System.Linq;

namespace Bogus.Auto
{
    public static class FakerExtensions
    {
        private const int DefaultCount = 3;

        public static TType AutoGenerate<TType>(this Faker<TType> faker, Action<ConventionsBuilder> configure = null)
            where TType : class
        {
            if (faker == null)
            {
                throw new ArgumentNullException(nameof(faker));
            }

            // Generate a single instance of the requested type
            var generator = CreateGenerator(faker, configure);
            var binding = CreateBinding(faker);

            return Generate(generator, faker, binding);
        }

        public static IEnumerable<TType> AutoGenerateMany<TType>(this Faker<TType> faker, Action<ConventionsBuilder> configure = null)
            where TType : class
        {
            return faker.AutoGenerateMany(DefaultCount, configure);
        }

        public static IEnumerable<TType> AutoGenerateMany<TType>(this Faker<TType> faker, int count, Action<ConventionsBuilder> configure = null)
            where TType : class
        {
            if (faker == null)
            {
                throw new ArgumentNullException(nameof(faker));
            }

            // Generate a list of instances for the requested type
            var items = new List<TType>();
            var generator = CreateGenerator(faker, configure);

            foreach (var index in Enumerable.Range(0, count))
            {
                var binding = CreateBinding(faker);
                var item = Generate(generator, faker, binding);

                items.Add(item);
            }

            return items;
        }

        private static IGenerator CreateGenerator<TType>(Faker<TType> faker, Action<ConventionsBuilder> configure)
            where TType : class
        {
            // Allow the conventions to be configured
            var builder = new ConventionsBuilder();
            configure?.Invoke(builder);

            // Then build the conventions list and create the generator
            var conventions = builder.Build();
            return new Generator(DefaultCount, faker.binder, faker.FakerHub, conventions);
        }

        private static BindingInfo CreateBinding<TType>(Faker<TType> faker)
            where TType : class
        {
            var type = typeof(TType);
            var binding = new BindingInfo(type, type.Name);

            // Check if the faker has specified a create action
            if (faker.CreateActions.ContainsKey(Faker.DefaultRuleSetName))
            {
                var action = faker.CreateActions[Faker.DefaultRuleSetName];
                binding.Value = action.Invoke(faker.FakerHub);
            }

            return binding;
        }

        private static TType Generate<TType>(IGenerator generator, Faker<TType> faker, BindingInfo binding)
            where TType : class
        {
            // Invoke the generator to build the instance
            generator.Generate(binding);

            // Call the faker populate to ensure the configured rules are applied
            // This is done after the auto generation to ensure the rules are not overwritten
            var instance = (TType)binding.Value;
            faker.Populate(instance);

            return instance;
        }
    }
}

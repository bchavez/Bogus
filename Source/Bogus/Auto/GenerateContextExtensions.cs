using System;
using System.Linq;

namespace Bogus.Auto
{
    public static class GenerateContextExtensions
    {
        public static object Generate(this GenerateContext context, BindingInfo binding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (binding == null)
            {
                throw new ArgumentNullException(nameof(binding));
            }

            // Create a new binding to ensure the original is not overwritten
            binding = new BindingInfo(binding.Type, binding.Name, binding.Parent);

            // Generate and return the binding value
            context.Generator.Generate(binding);
            return binding.Value;
        }

        public static Array GenerateMany(this GenerateContext context, BindingInfo binding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.GenerateMany(context.Count, binding);
        }

        public static Array GenerateMany(this GenerateContext context, int count, BindingInfo binding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (binding == null)
            {
                throw new ArgumentNullException(nameof(binding));
            }

            // Generate a value and add it to the return list
            var items = Array.CreateInstance(binding.Type, count);

            foreach (var index in Enumerable.Range(0, count))
            {
                var item = context.Generate(binding);
                items.SetValue(item, index);
            }

            return items;
        }
    }
}

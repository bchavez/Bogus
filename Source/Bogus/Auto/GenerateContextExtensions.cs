using System;
using System.Linq;

namespace Bogus.Auto
{
    internal static class GenerateContextExtensions
    {
        internal static object Generate(this GenerateContext context, BindingInfo binding)
        {
            // Create a new binding to ensure the original is not overwritten
            binding = new BindingInfo(binding.Type, binding.Name, binding.Parent);

            // Generate and return the binding value
            context = new GenerateContext(context.Count, context.Generator, context.Conventions, context.Binder, binding, context.FakerHub);
            context.Generator.Generate(context);

            return binding.Value;
        }

        internal static Array GenerateMany(this GenerateContext context, BindingInfo binding)
        {
            return context.GenerateMany(context.Count, binding);
        }

        internal static Array GenerateMany(this GenerateContext context, int count, BindingInfo binding)
        {
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

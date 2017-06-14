using System;

namespace Bogus.Auto
{
    public static class ConventionsExtensions
    {
        public static void Add(this Conventions conventions, string name, Action<GenerateContext> action, ConventionPipeline where = ConventionPipeline.Default)
        {
            conventions.Add(name, b => true, action, where);
        }

        public static void Add(this Conventions conventions, string name, Func<BindingInfo, bool> predicate, Action<GenerateContext> action, ConventionPipeline where = ConventionPipeline.Default)
        {
            var convention = new CommandConvention(name, predicate, action);
            conventions.Add(convention, where);
        }

        public static ConventionsBuilder Add(this ConventionsBuilder builder, string name, Action<GenerateContext> action, ConventionPipeline where = ConventionPipeline.Default)
        {
            return builder.Add(name, b => true, action, where);
        }

        public static ConventionsBuilder Add(this ConventionsBuilder builder, string name, Func<BindingInfo, bool> predicate, Action<GenerateContext> action, ConventionPipeline where = ConventionPipeline.Default)
        {
            var convention = new CommandConvention(name, predicate, action);
            return builder.Add(convention, where);
        }
    }
}

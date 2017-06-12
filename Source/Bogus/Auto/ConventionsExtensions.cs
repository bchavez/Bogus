using System;

namespace Bogus.Auto
{
    public static class ConventionsExtensions
    {
        public static void Add(this Conventions conventions, string name, Action<GenerateContext> action)
        {
            conventions.Add(name, b => true, action);
        }

        public static void Add(this Conventions conventions, string name, Func<BindingInfo, bool> predicate, Action<GenerateContext> action)
        {
            var convention = new CommandConvention(name, predicate, action);
            conventions.Add(convention);
        }

        public static ConventionsBuilder Add(this ConventionsBuilder builder, string name, Action<GenerateContext> action)
        {
            return builder.Add(name, b => true, action);
        }

        public static ConventionsBuilder Add(this ConventionsBuilder builder, string name, Func<BindingInfo, bool> predicate, Action<GenerateContext> action)
        {
            var convention = new CommandConvention(name, predicate, action);
            return builder.Add(convention);
        }
    }
}

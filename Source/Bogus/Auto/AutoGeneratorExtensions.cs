using System;
using System.Linq.Expressions;

namespace Bogus.Auto
{
    public static class AutoGeneratorExtensions
    {
        public static AutoGenerator UsingConvention(this AutoGenerator generator, IConvention conventions, ConventionPipeline where = ConventionPipeline.Default)
        {
            return generator;
        }

        public static AutoGenerator RuleFor<TType, TMember>(this AutoGenerator generator, Expression<Func<TType, TMember>> member, Func<GenerateContext, TMember> factory, ConventionPipeline where = ConventionPipeline.Default)
        {
            return generator;
        }

        public static AutoGenerator RuleForType<TType>(this AutoGenerator generator, Func<GenerateContext, TType> factory, ConventionPipeline where = ConventionPipeline.Default)
        {
            return generator;
        }
    }
}

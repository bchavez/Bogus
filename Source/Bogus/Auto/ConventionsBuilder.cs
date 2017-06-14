using System;
using System.Collections.Generic;
using System.Linq;

namespace Bogus.Auto
{
    public sealed class ConventionsBuilder
    {
        internal ConventionsBuilder()
        {
            Conventions = new Conventions();
            ConventionGroups = Enum.GetValues(typeof(ConventionGroup)).Cast<ConventionGroup>();
            SkipFilters = new List<Func<IConvention, bool>>();
        }

        private Conventions Conventions { get; }
        private IEnumerable<ConventionGroup> ConventionGroups { get; set; }
        private IList<Func<IConvention, bool>> SkipFilters { get; }

        public ConventionsBuilder Add(IConvention convention, ConventionPipeline where = ConventionPipeline.Default)
        {
            Conventions.Add(convention, where);
            return this;
        }

        public ConventionsBuilder Skip<TConvention>()
            where TConvention : IConvention
        {
            SkipFilters.Add(c => c.GetType() == typeof(TConvention));
            return this;
        }

        public ConventionsBuilder Skip(string name)
        {
            SkipFilters.Add(c =>
            {
                var convention = c as CommandConvention;
                return convention != null && convention.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
            });

            return this;
        }

        public ConventionsBuilder Skip(params ConventionGroup[] groups)
        {
            if (groups == null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            ConventionGroups = ConventionGroups.Where(t => !groups.Contains(t));
            return this;
        }

        internal IEnumerable<IConvention> Build()
        {
            var conventions = new List<IConvention>();

            // Add the conventions needed at the start of the pipeline
            Build(Conventions, ConventionPipeline.Start, conventions);

            if (ConventionGroups.Contains(ConventionGroup.Global))
            {
                Build(GlobalConventions.Conventions, ConventionPipeline.Start, conventions);
            }

            // Then include those registered for as a default placement
            Build(Conventions, ConventionPipeline.Default, conventions);

            if (ConventionGroups.Contains(ConventionGroup.Global))
            {
                Build(GlobalConventions.Conventions, ConventionPipeline.Default, conventions);
            }

            // Register the default type and name bound generators
            conventions.Add(new RecursionGuard());

            if (ConventionGroups.Contains(ConventionGroup.NameBinding))
            {
                conventions.AddRange(ConventionsRegistry.NameBindingGenerators);
            }

            conventions.AddRange(ConventionsRegistry.TypeBindingGenerators);

            conventions.Add(new ArrayGenerator());
            conventions.Add(new EnumGenerator());
            conventions.Add(new DictionaryGenerator());
            conventions.Add(new EnumerableGenerator());
            conventions.Add(new NullableGenerator());

            conventions.Add(new TypeActivator());
            conventions.Add(new MembersBinder());

            // Finally, add any ending conventions
            Build(Conventions, ConventionPipeline.End, conventions);

            if (ConventionGroups.Contains(ConventionGroup.Global))
            {
                Build(GlobalConventions.Conventions, ConventionPipeline.End, conventions);
            }

            // Filter any skipped conventions
            foreach (var filter in SkipFilters)
            {
                conventions = (from c in conventions
                               where !filter.Invoke(c)
                               select c).ToList();
            }            

            return conventions;
        }

        private void Build(Conventions source, ConventionPipeline @where, List<IConvention> target)
        {
            target.AddRange(from p in source.Pipeline
                            where p.Where == @where
                            select p.Convention);
        }
    }
}

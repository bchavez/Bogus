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

        public ConventionsBuilder Add(IConvention convention)
        {
            Conventions.Add(convention);
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
            var conventions = new List<IConvention>(Conventions._conventions);

            // Append the global conventions if not ignored
            if (ConventionGroups.Contains(ConventionGroup.Global))
            {
                conventions.AddRange(GlobalConventions.Conventions._conventions);
            }

            // Filter any skipped conventions
            foreach (var filter in SkipFilters)
            {
                conventions = (from c in conventions
                               where !filter.Invoke(c)
                               select c).ToList();
            }

            // Add a check that ensures recursive generation doesn't happen
            conventions.Add(new RecursionGuard());

            // Add the conventions used to generate values by name binding
            if (ConventionGroups.Contains(ConventionGroup.NameBinding))
            {
                conventions.AddRange(ConventionsRegistry.NameBindingGenerators);
            }

            // Finally add all the fall back type generators and binders
            conventions.AddRange(ConventionsRegistry.TypeBindingGenerators);

            conventions.Add(new ArrayGenerator());
            conventions.Add(new EnumGenerator());
            conventions.Add(new DictionaryGenerator());
            conventions.Add(new EnumerableGenerator());
            conventions.Add(new NullableGenerator());

            conventions.Add(new TypeActivator());
            conventions.Add(new MembersBinder());

            return conventions;
        }
    }
}

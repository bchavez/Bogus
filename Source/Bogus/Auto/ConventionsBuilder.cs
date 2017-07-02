using System.Collections.Generic;
using System.Linq;

namespace Bogus.Auto
{
    internal sealed class ConventionsBuilder
    {
        internal IEnumerable<IConvention> Build(Conventions source)
        {
            var conventions = new List<IConvention>();

            // Add the conventions needed at the start of the pipeline
            Build(source, ConventionPipeline.Start, conventions);

            // Then include those registered for as a default placement
            Build(source, ConventionPipeline.Default, conventions);
            
            // Register the default type and name bound generators
            conventions.Add(new RecursionGuard());

            conventions.AddRange(ConventionsRegistry.NameBindingGenerators);
            conventions.AddRange(ConventionsRegistry.TypeBindingGenerators);

            conventions.Add(new ArrayGenerator());
            conventions.Add(new EnumGenerator());
            conventions.Add(new DictionaryGenerator());
            conventions.Add(new EnumerableGenerator());
            conventions.Add(new NullableGenerator());

            conventions.Add(new TypeActivator());
            conventions.Add(new MembersBinder());

            // Finally, add any ending conventions
            Build(source, ConventionPipeline.End, conventions);

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

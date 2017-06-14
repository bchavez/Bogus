using System;

namespace Bogus.Auto
{
    internal sealed class ConventionPipelineItem
    {
        internal ConventionPipelineItem(IConvention convention, ConventionPipeline where)
        {
            Convention = convention ?? throw new ArgumentNullException(nameof(convention));
            Where = where;
        }

        internal IConvention Convention { get; }
        internal ConventionPipeline Where { get; }
    }
}

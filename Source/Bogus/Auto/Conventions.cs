using System.Collections.Generic;

namespace Bogus.Auto
{
    public sealed class Conventions
    {
        private IList<ConventionPipelineItem> _pipeline;

        internal Conventions()
        {
            _pipeline = new List<ConventionPipelineItem>();
        }

        internal IEnumerable<ConventionPipelineItem> Pipeline => _pipeline;

        public void Add(IConvention convention, ConventionPipeline where = ConventionPipeline.Default)
        {
            var item = new ConventionPipelineItem(convention, where);
            _pipeline.Add(item);
        }

        public void Clear()
        {
            _pipeline.Clear();
        }
    }
}

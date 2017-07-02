using System.Collections.Generic;
using System.Linq;

namespace Bogus.Auto
{
    public sealed class Conventions
    {
        private IList<ConventionPipelineItem> _pipeline;

        internal Conventions()
        {
            _pipeline = new List<ConventionPipelineItem>();
        }

        internal IEnumerable<ConventionPipelineItem> Pipeline
        {
            get { return _pipeline; }
            set { _pipeline = new List<ConventionPipelineItem>(value ?? Enumerable.Empty<ConventionPipelineItem>()); }
        }

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

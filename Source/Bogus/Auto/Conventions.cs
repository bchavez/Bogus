using System;
using System.Collections.Generic;

namespace Bogus.Auto
{
    public sealed class Conventions
    {
        internal IList<IConvention> _conventions;

        internal Conventions()
        {
            _conventions = new List<IConvention>();
        }

        public void Add(IConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException(nameof(convention));
            }

            _conventions.Add(convention);
        }
    }
}

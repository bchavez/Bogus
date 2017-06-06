using System.Collections.Generic;

namespace Bogus.Generators
{
    internal sealed class DictionaryGenerator<TKey, TValue>
        : IAutoGenerator
    {
        object IAutoGenerator.Generate(AutoGenerateContext context)
        {
            var items = new Dictionary<TKey, TValue>();

            // Get a list of keys
            var keys = context.GenerateMany<TKey>(context);

            foreach( var key in keys )
            {
                // Get a matching value for the current key and add to the dictionary
                var value = context.Generate<TValue>(context);

                if( value != null )
                {
                    items.Add(key, value);
                }
            }

            return items;
        }
    }
}
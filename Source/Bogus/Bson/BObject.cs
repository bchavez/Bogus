#pragma warning disable 1591

using System.Collections;
using System.Collections.Generic;

namespace Bogus.Bson
{
   public class BObject : BValue, IEnumerable
   {
      private Dictionary<string, BValue> map = new Dictionary<string, BValue>();

      public BObject() : base(BValueType.Object)
      {
      }

      public ICollection<string> Keys => map.Keys;

      public ICollection<BValue> Values => map.Values;

      public int Count => map.Count;

      public override BValue this[string key]
      {
         get
         {
            map.TryGetValue(key, out BValue val);
            return val;
         }
         set => map[key] = value;
      }

      public override void Clear() => map.Clear();

      public override void Add(string key, BValue value) => map.Add(key, value);


      public override bool Contains(BValue v) => map.ContainsValue(v);

      public override bool ContainsKey(string key) => map.ContainsKey(key);

      public bool Remove(string key) => map.Remove(key);

      public bool TryGetValue(string key, out BValue value) => map.TryGetValue(key, out value);

      IEnumerator IEnumerable.GetEnumerator()
      {
         return map.GetEnumerator();
      }
   }
}
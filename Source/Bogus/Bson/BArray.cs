#pragma warning disable 1591

using System.Collections;
using System.Collections.Generic;

namespace Bogus.Bson
{
   public class BArray : BValue, IEnumerable
   {
      private readonly List<BValue> items = new List<BValue>();

      public BArray() : base(BValueType.Array)
      {
      }

      public override BValue this[int index]
      {
         get => items[index];
         set => items[index] = value;
      }

      public bool HasValues => items.Count > 0;

      public int Count => items.Count;

      public override void Add(BValue v) => items.Add(v);

      public int IndexOf(BValue item) => items.IndexOf(item);

      public void Insert(int index, BValue item) => items.Insert(index, item);

      public bool Remove(BValue v) => items.Remove(v);

      public void RemoveAt(int index) => items.RemoveAt(index);

      public override void Clear() => items.Clear();

      public virtual bool Contains(BValue v) => items.Contains(v);

      IEnumerator IEnumerable.GetEnumerator()
      {
         return items.GetEnumerator();
      }
   }
}
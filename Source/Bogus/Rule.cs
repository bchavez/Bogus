using System;
using System.Collections.Generic;

namespace Bogus
{
   /// <summary>
   /// Represents a Faker rule
   /// </summary>
   public class Rule<T>
   {
      /// <summary>
      /// Populate action
      /// </summary>
      public T Action { get; set; }

      /// <summary>
      /// Property name, maybe null for finalize or create.
      /// </summary>
      public string PropertyName { get; set; }

      /// <summary>
      /// The rule set this rule belongs to.
      /// </summary>
      public string RuleSet { get; set; } = string.Empty;

      /// <summary>
      /// Prohibits the rule from being applied in strict mode.
      /// </summary>
      public bool ProhibitInStrictMode { get; set; } = false;
   }

   public class PopulateAction<T> : Rule<Func<Faker, T, object>>
   {
   }

   public class FinalizeAction<T> : Rule<Action<Faker, T>>
   {
   }

   public class MultiDictionary<Key, Key2, Value> : Dictionary<Key, Dictionary<Key2, Value>>
   {
      public MultiDictionary(IEqualityComparer<Key> comparer) : base(comparer)
      {
      }

      public void Add(Key key, Key2 key2, Value value)
      {
         if( !this.TryGetValue(key, out var values) )
         {
            values = new Dictionary<Key2, Value>();
            this.Add(key, values);
         }
         values[key2] = value;
      }
   }

   public class MultiSetDictionary<Key, Value> : Dictionary<Key, HashSet<Value>>
   {
      public MultiSetDictionary(IEqualityComparer<Key> comparer) : base(comparer)
      {
      }

      public void Add(Key key, Value value)
      {
         if( !this.TryGetValue(key, out var values) )
         {
            values = new HashSet<Value>();
            this.Add(key, values);
         }
         if( values.Contains(value) )
            throw new ArgumentException("An item with the same key has already been added.");
         values.Add(value);
      }
   }
}
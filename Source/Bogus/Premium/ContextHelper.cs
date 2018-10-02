#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;

namespace Bogus.Premium
{
   public static class ContextHelper
   {
      public static T GetOrSet<T>(string key, Faker f, Func<T> factory) where T : DataSet
      {
         var context = (f as IHasContext).Context;

         if( context.TryGetValue(key, out var t) )
         {
            return t as T;
         }

         var dataset = factory();
         var notifier = (f as IHasRandomizer).GetNotifier();
         notifier.Flow(dataset);

         context[key] = dataset;
         return dataset;
      }

      public static T GetOrSet<T>(Faker f, Func<T> factory) where T : DataSet
      {
         var key = typeof(T).Name.ToLowerInvariant();
         return GetOrSet($"__{key}", f, factory);
      }
   }
}
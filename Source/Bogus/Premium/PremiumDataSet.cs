using System;

namespace Bogus.Premium
{
   /// <summary>
   /// Root object for premium data sets.
   /// </summary>
   public abstract class PremiumDataSet : DataSet
   {
      public override string GetRandomArrayItem(string path, int? min = null, int? max = null)
      {
         CheckLicense();
         return base.GetRandomArrayItem(path, min, max);
      }

      /// <summary>
      /// Called to check the license state
      /// </summary>
      protected abstract void CheckLicense();
   }

   public static class ContextHelper
   {
      public static T GetOrSet<T>(string key, Faker f, Func<T> factory) where T : DataSet, new()
      {
         var context = (f as IHasContext).Context;

         if( context.TryGetValue(key, out var t) )
         {
            return t as T;
         }

         var dataset = factory();
         var notifier = (f as IHasNotifier).GetNotifier();
         notifier.Flow(dataset);

         context[key] = dataset;
         return dataset;
      }

      public static T GetOrSet<T>(Faker f, Func<T> factory) where T : DataSet, new()
      {
         var key = typeof(T).Name.ToLower();
         return GetOrSet($"__{key}", f, factory);
      }
   }
}
using System;
using System.Reflection;

namespace Bogus.Extensions
{
   public static class ExtensionsForPropertyInfo
   {
      private static readonly MethodInfo GenericSetterCreationMethod = 
         typeof(ExtensionsForPropertyInfo).GetMethod(nameof(CreateSetterGeneric), BindingFlags.Static | BindingFlags.NonPublic);

      public static Action<T, object> CreateSetter<T>(this PropertyInfo property)
      {
         if (property == null) throw new ArgumentNullException(nameof(property));

         var setter = property.GetSetMethod(true);
         if (setter == null) throw new ArgumentException($"The specified property '{property.Name}' does not have a setter method.");

         var genericHelper = GenericSetterCreationMethod.MakeGenericMethod(property.DeclaringType, property.PropertyType);
         return (Action<T, object>)genericHelper.Invoke(null, new object[] { setter });
      }

      private static Action<T, object> CreateSetterGeneric<T, V>(MethodInfo setter) where T : class
      {
         var setterTypedDelegate = 
#if STANDARD
               (Action<T, V>) setter.CreateDelegate(typeof(Action<T, V>))
#else
               (Action<T, V>) Delegate.CreateDelegate(typeof(Action<T, V>), setter)
#endif
         ;
         var setterDelegate = (Action<T, object>)((T instance, object value) => { setterTypedDelegate(instance, (V)value); });
         return setterDelegate;
      } 

   }
}
using System;
using System.IO;
using System.Reflection;

namespace Bogus.Platform
{
   public static class ExtensionsForType
   {
      public static T GetCustomAttributeX<T>(this Type type) where T : Attribute
      {
#if   STANDARD20
         return type.GetCustomAttribute<T>();
#elif STANDARD13
         return type.GetTypeInfo().GetCustomAttribute<T>();
#else
         return Attribute.GetCustomAttribute(type, typeof(T)) as T;
#endif
      }

      public static bool IsEnum(this Type type)
      {
#if STANDARD13
         return type.GetTypeInfo().IsEnum;
#else
         return type.IsEnum;
#endif
      }

      public static Assembly GetAssembly(this Type type)
      {
#if STANDARD13
         return type.GetTypeInfo().Assembly;
#else
         return type.Assembly;
#endif
      }
   }
}
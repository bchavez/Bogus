using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bogus.Platform
{
   /// <summary>
   /// Extension methods on <see cref="Type"/>.
   /// </summary>
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

      /// <summary>
      /// Returns all the members of a type, based on <paramref name="bindingFlags"/>.
      /// </summary>
      /// <remarks>
      /// For class types, it will simply call <see cref="Type.GetMembers(BindingFlags)"/>.
      /// For interface types however, it will inspect *all* interfaces that <paramref name="type"/> implements,
      /// and return all the members.
      /// </remarks>
      /// <param name="type">The type to inspect.</param>
      /// <param name="bindingFlags">The binding flags to use.</param>
      /// <see href="https://stackoverflow.com/a/47277547/15393"/>
      /// <returns>The relevant members of <paramref name="type"/></returns>
      public static IEnumerable<MemberInfo> GetAllMembers(this Type type, BindingFlags bindingFlags)
      {
#if NETSTANDARD1_3
         if (type.GetTypeInfo().IsInterface)
#else
         if (type.IsInterface)
#endif
         {
            return type.GetInterfaces().Union(new[] { type }).SelectMany(i => i.GetMembers(bindingFlags)).Distinct();
         }
         else
         {
            return type.GetMembers(bindingFlags);
         }
      }
   }
}
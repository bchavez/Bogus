using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bogus.Extensions
{
   /// <summary>
   /// Extension methods on <see cref="Type"/>.
   /// </summary>
   internal static class ExtensionsForType
   {
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

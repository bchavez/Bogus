using System;
using System.Reflection;

namespace Bogus.Platform
{
    internal static class ExtensionsForType
    {
        public static T GetCustomAttributeX<T>(this Type type) where T : Attribute 
        {
#if STANDARD
            return type.GetCustomAttribute<T>();
#else
            return Attribute.GetCustomAttribute(type, typeof(T)) as T;
#endif
        }
    }
}
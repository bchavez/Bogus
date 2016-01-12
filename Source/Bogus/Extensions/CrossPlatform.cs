using System;
using System.Reflection;

#if DNX
using System.Reflection;
#endif

namespace Bogus.Extensions
{
    internal static class ExtensionsForType
    {
#if DNX
        public static bool IsSubclassOf(this Type type, Type other)
        {
            return type.GetTypeInfo().IsSubclassOf(other);
        }
#endif

        public static bool IsGenericType(this Type type)
        {
#if DNX
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static Type BaseType(this Type type)
        {
#if DNX
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static T GetCustomAttributeX<T>(this Type type) where T : Attribute 
        {
#if DNX
            return type.GetTypeInfo().GetCustomAttribute<T>();
#else
            return Attribute.GetCustomAttribute(type, typeof(T)) as T;
#endif
        }

        public static Assembly GetAssembly(this Type type)
        {
#if DNX
            return type.GetTypeInfo().Assembly;
#else
            return type.Assembly;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if DNX
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }
    }
}
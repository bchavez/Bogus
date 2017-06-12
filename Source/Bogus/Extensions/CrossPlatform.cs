using System;
using System.Reflection;

namespace Bogus.Extensions
{
    internal static class ExtensionsForType
    {
#if STANDARD
        public static bool IsSubclassOf(this Type type, Type other)
        {
            return type.GetTypeInfo().IsSubclassOf(other);
        }
#endif

        public static bool IsGenericType(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().IsGenericType;
#else
            return type.IsGenericType;
#endif
        }

        public static Type BaseType(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }

        public static T GetCustomAttributeX<T>(this Type type) where T : Attribute 
        {
#if STANDARD
            return type.GetTypeInfo().GetCustomAttribute<T>();
#else
            return Attribute.GetCustomAttribute(type, typeof(T)) as T;
#endif
        }

        public static Assembly GetAssembly(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().Assembly;
#else
            return type.Assembly;
#endif
        }

        public static bool IsEnum(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().IsEnum;
#else
            return type.IsEnum;
#endif
        }

        public static bool IsInterface(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().IsInterface;
#else
            return type.IsInterface;
#endif
        }

        public static bool IsAbstract(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().IsAbstract;
#else
            return type.IsAbstract;
#endif
        }

        public static bool IsClass(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().IsClass;
#else
            return type.IsClass;
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if STANDARD
            return type.GetTypeInfo().IsValueType;
#else
            return type.IsValueType;
#endif
        }
    }
}
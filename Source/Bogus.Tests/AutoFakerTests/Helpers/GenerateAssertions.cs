using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bogus.Tests.AutoFakerTests.Models.Complex;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Bogus.Tests.AutoFakerTests.Helpers
{
   public sealed class GenerateAssertions
      : ReferenceTypeAssertions<Order, GenerateAssertions>
   {
      private MethodInfo DefaultValueFactory;
      private IDictionary<Func<Type, bool>, Func<string, Type, object, string>> Assertions = new Dictionary<Func<Type, bool>, Func<string, Type, object, string>>();

      internal GenerateAssertions(Order order)
      {
         var type = GetType();

         this.Order = order;
         DefaultValueFactory = type.GetMethod("GetDefaultValue", BindingFlags.Instance | BindingFlags.NonPublic);

         // Add the assertions to type mappings
         Assertions.Add(IsInt, AssertInt);
         Assertions.Add(IsDecimal, AssertDecimal);
         Assertions.Add(IsGuid, AssertGuid);
         Assertions.Add(IsDateTime, AssertDateTime);
         Assertions.Add(IsString, AssertString);

         Assertions.Add(IsEnum, AssertEnum);
         Assertions.Add(IsNullable, AssertNullable);
         Assertions.Add(IsArray, AssertArray);
         Assertions.Add(IsDictionary, AssertDictionary);
         Assertions.Add(IsEnumerable, AssertEnumerable);
      }

      private Order Order { get; }
      private AssertionScope Scope { get; set; }

      protected override string Context => "order";

      public AndConstraint<Order> BePopulatedWithMocks()
      {
         // Ensure the mocked objects are asserted as null
         Assertions.Add(IsInterface, AssertMock);
         Assertions.Add(IsAbstract, AssertMock);

         return BePopulated();
      }

      public AndConstraint<Order> BePopulatedWithoutMocks()
      {
         // Ensure the mocked objects are asserted as null
         Assertions.Add(IsInterface, AssertNull);
         Assertions.Add(IsAbstract, AssertNull);

         return BePopulated();
      }

      public AndConstraint<Order> BeNotPopulated()
      {
         var type = typeof(Order);
         var memberInfos = GetMemberInfos(type);

         this.Scope = Execute.Assertion;

         foreach( var memberInfo in memberInfos )
         {
            AssertDefaultValue(memberInfo);
         }

         return new AndConstraint<Order>(this.Order);
      }

      private AndConstraint<Order> BePopulated()
      {
         var type = typeof(Order);
         var assertion = GetAssertion(type);

         this.Scope = Execute.Assertion;

         assertion.Invoke(type.Name, type, this.Order);

         return new AndConstraint<Order>(this.Order);
      }

      private string AssertType(string path, Type type, object instance)
      {
         // Iterate the members for the instance and assert their values
         var members = GetMemberInfos(type);

         foreach( var member in members )
         {
            AssertMember(path, member, instance);
         }

         return null;
      }

      private void AssertDefaultValue(MemberInfo memberInfo)
      {
         ExtractMemberInfo(memberInfo, out Type memberType, out Func<object, object> memberGetter);

         // Resolve the default value for the current member type and check it matches
         var factory = DefaultValueFactory.MakeGenericMethod(memberType);
         var defaultValue = factory.Invoke(this, new object[0]);
         var value = memberGetter.Invoke(this.Order);
         var equal = value == null && defaultValue == null;

         if( !equal )
         {
            // Ensure Equals() is called on a non-null instance
            if( value != null )
            {
               equal = value.Equals(defaultValue);
            }
            else
            {
               equal = defaultValue.Equals(value);
            }
         }

         this.Scope = this.Scope
            .ForCondition(equal)
            .FailWith($"Expected a default '{memberType.FullName}' value for '{memberInfo.Name}'.")
            .Then;
      }

      private static bool IsInt(Type type) => type == typeof(int);
      private static bool IsDecimal(Type type) => type == typeof(decimal);
      private static bool IsGuid(Type type) => type == typeof(Guid);
      private static bool IsDateTime(Type type) => type == typeof(DateTime);
      private static bool IsString(Type type) => type == typeof(string);
      private static bool IsEnum(Type type) => type.GetTypeInfo().IsEnum;
      private static bool IsNullable(Type type) => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
      private static bool IsArray(Type type) => type.IsArray;
      private static bool IsDictionary(Type type) => IsType(type, typeof(IDictionary<,>));
      private static bool IsEnumerable(Type type) => IsType(type, typeof(IEnumerable<>));
      private static bool IsAbstract(Type type) => type.GetTypeInfo().IsAbstract;
      private static bool IsInterface(Type type) => type.GetTypeInfo().IsInterface;

      private static string AssertInt(string path, Type type, object value) => value.Equals(0) ? GetAssertionMessage(path, type) : null;
      private static string AssertDecimal(string path, Type type, object value) => value.Equals(0d) ? GetAssertionMessage(path, type) : null;
      private static string AssertGuid(string path, Type type, object value) => Guid.TryParse(value.ToString(), out Guid result) ? null : GetAssertionMessage(path, type);

      private static string AssertDateTime(string path, Type type, object value) => DateTime.TryParse(value.ToString(), out DateTime result)
         ? null
         : GetAssertionMessage(path, type);

      private static string AssertEnum(string path, Type type, object value) => Enum.IsDefined(type, value) ? null : GetAssertionMessage(path, type);
      private static string AssertNull(string path, Type type, object value) => value == null ? null : $"Expected value to be null for '{path}'.";

      private static string AssertString(string path, Type type, object value)
      {
         var str = value?.ToString();
         return string.IsNullOrWhiteSpace(str) ? GetAssertionMessage(path, type) : null;
      }

      private string AssertNullable(string path, Type type, object value)
      {
         var genericType = type.GenericTypeArguments.Single();
         var assertion = GetAssertion(genericType);

         return assertion.Invoke(path, genericType, value);
      }

      private string AssertMock(string path, Type type, object value)
      {
         if( value == null )
         {
            return $"Excepted value to not be null for '{path}'.";
         }

         // Assert via assignment rather than explicit checks (the actual instance could be a sub class)
         var valueType = value.GetType();
         return type.IsAssignableFrom(valueType) ? null : GetAssertionMessage(path, type);
      }

      private string AssertArray(string path, Type type, object value)
      {
         var itemType = type.GetElementType();
         return AssertItems(path, itemType, value as Array);
      }

      private string AssertDictionary(string path, Type type, object value)
      {
         var typeInfo = type.GetTypeInfo();
         var genericTypes = typeInfo.GetGenericArguments();
         var keyType = genericTypes.ElementAt(0);
         var valueType = genericTypes.ElementAt(1);
         var dictionary = value as IDictionary;

         if( dictionary == null )
         {
            return $"Excepted value to not be null for '{path}'.";
         }

         // Check the keys and values individually
         var keysMessage = AssertItems(path, keyType, dictionary.Keys, "keys", ".Key");

         if( keysMessage == null )
         {
            return AssertItems(path, valueType, dictionary.Values, "values", ".Value");
         }

         return keysMessage;
      }

      private string AssertEnumerable(string path, Type type, object value)
      {
         var typeInfo = type.GetTypeInfo();
         var genericTypes = typeInfo.GetGenericArguments();
         var itemType = genericTypes.Single();

         return AssertItems(path, itemType, value as IEnumerable);
      }

      private string AssertItems(string path, Type type, IEnumerable items, string elementType = null, string suffix = null)
      {
         // Check the list of items is not null
         if( items == null )
         {
            return $"Excepted value to not be null for '{path}'.";
         }

         // Check the count state of the items
         var count = 0;
         var enumerator = items.GetEnumerator();

         while( enumerator.MoveNext() )
         {
            count++;
         }

         if( count > 0 )
         {
            // If we have any items, check each of them 
            var index = 0;
            var assertion = GetAssertion(type);

            foreach( var item in items )
            {
               var element = string.Format("{0}[{1}]{2}", path, index++, suffix);
               var message = assertion.Invoke(element, type, item);

               if( message != null )
               {
                  return message;
               }
            }
         }
         else
         {
            // Otherwise ensure we are not dealing with interface or abstract class
            // These types will result in an empty list by default because they cannot be generated
            var typeInfo = type.GetTypeInfo();

            if( !typeInfo.IsInterface && !typeInfo.IsAbstract )
            {
               elementType = elementType ?? "value";
               return $"Excepted {elementType} to not be empty for '{path}'.";
            }
         }

         return null;
      }

      private void AssertMember(string path, MemberInfo memberInfo, object instance)
      {
         ExtractMemberInfo(memberInfo, out Type memberType, out Func<object, object> memberGetter);

         // Create a trace path for the current member
         path = string.Concat(path, ".", memberInfo.Name);

         // Resolve the assertion and value for the member type
         var value = memberGetter.Invoke(instance);
         var assertion = GetAssertion(memberType);
         var message = assertion.Invoke(path, memberType, value);

         // Register an assertion for each member
         this.Scope = this.Scope
            .ForCondition(message == null)
            .FailWith(message)
            .Then;
      }

      private object GetDefaultValue<TType>()
      {
         return default(TType);
      }

      private static string GetAssertionMessage(string path, Type type)
      {
         return $"Excepted a value of type '{type.FullName}' for '{path}'.";
      }

      private Func<string, Type, object, string> GetAssertion(Type type)
      {
         var assertion = (from k in Assertions.Keys
            where k.Invoke(type)
            select Assertions[k]).FirstOrDefault();

         return assertion ?? AssertType;
      }

      private IEnumerable<MemberInfo> GetMemberInfos(Type type)
      {
         return (from m in type.GetMembers()
            where m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field
            select m);
      }

      private static bool IsType(Type type, Type baseType)
      {
         var typeInfo = type.GetTypeInfo();
         var baseTypeInfo = baseType.GetTypeInfo();

         // We may need to do some generics magic to compare the types
         if( typeInfo.IsGenericType && baseTypeInfo.IsGenericType )
         {
            var types = typeInfo.GetGenericArguments();
            var baseTypes = baseType.GetGenericArguments();

            if( types.Length == baseTypes.Length )
            {
               baseType = baseType.MakeGenericType(types);
            }
         }

         return baseType.IsAssignableFrom(type);
      }

      private static void ExtractMemberInfo(MemberInfo member, out Type memberType, out Func<object, object> memberGetter)
      {
         memberType = null;
         memberGetter = null;

         // Extract the member type and getter action
         if( member.MemberType == MemberTypes.Field )
         {
            var fieldInfo = member as FieldInfo;

            memberType = fieldInfo.FieldType;
            memberGetter = fieldInfo.GetValue;
         }
         else if( member.MemberType == MemberTypes.Property )
         {
            var propertyInfo = member as PropertyInfo;

            memberType = propertyInfo.PropertyType;
            memberGetter = propertyInfo.GetValue;
         }
      }
   }
}
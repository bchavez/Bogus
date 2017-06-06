using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bogus.Generators;
using Bogus.Tests.AutoFakerTests.Models.Simple;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.AutoFakerTests
{
   public class AutoGeneratorsFixture
   {
      private AutoGenerateContext _context;

      public AutoGeneratorsFixture()
      {
         var faker = new Faker();
         var binder = new AutoBinder();

         _context = new AutoGenerateContext(faker, Enumerable.Empty<string>(), binder);
      }

      public class RegisteredGenerator
         : AutoGeneratorsFixture
      {
         [Theory]
         [MemberData("GetRegisteredTypes")]
         public void Generate_Should_Return_Value(Type type)
         {
            var generator = AutoGeneratorFactory.Generators[type];

            InvokeGenerator(generator).Should().BeOfType(type);
         }

         [Theory]
         [MemberData("GetRegisteredTypes")]
         public void GetGenerator_Should_Return_Generator(Type type)
         {
            var generator = AutoGeneratorFactory.Generators[type];

            AutoGeneratorFactory.GetGenerator(type, _context).Should().Be(generator);
         }
      }

      public class ArrayGenerator
         : AutoGeneratorsFixture
      {
         [Theory]
         [InlineData(typeof(TestEnum[]))]
         [InlineData(typeof(TestStruct[]))]
         [InlineData(typeof(TestClass[]))]
         [InlineData(typeof(TestInterface[]))]
         [InlineData(typeof(TestAbstractClass[]))]
         public void Generate_Should_Return_Array(Type type)
         {
            var itemType = type.GetElementType();
            var generator = CreateGenerator(typeof(ArrayGenerator<>), itemType);
            var array = InvokeGenerator(generator) as Array;

            array.Should().NotBeNull().And.NotContainNulls();
         }

         [Theory]
         [InlineData(typeof(TestEnum[]))]
         [InlineData(typeof(TestStruct[]))]
         [InlineData(typeof(TestClass[]))]
         [InlineData(typeof(TestInterface[]))]
         [InlineData(typeof(TestAbstractClass[]))]
         public void GetGenerator_Should_Return_ArrayGenerator(Type type)
         {
            var itemType = type.GetElementType();
            var generatorType = GetGeneratorType(typeof(ArrayGenerator<>), itemType);

            AutoGeneratorFactory.GetGenerator(type, _context).Should().BeOfType(generatorType);
         }
      }

      public class DictionaryGenerator
         : AutoGeneratorsFixture
      {
         [Theory]
         [InlineData(typeof(IDictionary<int, TestEnum>))]
         [InlineData(typeof(IDictionary<int, TestStruct>))]
         [InlineData(typeof(IDictionary<int, TestClass>))]
         [InlineData(typeof(IDictionary<int, TestInterface>))]
         [InlineData(typeof(IDictionary<int, TestAbstractClass>))]
         public void Generate_Should_Return_Dictionary(Type type)
         {
            var genericTypes = GetGenericTypes(type);
            var keyType = genericTypes.ElementAt(0);
            var valueType = genericTypes.ElementAt(1);
            var generator = CreateGenerator(typeof(DictionaryGenerator<,>), keyType, valueType);
            var dictionary = InvokeGenerator(generator) as IDictionary;

            dictionary.Should().NotBeNull().And.NotContainNulls();

            foreach( var key in dictionary.Keys )
            {
               var value = dictionary[key];

               key.Should().BeOfType(keyType);
               value.Should().BeOfType(valueType);
            }
         }

         [Theory]
         [InlineData(typeof(IDictionary<int, TestEnum>))]
         [InlineData(typeof(IDictionary<int, TestStruct>))]
         [InlineData(typeof(IDictionary<int, TestClass>))]
         [InlineData(typeof(IDictionary<int, TestInterface>))]
         [InlineData(typeof(IDictionary<int, TestAbstractClass>))]
         public void GetGenerator_Should_Return_DictionaryGenerator(Type type)
         {
            var genericTypes = GetGenericTypes(type);
            var keyType = genericTypes.ElementAt(0);
            var valueType = genericTypes.ElementAt(1);
            var generatorType = GetGeneratorType(typeof(DictionaryGenerator<,>), keyType, valueType);

            AutoGeneratorFactory.GetGenerator(type, _context).Should().BeOfType(generatorType);
         }
      }

      public class EnumerableGenerator
         : AutoGeneratorsFixture
      {
         [Theory]
         [InlineData(typeof(IEnumerable<TestEnum>))]
         [InlineData(typeof(IEnumerable<TestStruct>))]
         [InlineData(typeof(IEnumerable<TestClass>))]
         [InlineData(typeof(IEnumerable<TestInterface>))]
         [InlineData(typeof(IEnumerable<TestAbstractClass>))]
         public void Generate_Should_Return_Enumerable(Type type)
         {
            var genericTypes = GetGenericTypes(type);
            var itemType = genericTypes.ElementAt(0);
            var generator = CreateGenerator(typeof(EnumerableGenerator<>), itemType);
            var enumerable = InvokeGenerator(generator) as IEnumerable;

            enumerable.Should().NotBeNull().And.NotContainNulls();
         }

         [Theory]
         [InlineData(typeof(IEnumerable<TestEnum>))]
         [InlineData(typeof(IEnumerable<TestStruct>))]
         [InlineData(typeof(IEnumerable<TestClass>))]
         [InlineData(typeof(IEnumerable<TestInterface>))]
         [InlineData(typeof(IEnumerable<TestAbstractClass>))]
         public void GetGenerator_Should_Return_EnumerableGenerator(Type type)
         {
            var genericTypes = GetGenericTypes(type);
            var itemType = genericTypes.ElementAt(0);
            var generatorType = GetGeneratorType(typeof(EnumerableGenerator<>), itemType);

            AutoGeneratorFactory.GetGenerator(type, _context).Should().BeOfType(generatorType);
         }
      }

      public class EnumGenerator
         : AutoGeneratorsFixture
      {
         [Fact]
         public void Generate_Should_Return_Enum()
         {
            var generator = new EnumGenerator<TestEnum>();

            InvokeGenerator(generator).Should().BeOfType<TestEnum>();
         }

         [Fact]
         public void GetGenerator_Should_Return_EnumGenerator()
         {
            AutoGeneratorFactory.GetGenerator<TestEnum>(_context).Should().BeOfType<EnumGenerator<TestEnum>>();
         }
      }

      public class NullableGenerator
         : AutoGeneratorsFixture
      {
         [Fact]
         public void Generate_Should_Return_Value()
         {
            var generator = new NullableGenerator<TestEnum>();

            InvokeGenerator(generator).Should().BeOfType<TestEnum>();
         }

         [Fact]
         public void GetGenerator_Should_Return_NullableGenerator()
         {
            AutoGeneratorFactory.GetGenerator<TestEnum?>(_context).Should().BeOfType<NullableGenerator<TestEnum>>();
         }
      }

      public class TypeGenerator
         : AutoGeneratorsFixture
      {
         [Theory]
         [InlineData(typeof(TestStruct))]
         [InlineData(typeof(TestClass))]
         [InlineData(typeof(TestInterface))]
         [InlineData(typeof(TestAbstractClass))]
         [InlineData(typeof(List<TestClass>))]
         [InlineData(typeof(SortedList<int, TestClass>))]
         public void Generate_Should_Return_Value(Type type)
         {
            var typeInfo = type.GetTypeInfo();
            var generator = CreateGenerator(typeof(TypeGenerator<>), type);

            if( typeInfo.IsInterface || typeInfo.IsAbstract )
            {
               InvokeGenerator(generator).Should().BeNull();
            }
            else
            {
               InvokeGenerator(generator).Should().BeAssignableTo(type);
            }
         }

         [Theory]
         [InlineData(typeof(TestStruct))]
         [InlineData(typeof(TestClass))]
         [InlineData(typeof(TestInterface))]
         [InlineData(typeof(TestAbstractClass))]
         [InlineData(typeof(List<TestClass>))]
         [InlineData(typeof(SortedList<int, TestClass>))]
         public void GetGenerator_Should_Return_TypeGenerator(Type type)
         {
            var generatorType = GetGeneratorType(typeof(TypeGenerator<>), type);

            AutoGeneratorFactory.GetGenerator(type, _context).Should().BeOfType(generatorType);
         }
      }

      private object InvokeGenerator(IAutoGenerator generator)
      {
         return generator.Generate(_context);
      }

      private static IEnumerable<Type> GetGenericTypes(Type type)
      {
         var typeInfo = type.GetTypeInfo();
         return typeInfo.GetGenericArguments();
      }

      private static Type GetGeneratorType(Type type, params Type[] types)
      {
         return type.MakeGenericType(types);
      }

      private static IAutoGenerator CreateGenerator(Type type, params Type[] types)
      {
         type = GetGeneratorType(type, types);
         return (IAutoGenerator)Activator.CreateInstance(type);
      }

      private static IEnumerable<object[]> GetRegisteredTypes()
      {
         return AutoGeneratorFactory.Generators.Select(g => new object[]
            {
               g.Key
            });
      }
   }
}
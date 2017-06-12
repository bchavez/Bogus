using Bogus.Auto;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Bogus.Tests.Auto
{
  public class ConventionsTests
  {
    public enum Options
    {
      Option1,
      Option2,
      Option3
    }

    private const string DummyName = "DUMMY";

    private IGenerator _generator;
    private IBinder _binder;
    private Faker _fakerHub;

    public ConventionsTests()
    {
      _generator = Substitute.For<IGenerator>();
      _binder = Substitute.For<IBinder>();

      _fakerHub = new Faker();
    }

    public class TypeActivatorTests
      : ConventionsTests
    {
      private interface ITestInterface
      { }

      private abstract class TestAbstract
      { }

      private class TestClass
      {
        public int Parameter { get; }
      }

      private struct TestStruct
      {
        public int Parameter { get; }
      }

      private class TestClassParameterizedConstructor
      {
        public TestClassParameterizedConstructor(int parameter)
        {
          Parameter = parameter;
        }

        public int Parameter { get; }
      }

      private struct TestStructParameterizedConstructor
      {
        public TestStructParameterizedConstructor(int parameter)
        {
          Parameter = parameter;
        }

        public int Parameter { get; }
      }

      private class TestClassNoConstructor
      {
        private TestClassNoConstructor(int parameter)
        {
          Parameter = parameter;
        }

        public int Parameter { get; }
      }

      private struct TestStructNoConstructor
      {
        private TestStructNoConstructor(int parameter)
        {
          Parameter = parameter;
        }

        public int Parameter { get; }
      }

      private BindingInfo _binding;
      private IConditionalConvention _typeActivator;

      public TypeActivatorTests()
      {
        _binding = new BindingInfo(typeof(TestClass), DummyName);
        _typeActivator = new TypeActivator();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null()
      {
        _binding.Value = new TestClass();

        _typeActivator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_An_Interface()
      {
        _binding = new BindingInfo(typeof(ITestInterface), DummyName);

        _typeActivator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Abstract()
      {
        _binding = new BindingInfo(typeof(TestAbstract), DummyName);

        _typeActivator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null()
      {
        _typeActivator.CanInvoke(_binding).Should().BeTrue();
      }

      [Theory]
      [InlineData(typeof(TestClass))]
      [InlineData(typeof(TestStruct))]
      public void Invoke_Should_Set_Binding_Value_With_Default_Constructor(Type type)
      {
        _binding = new BindingInfo(type, DummyName);

        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _typeActivator.Invoke(context);

        _binding.Value.ShouldBeEquivalentTo(new
        {
          Parameter = default(int)
        });
      }

      [Theory]
      [InlineData(typeof(TestClassParameterizedConstructor))]
      [InlineData(typeof(TestStructParameterizedConstructor))]
      public void Invoke_Should_Set_Binding_Value_With_Parameterized_Constructor(Type type)
      {
        _binding = new BindingInfo(type, DummyName);

        var value = GetValue<int>();
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x => x.Arg<BindingInfo>().Value = value);

        _typeActivator.Invoke(context);

        _binding.Value.ShouldBeEquivalentTo(new
        {
          Parameter = value
        });
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value_With_Dictionary_Constructor()
      {
        _binding = new BindingInfo(typeof(Dictionary<int, string>), DummyName);
        
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);
        var value = new Dictionary<int, string>
        {
          { GetValue<int>(), GetValue<string>() }
        };

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x => x.Arg<BindingInfo>().Value = value);

        _typeActivator.Invoke(context);

        _binding.Value.ShouldBeEquivalentTo(value);
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value_With_Enumerable_Constructor()
      {
        _binding = new BindingInfo(typeof(List<int>), DummyName);

        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);
        var value = new List<int>
        {
          GetValue<int>()
        };

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x => x.Arg<BindingInfo>().Value = value);

        _typeActivator.Invoke(context);

        _binding.Value.ShouldBeEquivalentTo(value);
      }

      [Theory]
      [InlineData(typeof(TestClassNoConstructor))]
      [InlineData(typeof(TestStructNoConstructor))]
      public void Invoke_Should_Set_Binding_Value_With_No_Constructor(Type type)
      {
        _binding = new BindingInfo(type, DummyName);

        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);
        var value = GetDefaultValue(type);

        _typeActivator.Invoke(context);

        _binding.Value.Should().Be(value);
      }
    }

    public class MembersBinderTests
      : ConventionsTests
    {
      private static readonly Type Type = typeof(TestClass);

      private class TestClass
      {
        public int Id { get; set; }
        public string Name { get; set; }
      }

      private TestClass _value;
      private BindingInfo _binding;
      private IConditionalConvention _membersBinder;

      public MembersBinderTests()
      {
        _value = new TestClass();
        _binding = new BindingInfo(Type, DummyName)
        {
          Value = _value
        };

        _membersBinder = new MembersBinder();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Null()
      {
        _binding.Value = null;

        _membersBinder.CanInvoke(_binding).Should().BeFalse();
      }

      [Theory]
      [InlineData(typeof(Options), null)]
      [InlineData(typeof(int[]), null)]
      [InlineData(typeof(IDictionary<int, string>), null)]
      [InlineData(typeof(IEnumerable<Guid>), null)]
      [MemberData("GetTypeBindingGenerators")]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Invalid(Type type, ITypeBindingGenerator generator)
      {
        _binding = new BindingInfo(type, DummyName)
        {
          Value = _value
        };

        _membersBinder.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Not_Null()
      {
        _membersBinder.CanInvoke(_binding).Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var id = GetValue<int>();
        var name = GetValue<string>();
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);
        var members = (from m in Type.GetMembers()
                       where m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property
                       select m).ToDictionary(m => m.Name, m => m);

        _binder.GetMembers(Type).Returns(members);

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x =>
          {
            if (x.Arg<BindingInfo>().Name == "Id")
            {
              x.Arg<BindingInfo>().Value = id;
            }
            else
            {
              x.Arg<BindingInfo>().Value = name;
            }
          });

        _membersBinder.Invoke(context);

        _binding.Value.ShouldBeEquivalentTo(new
        {
          Id = id,
          Name = name
        });
      }
    }

    public class RecursionGuardTests
      : ConventionsTests
    {
      private IConvention _resursionGuard;

      public RecursionGuardTests()
      {
        _resursionGuard = new RecursionGuard();
      }

      [Fact]
      public void Should_Throw_If_Binding_Hierarchy_Is_Recursive()
      {
        Action action = () =>
        {
          var binding1 = new BindingInfo(typeof(object), DummyName);
          var binding2 = new BindingInfo(typeof(int), DummyName, binding1);
          var binding3 = new BindingInfo(typeof(string), DummyName, binding2);
          var binding4 = new BindingInfo(typeof(object), DummyName, binding3);
          var context = new GenerateContext(1, _generator, _binder, binding1, _fakerHub);

          _resursionGuard.Invoke(context);
        };

        action.ShouldNotThrow<GenerateException>();
      }

      [Fact]
      public void Should_Not_Throw_If_Binding_Hierarchy_Is_Not_Recursive()
      {
        Action action = () =>
        {
          var binding1 = new BindingInfo(typeof(int), DummyName);
          var binding2 = new BindingInfo(typeof(string), DummyName, binding1);
          var binding3 = new BindingInfo(typeof(object), DummyName, binding2);
          var context = new GenerateContext(1, _generator, _binder, binding3, _fakerHub);

          _resursionGuard.Invoke(context);
        };

        action.ShouldNotThrow<GenerateException>();
      }
    }

    public class CommandConventionTests
      : ConventionsTests
    {
      private bool _invoked;
      private BindingInfo _binding;
      private IConditionalConvention _command;

      public CommandConventionTests()
      {
        _invoked = false;
        _binding = new BindingInfo(typeof(bool), DummyName);
        _command = new CommandConvention(DummyName, b => _invoked = true, c => { _invoked = true; });
      }

      [Fact]
      public void CanInvoke_Should_Invoke_Predicate()
      {
        _command.CanInvoke(_binding);

        _invoked.Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Invoke_Action()
      {
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _command.Invoke(context);

        _invoked.Should().BeTrue();
      }
    }

    public class GenericConventionTests
      : ConventionsTests
    {
      private class TestGenericConvention
        : GenericConvention
      {
        internal TestGenericConvention(object value)
        {
          Value = value;
        }

        private object Value { get; }

        protected override bool CanInvoke<TType>(BindingInfo binding)
        {
          return true;
        }

        protected override void Invoke<TType>(GenerateContext context)
        {
          context.Binding.Value = Value;
        }
      }

      private object _value;
      private IConditionalConvention _genericConvention;

      public GenericConventionTests()
      {
        _value = new object();
        _genericConvention = new TestGenericConvention(_value);
      }

      [Theory]
      [InlineData(typeof(int))]
      [InlineData(typeof(string))]
      [InlineData(typeof(object))]
      public void CanInvoke_Should_Return_True_For_Any_Type(Type type)
      {
        var binding = new BindingInfo(type, DummyName);

        _genericConvention.CanInvoke(binding).Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var binding = new BindingInfo(typeof(object), DummyName);
        var context = new GenerateContext(1, _generator, _binder, binding, _fakerHub);

        _genericConvention.Invoke(context);

        binding.Value.Should().Be(_value);
      }
    }

    public class GenericReferenceTypeConventionTests
      : ConventionsTests
    {
      private class TestGenericReferenceTypeConvention
        : GenericReferenceTypeConvention
      {
        internal TestGenericReferenceTypeConvention(object value)
        {
          Value = value;
        }

        private object Value { get; }

        protected override bool CanInvoke<TType>(BindingInfo binding)
        {
          return true;
        }

        protected override void Invoke<TType>(GenerateContext context)
        {
          context.Binding.Value = Value;
        }
      }

      private object _value;
      private IConditionalConvention _genericConvention;

      public GenericReferenceTypeConventionTests()
      {
        _value = new object();
        _genericConvention = new TestGenericReferenceTypeConvention(_value);
      }

      [Theory]
      [InlineData(typeof(int), false)]
      [InlineData(typeof(string), true)]
      [InlineData(typeof(object), true)]
      public void CanInvoke_Should_Return_Expected_Result_For_Type(Type type, bool expected)
      {
        var binding = new BindingInfo(type, DummyName);

        _genericConvention.CanInvoke(binding).Should().Be(expected);
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var binding = new BindingInfo(typeof(object), DummyName);
        var context = new GenerateContext(1, _generator, _binder, binding, _fakerHub);

        _genericConvention.Invoke(context);

        binding.Value.Should().Be(_value);
      }

      [Fact]
      public void Invoke_Should_Throw_For_Constraint_Violation()
      {
        Action action = () =>
        {
          var binding = new BindingInfo(typeof(int), DummyName);
          var context = new GenerateContext(1, _generator, _binder, binding, _fakerHub);

          _genericConvention.Invoke(context);
        };

        action.ShouldThrowExactly<ArgumentException>();
      }
    }

    public class GenericValueTypeConventionTests
      : ConventionsTests
    {
      private class TestGenericValueTypeConvention
        : GenericValueTypeConvention
      {
        internal TestGenericValueTypeConvention(object value)
        {
          Value = value;
        }

        private object Value { get; }

        protected override bool CanInvoke<TType>(BindingInfo binding)
        {
          return true;
        }

        protected override void Invoke<TType>(GenerateContext context)
        {
          context.Binding.Value = Value;
        }
      }

      private int _value;
      private IConditionalConvention _genericConvention;

      public GenericValueTypeConventionTests()
      {
        _value = new int();
        _genericConvention = new TestGenericValueTypeConvention(_value);
      }

      [Theory]
      [InlineData(typeof(int), true)]
      [InlineData(typeof(string), false)]
      [InlineData(typeof(object), false)]
      public void CanInvoke_Should_Return_Expected_Result_For_Type(Type type, bool expected)
      {
        var binding = new BindingInfo(type, DummyName);

        _genericConvention.CanInvoke(binding).Should().Be(expected);
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var binding = new BindingInfo(typeof(int), DummyName);
        var context = new GenerateContext(1, _generator, _binder, binding, _fakerHub);

        _genericConvention.Invoke(context);

        binding.Value.Should().Be(_value);
      }

      [Fact]
      public void Invoke_Should_Throw_For_Constraint_Violation()
      {
        Action action = () =>
        {
          var binding = new BindingInfo(typeof(object), DummyName);
          var context = new GenerateContext(1, _generator, _binder, binding, _fakerHub);

          _genericConvention.Invoke(context);
        };

        action.ShouldThrowExactly<ArgumentException>();
      }
    }

    public class ArrayGeneratorTests
      : ConventionsTests
    {
      private static readonly Type ItemType = typeof(int);
      private static readonly Type ArrayType = typeof(int[]);

      private BindingInfo _binding;
      private IConditionalConvention _arrayGenerator;

      public ArrayGeneratorTests()
      {
        _binding = new BindingInfo(ArrayType, DummyName);
        _arrayGenerator = new ArrayGenerator();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Not_Matched()
      {
        _binding = new BindingInfo(_arrayGenerator.GetType(), DummyName);

        _arrayGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null()
      {
        _binding.Value = new int[0];

        _arrayGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null()
      {
        _arrayGenerator.CanInvoke(_binding).Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x => x.Arg<BindingInfo>().Value = GetValue(ItemType));

        _arrayGenerator.Invoke(context);

        _binding.Value.Should().NotBeNull().And.NotContainDefaultFor(ItemType);
      }
    }

    public class DictionaryGeneratorTests
      : ConventionsTests
    {
      private static readonly Type KeyType = typeof(int);
      private static readonly Type ValueType = typeof(string);
      private static readonly Type DictionaryType = typeof(IDictionary<int, string>);

      private BindingInfo _binding;
      private IConditionalConvention _dictionaryGenerator;

      public DictionaryGeneratorTests()
      {
        _binding = new BindingInfo(DictionaryType, DummyName);
        _dictionaryGenerator = new DictionaryGenerator();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Not_Matched()
      {
        _binding = new BindingInfo(_dictionaryGenerator.GetType(), DummyName);

        _dictionaryGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null()
      {
        _binding.Value = new Dictionary<int, string>();

        _dictionaryGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null()
      {
        _dictionaryGenerator.CanInvoke(_binding).Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var type = KeyType;
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x =>
          {
            x.Arg<BindingInfo>().Value = GetValue(type);
            type = ValueType;
          });

        _dictionaryGenerator.Invoke(context);

        _binding.Value.Should().NotBeNull().And.NotContainDefaultFor(KeyType, ValueType);
      }
    }

    public class EnumerableGeneratorTests
      : ConventionsTests
    {
      private static readonly Type ItemType = typeof(Guid);
      private static readonly Type EnumerableType = typeof(IEnumerable<Guid>);

      private BindingInfo _binding;
      private IConditionalConvention _enumerableGenerator;

      public EnumerableGeneratorTests()
      {
        _binding = new BindingInfo(EnumerableType, DummyName);
        _enumerableGenerator = new EnumerableGenerator();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Not_Matched()
      {
        _binding = new BindingInfo(_enumerableGenerator.GetType(), DummyName);

        _enumerableGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null()
      {
        _binding.Value = Enumerable.Empty<Guid>();

        _enumerableGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null()
      {
        _enumerableGenerator.CanInvoke(_binding).Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x => x.Arg<BindingInfo>().Value = GetValue(ItemType));

        _enumerableGenerator.Invoke(context);

        _binding.Value.Should().NotBeNull().And.NotContainDefaultFor(ItemType);
      }
    }

    public class EnumGeneratorTests
      : ConventionsTests
    {
      private static readonly Type Type = typeof(Options);

      private BindingInfo _binding;
      private IConditionalConvention _enumGenerator;

      public EnumGeneratorTests()
      {
        _binding = new BindingInfo(Type, DummyName);
        _enumGenerator = new EnumGenerator();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Not_Matched()
      {
        _binding = new BindingInfo(_enumGenerator.GetType(), DummyName);

        _enumGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null_Or_Default()
      {
        _binding.Value = GetValue(Type);

        _enumGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null()
      {
        _enumGenerator.CanInvoke(_binding).Should().BeTrue();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Default()
      {
        _binding.Value = GetDefaultValue(Type);

        _enumGenerator.CanInvoke(_binding).Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _enumGenerator.Invoke(context);

        _binding.Value.Should().NotBeNull().And.NotBeDefaultFor(Type);
      }
    }

    public class NullableGeneratorTests
      : ConventionsTests
    {
      private static readonly Type ValueType = typeof(DateTime);
      private static readonly Type NullableType = typeof(DateTime?);

      private BindingInfo _binding;
      private IConditionalConvention _nullableGenerator;

      public NullableGeneratorTests()
      {
        _binding = new BindingInfo(NullableType, DummyName);
        _nullableGenerator = new NullableGenerator();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Not_Matched()
      {
        _binding = new BindingInfo(_nullableGenerator.GetType(), DummyName);

        _nullableGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null()
      {
        _binding.Value = GetValue(ValueType);

        _nullableGenerator.CanInvoke(_binding).Should().BeFalse();
      }

      [Fact]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null()
      {
        _nullableGenerator.CanInvoke(_binding).Should().BeTrue();
      }

      [Fact]
      public void Invoke_Should_Set_Binding_Value()
      {
        var context = new GenerateContext(1, _generator, _binder, _binding, _fakerHub);

        _generator
          .When(x => x.Generate(Arg.Any<BindingInfo>()))
          .Do(x => x.Arg<BindingInfo>().Value = GetValue(ValueType));

        _nullableGenerator.Invoke(context);

        _binding.Value.Should().NotBeNull().And.NotBeDefaultFor(ValueType);
      }
    }

    public class NameBindingGeneratorTests
      : ConventionsTests
    {
      [Theory]
      [MemberData("GetNameBindingGenerators")]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Not_Matched(Type type, string name, INameBindingGenerator generator)
      {
        var binding = new BindingInfo(generator.GetType(), name);

        generator.CanInvoke(binding).Should().BeFalse();
      }

      [Theory]
      [MemberData("GetNameBindingGenerators")]
      public void CanInvoke_Should_Return_False_If_Binding_Name_Is_Not_Matched(Type type, string name, INameBindingGenerator generator)
      {
        var binding = new BindingInfo(type, name + DummyName);

        generator.CanInvoke(binding).Should().BeFalse();
      }

      [Theory]
      [MemberData("GetNameBindingGenerators")]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null_Or_Default(Type type, string name, INameBindingGenerator generator)
      {
        var binding = new BindingInfo(type, name)
        {
          Value = GetValue(type)
        };

        generator.CanInvoke(binding).Should().BeFalse();
      }

      [Theory]
      [MemberData("GetNameBindingGenerators")]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null(Type type, string name, INameBindingGenerator generator)
      {
        var binding = new BindingInfo(type, name);

        generator.CanInvoke(binding).Should().BeTrue();
      }

      [Theory]
      [MemberData("GetNameBindingGenerators")]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Default(Type type, string name, INameBindingGenerator generator)
      {
        var binding = new BindingInfo(type, name)
        {
          Value = GetDefaultValue(type)
        };

        generator.CanInvoke(binding).Should().BeTrue();
      }

      [Theory]
      [MemberData("GetNameBindingGenerators")]
      public void Invoke_Should_Set_Binding_Value(Type type, string name, INameBindingGenerator generator)
      {
        var binding = new BindingInfo(type, name);
        var context = new GenerateContext(1, _generator, _binder, binding, _fakerHub);

        generator.Invoke(context);

        binding.Value.Should().NotBeNull().And.NotBeDefaultFor(type);
      }
    }

    public class TypeBindingGeneratorTests
      : ConventionsTests
    {
      [Theory]
      [MemberData("GetTypeBindingGenerators")]
      public void CanInvoke_Should_Return_False_If_Binding_Type_Is_Not_Matched(Type type, ITypeBindingGenerator generator)
      {
        var binding = new BindingInfo(generator.GetType(), DummyName);

        generator.CanInvoke(binding).Should().BeFalse();
      }

      [Theory]
      [MemberData("GetTypeBindingGenerators")]
      public void CanInvoke_Should_Return_False_If_Binding_Value_Is_Not_Null_Or_Default(Type type, ITypeBindingGenerator generator)
      {
        var binding = new BindingInfo(type, DummyName)
        {
          Value = GetValue(type)
        };

        generator.CanInvoke(binding).Should().BeFalse();
      }

      [Theory]
      [MemberData("GetTypeBindingGenerators")]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Null(Type type, ITypeBindingGenerator generator)
      {
        var binding = new BindingInfo(type, DummyName);

        generator.CanInvoke(binding).Should().BeTrue();
      }

      [Theory]
      [MemberData("GetTypeBindingGenerators")]
      public void CanInvoke_Should_Return_True_If_Binding_Is_Valid_And_Value_Is_Default(Type type, ITypeBindingGenerator generator)
      {
        var binding = new BindingInfo(type, DummyName)
        {
          Value = GetDefaultValue(type)
        };

        generator.CanInvoke(binding).Should().BeTrue();
      }

      [Theory]
      [MemberData("GetTypeBindingGenerators")]
      public void Invoke_Should_Set_Binding_Value(Type type, ITypeBindingGenerator generator)
      {
        var binding = new BindingInfo(type, DummyName);
        var context = new GenerateContext(1, _generator, _binder, binding, _fakerHub);

        generator.Invoke(context);

        binding.Value.Should().NotBeNull().And.NotBeDefaultFor(type);
      }
    }

    private static TType GetValue<TType>()
    {
      var type = typeof(TType);
      return (TType)GetValue(type);
    }

    private static object GetValue(Type type)
    {
      var random = new Random();
      var dayOfWeek = DateTime.Now.DayOfWeek.ToString();

      if (type.IsEnum)
      {
        var values = Enum.GetValues(type).Cast<int>();
        var value = random.Next(values.Min() + 1, values.Max());

        while (!Enum.IsDefined(type, value))
        {
          value = random.Next(values.Min(), values.Max());
        }

        return Enum.GetName(type, value);
      }

      switch (type.Name)
      {
        case "Boolean":
          return true;

        case "Byte":
        case "SByte":
          return 1;

        case "Char":
          return dayOfWeek[0];

        case "DateTime":
          return DateTime.Now;

        case "Guid":
          return Guid.NewGuid();

        case "String":
          return dayOfWeek;
      }

      return Convert.ChangeType(DateTime.Now.Millisecond, type);
    }

    private static object GetDefaultValue(Type type)
    {
      if (type.IsValueType)
      {
        return Activator.CreateInstance(type);
      }

      return null;
    }

    private static IEnumerable<object[]> GetNameBindingGenerators()
    {
      foreach (var generator in ConventionsRegistry.NameBindingGenerators)
      {
        foreach (var name in generator.Names)
        {
          yield return new object[]
          {
            generator.Type,
            name,
            generator
          };
        }
      }
    }

    private static IEnumerable<object[]> GetTypeBindingGenerators()
    {
      foreach (var generator in ConventionsRegistry.TypeBindingGenerators)
      {
        yield return new object[]
        {
          generator.Type,
          generator
        };
      }
    }
  }
}

using Bogus.Auto;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Bogus.Tests.Auto
{
    public class BindingInfoTests
    {
        public class Class
        {
            public int Field;

            public Class()
            { }

            public int Property { get; set; }

            public void Method()
            { }
        }

        private static readonly Type Type = typeof(Class);
        private static readonly PropertyInfo Property = Type.GetProperty("Property");

        private Class _instance;
        private BindingInfo _parent;

        public BindingInfoTests()
        {
            _instance = new Class();
            _parent = new BindingInfo(Type, Type.Name)
            {
                Value = _instance
            };
        }

        public class MemberInfoTests
            : BindingInfoTests
        {
            [Theory]
            [MemberData("GetMemberInfos")]
            public void Should_Resolve_MemberInfo(MemberInfo memberInfo)
            {
                Action action = () => new BindingInfo(_parent, memberInfo);

                if (memberInfo.MemberType == MemberTypes.Field || memberInfo.MemberType == MemberTypes.Property)
                {
                    action.ShouldNotThrow();
                }
                else
                {
                    action.ShouldThrowExactly<ArgumentException>();
                }
            }
        }

        public class BindTests
            : BindingInfoTests
        {
            [Fact]
            public void Should_Bind_Value_To_Parent()
            {                
                var value = DateTime.Now.Millisecond;
                var binding = new BindingInfo(_parent, Property);

                binding.Bind(value);

                _instance.Property.Should().Be(value);
            }
        }

        private static IEnumerable<object[]> GetMemberInfos()
        {
            yield return new object[] { Type.GetField("Field") };
            yield return new object[] { Property };
            yield return new object[] { Type.GetConstructor(new Type[0]) };
            yield return new object[] { Type.GetMethod("Method") };
        }
    }
}

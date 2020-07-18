using System;
using System.CodeDom;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   public class Issue280 : SeededTest
   {
      public class Foo
      {
         public string Baz;
         public int Bar;
         public ushort Boo;
      }

      [Fact]
      public void automatic_type_conversion_fails_by_default()
      {
         var fooFaker = new Faker<Foo>()
            .RuleFor(o => o.Baz, f => f.Hacker.Phrase())
            .RuleFor(o => o.Bar, f => f.PickRandom(10, 20, 30))
            .RuleFor(o => o.Boo, f => f.PickRandom(40, 50, 60));

         Action generate = () => fooFaker.Generate();

         generate.Should().Throw<ArgumentException>();
      }

      [Fact]
      public void custom_faker_with_automatic_conversion_does_not_throw()
      {
         var fooFaker = new MagicFaker<Foo>()
            .RuleFor(o => o.Baz, f => f.Hacker.Phrase())
            .RuleFor(o => o.Bar, f => f.PickRandom(10, 20, 30))
            .RuleFor(o => o.Boo, f => f.PickRandom(40, 50, 60));

         var foo = fooFaker.Generate();

         foo.Baz.Should().Be("Use the neural RAM driver, then you can calculate the neural driver!");
         foo.Bar.Should().Be(10);
         foo.Boo.Should().Be(60);
      }

      public class MagicFaker<T> : Faker<T> where T : class
      {
         protected override Faker<T> AddRule(string propertyOrField, Func<Faker, T, object> invoker)
         {
            Func<Faker, T, object> hook = (faker, t) =>
               {
                  var initialValue = invoker(faker, t);
                  return ConvertValue(propertyOrField, initialValue);
               };
            return base.AddRule(propertyOrField, hook);
         }

         private object ConvertValue(string propertyOrField, object initialValue)
         {
            if( initialValue is null ) return null;

            if( !this.TypeProperties.TryGetValue(propertyOrField, out var memberInfo) )
               return initialValue;

            var targetType = memberInfo switch
               {
                  PropertyInfo pi => pi.PropertyType,
                  FieldInfo fi => fi.FieldType,
                  _ => throw new Exception("Unknown reflection type.")
               };

            var convertedValue = targetType switch
               {
                  Type t when t == typeof(int) => Convert.ToInt32(initialValue),
                  Type t when t == typeof(uint) => Convert.ToUInt32(initialValue),
                  Type t when t == typeof(long) => Convert.ToInt64(initialValue),
                  Type t when t == typeof(ulong) => Convert.ToUInt64(initialValue),
                  Type t when t == typeof(short) => Convert.ToInt16(initialValue),
                  Type t when t == typeof(ushort) => Convert.ToUInt16(initialValue),
                  Type t when t == typeof(float) => Convert.ToSingle(initialValue),
                  Type t when t == typeof(double) => Convert.ToDouble(initialValue),
                  Type t when t == typeof(decimal) => Convert.ToDecimal(initialValue),
                  Type t when t == typeof(byte) => Convert.ToByte(initialValue),
                  Type t when t == typeof(sbyte) => Convert.ToSByte(initialValue),
                  Type t when t == typeof(bool) => Convert.ToBoolean(initialValue),
                  Type t when t == typeof(char) => Convert.ToChar(initialValue),
                  _ => initialValue
               };

            return convertedValue;
         }
      }
   }
}

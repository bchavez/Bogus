using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.StackOverflowQuestions
{
   //https://stackoverflow.com/questions/66444118/how-to-use-bogus-faker-with-initialization-properties
   public class Question66444118 : SeededTest
   {
      [Fact]
      public void can_reflect_private_backing_fields_in_fakerT()
      {
         var backingFieldBinder = new BackingFieldBinder();
         var fooFaker = new Faker<Foo>(binder: backingFieldBinder)
            .SkipConstructor()
            .RuleFor(f => f.Name, f => f.Name.FullName());

         var foo = fooFaker.Generate();
         foo.Name.Should().NotBeNullOrWhiteSpace();
      }

      public class BackingFieldBinder : IBinder
      {
         public Dictionary<string, MemberInfo> GetMembers(Type t)
         {
            var availableFieldsForFakerT = new Dictionary<string, MemberInfo>();
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var allMembers = t.GetMembers(bindingFlags);
            var allBackingFields = allMembers
               .OfType<FieldInfo>()
               .Where(fi => fi.IsPrivate && fi.IsInitOnly)
               .Where(fi => fi.Name.EndsWith("__BackingField"))
               .ToList();

            foreach (var backingField in allBackingFields)
            {
               var fieldName = backingField.Name.Substring(1).Replace(">k__BackingField", "");
               availableFieldsForFakerT.Add(fieldName, backingField);
            }
            return availableFieldsForFakerT;
         }
      }

      public class Foo
      {
         public Foo(string name)
         {
            this.Name = name;
         }
         public string Name { get; }
      }
   }

   public static class MyExtensionsForFakerT
   {
      public static Faker<T> SkipConstructor<T>(this Faker<T> fakerOfT) where T : class
      {
         return fakerOfT.CustomInstantiator(_ => FormatterServices.GetUninitializedObject(typeof(T)) as T);
      }
   }
}
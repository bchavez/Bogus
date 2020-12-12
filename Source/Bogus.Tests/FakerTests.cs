using System;
using System.Collections.Generic;
using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions;
using Bogus.Tests.Models;
using FluentAssertions;
using Xunit;


namespace Bogus.Tests
{
   public class FakerTests
   {
      [Fact]
      public void SkipConstructor_Default_StrictMode_Satisfied()
      {
         _ = new Faker<NoParameterlessCtorClass>()
            .SkipConstructor()
            .RuleFor(x => x.Obj, x => null)
            .RuleFor(x => x.Obj2, x => null)
            .RuleFor(x => x.Obj3, x => null)
            .Generate(1);
      }

      [Fact]
      public void SkipConstructor_WithRule_Default_StrictMode_Not_Satisfied()
      {
         Assert.Throws<ValidationException>(() =>
         {
            _ = new Faker<NoParameterlessCtorClass>()
               .SkipConstructor()
               .RuleFor(x => x.Obj, x => null)
               .Generate(1);
         });
      }

      [Fact]
      public void SkipConstructor_WithoutRules_Default_StrictMode_Not_Satisfied()
      {
         Assert.Throws<ValidationException>(() =>
         {
            _ = new Faker<NoParameterlessCtorClass>()
               .SkipConstructor()
               .Generate(1);
         });
      }

      [Fact]
      public void Not_Skipping_Constructor_Without_Parameterless_Ctor()
      {
         Assert.Throws<NoParameterlessCtorException<NoParameterlessCtorClass>>(() =>
         {
            _ = new Faker<NoParameterlessCtorClass>()
             .Generate(1);
         });
      }

      [Fact]
      public void Skipping_Constructor_Overiding_StrictMode()
      {

         var faker = new Faker<NoParameterlessCtorClass>()
          .SkipConstructor()
          .StrictMode(false);

         Assert.Contains(faker.StrictModes, (x => x.Value == false));
         _ = faker.Generate(1);



         Assert.Throws<ValidationException>(() =>
         {
            faker = new Faker<NoParameterlessCtorClass>()
               .SkipConstructor()
                .StrictMode(true);

            Assert.Contains(faker.StrictModes, (x => x.Value == true));
            _ = faker.Generate(1);

         });

      }


      [Fact]
      public void UseConstructor()
      {
         var faker = new Faker<NoParameterlessCtorClass>()
            .UseConstructor(x => new NoParameterlessCtorClass(new Order(), 5));
         faker.Generate(1);

         var faker2 = new Faker<Order>()
             .UseConstructor(x => new Order() { OrderId = 68 });
         var order = faker2.Generate(1)[0];

         Assert.Equal(68, order.OrderId);
      }


      [Fact]
      public void UseConstructor_WithRule_Default_StrictMode_Not_Satisfied()
      {
         Assert.Throws<ValidationException>(() =>
         {
            _ = new Faker<NoParameterlessCtorClass>()
               .UseConstructor(x => new NoParameterlessCtorClass(default, default))
               .StrictMode(true)
               .RuleFor(x => x.Obj, x => null)
               .Generate(1);
         });
      }

      [Fact]
      public void UseConstructor_WithoutRules_Default_StrictMode_Not_Satisfied()
      {
         Assert.Throws<ValidationException>(() =>
         {
            _ = new Faker<NoParameterlessCtorClass>()
                  .StrictMode(true)
                  .UseConstructor(x => new NoParameterlessCtorClass(default, default))
                  .Generate(1);
         });
      }


      [Fact]
      public void UseContructor_After_SkipConstructor()
      {
         var faker = new Faker<NoParameterlessCtorClass>()
              .StrictMode(false)
               .UseConstructor(x => throw new Exception("Use Constructor was not overriden"))
               .SkipConstructor();
         _ = faker.Generate(1);

         var faker2 = new Faker<Order>()
              .StrictMode(false)
             .UseConstructor(x => throw new Exception("Use Constructor was not overriden"))
             .SkipConstructor();

         _ = faker2.Generate(1);

      }

      [Fact]
      public void SkipConstructor_After_UseContructor()
      {

         var faker = new Faker<NoParameterlessCtorClass>()
         .StrictMode(false)
         .SkipConstructor()
         .UseConstructor(x => new NoParameterlessCtorClass(new Order(), 5));
         var instance = faker.Generate(1)[0];
         Assert.Equal(5, instance.Obj2);

         var faker2 = new Faker<Order>()
             .StrictMode(false)
             .SkipConstructor()
             .UseConstructor(x => new Order() { OrderId = 68 });
         var order = faker2.Generate(1)[0];
         Assert.Equal(68, order.OrderId);

      }


      [Fact]
      public void UseParameterlessConstructor_Hidden_Ctor()
      {
         Assert.Throws<NoParameterlessCtorException<NoParameterlessCtorClass>>(() =>
         {
            var faker = new Faker<NoParameterlessCtorClass>()
            .UseConstructor(true);
            _ = faker.Generate(1);
         });

         Assert.Throws<NoParameterlessCtorException<Order>>(() =>
         {
            var faker2 = new Faker<Order>()
             .UseConstructor(true);
            _ = faker2.Generate(1);
         });

         var faker2 = new Faker<HiddenParameterlessCtorClass>()
           .UseConstructor(true);
         _ = faker2.Generate(1);
      }

      [Fact]
      public void UseParameterlessConstructor_Public_Ctor()
      {
         Assert.Throws<NoParameterlessCtorException<NoParameterlessCtorClass>>(() =>
         {
            var faker = new Faker<NoParameterlessCtorClass>()
            .UseConstructor(false);
            _ = faker.Generate(1);
         });

         var faker2 = new Faker<Order>()
             .UseConstructor(false);
         _ = faker2.Generate(1);

         Assert.Throws<NoParameterlessCtorException<HiddenParameterlessCtorClass>>(() =>
         {
            var faker3 = new Faker<HiddenParameterlessCtorClass>()
           .UseConstructor(false);
         _ = faker3.Generate(1);
         });
      }

      [Fact]
      public void GenerateDefaultConstructor()
      {

         Assert.Throws<NoParameterlessCtorException<NoParameterlessCtorClass>>(() =>
         {
            var faker = new Faker<NoParameterlessCtorClass>();
            _ = faker.Generate(1);
         });

         var faker2 = new Faker<Order>();
         _ = faker2.Generate(1);

         Assert.Throws<NoParameterlessCtorException<HiddenParameterlessCtorClass>>(() =>
         {

            var faker3 = new Faker<HiddenParameterlessCtorClass>();
            _ = faker3.Generate(1);
         });

      }



   }


}

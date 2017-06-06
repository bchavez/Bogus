using System;
using Bogus.Tests.AutoFakerTests.Helpers;
using Bogus.Tests.AutoFakerTests.Models.Complex;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Bogus.Tests.AutoFakerTests
{
   public class AutoFakerFixture : SeededTest
   {
      [Fact]
      public void Should_Generate_Type()
      {
         AutoFaker.Generate<Order>().Should().BePopulatedWithoutMocks();
      }

      [Fact]
      public void Should_Generate_Many_Types()
      {
         var instances = AutoFaker.Generate<Order>(2);

         instances.Should().HaveCount(2);

         foreach( var instance in instances )
         {
            instance.Should().BePopulatedWithoutMocks();
         }
      }

      [Fact]
      public void Should_Use_Custom_Instantiator()
      {
         int id = 0;
         ICalculator calculator = null;

         var binder = Substitute.For<IAutoBinder>();
         var order = new AutoFaker<Order>(binder)
            .CustomInstantiator(faker =>
               {
                  id = faker.Random.Int();
                  calculator = Substitute.For<ICalculator>();

                  return new Order(faker.Random.Int(), calculator);
               });

         binder.DidNotReceive().CreateInstance<Order>(Arg.Any<AutoGenerateContext>());
      }

      [Fact]
      public void Should_Not_Populate_Rule_Set_Members()
      {
         var code = Guid.NewGuid();
         var order = new AutoFaker<Order>()
            .RuleFor(o => o.Code, code);

         var instance = order.Generate();

         instance.Should().BePopulatedWithoutMocks().And.Code.Should().Be(code);
      }

      [Fact]
      public void Should_Not_Populate_If_No_Default_Rule_Set()
      {
         var order = new AutoFaker<Order>()
            .RuleSet("test", rules =>
               {
                  // No default constructor so ensure a create action is defined
                  // Make the values default so the BeNotPopulated() check passes
                  rules.CustomInstantiator(f => new Order(default(int), default(ICalculator)));
               });

         var instance = order.Generate("test");

         instance.Should().BeNotPopulated();
      }
   }
}
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.GitHubIssues
{
   //https://github.com/bchavez/Bogus/issues/70
   public class Issue70 : SeededTest
   {
      [Fact]
      public void should_be_able_to_create_derived_faker_with_class_hierarchy()
      {
         var baseBFaker = new Faker<BaseB>()
            .RuleFor(b => b.Value, f => f.Random.Int(1, 5));

         var derivedBFaker = new Faker<DerivedB>()
            .RuleFor(b => b.Value, f => f.Random.Int(6, 10));

         //Works
         var baseAFaker = new Faker<BaseA>()
            .RuleFor(a => a.SomeProp, () => baseBFaker.Generate());
         ;

         //Threw System.ArgumentException: 'An item with the same key has already been added.'
         var derivedAFaker = new Faker<DerivedA>()
            .RuleFor(da => da.SomeProp, () => derivedBFaker.Generate());


         DerivedA derivedA = derivedAFaker.Generate();

         derivedA.SomeProp.Value.Should().BeInRange(6, 10);
         BaseA baseA = derivedA;
         baseA.SomeProp.Should().BeNull();
      }

      [Fact]
      public void quick_test_for_derivedC()
      {
         var fakerC = new Faker<ClassC>()
            .RuleFor(c => c.Value2, f => f.Random.Int(1, 10))
            .RuleFor(c => c.Value, f => f.Random.Int(11, 20));

         ClassC fakeC = fakerC;

         fakeC.Value2.Should().BeInRange(1, 10);
         fakeC.Value.Should().BeInRange(11, 20);
      }
   }

   class BaseA
   {
      public BaseB SomeProp { get; set; }
   }

   class DerivedA : BaseA
   {
      public new DerivedB SomeProp { get; set; }
   }


   class BaseB
   {
      public int Value { get; set; }
   }

   class DerivedB : BaseB
   {
      public new int Value { get; set; }
   }

   class ClassC : BaseB
   {
      public int Value2 { get; set; }
   }
}
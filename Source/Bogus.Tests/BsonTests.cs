using System;
using System.Linq;
using Bogus.Bson;
using Bogus.DataSets;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests
{
   public class ExtraStuff : DataSet
   {
      public string JunkFood()
      {
         return this.GetRandomArrayItem("junkfood");
      }

      public string Drink()
      {
         return this.GetRandomArrayItem("Drink");
      }
   }

   public class BsonTests : SeededTest, IDisposable
   {
      private readonly ITestOutputHelper console;

      public BsonTests(ITestOutputHelper console)
      {
         this.console = console;
      }

      public void Dispose()
      {
         Database.ResetLocale("en");
         Database.ResetLocale("it");
      }

      [Fact]
      public void can_add_new_key_to_database()
      {
         PatchEnLocaleWithExtraStuff();

         var extra = new ExtraStuff();
         extra.JunkFood().Should().Be("Pizza");
         extra.Drink().Should().Be("Pepsi");
      }

      [Fact]
      public void should_be_able_to_reset_a_locale()
      {
         PatchEnLocaleWithExtraStuff();

         var extra = new ExtraStuff();

         extra.JunkFood().Should().Be("Pizza");

         Database.ResetLocale("en");

         Action error = () => extra.JunkFood().Should().Be("Pizza");

         error.Should().Throw<NullReferenceException>();
      }


      [Fact]
      public void can_patch_an_existing_category()
      {
         //names use only these first names
         var names = new[] {"Brian", "Chris", "Anthony", "Charlie", "Megan"};
         
         //make a BArray out of the names we wish to use
         var firstNames = names.Aggregate(new BArray(), (ba, name) =>
            {
               ba.Add(name);
               return ba;
            });
         
         //Get the locale we wish to mutate.
         var itLocale = Database.GetLocale("it");

         //get the locale's name category
         var namesObject = itLocale["name"] as BObject;

         //In the name category, over-write the first_name category
         namesObject["first_name"] = firstNames;

         //now test everything. get the 'it' locale.
         var nameDataSet = new Name("it");

         //get 10 names,
         var namesFromDataSet = Make(10, () => nameDataSet.FirstName())
            .Distinct();

         //and we should only have the one's we have patched.
         namesFromDataSet.Should().BeEquivalentTo(names);
      }


      private void PatchEnLocaleWithExtraStuff()
      {
         var patch = CreateExtraData();
         var enLocale = Database.GetLocale("en");

         // DATA SET NAMES ARE LOWERCASE.
         enLocale.Add("extrastuff", patch);
      }

      private BObject CreateExtraData()
      {
         //patching a data set looks like:
         // { 
         //    "category1": [item1, item2,...]
         //    "category2": [item1, item2,...]
         // }
         return new BObject
            {
               {
                  "junkfood", new BArray
                     {
                        "Cookies",
                        "Pizza",
                        "Chips"
                     }
               },
               {
                  "Drink", new BArray
                     {
                        "Pepsi",
                        "Coca-Cola",
                        "Sprite"
                     }
               }
            };
      }
   }

}

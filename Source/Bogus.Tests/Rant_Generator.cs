using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rant;
using Rant.Resources;
using Xunit;

namespace Bogus.Tests
{
   public class Rant_Generator
   {
      public const string Package = "Rantionary-3.0.17.rantpkg";
      public const int Seed = 90;

      private RantEngine rant;
      private RNG rng;

      public Rant_Generator()
      {
         rant = new RantEngine();
         rng = new RNG(Seed);

         Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

         var package = RantPackage.Load(Package);

         rant.LoadPackage(package);
      }

      [Fact(Skip = "Explicit")]
      public void generate_product_reviews()
      {
         var reviews = new[]
            {
               "this $product is <adj>.",
               "I tried to <verb-violent> it but got <noun-food> all over it.",
               "i use it <timeadv-frequency> when i'm in my <noun-place-indoor>.",
               "My <noun-living-animal> loves to play with it.",
               "[vl:ending][ladd:ending;!;!!;!!!;.]The box this comes in is [num:3;5] <unit-length> by [num:5;6] <unit-length> and weights [num:10;20] <unit-weight>[lrand:ending]",
               "This $product works <advattr> well. It <adv> improves my <activity> by a lot.",
               "I saw one of these in <country> and I bought one.",
               "one of my hobbies is <hobby::=A>. and when i'm <hobby.pred::=A> this works great.",
               "It only works when I'm <country>.",
               "My neighbor <name-female> has one of these. She works as a <noun-living-job> and she says it looks <adj-appearance>.",
               "My co-worker <name-male> has one of these. He says it looks <adj-appearance>.",
               "heard about this on <musicgenre> radio, decided to give it a try.",
               "[vl:ending][ladd:ending;!;!!;!!!;.]talk about <noun-uc-emotion>[lrand:ending]"
            };

         var singles = new[]
            {
               "This $product, does exactly what it's suppose to do.",
               "SoCal cockroaches are unwelcome, crafty, and tenacious. This $product keeps them away.",
               "works okay.",
               "I saw this on TV and wanted to give it a try.",
               "This is a really good $product."
            };

         var genReviews = reviews
            .Select(rant => RantProgram.CompileString(rant))
            .SelectMany(pgm =>
               {
                  return Enumerable.Range(1, 25)
                     .Select(_ => rant.Do(pgm, rng).Main);
               })
            .Concat(singles)
            .Distinct()
            .ToList();
         Inject("en", "rant", "review", genReviews);
      }

      private void Inject(string locale, string category, string section, List<string> genReviews)
      {
         var path = $@"..\..\..\Bogus\data_extend\{locale}.locale.json";
         var json = File.ReadAllText(path);
         var j = JObject.Parse(json);
         if( j[category] is null )
            j[category] = new JObject();
         j[category][section] = JArray.FromObject(genReviews);

         File.WriteAllText(path, j.ToString(Formatting.Indented));
      }
   }
}
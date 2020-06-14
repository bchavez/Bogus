using System.Linq;
using BenchmarkDotNet.Attributes;
using Bogus;

namespace Benchmark
{
   [MarkdownExporter, MemoryDiagnoser]
   public class BenchGenerate
   {
      public class Project
      {
         public long Id { get; set; }
         public string Name { get; set; }
         public string Description { get; set; }
      }

      private static Faker<Project> FakerDefault { get; set; }
      private static Faker<Project> FakerCustom { get; set; }
      private static Faker<Project> FakerWithRules { get; set; }
      private static Faker<Project> FakerWithRulesComplex { get; set; }

      [GlobalSetup]
      public void Setup()
      {
         FakerDefault = new Faker<Project>().UseSeed(1337);
         FakerCustom = new Faker<Project>()
            .CustomInstantiator(f=> new Project())
            .UseSeed(1337);
         FakerWithRules = new Faker<Project>()
            .CustomInstantiator(f=> new Project())
            .RuleFor(p=>p.Id, f => f.IndexGlobal)
            .UseSeed(1337);
         FakerWithRulesComplex = new Faker<Project>()
            .CustomInstantiator(f=> new Project())
            .RuleFor(p=>p.Id, f => f.IndexGlobal)
            .RuleFor(p => p.Name, f => f.Person.Company.Name + f.UniqueIndex.ToString())
            .RuleFor(p => p.Description, f => f.Lorem.Paragraphs(3))
            .UseSeed(1337);
      }

//      [Benchmark]
      public void Generate_Default()
      {
         var projects = FakerDefault.Generate(10_000).ToList();
      }

//      [Benchmark]
      public void Generate_CustomInstantiator()
      {
         var projects = FakerDefault.Generate(10_000).ToList();
      }

      [Benchmark]
      public void Generate_WithRules()
      {
         var projects = FakerWithRules.Generate(10_000).ToList();
      }

      //[Benchmark]
      public void Generate_WithRulesComplex()
      {
         var projects = FakerWithRulesComplex.Generate(10_000).ToList();
      }

//      [Benchmark]
      public void Constructor()
      {
         var projects = Enumerable.Range(0, 10_000).Select(i => new Project {Id = i}).ToList();
      }
   }
}
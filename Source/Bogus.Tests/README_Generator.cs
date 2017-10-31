using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Bogus.DataSets;
using FluentAssertions;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;
using Z.ExtensionMethods;

namespace Bogus.Tests
{
   public class README_Generator
   {
      private readonly ITestOutputHelper output;

      public README_Generator(ITestOutputHelper output)
      {
         this.output = output;
      }

      [Fact]
      public void get_available_methods()
      {
         var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         var bogusXml = Path.Combine(workingDir, "Bogus.XML");
         var x = XElement.Load(bogusXml);
         var json = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeXNode(x));

         var all = json.SelectTokens("doc.members.member").SelectMany(jt => jt)
            .Select(m =>
               {
                  var member = m["@name"];
                  var summary = m["summary"];
                  if( member == null || summary == null ) return null;

                  var declare = member.ToString();
                  var argPos = declare.IndexOf('(');
                  if( argPos > 0 )
                  {
                     declare = declare.Substring(0, argPos);
                  }
                  if( !declare.StartsWith("M:Bogus.DataSets.") ) return null;

                  var method = declare.TrimStart('M', ':');
                  method = method.Replace("Bogus.DataSets.", "");

                  var methodSplit = method.Split('.');

                  var dataset = methodSplit[0];
                  var call = methodSplit[1];

                  if( call == "#ctor" ) return null;

                  return new {dataset = dataset, method = call, summary = summary.ToString().Trim()};
               })
            .Where(a => a != null)
            .GroupBy(k => k.dataset)
            .OrderBy(k => k.Key);


         //get all publicly accessible types.
         var datasets = typeof(DataSet).Assembly.ExportedTypes
            .Where(t => typeof(DataSet).IsAssignableFrom(t) && t != typeof(DataSet))
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Select(mi => new {dataset = mi.DeclaringType.Name, method = mi.Name})
            .GroupBy(g => g.dataset, u => u.method)
            .ToDictionary(g => g.Key);

         foreach( var g in all )
         {
            if( !datasets.ContainsKey(g.Key) ) return; //check if it's accessible
            var methods = datasets[g.Key];

            var distinctMethods = g.DistinctBy(u => u.method);

            output.WriteLine("* **`" + g.Key + "`**");
            foreach( var m in distinctMethods )
            {
               if( !methods.Any(s => s.Contains(m.method)) ) continue; //check if it's accessible
               output.WriteLine("\t* `" + m.method + "` - " + m.summary);
            }
         }
      }

      [Fact]
      public void get_all_locales()
      {
         var data = Database.Data.Value;

         var locales = new List<string>();

         int count = 0;

         //load all locales
         Database.GetAllLocales().Select(Database.GetLocale).ToArray();

         var lcs = Database.Data.Value.OrderBy(kv => kv.Key).Select(kv =>
            {
               count++;
               var code = kv.Key;
               var title = kv.Value["title"].StringValue;
               title = title.Replace("Ελληνικά", "Greek");

               return new {code, title};
            }).ToArray();

         var col1 = lcs.Take(lcs.Length / 2 + lcs.Length % 2).ToArray();
         var col2 = lcs.Skip(lcs.Length / 2 + lcs.Length % 2).ToArray();

         for( int i = 0; i < col1.Length; i++ )
         {
            var c1 = col1[i];
            var c2 = i == col2.Length ? null : col2[i];

            var c2code = c2 is null ? string.Empty : $"`{c2.code,-14}`";
            var c2title = c2 is null ? string.Empty : $"{c2.title,-26}";

            var str = $"|`{c1.code,-14}`|{c1.title,-26}||{c2code}|{c2title}|";
            locales.Add(str);
         }


         //make sure # of embedded locales matches the number of imported on disk.
         var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         var dataDir = Path.Combine(workingDir, @"..\..\..\Bogus\data");
         count.Should().Be(Directory.GetFiles(dataDir, "*.locale.json").Length);

         output.WriteLine(string.Join("\n", locales));
      }

      [Fact]
      public void get_extension_namespaces()
      {
         var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         var bogusXml = Path.Combine(workingDir, "Bogus.XML");
         var x = XElement.Load(bogusXml);
         var json = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeXNode(x));

         var all = json.SelectTokens("doc.members.member").SelectMany(jt => jt)
            .Select(m =>
               {
                  var member = m["@name"];
                  var summary = m["summary"];
                  if( member == null || summary == null ) return null;

                  var declare = member.ToString();
                  if( !declare.StartsWith("M:Bogus.Extensions.") ) return null;

                  var method = declare.TrimStart('M', ':');
                  if( method.Contains("#ctor") ) return null;

                  //Bogus.Extensions.Canada.ExtensionsForCanada.Sin(Bogus.Person)
                  var ns = method.GetBetween("Bogus.Extensions.", ".ExtensionsFor");
                  if( ns.IsNullOrEmpty() )
                  {
                     return null;
                  }
                  ns = $"Bogus.Extensions.{ns}";
                  var em = method.GetAfter("ExtensionsFor").GetAfter(".");

                  return new {ns = ns, em = em, summary = summary.ToString().Trim()};
               })
            .Where(a => a != null)
            .GroupBy(k => k.ns)
            .OrderBy(k => k.Key);


         foreach( var g in all )
         {
            output.WriteLine("* **`using " + g.Key + ";`**");
            foreach( var i in g )
            {
               var method = i.em.GetBefore("(");
               var objectExtends = i.em.GetBetween("(", ")");
               if( objectExtends.Contains(",") )
                  objectExtends = objectExtends.GetBefore(",");
               output.WriteLine($"\t* `{objectExtends}.{method}()` - {i.summary}");
            }
         }
      }
   }
}
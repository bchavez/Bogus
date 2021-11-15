using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
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

      public class Record
      {
         public string Dataset;
         public string Method;
         public string Summary;
      }

      [Fact]
      public void get_available_methods()
      {
         var (_, buildDir) = GetWorkingFolders();
         var bogusXml = Path.Combine(buildDir, "Bogus.XML");
         var xml = XDocument.Load(bogusXml);

         var nav = xml.CreateNavigator();
         var sel = nav.Select("/doc/members/member");

         var list = new List<Record>();

         foreach (XPathNavigator node in sel)
         {
            if( !node.HasAttributes ) continue;

            var member = node.GetAttribute("name", "");
            var summaryNode = node.SelectSingleNode("summary");
            if (summaryNode == null) continue;

            var summary = summaryNode.ExtractContent()
               .Replace("`System.", "`");

            var declare = member;
            var argPos = declare.IndexOf('(');
            if (argPos > 0)
            {
               declare = declare.Substring(0, argPos);
            }

            if( !declare.StartsWith("M:Bogus.DataSets.") ) continue;

            var method = declare.TrimStart('M', ':');
            method = method.Replace("Bogus.DataSets.", "");

            var methodSplit = method.Split('.');

            var dataset = methodSplit[0];
            var call = methodSplit[1];

            if (call == "#ctor") continue;

            var r = new Record
               {
                  Dataset = dataset,
                  Method = call,
                  Summary = summary
               };
            list.Add(r);
         }

         var all = list
            .GroupBy(k => k.Dataset)
            .OrderBy(k => k.Key);

         //get all publicly accessible types.
         var datasets = typeof(DataSet).Assembly.ExportedTypes
            .Where(t => typeof(DataSet).IsAssignableFrom(t) && t != typeof(DataSet))
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Where( m => m.GetCustomAttribute<ObsoleteAttribute>() == null)
            .Select(mi => new {dataset = mi.DeclaringType.Name, method = mi.Name})
            .GroupBy(g => g.dataset, u => u.method)
            .ToDictionary(g => g.Key);

         foreach( var g in all )
         {
            if( !datasets.ContainsKey(g.Key) ) return; //check if it's accessible
            var methods = datasets[g.Key];

            var distinctMethods = MoreEnumerable.DistinctBy(g, u => u.Method);

            output.WriteLine("* **`" + g.Key + "`**");
            foreach( var m in distinctMethods )
            {
               if( !methods.Any(s => s.Contains(m.Method)) ) continue; //check if it's accessible
               output.WriteLine("\t* `" + m.Method + "` - " + m.Summary);
            }
         }
      }


      [Fact]
      public void get_randomizer_methods()
      {
         var (_, buildDir) = GetWorkingFolders();
         var bogusXml = Path.Combine(buildDir, "Bogus.XML");
         var xml = XDocument.Load(bogusXml);

         var nav = xml.CreateNavigator();
         var sel = nav.Select("/doc/members/member");

         var list = new List<Record>();

         foreach (XPathNavigator node in sel)
         {
            if (!node.HasAttributes) continue;

            var member = node.GetAttribute("name", "");
            var summaryNode = node.SelectSingleNode("summary");
            if (summaryNode == null) continue;

            var summary = summaryNode.ExtractContent()
               .Replace("`System.", "`");

            var declare = member;
            var argPos = declare.IndexOf('(');
            if (argPos > 0)
            {
               declare = declare.Substring(0, argPos);
            }

            if (!declare.StartsWith("M:Bogus.Randomizer.")) continue;

            if( summary.Contains("\r") )
               summary = summary.GetBefore("\r");

            var method = declare.TrimStart('M', ':');
            method = method.Replace("Bogus.", "");

            var methodSplit = method.Split('.');

            var dataset = methodSplit[0];
            var call = methodSplit[1];

            if (call == "#ctor") continue;

            call = call.Replace("``1", "<T>");

            var r = new Record
            {
               Dataset = dataset,
               Method = call,
               Summary = summary
            };
            list.Add(r);
         }

         var all = list
            .GroupBy(k => k.Dataset)
            .OrderBy(k => k.Key);

         //get all publicly accessible types.
         var publicMethods = typeof(Randomizer)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(mi => new {dataset = mi.DeclaringType.Name, method = mi.Name});
            //.GroupBy(g => g.dataset, u => u.method)
            //.ToDictionary(g => g.Key);

         foreach (var g in all)
         {
            //if (!datasets.ContainsKey(g.Key)) return; //check if it's accessible
            var sortedMethods = g
               .OrderBy(x => x.Method)
               .ThenBy(x => x.Summary.Length);
            var distinctMethods = MoreEnumerable.DistinctBy(sortedMethods, u => u.Method);
            //we need to do this ordering so we select the most
            //succinct description for any method overloads.

            //then just preserve the ordering as source code in source code
            distinctMethods = g.Intersect(distinctMethods);

            output.WriteLine("* **`Random`/`" + g.Key + "`**");
            foreach (var m in distinctMethods)
            {
               if (!publicMethods.Any(s => m.Method.Contains(s.method))) continue; //check if it's accessible
               output.WriteLine("\t* `" + m.Method + "` - " + m.Summary);
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
         //var workingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         var (projectDir, _) = GetWorkingFolders();
         var dataDir = projectDir.PathCombine(@"..\Bogus\data");
         count.Should().Be(Directory.GetFiles(dataDir, "*.locale.json").Length);

         output.WriteLine(string.Join("\n", locales));
      }

      [Fact]
      public void get_extension_namespaces()
      {
         var (_, buildDir) = GetWorkingFolders();
         var bogusXml = Path.Combine(buildDir, "Bogus.XML");
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


      //ReSharper, .NET Full Framework, and .NET Core all 
      //screw up the working folder path; this brings some
      //sanity back. Very hack, but works.
      private (string projectDir, string buildDir) GetWorkingFolders()
      {
         string FindRoot(string path)
         {
            if( path.ToUpperInvariant().EndsWith("BOGUS.TESTS") )
               return path;

            return FindRoot(Path.GetFullPath(path.PathCombine("..")));
         }

         var asmLoc = typeof(README_Generator).GetTypeInfo().Assembly.Location;
         var asmDir = Path.GetDirectoryName(asmLoc);
         var iniFile = Path.Combine(asmDir, "__AssemblyInfo__.ini");
         if( File.Exists(iniFile) )
         {
            var content = File.ReadAllText(iniFile, Encoding.Unicode);
            var file = content.GetAfter("file:///").GetBefore("\0");
            return (FindRoot(file), Path.GetDirectoryName(file));
         }

         return (FindRoot(asmLoc), Path.GetDirectoryName(asmLoc));
      }
   }


   public static class XmlExtensions
   {
      private static Regex ParamPattern = new Regex(@"<(see|paramref) (name|cref)=""([TPF]{1}:)?(?<display>.+?)"" />");
      private static Regex ConstPattern = new Regex(@"<c>(?<display>.+?)</c>");
      /// <summary>
      /// Extracts the display content of the specified <paramref name="node"/>, replacing
      /// paramref and c tags with a human-readable equivalent.
      /// </summary>
      /// <param name="node">The XML node from which to extract content.</param>
      /// <returns>The extracted content.</returns>
      public static string ExtractContent(this XPathNavigator node)
      {
         if (node == null) return null;
         return ConstPattern.Replace(
            ParamPattern.Replace(node.InnerXml, GetParamRefName),
            GetConstRefName).Trim();
      }
      private static string GetConstRefName(Match match)
      {
         if (match.Groups.Count != 2) return null;
         return match.Groups["display"].Value;
      }
      private static string GetParamRefName(Match match)
      {
         if (match.Groups.Count != 5) return null;
         return "`" + match.Groups["display"].Value + "`";
      }
   }
}
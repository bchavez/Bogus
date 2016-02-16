using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Bogus.Tests
{
    [TestFixture]
    public class README_Generator
    {
        [Test]
        public void get_available_methods()
        {
            var x = XElement.Load(@"Bogus.XML");
            var json = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeXNode(x));

            var all = json.SelectTokens("doc.members.member").SelectMany(jt => jt)
                .Select(m =>
                {
                    var member = m["@name"];
                    var summary = m["summary"];
                    if (member == null || summary == null) return null;

                    var declare = member.ToString();
                    var argPos = declare.IndexOf('(');
                    if (argPos > 0)
                    {
                        declare = declare.Substring(0, argPos);
                    }
                    if (!declare.StartsWith("M:Bogus.DataSets.")) return null;

                    var method = declare.TrimStart('M', ':');
                    method = method.Replace("Bogus.DataSets.", "");

                    var methodSplit = method.Split('.');

                    var dataset = methodSplit[0];
                    var call = methodSplit[1];

                    if (call == "#ctor") return null;

                    return new { dataset = dataset, method = call, summary = summary.ToString().Trim() };
                })
                .Where(a => a != null)
                .GroupBy(k => k.dataset)
                .OrderBy(k => k.Key);

            foreach (var g in all)
            {
                Console.WriteLine("* **`" + g.Key + "`**");
                foreach (var m in g)
                {
                    Console.WriteLine("\t* `" + m.method + "` - " + m.summary);
                }
            }
        }

        [Test]
        public void get_all_locales()
        {
            var data = Database.Data.Value;

            var locales = new List<string>();

            int count = 0;

            foreach (var prop in data.Properties().OrderBy(p => p.Name))
            {
                count++;

                var code = prop.Name;
                var title = prop.First["title"].ToString();

                title = title.Replace("Ελληνικά", "Greek");

                var str = string.Format("|{0,-14}|{1}", "`" + code + "`", title);
                locales.Add(str);
            }

            //make sure # of embedded locales matches the number of imported on disk.
            count.Should().Be(Directory.GetFiles(@"..\..\..\Bogus\data", "*.locale.json").Length);

            Console.WriteLine(string.Join("\n", locales));
        }
        
    }

}
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Builder.Extensions;
using FluentBuild;
using JavaScriptEngineSwitcher.Jint;
using JavaScriptEngineSwitcher.Jint.Configuration;
using MoreLinq;
using Newtonsoft.Json;

namespace Builder.Tasks
{
    public class LocaleBuildTask : BuildFile
    {
        private FluentFs.Core.Directory DataDir = Projects.BogusProject.Folder.SubFolder("data");
        private FluentFs.Core.Directory SourceDir = Projects.BogusProject.Folder.SubFolder("locales");

        public LocaleBuildTask()
        {
            AddTask("clean", Clean);
            AddTask("build", BuildLocales);
        }

        private void BuildLocales()
        {
            //wipe all json files.
            var locales = SourceDir.Files("*.js").Files;

            foreach( var locale in locales )
            {
                Defaults.Logger.Write("LOCALE", "Reading: " + locale);
                var data = System.IO.File.ReadAllLines(locale, Encoding.UTF8)
                    .ToList();
                data.RemoveAt(1); //remove modules line

                var clean = string.Join("\n", data);

                using( var engine = new JintJsEngine(new JintConfiguration() {EnableDebugging = true}) )
                {
                    engine.Execute(clean);

                    var localeName = System.IO.Path.GetFileNameWithoutExtension(locale);

                    var language = engine.GetVariableValue(localeName);

                    var localeJson = JsonConvert.SerializeObject(new Dictionary<string, object>
                        {
                            {localeName, language}
                        }, Formatting.Indented);

                    var pathJson = DataDir.File(localeName + ".locale.json").Path;
                    System.IO.File.WriteAllText(pathJson, localeJson);
                }
            }
        }

        private void Clean()
        {
            DataDir.Files("*.json")
                .Files
                .Select(j => new Fluent.IO.Path(j))
                .ForEach(p =>
                    {
                        p.Delete();
                    });
        }
    }
}

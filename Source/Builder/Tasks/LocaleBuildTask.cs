using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Builder.Extensions;
using FluentBuild;
using JavaScriptEngineSwitcher.Jint;
using JavaScriptEngineSwitcher.Jint.Configuration;
using Newtonsoft.Json;

namespace Builder.Tasks
{
    public class LocaleBuildTask : BuildFile
    {
        private FluentFs.Core.Directory ProjectLocale = Projects.FluentFakerProject.Folder.SubFolder("data");

        public LocaleBuildTask()
        {
            AddTask("clean", Clean);
            AddTask("build", BuildLocales);
        }

        private void BuildLocales()
        {
            var locales = Folders.Source.SubFolder("fakerjs").SubFolder("locales").Files("*.js").Files;

            var dictionary = new Dictionary<string, object>();

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

                    dictionary.Add(localeName, language);
                }
            }

            var allLocales = JsonConvert.SerializeObject(dictionary, Formatting.Indented);

            var path = ProjectLocale.File("locales.json").Path;

            System.IO.File.WriteAllText(path, allLocales);
        }

        private void Clean()
        {
            ProjectLocale.Wipe();
        }
    }
}
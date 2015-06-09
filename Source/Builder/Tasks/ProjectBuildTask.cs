using System;
using Builder.Extensions;
using Fluent.IO;
using FluentBuild;

namespace Builder.Tasks
{
    public class ProjectBuildTask : BuildFile
    {
        public ProjectBuildTask()
        {
            AddTask("clean", Clean);
            AddTask("build", CompileSources);
        }

        public void Clean()
        {
            Folders.CompileOutput.Wipe();
            Folders.Package.Wipe();
        }

        public void CompileSources()
        {
            //File assemblyInfoFile = Folders.CompileOutput.File("Global.AssemblyInfo.cs");

            Task.CreateAssemblyInfo.Language.CSharp(aid =>
                {
                    Projects.BogusProject.AssemblyInfo(aid);
                    aid.OutputPath(Projects.BogusProject.Folder.SubFolder("Properties").File("AssemblyInfo.cs"));
                });


            Task.Build.MsBuild(msb =>
                {
                    msb.Configuration("Release")
                        .ProjectOrSolutionFilePath(Projects.BogusProject.ProjectFile)
                        .AddTarget("Rebuild")
                        .OutputDirectory(Projects.BogusProject.OutputDirectory);
                });

            Defaults.Logger.WriteHeader("BUILD COMPLETE. Packaging ...");

            //copy compile directory to package directory
            Path.Get(Projects.BogusProject.OutputDirectory.ToString())
                .Copy(Projects.BogusProject.PackageDir.ToString(), Overwrite.Always, true);

            string version = Properties.CommandLineProperties.Version();

            Defaults.Logger.Write("RESULTS", "NuGet packing");

            Path nuget = Path.Get(Folders.Lib.ToString())
                .Files("NuGet.exe", true).First();

            Task.Run.Executable(e => e.ExecutablePath(nuget.FullPath)
                .WithArguments("pack", Projects.BogusProject.NugetSpec.Path, "-Version", version, "-OutputDirectory",
                    Folders.Package.ToString()));

            Defaults.Logger.Write("RESULTS", "Setting NuGet PUSH script");


            //Defaults.Logger.Write( "RESULTS", pushcmd );
            System.IO.File.WriteAllText("nuget.push.bat",
                "{0} push {1}".With(nuget.MakeRelative().ToString(),
                    Path.Get(Projects.BogusProject.NugetNupkg.ToString()).MakeRelative().ToString()) +
                Environment.NewLine);
        }
    }
}

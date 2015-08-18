using System;
using System.Collections.Generic;
using System.Diagnostics;
using Builder.Extensions;
using Fluent.IO;
using FluentBuild;
using NUnit.Framework;

namespace Builder.Tasks
{
    [TestFixture]
    public class BuildTasks
    {
        [TestFixtureSetUp]
        public void BeforeRunningTestSession()
        {
            Defaults.Logger.Verbosity = VerbosityLevel.Full;

            System.IO.Directory.SetCurrentDirectory( @"..\..\..\.." );

            var version = Environment.GetEnvironmentVariable("BOGUS_VERSION");
            if( string.IsNullOrWhiteSpace(version) )
                version = "0.0.0.0";

            Properties.CommandLineProperties.Add("Version", version);
        }

        [Test]
        [Explicit]
        public void Clean()
        {
            Folders.CompileOutput.Wipe();
            Folders.Package.Wipe();
        }

        [Test]
        [Explicit]
        public void Prep()
        {
            //File assemblyInfoFile = Folders.CompileOutput.File("Global.AssemblyInfo.cs");
            Task.CreateAssemblyInfo.Language.CSharp(aid =>
                {
                    Projects.BogusProject.AssemblyInfo(aid);
                    aid.OutputPath(Projects.BogusProject.Folder.SubFolder("Properties").File("AssemblyInfo.cs"));
                });


            //build the command line switch for msbuild
            //msbuild Source\Bogus.sln /target:Rebuild /property:Configuration=Release;OutDir=c:\temp\outout
            var msBuildArgs = new List<string>
                {
                    $"{Projects.SolutionFile}",
                    $"/target:Rebuild",
                    $"/property:Configuration=Release;OutDir={Projects.BogusProject.OutputDirectory}"
                };

            var msBuildArgsLine = string.Join(" ", msBuildArgs);
            System.IO.File.WriteAllText("build.MSBuildArgs", msBuildArgsLine);
        }

        [Test]
        [Explicit]
        public void Package()
        {
            Defaults.Logger.WriteHeader("PACKAGE");
            //copy compile directory to package directory
            Fluent.IO.Path.Get(Projects.BogusProject.OutputDirectory.ToString())
                .Copy(Projects.BogusProject.PackageDir.ToString(), Overwrite.Always, true);

            string version = Properties.CommandLineProperties.Version();

            Defaults.Logger.Write("RESULTS", "NuGet packing");

            Fluent.IO.Path nuget = Fluent.IO.Path.Get(Folders.Lib.ToString())
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
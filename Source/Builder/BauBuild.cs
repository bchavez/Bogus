using System;
using System.IO;
using BauCore;
using BauMSBuild;
using BauNuGet;
using Builder.Utils;
using FluentAssertions;
using FluentBuild;
using FluentFs.Core;
using Directory = System.IO.Directory;

namespace Builder
{
	public static class BauBuild
	{
		//Build Tasks
		public const string Build = "build";
		public const string Clean = "clean";
		public const string Restore = "restore";
		public const string BuildInfo = "buildinfo";
		public const string Pack = "pack";
		public const string Push = "push";

		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~");
				Console.WriteLine("     BUILDER ERROR    ");
				Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~");
				Console.WriteLine(e.ExceptionObject);
				Environment.Exit(1);
			};

			var nugetExe = FindNugetExe();

			new Bau(Arguments.Parse(args))
				.DependsOn(Clean, Restore, Build)
				.MSBuild(Build).Desc("Invokes MSBuild to build solution")
				.DependsOn(Clean, BuildInfo)
				.Do(msb =>
				{
					msb.ToolsVersion = "14.0";
					msb.Solution = Projects.SolutionFile.ToString();
					msb.Properties = new
					{
						Configuration = "Release",
						OutDir = Folders.CompileOutput
					};
					msb.Targets = new[] { "Rebuild" };
				})
				.Task(BuildInfo).Desc("Creates dynamic AssemblyInfos for projects")
				.Do(() =>
				{
					Task.CreateAssemblyInfo.Language.CSharp(aid =>
					{
						Projects.BogusProject.AssemblyInfo(aid);
						var outputPath = Projects.BogusProject.Folder.SubFolder("Properties").File("AssemblyInfo.cs");
						Console.WriteLine($"Creating AssemblyInfo file: {outputPath}");
						aid.OutputPath(outputPath);
					});
				})
				.Task(Clean).Desc("Cleans project files")
				.Do(() =>
				{
					Console.WriteLine($"Removing {Folders.CompileOutput}");
					Folders.CompileOutput.Wipe();
					Directory.CreateDirectory(Folders.CompileOutput.ToString());
					Console.WriteLine($"Removing {Folders.Package}");
					Folders.Package.Wipe();
					Directory.CreateDirectory(Folders.Package.ToString());
				})
				.NuGet(Pack).Desc("Packs NuGet packages")
				.DependsOn(Build).Do(ng =>
				{
                    var nuspec = Projects.BogusProject.NugetSpec.WithExt("history.nuspec");
                    nuspec.Delete(OnError.Continue);

                    Projects.BogusProject.NugetSpec
                        .Copy
                        .ReplaceToken("history")
                        .With(History.NugetText())
                        .To(nuspec.ToString());

                    ng.Pack(nuspec.ToString(),
						p =>
						{
							p.BasePath = Folders.CompileOutput.ToString();
							p.Version = BuildContext.FullVersion;
							p.Symbols = true;
							p.OutputDirectory = Folders.Package.ToString();
						})
						.WithNuGetExePathOverride(nugetExe.FullName);
				})
				.NuGet(Push).Desc("Pushes NuGet packages")
				.DependsOn(Pack).Do(ng =>
				{
					ng.Push(Projects.BogusProject.NugetNupkg.ToString())
						.WithNuGetExePathOverride(nugetExe.FullName);
				})
				.NuGet(Restore).Desc("Restores NuGet packages")
				.Do(ng =>
				{
					ng.Restore(Projects.SolutionFile.ToString())
						.WithNuGetExePathOverride(nugetExe.FullName);
				})

				.Run();
		}

		private static FileInfo FindNugetExe()
		{
			Directory.SetCurrentDirectory(Folders.Lib.ToString());
			var nugetExe = NuGetFileFinder.FindFile();
			nugetExe.Should().NotBeNull();
			Directory.SetCurrentDirectory(Folders.WorkingFolder.ToString());
			return nugetExe;
		}
	}
}
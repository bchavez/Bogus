using System;
using Builder.Utils;
using FluentBuild;
using FluentBuild.AssemblyInfoBuilding;
using FluentFs.Core;

namespace Builder
{
    public class Folders
    {
        public static readonly Directory WorkingFolder = new Directory( Properties.CurrentDirectory );
        public static readonly Directory CompileOutput = WorkingFolder.SubFolder( "__compile" );
        public static readonly Directory Package = WorkingFolder.SubFolder( "__package" );
        public static readonly Directory Source = WorkingFolder.SubFolder( "source" );
		public static readonly Directory Builder = Source.SubFolder("Builder");

		public static readonly Directory Lib = Source.SubFolder( "packages" );
    }

	public class BuildContext
	{
		public static readonly string FullVersion = Environment.GetEnvironmentVariable("BUILD_VERSION").Trim() ?? "0.0.0.0";
		public static readonly string Version = FullVersion.WithoutPreReleaseName();
		public static readonly DateTime BuildDate = DateTime.UtcNow;
	}

	public class Projects
    {
        private static void GlobalAssemblyInfo(IAssemblyInfoDetails aid)
        {
            aid.Company( "Brian Chavez" )
               .Copyright( "Brian Chavez © " + BuildContext.BuildDate.Year)
               .Version( BuildContext.Version )
               .FileVersion( BuildContext.Version )
               .InformationalVersion( $"{BuildContext.FullVersion} built on {BuildContext.BuildDate} UTC" )
               .Trademark( "MIT License" )
               .Description( "http://www.github.com/bchavez/Bogus" )
			   .AddCustomAttribute("System.Runtime.CompilerServices", "InternalsVisibleTo", true, "Bogus.Tests")
			   .ComVisible(false);
        }

        public static readonly File SolutionFile = Folders.Source.File( "Bogus.sln" );

        public class BogusProject
        {
            public const string Name = "Bogus";
            public static readonly Directory Folder = Folders.Source.SubFolder( Name );
            public static readonly File ProjectFile = Folder.File( $"{Name}.csproj" );
            public static readonly Directory OutputDirectory = Folders.CompileOutput.SubFolder( Name );
            public static readonly File OutputDll = OutputDirectory.File( $"{Name}.dll" );
            public static readonly Directory PackageDir = Folders.Package.SubFolder( Name );
            
            public static readonly File NugetSpec = Folders.Builder.SubFolder("NuGet").File( $"{Name}.nuspec" );
            public static readonly File NugetNupkg = Folders.Package.File( $"{Name}.{BuildContext.FullVersion}.nupkg");

            public static readonly Action<IAssemblyInfoDetails> AssemblyInfo =
                i =>
                    {
                        i.Title("Bogus Fake Data Generator for .NET")
                            .Product(Name);

                        GlobalAssemblyInfo(i);
                    };
        }

        public class Tests
        {
            public static readonly Directory Folder = Folders.Source.SubFolder( "Bogus.Tests" );
        }
    }


}

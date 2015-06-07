using System;
using Builder.Extensions;
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

        public static readonly Directory Lib = Source.SubFolder( "packages" );
    }

    public class Projects
    {
        private static void GlobalAssemblyInfo(IAssemblyInfoDetails aid)
        {
            aid.Company( "Brian Chavez" )
               .Copyright( "Brian Chavez © " + DateTime.UtcNow.Year )
               .Version( Properties.CommandLineProperties.Version() )
               .FileVersion( Properties.CommandLineProperties.Version() )
               .InformationalVersion( "{0} built on {1} UTC".With( Properties.CommandLineProperties.Version(), DateTime.UtcNow ) )
               .Trademark( "MIT License" )
               .Description( "http://www.github.com/bchavez/FluentFaker" )
               .ComVisible(false);
        }

        public static readonly File SolutionFile = Folders.Source.File( "FluentFaker.sln" );

        public class FluentFakerProject
        {
            public static readonly Directory Folder = Folders.Source.SubFolder( "FluentFaker" );
            public static readonly File ProjectFile = Folder.File( "FluentFaker.csproj" );
            public static readonly Directory OutputDirectory = Folders.CompileOutput.SubFolder( "FluentFaker" );
            public static readonly File OutputDll = OutputDirectory.File( "FluentFaker.dll" );
            public static readonly Directory PackageDir = Folders.Package.SubFolder( "FluentFaker" );
            
            public static readonly File NugetSpec = Folders.Source.SubFolder(".nuget").File( "FluentFaker.nuspec" );
            public static readonly File NugetNupkg = Folders.Package.File( "FluentFaker.{0}.nupkg".With( Properties.CommandLineProperties.Version() ) );

            public static readonly Action<IAssemblyInfoDetails> AssemblyInfo =
                i =>
                    {
                        i.Title("FluentFaker API for .NET")
                            .Product("FluentFaker API");

                        GlobalAssemblyInfo(i);
                    };
        }

        public class Tests
        {
            public static readonly Directory Folder = Folders.Source.SubFolder( "FluentFaker.Tests" );
        }
    }


}

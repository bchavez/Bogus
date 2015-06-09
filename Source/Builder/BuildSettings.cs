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
               .Description( "http://www.github.com/bchavez/Bogus" )
               .ComVisible(false);
        }

        public static readonly File SolutionFile = Folders.Source.File( "Bogus.sln" );

        public class BogusProject
        {
            public static readonly Directory Folder = Folders.Source.SubFolder( "Bogus" );
            public static readonly File ProjectFile = Folder.File( "Bogus.csproj" );
            public static readonly Directory OutputDirectory = Folders.CompileOutput.SubFolder( "Bogus" );
            public static readonly File OutputDll = OutputDirectory.File( "Bogus.dll" );
            public static readonly Directory PackageDir = Folders.Package.SubFolder( "Bogus" );
            
            public static readonly File NugetSpec = Folders.Source.SubFolder(".nuget").File( "Bogus.nuspec" );
            public static readonly File NugetNupkg = Folders.Package.File( "Bogus.{0}.nupkg".With( Properties.CommandLineProperties.Version() ) );

            public static readonly Action<IAssemblyInfoDetails> AssemblyInfo =
                i =>
                    {
                        i.Title("Bogus Fake Data Generator for .NET")
                            .Product("Bogus");

                        GlobalAssemblyInfo(i);
                    };
        }

        public class Tests
        {
            public static readonly Directory Folder = Folders.Source.SubFolder( "Bogus.Tests" );
        }
    }


}

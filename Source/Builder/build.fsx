//#if INTERACTIVE
//open System
//Environment.CurrentDirectory <- workingDir
//#else
//#endif

// include Fake lib
#I @"../packages/build/FAKE/tools"
#I @"../packages/build/DotNetZip/lib/net20"
#r @"FakeLib.dll"
#r @"DotNetZip.dll"

#load @"Utils.fsx"

open Fake
open Utils
open System.Reflection
open Helpers
open Fake.Testing.NUnit3

let workingDir = ChangeWorkingFolder();

trace (sprintf "WORKING DIR: %s" workingDir)

let ProjectName = "Bogus";
let GitHubUrl = "https://github.com/bchavez/Bogus"

let Folders = Setup.Folders(workingDir)
let Files = Setup.Files(Folders)
let Projects = Setup.Projects(ProjectName, Folders)

let BogusProject = NugetProject("Bogus", "Bogus Fake Data Generator for .NET", Folders)
let TestProject = TestProject("Bogus.Tests", Folders)



Target "msb" (fun _ ->
    
    let tag = "msb_build";

    let buildProps = [ 
                        "AssemblyOriginatorKeyFile", Projects.SnkFile
                        "SignAssembly", BuildContext.IsTaggedBuild.ToString()
                     ]

    !! BogusProject.ProjectFile
    |> MSBuildReleaseExt (BogusProject.OutputDirectory @@ tag) buildProps "Build"
    |> Log "AppBuild-Output: "


    !! TestProject.ProjectFile
    |> MSBuild "" "Build" (("Configuration", "Debug")::buildProps)
    |> Log "AppBuild-Output: "
)



Target "dnx" (fun _ ->
    trace "DNX Build Task"

    let tag = "dnx_build"
    
    Dotnet DotnetCommands.Restore BogusProject.Folder
    //Dotnet DotnetCommands.Restore TestProject.Folder
    DotnetBuild BogusProject (BogusProject.OutputDirectory @@ tag)
)

Target "restore" (fun _ -> 
     trace "MS NuGet Project Restore"
     let lookIn = Folders.Lib @@ "build"
     let toolPath = findToolInSubPath "NuGet.exe" lookIn

     tracefn "NuGet Tool Path: %s" toolPath

     Projects.SolutionFile
     |> RestoreMSSolutionPackages (fun p ->
            { 
              p with 
                OutputPath = (Folders.Source @@ "packages" )
                ToolPath = toolPath
            }
        )

     trace ".NET Core Restore"
     Dotnet DotnetCommands.Restore BogusProject.Folder
 )

open Ionic.Zip
open System.Xml

Target "nuget" (fun _ ->
    trace "NuGet Task"
    
    DotnetPack BogusProject Folders.Package   
)

Target "push" (fun _ ->
    trace "NuGet Push Task"
    
    failwith "Only CI server should publish on NuGet"
)



Target "zip" (fun _ -> 
    trace "Zip Task"

    !!(BogusProject.OutputDirectory @@ "**") |> Zip Folders.CompileOutput (Folders.Package @@ BogusProject.Zip)
)

open AssemblyInfoFile

let MakeAttributes (includeSnk:bool) =
    let attrs = [
                    Attribute.Description GitHubUrl
                ]
    if includeSnk then
        let pubKey = ReadFileAsHexString Projects.SnkFilePublic
        let visibleTo = sprintf "%s, PublicKey=%s" TestProject.Name pubKey
        attrs @ [ Attribute.InternalsVisibleTo(visibleTo) ]
    else
        attrs @ [ Attribute.InternalsVisibleTo(TestProject.Name) ]


Target "BuildInfo" (fun _ ->
    
    trace "Writing Assembly Build Info"

    MakeBuildInfo BogusProject Folders (fun bip -> 
        { bip with
            ExtraAttrs = MakeAttributes(BuildContext.IsTaggedBuild) } )

    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/Version" BuildContext.FullVersion

    let releaseNotes = History.NugetText Files.History GitHubUrl
    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/PackageReleaseNotes" releaseNotes
)


Target "Clean" (fun _ ->
    DeleteFile Files.TestResultFile
    CleanDirs [Folders.CompileOutput; Folders.Package]

    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/Version" "0.0.0-localbuild"
    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/PackageReleaseNotes" ""
    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/AssemblyOriginatorKeyFile" ""
    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/SignAssembly" "false"

    MakeBuildInfo BogusProject Folders (fun bip ->
         {bip with
            DateTime = System.DateTime.Parse("1/1/2015")
            ExtraAttrs = MakeAttributes(false) } )

)

open Fake.Testing

let RunTests() =
    CreateDir Folders.Test
    let xunit = findToolInSubPath "xunit.console.exe" Folders.Lib

    !! TestProject.TestAssembly
    |> xUnit2 (fun p -> { p with 
                            ToolPath = xunit
                            ShadowCopy = false
                            XmlOutputPath = Some(Files.TestResultFile)
                            ErrorLevel = TestRunnerErrorLevel.Error }) 


open Fake.AppVeyor

Target "ci" (fun _ ->
    trace "ci Task"
)

Target "test" (fun _ ->
    trace "TEST"
    RunTests()
)

Target "citest" (fun _ ->
    trace "CI TEST"
    RunTests()
    UploadTestResultsXml TestResultsType.Xunit Folders.Test
)


Target "setup-snk"(fun _ ->
    trace "Decrypting Strong Name Key (SNK) file."
    let decryptSecret = environVarOrFail "SNKFILE_SECRET"
    decryptFile Projects.SnkFile decryptSecret

    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/AssemblyOriginatorKeyFile" Projects.SnkFile
    XmlPokeInnerText BogusProject.ProjectFile "/Project/PropertyGroup/SignAssembly" "true"
)

open LZ4;
open System.IO;

Target "compress-data" (fun _ ->
   traceHeader "Compressing data folder and removing whitespace"
   
   for file in !!(BogusProject.Folder @@ "data" @@ "*.bson") do
      traceFAKE "Compressing %s" file
      let bytes = File.ReadAllBytes(file)
      let compressedBytes = LZ4Codec.WrapHC(bytes)
      let compressedFileName = changeExt "bson.lz4" file
      File.WriteAllBytes(compressedFileName, compressedBytes)
)

Target "import-lz4" (fun _ -> 

   let files = [
            "LZ4.silverlight\LZ4Codec.silverlight.cs";
            "LZ4ps\LZ4Codec.cs";
            "LZ4ps\LZ4Codec.Safe.cs";
            "LZ4ps\LZ4Codec.Safe32.Dirty.cs";
            "LZ4ps\LZ4Codec.Safe32HC.Dirty.cs";
            "LZ4ps\LZ4Codec.Safe64.Dirty.cs";
            "LZ4ps\LZ4Codec.Safe64HC.Dirty.cs";
            "LZ4\ILZ4Service.cs";
            "LZ4\LZ4Codec.cs";
            "LZ4\LZ4Stream.cs";
            "LZ4\LZ4Stream.windows.cs";
            "LZ4\LZ4StreamFlags.cs";
            "LZ4\LZ4StreamMode.cs";
            "LZ4\Services\Safe32LZ4Service.cs";
            "LZ4\Services\Safe64LZ4Service.cs";
            ]
   let lzDir = Folders.Source @@ "lz4net" @@ "src"
   let compDir = BogusProject.Folder @@ "Compression"

   trace lzDir
   trace compDir

   files
   |> Seq.iter( fun f ->
         let src = lzDir @@ f;
         let dest = compDir @@ f;
         CreateDir (directory dest)
         CopyFile dest src)

   for file in !!(BogusProject.Folder @@ "Compression" @@ "**/*.cs") do
      traceFAKE "Enclosing namespace in %s" file
      let lines = File.ReadAllLines file 
                  |> List.ofArray
      
      let out = ["namespace Bogus.Compression {"] @ 
                  lines @ 
                  ["}"]          
         
      File.WriteAllLines(file, out)
)


"Clean"
    ==> "restore"
    ==> "BuildInfo"

//build systems, order matters
"BuildInfo"
    =?> ("setup-snk", BuildContext.IsTaggedBuild)
    ==> "dnx"
    ==> "zip"

"BuildInfo"
    =?> ("setup-snk", BuildContext.IsTaggedBuild)
    ==> "msb"
    ==> "zip"

"BuildInfo"
    =?> ("setup-snk", BuildContext.IsTaggedBuild)
    ==> "zip"

"dnx"
    ==> "nuget"


"nuget"
    ==> "ci"

"nuget"
    ==> "push"

"zip"
    ==> "ci"


//test task depends on msbuild
"msb"
    ==> "test"



// start build
RunTargetOrDefault "msb"

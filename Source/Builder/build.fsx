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
    DotnetBuild BogusProject (BogusProject.OutputDirectory @@ tag)
)

Target "mono" (fun _ ->
     trace "Mono Task"

     let tag = "mono_build/"

     //Setup
     XBuild BogusProject.ProjectFile (BogusProject.OutputDirectory @@ tag)
)

Target "restore" (fun _ -> 
     trace "MS NuGet Project Restore"
     Projects.SolutionFile
     |> RestoreMSSolutionPackages (fun p ->
            { p with OutputPath = (Folders.Source @@ "packages" )}
        )
 )

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

    JsonPoke "version" BuildContext.FullVersion BogusProject.ProjectJson

    let releaseNotes = History.NugetText Files.History GitHubUrl
    JsonPoke "packOptions.releaseNotes" releaseNotes BogusProject.ProjectJson
)


Target "Clean" (fun _ ->
    DeleteFile Files.TestResultFile
    CleanDirs [Folders.CompileOutput; Folders.Package]

    JsonPoke "version" "0.0.0-localbuild" BogusProject.ProjectJson
    JsonPoke "packOptions.releaseNotes" "" BogusProject.ProjectJson
    JsonPoke "buildOptions.keyFile" "" BogusProject.ProjectJson

    MakeBuildInfo BogusProject Folders (fun bip ->
         {bip with
            DateTime = System.DateTime.Parse("1/1/2015")
            ExtraAttrs = MakeAttributes(false) } )

)

let RunTests() =
    CreateDir Folders.Test
    let nunit = findToolInSubPath "nunit3-console.exe" Folders.Lib
    let nunitFolder = System.IO.Path.GetDirectoryName(nunit)

    !! TestProject.TestAssembly
    |> NUnit3 (fun p -> { p with 
                            ProcessModel = NUnit3ProcessModel.SingleProcessModel
                            ToolPath = nunit
                            ResultSpecs = [Files.TestResultFile]
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
    UploadTestResultsXml TestResultsType.NUnit3 Folders.Test
)


Target "setup-snk"(fun _ ->
    trace "Decrypting Strong Name Key (SNK) file."
    let decryptSecret = environVarOrFail "SNKFILE_SECRET"
    decryptFile Projects.SnkFile decryptSecret

    JsonPoke "buildOptions.keyFile" Projects.SnkFile BogusProject.ProjectJson
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
    ==> "mono"
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

//#if INTERACTIVE
//open System
//Environment.CurrentDirectory <- workingDir
//#else
//#endif

// include Fake lib
#I @"../packages/build/FAKE/tools"
#I @"../packages/build/DotNetZip/lib/net20"
#r @"FakeLib.dll"
#r @"Ionic.Zip.dll"


#load @"Utils.fsx"

open Fake
open Utils
open System.Reflection
open Helpers

let workingDir = GetWorkingFolder();

trace (sprintf "WORKING DIR: %s" workingDir)

let ProjectName = "Bogus";

let Folders = Setup.Folders(workingDir)
let Files = Setup.Files(Folders)
let Projects = Setup.Projects(ProjectName, Folders)

let BogusProject = NugetProject("Bogus", "Bogus Fake Data Generator for .NET", Folders)
let TestProject = TestProject("Bogus.Tests", Folders)



Target "msb" (fun _ ->
    
    let tag = "msb_build";

    !! BogusProject.ProjectFile
    |> MSBuildRelease (BogusProject.OutputDirectory @@ tag) "Build"
    |> Log "AppBuild-Output: "


    !! TestProject.ProjectFile
    |> MSBuildDebug "" "Build"
    |> Log "AppBuild-Output: "

//    !! GridTestProject.ProjectFile
//    |> MSBuildDebug "" "Build"
//    |> Log "AppBuild-Output: "
)



Target "dnx" (fun _ ->
    trace "DNX Build Task"

    let tag = "dnx_build"
    
    DnvmUpdate()
    DnvmInstall Projects.DnvmVersion
    DnvmUse Projects.DnvmVersion
    
    // PROJECTS
    Dnu DnuCommands.Restore BogusProject.Folder
    DnuBuild BogusProject.Folder (BogusProject.OutputDirectory @@ tag)

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
    
    let driverConfig = NuGetConfig BogusProject Folders Files     
    NuGet ( fun p -> driverConfig) BogusProject.NugetSpec

)

Target "push" (fun _ ->
    trace "NuGet Push Task"
    
    let driverConfig = NuGetConfig BogusProject Folders Files     
    NuGetPublish ( fun p -> driverConfig)

)



Target "zip" (fun _ -> 
    trace "Zip Task"

    !!(BogusProject.OutputDirectory @@ "**") |> Zip Folders.CompileOutput (Folders.Package @@ BogusProject.Zip)
)


Target "BuildInfo" (fun _ ->
    
    trace "Writing Assembly Build Info"

    MakeBuildInfo BogusProject Folders

)


Target "Clean" (fun _ ->
    DeleteFile Files.TestResultFile
    CleanDirs [Folders.CompileOutput; Folders.Package]
)

let RunTests() =
    CreateDir Folders.Test
    let nunit = findToolInSubPath "nunit-console.exe" Folders.Lib
    let nunitFolder = System.IO.Path.GetDirectoryName(nunit)

    !! TestProject.TestAssembly
    |> NUnit (fun p -> { p with 
                            ToolPath = nunitFolder
                            OutputFile = Files.TestResultFile
                            ErrorLevel = TestRunnerErrorLevel.Error }) 

open Fake.AppVeyor

Target "ci" (fun _ ->
    trace "ci Task"
)

Target "test" (fun _ ->
    trace "CI TEST"
    RunTests()
)

Target "citest" (fun _ ->
    RunTests()
    UploadTestResultsXml TestResultsType.NUnit Folders.Test
)



"Clean"
    ==> "restore"
    ==> "BuildInfo"

//build systems
"BuildInfo"
    ==> "dnx"
    ==> "zip"

"BuildInfo"
    ==> "msb"
    ==> "zip"

"BuildInfo"
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

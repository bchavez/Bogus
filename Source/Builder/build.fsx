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
let BogusPlugIns = [
                        NugetProject("Bogus.FakeItEasy", "Bogus Fake Data Generator for .NET with FakeItEasy", Folders)
                        NugetProject("Bogus.Moq", "Bogus Fake Data Generator for .NET with Moq", Folders)
                        NugetProject("Bogus.NSubstitute", "Bogus Fake Data Generator for .NET with NSubstitute", Folders)
                   ]
let NugetProjects = BogusProject :: BogusPlugIns
let TestProject = TestProject("Bogus.Tests", Folders)



Target "msb" (fun _ ->
    
    let tag = "msb_build";

    let buildProps = [ 
                        "AssemblyOriginatorKeyFile", Projects.SnkFile
                        "SignAssembly", BuildContext.IsTaggedBuild.ToString()
                     ]
       
    for project in NugetProjects do
        !! project.ProjectFile
        |> MSBuildReleaseExt (project.OutputDirectory @@ tag) buildProps "Build"
        |> Log "AppBuild-Output: "

    !! TestProject.ProjectFile
    |> MSBuild "" "Build" (("Configuration", "Debug")::buildProps)
    |> Log "AppBuild-Output: "
)


Target "dnx" (fun _ ->
    trace "DNX Build Task"

    let tag = "dnx_build"
    
    for project in NugetProjects do
        Dotnet DotnetCommands.Restore project.Folder
        //Dotnet DotnetCommands.Restore TestProject.Folder
        DotnetBuild project (project.OutputDirectory @@ tag)
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
 )

open Ionic.Zip
open System.Xml

Target "nuget" (fun _ ->
    trace "NuGet Task"
    
    for project in NugetProjects do
        DotnetPack project Folders.Package

    traceHeader "Injecting Version Ranges"

    let exactNugetVersion = [ "Bogus" ]
  
    let extractNugetPackage (pkg : string) (extractPath : string) = 
        use zip = new ZipFile(pkg)
        zip.ExtractAll( extractPath )
  
    let repackNugetPackage (folderPath : string) (pkg : string) =
        use zip = new ZipFile()
        zip.AddDirectory(folderPath) |> ignore
        zip.Save(pkg)
  
    for project in BogusPlugIns do 
        let pkg = project.NugetPkg
        let spec = project.NugetSpec

        tracefn "FILE: %s" pkg
  
        let extractPath = Folders.Package @@ fileNameWithoutExt pkg
  
        extractNugetPackage pkg extractPath
        DeleteFile pkg
  
        let nuspecFile = extractPath @@ spec
  
        let xmlns = [("def", "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd")]
  
        let doc = new XmlDocument()
        doc.Load nuspecFile
  
        for exact in exactNugetVersion do
            let target = sprintf "//def:dependency[@id='%s']" exact
            let nodes = XPathSelectAllNSDoc doc xmlns target
            for node in nodes do
                let version = getAttribute "version" node
                node.Attributes.["version"].Value <- sprintf "[%s]" version
        
        doc.Save nuspecFile
    
        repackNugetPackage extractPath pkg
        DeleteDir extractPath
    
)

Target "push" (fun _ ->
    trace "NuGet Push Task"
    
    failwith "Only CI server should publish on NuGet"
)



Target "zip" (fun _ -> 
    trace "Zip Task"

    !!(Folders.CompileOutput @@ "**")
    --(Folders.CompileOutput @@ "**" @@ "*.deps.json")
    |> Zip Folders.CompileOutput (Folders.Package @@ BogusProject.Zip)
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

    for project in NugetProjects do
        MakeBuildInfo project Folders (fun bip -> 
            { bip with
                ExtraAttrs = MakeAttributes(BuildContext.IsTaggedBuild) } )

        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/Version" BuildContext.FullVersion

        let releaseNotes = History.NugetText Files.History GitHubUrl
        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/PackageReleaseNotes" releaseNotes
)


Target "Clean" (fun _ ->
    DeleteFile Files.TestResultFile
    CleanDirs [Folders.CompileOutput; Folders.Package]

    for project in NugetProjects do
        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/Version" "0.0.0-localbuild"
        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/PackageReleaseNotes" ""
        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/AssemblyOriginatorKeyFile" ""
        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/SignAssembly" "false"

        MakeBuildInfo project Folders (fun bip ->
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

    for project in NugetProjects do
        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/AssemblyOriginatorKeyFile" Projects.SnkFile
        XmlPokeInnerText project.ProjectFile "/Project/PropertyGroup/SignAssembly" "true"
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

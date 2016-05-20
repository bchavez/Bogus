module Utils

// include Fake lib
#I @"../packages/build/FAKE/tools"
#I @"../packages/build/FSharp.Data/lib/net40"
#I @"../packages/build/Z.ExtensionMethods.WithNamespace/lib/net40"
#I @"../packages/build/Newtonsoft.Json/lib/net45"

#r @"FakeLib.dll"
#r @"FSharp.Data.dll"
#r @"Z.ExtensionMethods.WithNamespace.dll"
#r @"Newtonsoft.Json"

open Fake
open AssemblyInfoFile

module BuildContext =     

    let private WithoutPreReleaseName (ver : string) =
        let dash = ver.IndexOf("-")
        if dash > 0 then ver.Substring(0, dash) else ver.Trim()
    
    let private PreReleaseName (ver : string) =
        let dash = ver.IndexOf("-")
        if dash > 0 then Some(ver.Substring(dash + 1)) else None

    let FullVersion = 
        let forced = environVarOrNone "FORCE_VERSION"
        let tagname = environVarOrNone "APPVEYOR_REPO_TAG_NAME"
        let buildver = environVarOrNone "APPVEYOR_BUILD_VERSION"

        match (forced, tagname, buildver) with
        | (Some f, _, _) -> f
        | (_, Some t, _) -> t.Trim(' ', 'v')
        | (_, _, Some b) -> sprintf "0.0.%s-ci" b
        | (_, _, _     ) -> "0.0.0-localbuild"

    let Version = WithoutPreReleaseName FullVersion

        


open System
open System.IO

let ChangeWorkingFolder() =
        let workingDir = currentDirectory
        if File.Exists("build.cmd") then 
            System.IO.Directory.SetCurrentDirectory workingDir
        else
            failwith (sprintf "I don't know where I am... '%s'" workingDir)  
        System.IO.Directory.GetCurrentDirectory()


module Setup =
    open FSharp.Data
    open FSharp.Data.JsonExtensions

    type Folders(workingFolder : string) =
        let compileOutput = workingFolder @@ "__compile"
        let package = workingFolder @@ "__package"
        let test = workingFolder @@ "__test"
        let source = workingFolder @@ "Source"
        let lib = source @@ "packages"
        let builder = workingFolder @@ "Builder"
    
        member this.WorkingFolder = workingFolder
        member this.CompileOutput = compileOutput
        member this.Package = package
        member this.Source = source
        member this.Lib = lib
        member this.Builder = builder
        member this.Test = test

    type Files(folders : Folders) =
        let history = folders.WorkingFolder @@ "HISTORY.md"
        let testResultFile = folders.Test @@ "results.xml"
        
        member this.History = history
        member this.TestResultFile = testResultFile

    type Projects(projectName : string, folders : Folders) = 
        let solutionFile = folders.Source @@ sprintf "%s.sln" projectName
        let globalJson = folders.Source @@ "global.json"
        let dnvmVersion = 
            let json = JsonValue.Parse(System.IO.File.ReadAllText(globalJson))
            json?sdk?version.AsString()

        member this.SolutionFile = solutionFile
        member this.GlobalJson = globalJson
        member this.DnvmVersion = dnvmVersion


    


open Setup


type Project(name : string, folders : Folders) =
    let folder = folders.Source @@ name
    let projectFile = folder @@ sprintf "%s.csproj" name
    member this.Folder = folder
    member this.ProjectFile = projectFile
    member this.Name = name

//Like an Extension Method in C#
type Project with 
    member this.Zip = sprintf "%s.zip" this.Name

type TestProject(name : string, folders : Folders) =
    inherit Project(name, folders)
    
    let testAssembly = base.Folder @@ "bin/Debug/" @@ sprintf "%s.dll" base.Name
    member this.TestAssembly = testAssembly



type NugetProject(name : string, assemblyTitle : string, folders : Folders) =
    inherit Project(name, folders)
    
    let projectJson = base.Folder @@ "project.json"
    let outputDirectory = folders.CompileOutput @@ name
    let outputDll = outputDirectory @@ sprintf "%s.dll" name
    let packageDir = folders.Package @@ name

    let nugetSpec = folders.Builder @@ "NuGet" @@ sprintf "%s.nuspec" name
    let nugetPkg = folders.Package @@ sprintf "%s.%s.nupkg" name BuildContext.FullVersion

    let zip = folders.Package @@ sprintf "%s.zip" name

    member this.ProjectJson = projectJson
    member this.OutputDirectory = outputDirectory
    member this.OutputDll = outputDll
    
    member this.NugetSpec = nugetSpec
    member this.NugetPkg = nugetPkg
    
    member this.Title = assemblyTitle




let MakeBuildInfo (project: NugetProject) (folders : Folders) = 
    let path = folders.Source @@ project.Name @@ "/Properties/AssemblyInfo.cs"
    let infoVersion = sprintf "%s built on %s" BuildContext.FullVersion (System.DateTime.UtcNow.ToString())
    let copyright = sprintf "Brian Chavez © %i" (System.DateTime.UtcNow.Year)
    let attrs = 
          [
              Attribute.Title project.Title
              Attribute.Product project.Name
              Attribute.Company "Brian Chavez"  
              Attribute.Copyright copyright
              Attribute.Version BuildContext.Version
              Attribute.FileVersion BuildContext.Version
              Attribute.InformationalVersion infoVersion
              Attribute.Trademark "Apache License v2.0"
              Attribute.Description "http://www.github.com/bchavez/RethinkDb.Driver"
          ]
    CreateCSharpAssemblyInfo path attrs


open System.Reflection

let DynInvoke (instance : obj) (methodName : string) (args : obj[]) =
    let objType = instance.GetType();
    let invoke = objType.InvokeMember(methodName, BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.InvokeMethod, null, instance, args )
    ()


let NuGetWorkingDir (project : NugetProject) =
    project.OutputDirectory @@ "dnx_build" @@ "release"


let NuGetSourceDir (project : NugetProject) =
    let dirInfo = directoryInfo project.Folder
    (dirInfo.FullName @@ @"**\*.cs", Some "src", Some @"**\obj\**")


let SetupNuGetPaths (p : NuGetParams) (project : NugetProject) =
    let workingDir = NuGetWorkingDir project
    let srcDir = NuGetSourceDir project
    {p with 
        WorkingDir = workingDir
        Files = [ srcDir ]
        }


module History =
    open Z.Core.Extensions
    open Z.Collections.Extensions
    open System.Linq
    
    let All historyFile =
        System.IO.File.ReadAllText(historyFile)

    let NugetText historyFile githubUrl =
        let allText = All historyFile
        //allText.Split("##").Where( fun s -> s.IsNullOrWhiteSpace() ).Take(5)
        let q = query{
                for str in allText.Split("##") do
                where(str.IsNotNullOrWhiteSpace())
                take 5
            }

        let text = q.StringJoin("##")
        let historyUrl = sprintf "%s/blob/master/HISTORY.md" githubUrl
        sprintf "##%s\r\nFull History Here: %s" text historyUrl
            

    let ChangesFor version historyFile =
        let all = All historyFile
        all.GetAfter(version).GetBefore("## ").Trim()



open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Linq

let JsonPoke (jsonPath: string) (value: string) (filePath: string) =
    let jsonText = File.ReadAllText(filePath)
    let obj = JsonConvert.DeserializeObject<JObject>(jsonText)
    let token = obj.SelectToken(jsonPath)
    token.Replace(new JValue(value));
    let newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
    File.WriteAllText(filePath, newJson);

let SetDependency (dependency:string) (dependencyVersion: string) (projectJson: string) =
    let jsonPath = sprintf "dependencies.['%s']" dependency
    JsonPoke jsonPath dependencyVersion projectJson

//////////////// DNVM

module Helpers = 
    open FSharp.Data
    open FSharp.Data.JsonExtensions

    let shellExec cmdPath args target = 
        let result = ExecProcess (
                      fun info ->
                        info.FileName <- cmdPath
                        info.WorkingDirectory <- target
                        info.Arguments <- args
                      ) System.TimeSpan.MaxValue
        if result <> 0 then failwith (sprintf "'%s' failed" cmdPath + " " + args)

    let findOnPath name = 
        let executable = tryFindFileOnPath name
        match executable with
            | Some exec -> exec
            | None -> failwith (sprintf "'%s' can't find" name)

  
    let dotnet args workingDir = 
        let executable = findOnPath "dotnet.exe"
        shellExec executable args workingDir

                                                          
    type DotnetCommands =
        | Restore
        | Build
        | Publish
     
    let Dotnet command target = 
        match command with
            | Restore -> (dotnet "restore" target)
            | Build -> (dotnet "build --configuration release" target)
            | Publish -> (failwith "Only CI server should publish on NuGet")

    let DotnetPack (project: NugetProject) (output: string) =
        let packArgs = sprintf "pack --configuration Release --output %s" output
        dotnet packArgs project.Folder

    let DotnetBuild (target: NugetProject) (output: string) = 
        let projectJson = JsonValue.Parse(File.ReadAllText(target.ProjectJson))
        for framework in projectJson?frameworks.Properties do
            //let moniker, _ = framework;
            let moniker = fst framework;
            let buildArgs = sprintf "build --configuration Release --output %s\\%s --framework %s" output moniker moniker
            dotnet buildArgs target.Folder

    let XBuild target output =
        let buildArgs = sprintf "%s /p:OutDir=%s" target output
        let monopath = ProgramFilesX86 @@ "Mono" @@ "bin"

        shellExec (monopath @@ "xbuild.bat") buildArgs ""
    

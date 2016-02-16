module Utils

// include Fake lib
#I @"../packages/build/FAKE/tools"
#I @"../packages/build/FSharp.Data/lib/net40"
#I @"../packages/build/Z.ExtensionMethods.WithNamespace\lib\net40"

#r @"FakeLib.dll"
#r @"FSharp.Data.dll"
#r @"Z.ExtensionMethods.WithNamespace.dll"

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

        
//        let picks = [
//                        (forced, "forced")
//                        (tagname, "tag")
//                        (buildver, "ci")
//                        (Some "0.0.0-localbuild", "local")
//                    ]
//
//        picks |> List.pick (fun ele -> 
//                                match ele with
//                                | (Some value, "forced") -> value
//                                | (Some value, "tag") -> value
//                                | (Some value, "ci") -> sprintf "0.0.%s" value
//                                | (Some value, "local") -> value
//                                | (_, _) -> None )

open System

let GetWorkingFolder() =
        let workingDir = currentDirectory
        let inRoot = TestDir(workingDir @@ "Source")
        if not inRoot then 
            failwith (sprintf "I don't know where I am... '%s'" workingDir)  
        workingDir


module Setup =
    open FSharp.Data
    open FSharp.Data.JsonExtensions

    type Folders(workingFolder : string) =
        let compileOutput = workingFolder @@ "__compile"
        let package = workingFolder @@ "__package"
        let test = workingFolder @@ "__test"
        let source = workingFolder @@ "Source"
        let lib = source @@ "packages"
        let builder = source @@ "Builder"
    
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


    
        //module Folders =
        //    let WorkingFolder = System.IO.Directory.GetCurrentDirectory()
        //    let CompileOutput = WorkingFolder @@ "__compile"
        //    let Package = WorkingFolder @@ "__package"
        //    let Source = WorkingFolder @@ "Source"
        //    let Lib = Source @@ "packages"
        //    let Builder = Source @@ "builder"
        //
        //module Files =
        //    let History = Folders.WorkingFolder @@ "HISTORY.md"
        //
        //
        //module Projects =
        //    let SolutionFile = Folders.Source @@ sprintf "%s.sln" ProjectName
        //    let GlobalJson = Folders.Source @@ "global.json"
        //    let DnvmVersion = 
        //        let json = JsonValue.Parse(System.IO.File.ReadAllText(GlobalJson))
        //        json?sdk?version.AsString()
        //
        //




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
    
    let dnxProjectFile = base.Folder @@ "project.json"
    let outputDirectory = folders.CompileOutput @@ name
    let outputDll = outputDirectory @@ sprintf "%s.dll" name
    let packageDir = folders.Package @@ name

    let nugetSpec = folders.Builder @@ "NuGet" @@ sprintf "%s.nuspec" name
    let nugetPkg = folders.Package @@ sprintf "%s.%s.nupkg" name BuildContext.FullVersion

    let zip = folders.Package @@ sprintf "%s.zip" name

    member this.DnxProjectFile = dnxProjectFile
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


//let MakeHistorySpec nuspec dest historyTxt =
//    trace "Making Hisotry Nuspec"
//    let historyFile = filename (changeExt "history.nuspec" nuspec)
//    let historyFullPath = dest @@ historyFile
//    CopyFile historyFullPath nuspec
//    XmlPoke historyFullPath "/package/metadata/releaseNotes/text()" historyTxt

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
    
    let All historyFile =
        System.IO.File.ReadAllText(historyFile)
    let NugetText historyFile =
        System.Security.SecurityElement.Escape(All historyFile)
    let ChangesFor version historyFile =
        let all = All historyFile
        all.GetAfter(version).GetBefore("## ").Trim()


let NuGetConfig (project : NugetProject) (folders :Folders) (files : Files) =
    let p = NuGetDefaults()
    {p with 
       Project = project.Name
       ReleaseNotes = History.NugetText files.History
       OutputPath = folders.Package
       Version = BuildContext.FullVersion
       SymbolPackage = NugetSymbolPackage.Nuspec
       WorkingDir = NuGetWorkingDir project
       Files = [
                  (@"**\**", Some "lib", None )
                  NuGetSourceDir project
               ]
       Publish = false
    }




//////////////// DNVM

module Helpers = 

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

  
    let dnu args workingDir = 
        let executable = findOnPath "dnu.cmd"
        shellExec executable args workingDir

          
    let dnx args workingDir = 
            let executable = findOnPath "dnx.exe"
            shellExec executable args workingDir    
   
    let dnvm args workingDir = 
        let executable = findOnPath "dnvm.cmd"
        shellExec executable args workingDir    
            
                                                          
    type DnuCommands =
        | Restore
        | Build
        | Publish
     
    let Dnu command target = 
        match command with
            | Restore -> (dnu "restore" target)
            | Build -> (dnu "build --configuration release" target)
            | Publish -> (dnu "publish --configuration release -o XXNOTXX XXXUSEDXXX" target)

    let DnuBuild target output = 
            let buildArgs = sprintf "build --configuration release --out %s" output
            dnu buildArgs target

    let DnvmInstall version =
            let installArgs = sprintf "install %s -r clr" version
            dnvm installArgs ""

    let DnvmUse version =
            let _use = sprintf "use %s -r clr -p" version;
            dnvm _use ""

    let DnvmUpdate() =
            dnvm "update-self" ""

    let XBuild target output =
        let buildArgs = sprintf "%s /p:OutDir=%s" target output
        let monopath = ProgramFilesX86 @@ "Mono" @@ "bin"

        shellExec (monopath @@ "xbuild.bat") buildArgs ""
    

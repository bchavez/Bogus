module Utils
open Fake.DotNet
open Fake.Runtime

#nowarn "52"

#load ".\\.fake\\build.fsx\\intellisense.fsx"

#r "System.Xml.Linq.dll"

#if !FAKE
  #r "netstandard"
#endif

open Fake
open Fake.Runtime
open Z.ExtensionMethods

open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
open Fake.DotNet
open Fake.Core
open Fake.Tools.Git

module BuildContext =     

    let WithoutPreReleaseName (ver : string) =
        let dash = ver.IndexOf("-")
        if dash > 0 then ver.Substring(0, dash) else ver.Trim()
    
    let private PreReleaseName (ver : string) =
        let dash = ver.IndexOf("-")
        if dash > 0 then Some(ver.Substring(dash + 1)) else None

    let FullVersion = 
        let forced = Environment.environVarOrNone "FORCE_VERSION"
        let tagname = Environment.environVarOrNone "APPVEYOR_REPO_TAG_NAME"
        let buildver = Environment.environVarOrNone "APPVEYOR_BUILD_VERSION"
        
        match (forced, tagname, buildver) with
        | (Some f, _, _) -> f
        | (_, Some t, _) -> t.Trim(' ', 'v')
        | (_, _, Some b) -> sprintf "0.0.%s-ci" b
        | (_, _, _     ) -> "0.0.0-localbuild"

    let Version = WithoutPreReleaseName FullVersion

    let IsTaggedBuild =
        Fake.BuildServer.AppVeyor.Environment.RepoTag

    let InAppVeyor =
        Environment.environVarAsBoolOrDefault "APPVEYOR" false


open System
open System.IO

let ChangeWorkingFolder() =
        let workingDir = Directory.GetCurrentDirectory()
        if File.Exists("build.cmd") then 
            System.IO.Directory.SetCurrentDirectory workingDir
        else
            failwithf "I don't know where I am... '%s'" workingDir
        System.IO.Directory.GetCurrentDirectory()


module Setup =
    open FSharp.Data
    open FSharp.Data.JsonExtensions

    type Folders(workingFolder : string) =
        let compileOutput = workingFolder @@ "__compile"
        let package = workingFolder @@ "__package"
        let test = workingFolder @@ "__test"
        let source = workingFolder @@ "Source"

        let builder = workingFolder @@ "Builder"
    
        member this.WorkingFolder = workingFolder
        member this.CompileOutput = compileOutput
        member this.Package = package
        member this.Source = source

        member this.Builder = builder
        member this.Test = test

        static member NuGetPackagePath =
           let userProfile = Environment.GetFolderPath Environment.SpecialFolder.UserProfile
           userProfile @@ ".nuget" @@ "packages"


    type Files(projectName : string, folders : Folders) =
        let history = folders.WorkingFolder @@ "HISTORY.md"
        let testResultFile = folders.Test @@ "results.xml"

        let solutionFile = folders.Source @@ sprintf "%s.sln" projectName
        let snkFile = folders.Source @@ sprintf "%s.snk" projectName
        let snkFilePublic = folders.Source @@ sprintf "%s.snk.pub" projectName 
        
        member this.History = history
        member this.TestResultFile = testResultFile

        member this.SolutionFile = solutionFile
        member this.SnkFile = snkFile
        member this.SnkFilePublic = snkFilePublic

open Setup
open Fake.IO

type Project(name : string, folders : Folders) =
    let folder = folders.Source @@ name
    let projectFile = folder @@ sprintf "%s.csproj" name
    member this.Folder = folder
    member this.ProjectFile = projectFile
    member this.Name = name

//Like an Extension Method in C#
type Project with 
    member this.Zip = sprintf "%s.zip" this.Name
    member this.MsBuildBin (configName: string) = this.Folder @@ "bin" @@ configName
    member this.MsBuildBinRelease = this.MsBuildBin "Release"

type TestProject(name : string, folders : Folders) =
    inherit Project(name, folders)
    
    let testAssembly = base.MsBuildBin("Debug") @@ "net471" @@ sprintf "%s.dll" base.Name
    member this.TestAssembly = testAssembly



type NugetProject(name : string, assemblyTitle : string, folders : Folders) =
    inherit Project(name, folders)
    
    //let projectJson = base.Folder @@ "project.json"
    let outputDirectory = folders.CompileOutput @@ name
    let outputDll = outputDirectory @@ sprintf "%s.dll" name
    
    let nugetSpecFileName = sprintf "%s.nuspec" name
    let nugetPkg = folders.Package @@ sprintf "%s.%s.nupkg" name BuildContext.FullVersion

    //member this.ProjectJson = projectJson
    member this.OutputDirectory = outputDirectory
    member this.OutputDll = outputDll
    
    member this.NugetSpec = nugetSpecFileName
    member this.NugetPkg = nugetPkg
    
    member this.Title = assemblyTitle

    member this.GetTargetFrameworks() =
         //Basically, check the TargetFrameworks (plural) MSBuild property
         let frameworks = Xml.read true this.ProjectFile "" "" "/Project/PropertyGroup/TargetFrameworks/text()"

         if Seq.isEmpty(frameworks) then 
             //Otherwise, it's the singular one.
             Xml.read true this.ProjectFile "" "" "/Project/PropertyGroup/TargetFramework/text()"
               |> Seq.toArray
         else
            frameworks
               |> Seq.head
               |> (fun x -> x.Split(';'))


let ReadFileAsHexString file =
    let bytes = File.readAsBytes file
    let sb = new System.Text.StringBuilder()
    let toHex (b : byte)=
        b.ToString("x2")
        
    let acc = bytes 
                |> Array.fold (fun (acc:System.Text.StringBuilder) b -> 
                    acc.Append(toHex b)
                ) sb
    acc.ToString()

[<NoComparisonAttribute>]
type BuildInfoParams = { 
                         DateTime:System.DateTime;
                         ExtraAttrs:list<AssemblyInfoFile.Attribute>;
                         VersionContext:string option
                       }

let MakeBuildInfo (project: NugetProject) (folders : Folders) setParams = 
    
    let bip : BuildInfoParams = { 
                                    DateTime = System.DateTime.UtcNow
                                    ExtraAttrs = []
                                    VersionContext = None
                                } |> setParams
    
    //get the version context. If one was set by the caller, use it,
    //otherwise, get the version from the executing build context.
    let fullVersion = match bip.VersionContext with
                       | Some s -> s
                       | None -> BuildContext.Version
    let version = BuildContext.WithoutPreReleaseName fullVersion

    let path = folders.Source @@ project.Name @@ "/Properties/AssemblyInfo.cs"
    let infoVersion = sprintf "%s built on %s" BuildContext.FullVersion (bip.DateTime.ToString())
    let copyright = sprintf "Brian Chavez © %i" (bip.DateTime.Year)
    let config = AssemblyInfoFileConfig(true, emitResharperSupressions=false);
    let attrs = 
          [
              AssemblyInfo.Title project.Title
              AssemblyInfo.Product project.Name
              AssemblyInfo.Company "Brian Chavez"  
              AssemblyInfo.Copyright copyright
              AssemblyInfo.Version version
              AssemblyInfo.FileVersion version
              AssemblyInfo.InformationalVersion infoVersion
              AssemblyInfo.Trademark "MIT License"
          ]

    AssemblyInfoFile.create path (attrs @ bip.ExtraAttrs) (Some config)

    Async.Sleep 100 |> Async.RunSynchronously


open System.Reflection

let DynInvoke (instance : obj) (methodName : string) (args : obj[]) =
    let objType = instance.GetType();
    objType.InvokeMember(methodName, BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.InvokeMethod, null, instance, args )
    |> ignore


module History =
    open Z.ExtensionMethods
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

open System.Xml;

let XmlStrip (fileName : string) xpath =
    let doc = new XmlDocument()
    doc.Load fileName
    let node = doc.SelectSingleNode xpath
    node.ParentNode.RemoveChild node |> ignore
    doc.Save fileName
   

let XPathSelectAllNSDoc (doc : XmlDocument) (namespaces : #seq<string * string>) xpath =
    let nsmgr = XmlNamespaceManager(doc.NameTable)
    namespaces |> Seq.iter nsmgr.AddNamespace
    let nodes = doc.SelectNodes(xpath, nsmgr)
    if nodes = null then failwithf "XML nodes '%s' not found" xpath
    nodes

let XPathSelectAllNSFile (filename:string) (namespaces : #seq<string * string>) xpath  =
    let doc = new XmlDocument();
    doc.Load filename
    XPathSelectAllNSDoc doc namespaces xpath

let XPathReplaceAllNS xpath value (namespaces : #seq<string * string>) (doc : XmlDocument) =
    let nodes = XPathSelectAllNSDoc doc namespaces xpath 
    if nodes = null then failwithf "XML nodes '%s' not found" xpath
    else
        for node in nodes do
            node.Value <- value
        doc

let XmlPokeAllNS (fileName : string) namespaces xpath value =
    let doc = new XmlDocument()
    doc.Load fileName
    XPathReplaceAllNS xpath value namespaces doc |> fun x -> x.Save fileName



let SetDependency (dependency:string) (dependencyVersion: string) (projectJson: string) =
    let jsonPath = sprintf "dependencies.['%s']" dependency
    JsonPoke jsonPath dependencyVersion projectJson

//////////////// DNVM

module Helpers = 
    open FSharp.Data
    open FSharp.Data.JsonExtensions
    open Fake.IO.Globbing

    let FindNuGetTool (cmdFileName : string) (nugetPackageName : string) (version : string option) =
         let probePath = if version.IsSome then version.Value else String.Empty
         let fullPath = (Folders.NuGetPackagePath @@ nugetPackageName @@ probePath)
         ProcessUtils.tryFindLocalTool "NA" cmdFileName [fullPath] 
    
    let shellExec cmdPath args workingDir = 
        CreateProcess.fromRawCommandLine cmdPath args
               |> CreateProcess.ensureExitCode
               |> CreateProcess.withWorkingDirectory workingDir
               |> CreateProcess.withTimeout System.TimeSpan.MaxValue
               |> Proc.run

    let shellExecSecret cmdPath args workingDir = 
        
        let result = CreateProcess.fromRawCommandLine cmdPath args
                  |> CreateProcess.withWorkingDirectory workingDir
                  |> Proc.run

        if result.ExitCode <> 0 then failwithf "'%s' failed" cmdPath

    let findOnPath name = 
        let executable = ProcessUtils.tryFindFileOnPath name
        match executable with
            | Some exec -> exec
            | None -> failwithf "'%s' can't find" name

    let encryptFile file secret =
        let tool = FindNuGetTool "secure-file.exe" "secure-file" (Some "1.0.31")
        match tool with
        | Some exe -> let args = sprintf "-encrypt %s -secret %s" file secret
                      shellExecSecret exe args "."
        | _ -> failwith "could not find secure-file.exe"

    let decryptFile file secret =
        let tool = FindNuGetTool "secure-file.exe" "secure-file" (Some "1.0.31")
        match tool with
        | Some exe -> let args = sprintf "-decrypt %s.enc -secret %s" file secret
                      shellExecSecret exe args "."
        | _ -> failwith "could not find secure-file.exe";
module Utils

// include Fake lib
#load @"../.paket/load/build/build.group.fsx"

#I @"../packages/build/FAKE/tools"
#r @"FakeLib.dll"

open Fake
open AssemblyInfoFile
open Fake.AppVeyor

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

    let IsTaggedBuild =
        AppVeyorEnvironment.RepoTag


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
        //let globalJson = folders.Source @@ "global.json"
        let snkFile = folders.Source @@ sprintf "%s.snk" projectName
        let snkFilePublic = folders.Source @@ sprintf "%s.snk.pub" projectName 

        //let dnvmVersion = 
        //    let json = JsonValue.Parse(System.IO.File.ReadAllText(globalJson))
        //    json?sdk?version.AsString()

        member this.SolutionFile = solutionFile
        //member this.GlobalJson = globalJson
        //member this.DnvmVersion = dnvmVersion
        member this.SnkFile = snkFile
        member this.SnkFilePublic = snkFilePublic


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
    member this.MsBuildBin (configName: string) = this.Folder @@ "bin" @@ configName
    member this.MsBuildBinRelease = this.MsBuildBin "Release"

type TestProject(name : string, folders : Folders) =
    inherit Project(name, folders)
    
    let testAssembly = base.MsBuildBin("Debug") @@ sprintf "%s.dll" base.Name
    member this.TestAssembly = testAssembly



type NugetProject(name : string, assemblyTitle : string, folders : Folders) =
    inherit Project(name, folders)
    
    //let projectJson = base.Folder @@ "project.json"
    let outputDirectory = folders.CompileOutput @@ name
    let outputDll = outputDirectory @@ sprintf "%s.dll" name
    let packageDir = folders.Package @@ name

    let nugetSpecFileName = sprintf "%s.nuspec" name
    let nugetPkg = folders.Package @@ sprintf "%s.%s.nupkg" name BuildContext.FullVersion
    let nugetPkgSymbols = changeExt "symbols.nupkg" nugetPkg


    let zip = folders.Package @@ sprintf "%s.zip" name

    //member this.ProjectJson = projectJson
    member this.OutputDirectory = outputDirectory
    member this.OutputDll = outputDll
    
    member this.NugetSpec = nugetSpecFileName
    member this.NugetPkg = nugetPkg
    member this.NugetPkgSymbols = nugetPkgSymbols
    
    member this.Title = assemblyTitle


let ReadFileAsHexString file =
    let bytes = ReadFileAsBytes file
    let sb = new System.Text.StringBuilder()
    let toHex (b : byte)=
        b.ToString("x2")
        
    let acc = bytes 
                |> Array.fold (fun (acc:System.Text.StringBuilder) b -> 
                    acc.Append(toHex b)
                ) sb
    acc.ToString()

type BuildInfoParams = { DateTime:System.DateTime; ExtraAttrs:list<AssemblyInfoFile.Attribute> }

let MakeBuildInfo (project: NugetProject) (folders : Folders) setParams = 
    
    let bip : BuildInfoParams = { 
                                    DateTime = System.DateTime.UtcNow
                                    ExtraAttrs = []
                                } |> setParams
    
    let path = folders.Source @@ project.Name @@ "/Properties/AssemblyInfo.cs"
    let infoVersion = sprintf "%s built on %s" BuildContext.FullVersion (bip.DateTime.ToString())
    let copyright = sprintf "Brian Chavez © %i" (bip.DateTime.Year)

    let attrs = 
          [
              Attribute.Title project.Title
              Attribute.Product project.Name
              Attribute.Company "Brian Chavez"  
              Attribute.Copyright copyright
              Attribute.Version BuildContext.Version
              Attribute.FileVersion BuildContext.Version
              Attribute.InformationalVersion infoVersion
              Attribute.Trademark "MIT License"
          ]

    CreateCSharpAssemblyInfo path (attrs @ bip.ExtraAttrs)


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

    let shellExec cmdPath args workingDir = 
        let result = ExecProcess (
                      fun info ->
                        info.FileName <- cmdPath
                        info.WorkingDirectory <- workingDir
                        info.Arguments <- args
                      ) System.TimeSpan.MaxValue
        if result <> 0 then failwith (sprintf "'%s' failed" cmdPath + " " + args)

    let shellExecSecret cmdPath args workingDir = 
        let ok = directExec (
                      fun info ->
                        info.FileName <- cmdPath
                        info.WorkingDirectory <- workingDir
                        info.Arguments <- args
                      )
        if not ok then failwith (sprintf "'%s' failed" cmdPath)

    let findOnPath name = 
        let executable = tryFindFileOnPath name
        match executable with
            | Some exec -> exec
            | None -> failwith (sprintf "'%s' can't find" name)

    let encryptFile file secret =
        let secureFile = findToolInSubPath "secure-file.exe" "."
        let args = sprintf "-encrypt %s -secret %s" file secret
        shellExecSecret secureFile args "."

    let decryptFile file secret =
        let secureFile = findToolInSubPath "secure-file.exe" "."
        let args = sprintf "-decrypt %s.enc -secret %s" file secret
        shellExecSecret secureFile args "."
  
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
        let packArgs = sprintf "pack --include-symbols --include-source --configuration Release --output %s" output
        dotnet packArgs project.Folder

    let DotnetBuild (target: NugetProject) (output: string) = 
        //let projectJson = JsonValue.Parse(File.ReadAllText(target.ProjectJson))
        let frameworks = XMLRead true target.ProjectFile "" "" "/Project/PropertyGroup/TargetFrameworks/text()"
                         |> Seq.head
                         |> (fun x -> x.Split(';'))
                     
        for framework in frameworks do
            //let moniker, _ = framework;
            let moniker = framework;
            let buildArgs = sprintf "build --configuration Release --output %s\\%s --framework %s" output moniker moniker
            dotnet buildArgs target.Folder

    let XBuild target output =
        let buildArgs = sprintf "%s /p:OutDir=%s" target output
        let monopath = ProgramFilesX86 @@ "Mono" @@ "bin"

        shellExec (monopath @@ "xbuild.bat") buildArgs ""
    

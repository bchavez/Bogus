using Fake.DotNet;
using Nuke.Common.CI.AppVeyor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.ExtensionMethods;

using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using Z.ExtensionMethods.ObjectExtensions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public static class BuildContext
{
   public static string GetFullVersion()
   {
      //Debugger.Launch();
      var forced = Environment.GetEnvironmentVariable("FORCE_VERSION"); //Fake.Core.Environment.environVarOrNone("FORCE_VERSION");
      var tagname = AppVeyor.Instance?.RepositoryTagName; // Fake.Core.Environment.environVarOrNone("APPVEYOR_REPO_TAG_NAME");
      var buildver = AppVeyor.Instance?.BuildVersion; // Fake.Core.Environment.environVarOrNone("APPVEYOR_BUILD_VERSION");

      var version = (forced, tagname, buildver) switch
      {
         var (f, _, _) when f.IsNotNullOrWhiteSpace() => f,
         var (_, t, _) when t.IsNotNullOrWhiteSpace() => t.Trim(' ', 'v'),
         var (_, _, b) when b.IsNotNullOrWhiteSpace() => $"0.0.{b}-ci",
         (_, _, _) => $"0.0.0-localbuild"
      };
      return version;
   }

   public static string GetVersionWithoutPreReleeaseName(string fullVersion)
   {
      var version = fullVersion switch
      {
         var v when v.GetBefore("-") is var b && b.IsNotNullOrWhiteSpace() => b.Trim(),
         _ => fullVersion.Trim()
      };

      return version;
   }

   public static bool IsTaggedBuild => AppVeyor.Instance?.RepositoryTag ?? false;
   public static bool IsReleaseBuild => IsTaggedBuild && NukeBuild.IsServerBuild;
}

partial class Build
{
   public record BuildTimeInfo(
      DateTime BuildTimeUtc,
      AssemblyInfo.Attribute[] ExtraAttributes,
      string FullVersion
      );


   public static void MakeBuildInfo(Project project, BuildTimeInfo bti)
   {
      var buildTimeUtc = bti.BuildTimeUtc;
      var path = Folders.Source / project.Name / "Properties" / "AssemblyInfo.cs";
      var fullVersion = bti.FullVersion;
      var version = BuildContext.GetVersionWithoutPreReleeaseName(fullVersion);
      var infoVersion = $"{fullVersion} built on {buildTimeUtc} UTC";
      var copyright = $"Brian Chavez © {buildTimeUtc.Year}";
      var title = project.GetProperty("NukeProjectTitle");
      var attrs = new List<AssemblyInfo.Attribute>
          {
             AssemblyInfo.Product(project.Name),
             AssemblyInfo.Title(title),
             AssemblyInfo.Company("Brian Chavez"),
             AssemblyInfo.Copyright(copyright),
             AssemblyInfo.Version(version),
             AssemblyInfo.FileVersion(version),
             AssemblyInfo.InformationalVersion(infoVersion),
             AssemblyInfo.Trademark("MIT License"),
             //AssemblyInfo.Metadata("CommitHash", )
          };
      attrs.AddRange(bti.ExtraAttributes);

      var config = new AssemblyInfoFileConfig(true, emitResharperSupressions: false, useNamespace: "System");
      AssemblyInfoFile.createCSharpWithConfig(path, attrs, config);
   }

}

public static class ExtensionMethodsForProject
{
   public static string BinFolder(this Project p)
   {
      var result = p.Directory / "bin";
      return result;
   }
   public static string CompileOutput(this Project p)
   {
      var result = Build.Folders.CompileOutput / p.Name;
      return result;
   }
   public static string ZipFile(this Project p)
   {
      var result = $"{p.Name}.zip";
      return result;
   }
}

public static class History
{
   public static string All(AbsolutePath historyFile)
   {
      return System.IO.File.ReadAllText(historyFile);
   }
   public static string NugetText(AbsolutePath historyFile, string githubUrl)
   {
      var allText = All(historyFile);
      var q = allText.Split("##")
         .Where(s => s.IsNotNullOrEmpty())
         .Take(5);

      var text = q.StringJoin("##");
      var historyUrl = $"{githubUrl}/blob/master/HISTORY.md";
      var sb = new StringBuilder();
      sb.AppendLine($"##{text}");
      sb.Append($"Full History Here: {historyUrl}");
      var result = sb.ToString();
      return result;
   }
}


public static class ExtensionsForNuke
{
   public static DotNetBuildSettings SetNoWarns2(this DotNetBuildSettings toolSettings, params int[] noWarn)
   {
      var nowarnstring = string.Join(",", noWarn);

      var arg = IsWin ? $"\\\"{nowarnstring}\\\"" : $"'\"{nowarnstring}\"'";

      var newSettings = toolSettings.AddProperty("NoWarn", arg);
      return newSettings;
   }
}


using System;
using FluentBuild.ApplicationProperties;
using FluentFs.Core;
using Z.ExtensionMethods;

namespace Builder.Utils
{
    public static class ExtensionsForString
    {
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }

    public static class ExtensionsForBuildFolders
    {
        public static Directory Wipe(this Directory f)
        {
            return f.Delete(OnError.Continue).Create();
        }
    }

    public static class ExtensionsForFile
    {
        public static File WithExt(this File f, string ext)
        {
            return new File(System.IO.Path.ChangeExtension(f.ToString(), ext));
        }
    }

    public static class ExtensionsForBuildContext
    {
        public static string WithoutPreReleaseName(this string version)
        {
            if (string.IsNullOrWhiteSpace(version)) return "0.0.0.0";
            var dash = version.IndexOf("-");
            return dash > 0 ? version.Substring(0, dash) : version.Trim();
        }

        public static string PreReleaseName(this string version)
        {
            var dash = version.IndexOf("-");
            return dash > 0 ? version.Substring(dash + 1) : null;
        }
    }


    public static class VersionGetter
    {
        public static string GetVersion()
        {
            var ver = Environment.GetEnvironmentVariable("FORCE_VERSION")?.Trim();
            if (ver.IsNotNullOrWhiteSpace())
                return ver;
            ver = Environment.GetEnvironmentVariable("APPVEYOR_REPO_TAG_NAME")?.Trim(' ', 'v');
            if (ver.IsNotNullOrWhiteSpace())
                return ver;
            ver = Environment.GetEnvironmentVariable("APPVEYOR_BUILD_VERSION")?.Trim();
            if( ver.IsNotNullOrWhiteSpace() )
                return $"0.0.{ver}-ci";
            return "0.0.0-localbuild";
        }
    }

    public static class History
    {
        public static string All()
        {
            return System.IO.File.ReadAllText(Files.History.ToString());
        }

        public static string NugetText()
        {
            return System.Security.SecurityElement.Escape(All());
        }

        public static string ChangesFor(string fullVersion)
        {
            var all = All();
            return all.GetAfter(fullVersion).GetBefore("## ").Trim();
        }
    }

}
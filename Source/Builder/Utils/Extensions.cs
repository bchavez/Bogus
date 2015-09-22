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
                return ver;
            return "0.0.0-localbuild";
        }
    }
}
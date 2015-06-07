using FluentBuild.ApplicationProperties;
using FluentFs.Core;

namespace Builder.Extensions
{
    public static class ExtensionsForCommandLineProperties
    {
        public static string Version(this CommandLineProperties p)
        {
            return System.Version.Parse( p.GetProperty( "Version" ) ).ToString();
        }
    }
    public static class ExtensionsForString
    {
        public static string With( this string format, params object[] args )
        {
            return string.Format( format, args );
        }
    }

    public static class ExtensionsForBuildFolders
    {
        public static Directory Wipe(this Directory f)
        {
            return f.Delete( OnError.Continue ).Create();
        }
    }
}
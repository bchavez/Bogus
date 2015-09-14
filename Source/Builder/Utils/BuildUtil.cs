using System.Linq;
using System.Xml.XPath;
using FluentFs.Core;

namespace Builder.Utils
{
    public class BuildUtil
    {
        public static FileSet GetProjectReferences(File projectFile, Directory libFolder)
        {
            var references = XDocUtil.LoadIgnoreingNamespace( projectFile.ToString() )
                .XPathSelectElements( "//HintPath" )
                .Select( h => System.IO.Path.GetFileNameWithoutExtension( h.Value ) )
                .ToList();


            return references.Aggregate( new FileSet(),
                                         ( set, assembly ) =>
                                             {
                                                 Folders.Lib.Files( "*{0}*".With( assembly ) )
                                                     .Files.ToList()
                                                     .ForEach( f => set.Include( f ) );

                                                 return set;
                                             }, set => set );
        }
    }
}
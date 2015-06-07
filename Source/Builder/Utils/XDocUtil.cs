using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Builder.Utils
{
    public class XDocUtil
    {
        // helper class to ignore namespaces when de-serializing
        private class IgnoreNamespaceXmlReader : XmlTextReader
        {
            public IgnoreNamespaceXmlReader( TextReader reader )
                : base( reader )
            {
            }

            public override string NamespaceURI
            {
                get { return ""; }
            }
        }

        public static XDocument LoadIgnoreingNamespace(string path)
        {
            return XDocument.Load( new IgnoreNamespaceXmlReader( File.OpenText( path ) ) );
        }
    }
}
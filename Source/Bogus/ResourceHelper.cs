using System.IO;
using Bogus.Bson;

namespace Bogus
{
   /// <summary>
   /// Helper utility class to read resource manifest streams.
   /// </summary>
   public static class ResourceHelper
   {
      /// <summary>
      /// Checks to see if a resource exists in an assembly.
      /// </summary>
      public static bool ResourceExists(System.Reflection.Assembly assembly, string resourceName)
      {
         return assembly.GetManifestResourceInfo(resourceName) != null;
      }

      /// <summary>
      /// Reads a byte[] resource from an assembly.
      /// </summary>
      public static byte[] ReadResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using( var s = assembly.GetManifestResourceStream(resourceName) )
         using( var ms = new MemoryStream() )
         {
            s.CopyTo(ms);

            return ms.ToArray();
         }
      }

      /// <summary>
      /// Reads a BSON <see cref="BValue"/> resource from an assembly.
      /// </summary>
      public static BValue ReadBValueResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using( var s = assembly.GetManifestResourceStream(resourceName) )
         using( var ms = new MemoryStream() )
         {
            s.CopyTo(ms);

            return Bson.Bson.Load(ms.ToArray());
         }
      }

      /// <summary>
      /// Reads a BSON <see cref="BObject"/> resource from an assembly.
      /// </summary>
      public static BObject ReadBObjectResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using( var s = assembly.GetManifestResourceStream(resourceName) )
         using( var ms = new MemoryStream() )
         {
            s.CopyTo(ms);

            return Bson.Bson.Load(ms.ToArray());
         }
      }
   }
}
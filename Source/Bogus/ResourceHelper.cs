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
      /// <param name="assembly">The assembly containing the resource.</param>
      /// <param name="resourceName">The name of the resource.</param>
      /// <returns>A boolean indicating if the resource exists in the assembly.</returns>
      public static bool ResourceExists(System.Reflection.Assembly assembly, string resourceName)
      {
         return assembly.GetManifestResourceInfo(resourceName) != null;
      }

      /// <summary>
      /// Reads a byte[] resource from an assembly.
      /// </summary>
      /// <param name="assembly">The assembly containing the resource.</param>
      /// <param name="resourceName">The name of the resource.</param>
      /// <returns>The resource in bytes.</returns>
      public static byte[] ReadResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using var s = assembly.GetManifestResourceStream(resourceName);
         using var ms = new MemoryStream();
         s.CopyTo(ms);

         return ms.ToArray();
      }

      /// <summary>
      /// Reads a BSON <see cref="BValue"/> resource from an assembly.
      /// </summary>
      /// <param name="assembly">The assembly containing the resource.</param>
      /// <param name="resourceName">The name of the resource.</param>
      /// <returns>The value of the resource as a <see cref="BValue"/> object.</returns>
      public static BValue ReadBValueResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using var s = assembly.GetManifestResourceStream(resourceName);
         using var ms = new MemoryStream();
         s.CopyTo(ms);

         return Bson.Bson.Load(ms.ToArray());
      }

      /// <summary>
      /// Reads a BSON <see cref="BObject"/> resource from an assembly.
      /// </summary>
      /// <param name="assembly">The assembly containing the resource.</param>
      /// <param name="resourceName">The name of the resource.</param>
      /// <returns>The value of the resource as a <see cref="BObject"/> object.</returns>
      public static BObject ReadBObjectResource(System.Reflection.Assembly assembly, string resourceName)
      {
         using var s = assembly.GetManifestResourceStream(resourceName);
         using var ms = new MemoryStream();
         s.CopyTo(ms);

         return Bson.Bson.Load(ms.ToArray());
      }
   }
}
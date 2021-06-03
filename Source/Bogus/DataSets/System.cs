using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bogus.Bson;

namespace Bogus.DataSets
{
   /// <summary>
   /// Generates fake data for many computer systems properties
   /// </summary>
   public class System : DataSet
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="System"/> class.
      /// </summary>
      /// <param name="locale">The locale that will be used to generate values.</param>
      public System(string locale = "en") : base(locale)
      {
         mimes = this.GetArray("mimeTypes");

         lookup = mimes.OfType<BObject>()
            .ToDictionary(o => o["mime"].StringValue);

         mimeKeys = mimes
            .OfType<BObject>()
            .Select(o => o["mime"].StringValue)
            .Distinct()
            .ToArray();

         exts = mimes
            .OfType<BObject>()
            .SelectMany(bObject =>
               {
                  if( bObject.ContainsKey("extensions") )
                  {
                     var extensions = bObject["extensions"] as BArray;
                     return extensions.OfType<BValue>().Select(s => s.StringValue);
                  }
                  return Enumerable.Empty<string>();
               })
            .ToArray();

         types = mimeKeys.Select(k => k.Substring(0, k.IndexOf('/')))
            .Distinct()
            .ToArray();
      }

      protected Lorem Lorem = null;
      private readonly Dictionary<string, BObject> lookup;
      private readonly BArray mimes;
      private readonly string[] exts;
      private readonly string[] types;
      private readonly string[] mimeKeys;

      private static readonly string[] commonFileTypes = 
         { "video", "audio", "image", "text", "application" };

      private static readonly string[] commonMimeTypes =
         {
            "application/pdf",
            "audio/mpeg",
            "audio/wav",
            "image/png",
            "image/jpeg",
            "image/gif",
            "video/mp4",
            "video/mpeg",
            "text/html"
         };

      /// <summary>
      /// Get a random file name.
      /// </summary>
      /// <param name="ext">
      /// The extension the file name will have.
      /// If null is provided, a random extension will be picked.
      /// </param>
      /// <returns>
      /// A random file name with the given <paramref name="ext"/>
      /// or a random extension
      /// </returns>
      public string FileName(string ext = null)
      {
         var filename = $"{this.Random.Words()}.{ext ?? FileExt()}";
         filename = filename.Replace(" ", "_");
         filename = filename.Replace(",", "_");
         filename = filename.Replace("-", "_");
         filename = filename.Replace(@"\", "_");
         filename = filename.Replace("/", "_");
         filename = filename.ToLower().Trim();
         return filename;
      }

      /// <summary>
      /// Get a random directory path (Unix).
      /// </summary>
      /// <returns>
      /// A random Unix directory path.
      /// </returns>
      public string DirectoryPath()
      {
         return GetRandomArrayItem("directoryPaths");
      }

      /// <summary>
      /// Get a random file path (Unix).
      /// </summary>
      /// <returns>
      /// A random Unix file path.
      /// </returns>
      public string FilePath()
      {
         return $"{DirectoryPath()}/{FileName()}";
      }

      /// <summary>
      /// Generates a random file name with a common file extension.
      /// Extension can be overwritten with <paramref name="ext"/>.
      /// </summary>
      /// <param name="ext">
      /// The extensions to be used for a file name.
      /// </param>
      /// <returns>
      /// A random file name with a common extension or <paramref name="ext"/>.
      /// </returns>
      public string CommonFileName(string ext = null)
      {
         var filename = $"{this.Random.Words()}.{ext ?? CommonFileExt()}";
         filename = filename.Replace(" ", "_");
         filename = filename.Replace(",", "_");
         filename = filename.Replace("-", "_");
         filename = filename.Replace(@"\", "_");
         filename = filename.Replace("/", "_");
         filename = filename.ToLower().Trim();
         return filename;
      }


      /// <summary>
      /// Get a random mime type.
      /// </summary>
      /// <returns>
      /// A random mime type.
      /// </returns>
      public string MimeType()
      {
         return this.Random.ArrayElement(this.mimeKeys);
      }


      /// <summary>
      /// Returns a commonly used file type.
      /// </summary>
      /// <returns>
      /// A commonly used file type.
      /// </returns>
      public string CommonFileType()
      {
         return this.Random.ArrayElement(commonFileTypes);
      }


      /// <summary>
      /// Returns a commonly used file extension.
      /// </summary>
      /// <returns>
      /// A commonly used file extension.
      /// </returns>
      public string CommonFileExt()
      {
         return FileExt(this.Random.ArrayElement(commonMimeTypes));
      }


      /// <summary>
      /// Returns any file type available as mime-type.
      /// </summary>
      /// <returns>
      /// Any file type available as mime-type.
      /// </returns>
      public string FileType()
      {
         return this.Random.ArrayElement(this.types);
      }


      /// <summary>
      /// Gets a random extension for the given mime type.
      /// </summary>
      /// <returns>
      /// A random extension for the given mime type.
      /// </returns>
      public string FileExt(string mimeType = null)
      {
         if( mimeType != null &&
             lookup.TryGetValue(mimeType, out var mime) &&
             mime.ContainsKey("extensions") )
         {
            return this.Random.ArrayElement(mime["extensions"] as BArray);
         }

         return this.Random.ArrayElement(exts);
      }

      /// <summary>
      /// Get a random semver version string.
      /// </summary>
      /// <returns>
      /// A random semver version string.
      /// </returns>
      public string Semver()
      {
         return $"{this.Random.Number(9)}.{this.Random.Number(9)}.{this.Random.Number(9)}";
      }

      /// <summary>
      /// Get a random `System.Version`.
      /// </summary>
      /// <returns>
      /// A random `System.Version`.
      /// </returns>
      public Version Version()
      {
         return new Version(this.Random.Number(9), this.Random.Number(9), this.Random.Number(9), this.Random.Number(9));
      }


      /// <summary>
      /// Get a random `Exception` with a fake stack trace.
      /// </summary>
      /// <returns>
      /// A random `Exception` with a fake stack trace.
      /// </returns>
      public Exception Exception()
      {
         Exception exception = null;
         switch( this.Random.Number(11) )
         {
            case 0:
               try
               {
                  throw new ArgumentException(Random.Words(), Random.Word());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 1:
               try
               {
                  throw new ArgumentNullException(Random.Word(), Random.Words());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 2:
               try
               {
                  throw new BadImageFormatException(Random.Words(), Random.Word());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 3:
               try
               {
                  throw new IndexOutOfRangeException(Random.Words());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 4:
               try
               {
                  throw new ArithmeticException(Random.Words());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 5:
               try
               {
                  throw new OutOfMemoryException(Random.Words());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 6:
               try
               {
                  throw new FormatException(Random.Words());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 7:
               try
               {
                  throw new DivideByZeroException();
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 8:
               try
               {
                  throw new EndOfStreamException(Random.Words());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
            case 9:
               try
               {
                  throw new FileNotFoundException("File not found...", Path.GetRandomFileName());
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;

            case 10:
               try
               {
                  throw new NotImplementedException();
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;

            case 11:
               try
               {
                  throw new UnauthorizedAccessException();
               }
               catch( Exception e )
               {
                  exception = e;
               }
               break;
         }

         return exception;
      }

      /// <summary>
      /// Get a random GCM registration ID.
      /// </summary>
      /// <returns>
      /// A random GCM registration ID.
      /// </returns>
      public string AndroidId()
      {
         const string androidIdCharacters =
            "0123456789abcefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_";

         return $"APA91{this.Random.String2(178, androidIdCharacters)}";
      }

      /// <summary>
      /// Get a random Apple Push Token.
      /// </summary>
      /// <returns>
      /// A random Apple Push Token.
      /// </returns>
      public string ApplePushToken()
      {
         return this.Random.String2(64, Chars.HexLowerCase);
      }

      /// <summary>
      /// Get a random BlackBerry Device PIN.
      /// </summary>
      /// <returns>
      /// A random BlackBerry Device PIN.
      /// </returns>
      public string BlackBerryPin()
      {
         return this.Random.Hash(8);
      }
   }
}
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
      /// Default constructor
      /// </summary>
      /// <param name="locale"></param>
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
            .SelectMany(o =>
               {
                  if( o.ContainsKey("extensions") )
                  {
                     var e = o["extensions"] as BArray;
                     return e.OfType<BValue>().Select(s => s.StringValue);
                  }
                  return Enumerable.Empty<string>();
               })
            .ToArray();

         types = mimeKeys.Select(k => k.Substring(0, k.IndexOf('/')))
            .Distinct()
            .ToArray();
      }

      protected Lorem Lorem = null;
      private Dictionary<string, BObject> lookup;
      private BArray mimes;
      private string[] exts;
      private string[] types;
      private string[] mimeKeys;

      /// <summary>
      /// Get a random file name.
      /// </summary>
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
      public string DirectoryPath()
      {
         return GetRandomArrayItem("directoryPaths");
      }

      /// <summary>
      /// Get a random file path (Unix).
      /// </summary>
      public string FilePath()
      {
         return $"{DirectoryPath()}/{FileName()}";
      }

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
      /// Get a random mime type
      /// </summary>
      public string MimeType()
      {
         return this.Random.ArrayElement(this.mimeKeys);
      }


      /// <summary>
      /// Returns a commonly used file type.
      /// </summary>
      public string CommonFileType()
      {
         var types = new[] {"video", "audio", "image", "text", "application"};
         return this.Random.ArrayElement(types);
      }


      /// <summary>
      /// Returns a commonly used file extension.
      /// </summary>
      /// <returns></returns>
      public string CommonFileExt()
      {
         var types = new[]
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

         return FileExt(this.Random.ArrayElement(types));
      }


      /// <summary>
      /// Returns any file type available as mime-type.
      /// </summary>
      public string FileType()
      {
         return this.Random.ArrayElement(this.types);
      }


      /// <summary>
      /// Gets a random extension for the given mime type.
      /// </summary>
      public string FileExt(string mimeType = null)
      {
         BObject mime;
         if( mimeType != null &&
             lookup.TryGetValue(mimeType, out mime) &&
             mime.ContainsKey("extensions") )
         {
            return this.Random.ArrayElement(mime["extensions"] as BArray);
         }

         return this.Random.ArrayElement(exts);
      }

      /// <summary>
      /// Get a random semver version string.
      /// </summary>
      public string Semver()
      {
         return $"{this.Random.Number(9)}.{this.Random.Number(9)}.{this.Random.Number(9)}";
      }

      /// <summary>
      /// Get a random `System.Version`.
      /// </summary>
      public Version Version()
      {
         return new Version(this.Random.Number(9), this.Random.Number(9), this.Random.Number(9), this.Random.Number(9));
      }


      /// <summary>
      /// Get a random `Exception` with a fake stack trace.
      /// </summary>
      public Exception Exception()
      {
         Exception exe = null;
         switch( this.Random.Number(11) )
         {
            case 0:
               try
               {
                  throw new ArgumentException(Random.Words(), Random.Word());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 1:
               try
               {
                  throw new ArgumentNullException(Random.Word(), Random.Words());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 2:
               try
               {
                  throw new BadImageFormatException(Random.Words(), Random.Word());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 3:
               try
               {
                  throw new IndexOutOfRangeException(Random.Words());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 4:
               try
               {
                  throw new ArithmeticException(Random.Words());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 5:
               try
               {
                  throw new OutOfMemoryException(Random.Words());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 6:
               try
               {
                  throw new FormatException(Random.Words());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 7:
               try
               {
                  throw new DivideByZeroException();
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 8:
               try
               {
                  throw new EndOfStreamException(Random.Words());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
            case 9:
               try
               {
                  throw new FileNotFoundException("File not found...", Path.GetRandomFileName());
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;

            case 10:
               try
               {
                  throw new NotImplementedException();
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;

            case 11:
               try
               {
                  throw new UnauthorizedAccessException();
               }
               catch( Exception e )
               {
                  exe = e;
               }
               break;
         }

         return exe;
      }

      /// <summary>
      /// Get a random GCM registration ID.
      /// </summary>
      public string AndroidId()
      {
         return $"APA91{this.Random.String2(178, "0123456789abcefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-_")}";
      }

      /// <summary>
      /// Get a random Apple Push Token
      /// </summary>
      public string ApplePushToken()
      {
         return this.Random.String2(64, Chars.HexLowerCase);
      }

      /// <summary>
      /// Get a random BlackBerry Device PIN
      /// </summary>
      public string BlackBerryPin()
      {
         return this.Random.Hash(8);
      }
   }
}
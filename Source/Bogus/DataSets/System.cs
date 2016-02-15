using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Bogus.DataSets
{
    /// <summary>
    /// Generates fake data for many computer systems properties
    /// </summary>
    internal class System : DataSet
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="locale"></param>
        public System(string locale = "en") : base(locale)
        {
            mimes = this.GetObject("mimeTypes");

            mimeKeys = mimes.Properties()
                .Select(p => p.Name)
                .Distinct()
                .ToArray();

            exts = mimes.SelectTokens("*.extensions.[*]")
                .Select(x => x.ToString()).ToArray();

            types = mimeKeys.Select(k => k.Substring(0, k.IndexOf('/')))
                .Distinct()
                .ToArray();
        }

        protected Lorem Lorem = null;
        private JObject mimes;
        private string[] exts;
        private string[] types;
        private string[] mimeKeys;

        /// <summary>
        /// Get a random file name
        /// </summary>
        public string FileName(string ext = null)
        {
            var filename = $"{this.Random.Words()}.{ext ?? FileExt()}";
            filename = filename.Replace(" ", "_");
            filename = filename.Replace(",", "_");
            filename = filename.Replace("-", "_");
            filename = filename.Replace(@"\", "_");
            filename = filename.ToLower().Trim();
            return filename;
        }


        public string CommonFileName(string ext = null)
        {
            var filename = $"{this.Random.Words()}.{ext ?? CommonFileExt()}";
            filename = filename.Replace(" ", "_");
            filename = filename.Replace(",", "_");
            filename = filename.Replace("-", "_");
            filename = filename.Replace(@"\", "_");
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
        /// Returns a commonly used file type
        /// </summary>
        public string CommonFileType()
        {
            var types = new[] {"video", "audio", "image", "text", "application"};
            return this.Random.ArrayElement(types);
        }


        /// <summary>
        /// Returns a commonly used file extension
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
        /// Returns any file type available as mime-type
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
            JToken mime;
            if(mimeType != null && mimes.TryGetValue(mimeType,out mime) && mime.Type == JTokenType.Object)
            {
                var mimeObject = mime as JObject;
                return this.Random.ArrayElement(mimeObject["extensions"] as JArray);
            }

            return this.Random.ArrayElement(exts);
        }
    }
}

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Bogus.Platform;

namespace Bogus.Premium
{
   public static class LicenseVerifier
   {
      public static bool VerifyLicense(string licenseTo, string licenseKey)
      {
         AssertKeyIsNotBanned(licenseKey);

         const string modulusString =
            "vBgOPQiBhRR22ClUzIBJCmxcaOWfuAweUNpodRuZWDn8whviOe4JdA/sjzqw54KGh1qHJIc7JY5sGTCxNZQiSuyZQ6iHK2ykmU0Yb+QBvbqG33x2R7Di8MoNA1Tv2fX7SSny++IKEOQEEvwYhYr6oRU8sVItMcybUjiaaSw1rbU=";
         const string exponentString = "AQAB";

         var data = Encoding.UTF8.GetBytes(licenseTo);

         var rsaParameters = new RSAParameters
         {
            Modulus = Convert.FromBase64String(modulusString),
            Exponent = Convert.FromBase64String(exponentString)
         };
         var licenseData = Convert.FromBase64String(licenseKey);
#if STANDARD
         using var rsa = RSA.Create();
         rsa.ImportParameters(rsaParameters);
         return rsa.VerifyData(data, licenseData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
#else
         using var rsa = new RSACryptoServiceProvider();
         rsa.ImportParameters(rsaParameters);
         return rsa.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), licenseData);
#endif
      }

      private static void AssertKeyIsNotBanned(string licenseKey)
      {
      }

      public const string LicenseFile = "Bogus.Premium.LicenseKey";

      public static string FindLicense()
      {
         foreach (var probePath in ProbePaths)
         {
            var licFile = FindLicense(probePath);
            if (licFile != null) return licFile;
         }

         return null;
      }

      public static string FindLicense(string probePath)
      {
         if (probePath.EndsWith(LicenseFile) && File.Exists(probePath)) return probePath;

         string dir;
         if (Directory.Exists(probePath))
         {
            dir = probePath;
         }
         else
         {
            dir = Path.GetDirectoryName(probePath);
         }

         while (dir != null)
         {
            var licFile = Path.Combine(dir, LicenseFile);

            if (File.Exists(licFile))
            {
               return licFile;
            }

            if (dir == Path.GetPathRoot(dir) || string.IsNullOrWhiteSpace(dir))
               break;

            dir = Path.GetFullPath(Path.Combine(dir, ".."));
         }

         return null;
      }

      public static void ReadLicense(string path, out string name, out string key)
      {
         var lines = File.ReadLines(path).Take(2).ToArray();
         name = lines[0];
         key = lines[1];
      }

      public static List<string> ProbePaths { get; } = new List<string>
      {
#if STANDARD
            AppContext.BaseDirectory,
#endif
#if !STANDARD13
            typeof(License).GetAssembly().Location,
#endif
            Directory.GetCurrentDirectory()
      };

   }
}
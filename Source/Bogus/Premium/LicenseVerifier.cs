#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Security.Cryptography;
using System.Text;

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
#if STANDARD
         using( var rsa = System.Security.Cryptography.RSA.Create() )
#else
         using( var rsa = new System.Security.Cryptography.RSACryptoServiceProvider() )
#endif
         {
            var licenseData = Convert.FromBase64String(licenseKey);
            rsa.ImportParameters(rsaParameters);

            bool verified = false;

#if STANDARD
            verified = rsa.VerifyData(data, licenseData, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
#else
            verified = rsa.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), licenseData);
#endif
            return verified;
         }
      }

      private static void AssertKeyIsNotBanned(string licenseKey)
      {
      }
   }
}
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System.Reflection;
using Bogus.Bson;

namespace Bogus.Premium
{
   /// <summary>
   /// Root object for premium data sets.
   /// </summary>
   public abstract class PremiumDataSet : DataSet
   {
      protected internal override BValue Get(string path)
      {
         CheckLicense();
         return base.Get(path);
      }

      protected internal override BValue Get(string category, string path)
      {
         CheckLicense();
         return base.Get(category, path);
      }

      protected internal override bool HasKey(string path, bool includeFallback = true)
      {
         CheckLicense();
         return base.HasKey(path, includeFallback);
      }

      protected virtual void CheckLicense()
      {
         if( string.IsNullOrWhiteSpace(License.LicenseTo) ||
             string.IsNullOrWhiteSpace(License.LicenseKey))
         {
            var path = LicenseVerifier.FindLicense();
            if( path != null )
            {
               LicenseVerifier.ReadLicense(path, out var licenseTo, out var licenseKey);
               License.LicenseTo = licenseTo;
               License.LicenseKey = licenseKey;
            }
         }
         if( !string.IsNullOrWhiteSpace(License.LicenseTo) &&
             !string.IsNullOrWhiteSpace(License.LicenseKey) &&
             LicenseVerifier.VerifyLicense(License.LicenseTo, License.LicenseKey) )
         {
            this.Initialize();
            return;
         }

         throw new BogusException(
            "A premium license is required to use this API. " +
            $"Please double check that your '{LicenseVerifier.LicenseFile}' file exists in the same folder as Bogus.dll. " +
            $"Also, you can add additional probing paths for the license file in {nameof(LicenseVerifier)}.{nameof(LicenseVerifier.ProbePaths)}. " +
            $"Lastly, you can set the following static properties manually: " +
            $"{nameof(Bogus)}.{nameof(License)}.{nameof(License.LicenseTo)} and " +
            $"{nameof(Bogus)}.{nameof(License)}.{nameof(License.LicenseKey)}. "+
            "For more information, please visit: https://github.com/bchavez/Bogus/wiki/Bogus-Premium");
      }


      protected abstract void Initialize();

      protected void LoadResource(Assembly asm, string resourceName)
      {
         var obj = ResourceHelper.ReadBObjectResource(asm, resourceName);
         //patch
         var enLocale = Database.GetLocale("en");

         foreach( var val in obj.Keys )
         {
            enLocale[val] = obj[val];
         }
      }
   }
}
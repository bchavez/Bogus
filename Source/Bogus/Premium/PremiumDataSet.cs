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
      public override BValue Get(string path)
      {
         CheckLicense();
         return base.Get(path);
      }

      protected override bool HasKey(string path, bool includeFallback = true)
      {
         CheckLicense();
         return base.HasKey(path, includeFallback);
      }

      protected virtual void CheckLicense()
      {
         if( !string.IsNullOrWhiteSpace(License.LicenseTo) &&
             !string.IsNullOrWhiteSpace(License.LicenseKey) &&
             LicenseVerifier.VerifyLicense(License.LicenseTo, License.LicenseKey) )
         {
            this.Initialize();
            return;
         }

         throw new BogusException(
            "A premium license is required to use this API. " +
            "The premium license for extended datasets is invalid. " +
            $"Please double check that {nameof(Bogus)}.{nameof(License)}.{nameof(License.LicenseTo)} and {nameof(Bogus)}.{nameof(License)}.{nameof(License.LicenseKey)} are correct and complete.");
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
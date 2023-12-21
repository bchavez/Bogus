#if NET6_0_OR_GREATER
using Argon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VerifyTests;
using VerifyXunit;
using Xunit;
using Z.ExtensionMethods;
using static VerifyXunit.Verifier;

namespace Bogus.Tests.SchemaTests;


public static class ModuleInit
{
   [ModuleInitializer]
   public static void Init()
   {
      VerifierSettings.SortJsonObjects();
   }
}


[UsesVerify]
public class LocaleSchemaTests
{
    const string DataFolder = "../../../../Bogus/data/";

   [Theory]
   [MemberData(nameof(GetLocaleCodes))]
   public Task ensure_wellknown_locale_schema(string localeCode)
   {
      var localeFile = Path.Combine(DataFolder, $"{localeCode}.locale.json");

      var localeJsonRaw = File.ReadAllText(localeFile);

      var locale = JToken.Parse(localeJsonRaw);

      var settings = new VerifySettings();

      settings.AddExtraSettings(jss => jss.ContractResolver = new InterceptedContractResolver(jss.ContractResolver));

      return Verify(locale, settings)
         .UseDirectory("../../Bogus/data/")
         .UseFileName($"{localeCode}.locale.schema");
   }

   public static IEnumerable<object[]> GetLocaleCodes()
   {
      var localeCodes = Directory.GetFiles(DataFolder, "*.locale.json")
                                     .Select(file => Path.GetFileNameWithoutExtension(file).GetBefore("."));
      foreach(var localeCode in localeCodes )
      {
         yield return new[] { localeCode };
      }
   }
}

public class InterceptedContractResolver : IContractResolver
{
   private readonly IContractResolver defaultResolver;

   public InterceptedContractResolver(IContractResolver defaultResolver)
   {
      this.defaultResolver = defaultResolver;
   }

   public JsonNameTable GetNameTable()
   {
      return defaultResolver.GetNameTable();
   }

   public JsonContract ResolveContract(Type type)
   {
      var contract = this.defaultResolver.ResolveContract(type);
      if( contract is JsonDictionaryContract jdc )
      {
         var defaultIntercept = jdc.InterceptSerializeItem;
         jdc.InterceptSerializeItem = (key, val) => { 
            if( val is JArray arr && arr.Children().OfType<JValue>().Any() )
            {
               var children = arr.Children();
               var first = children.First();
               return InterceptResult.Replace($"[Array {first.Type}; {children.Count()}]");
            }

            return defaultIntercept(key, val);
         };
      }
      return contract;
   }
}
#endif
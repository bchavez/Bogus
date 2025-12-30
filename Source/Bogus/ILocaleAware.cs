using System.Collections.Generic;

namespace Bogus;

/// <summary>
/// Marker interface for datasets that are locale aware.
/// </summary>
public interface ILocaleAware
{
   /// <summary>
   /// The current locale for the dataset.
   /// </summary>
   string Locale { get; set; }
}

/// <summary>
/// Marker interface for objects that have a context storage property.
/// </summary>
public interface IHasContext
{
   Dictionary<string, object> Context { get; }
}

/// <summary>
/// Represents all locales available for use, with language or dialect information and respective country or region.
/// </summary>
public enum LocaleType
{
   /// <summary>
   /// Afrikaans language (South Africa).
   /// </summary>
   af_ZA,

   /// <summary>
   /// Arabic language.
   /// </summary>
   ar,

   /// <summary>
   /// Azerbaijani language (Azerbaijan).
   /// </summary>
   az,

   /// <summary>
   /// Czech language (Czech Republic).
   /// </summary>
   cz,

   /// <summary>
   /// German language (Germany).
   /// </summary>
   de,

   /// <summary>
   /// German language (Austria).
   /// </summary>
   de_AT,

   /// <summary>
   /// German language (Switzerland).
   /// </summary>
   de_CH,

   /// <summary>
   /// Greek language (Greece).
   /// </summary>
   el,

   /// <summary>
   /// English language.
   /// </summary>
   en,

   /// <summary>
   /// English language (Australia).
   /// </summary>
   en_AU,

   /// <summary>
   /// English language (Australia Ocker).
   /// </summary>
   en_AU_ocker,

   /// <summary>
   /// English language in Bork dialect (fictional/humorous).
   /// </summary>
   en_BORK,

   /// <summary>
   /// English language (Canada).
   /// </summary>
   en_CA,

   /// <summary>
   /// English language (United Kingdom).
   /// </summary>
   en_GB,

   /// <summary>
   /// English language (Ireland).
   /// </summary>
   en_IE,

   /// <summary>
   /// English language (India).
   /// </summary>
   en_IND,

   /// <summary>
   /// English language (Nigeria).
   /// </summary>
   en_NG,

   /// <summary>
   /// English language (United States).
   /// </summary>
   en_US,

   /// <summary>
   /// English language (South Africa).
   /// </summary>
   en_ZA,

   /// <summary>
   /// Spanish language.
   /// </summary>
   es,

   /// <summary>
   /// Spanish language (Mexico).
   /// </summary>
   es_MX,

   /// <summary>
   /// Persian language (Iran).
   /// </summary>
   fa,

   /// <summary>
   /// Finnish language (Finland).
   /// </summary>
   fi,

   /// <summary>
   /// French language (France).
   /// </summary>
   fr,

   /// <summary>
   /// French language (Canada).
   /// </summary>
   fr_CA,

   /// <summary>
   /// French language (Switzerland).
   /// </summary>
   fr_CH,

   /// <summary>
   /// Georgian language (Georgia).
   /// </summary>
   ge,

   /// <summary>
   /// Croatian language (Croatia).
   /// </summary>
   hr,

   /// <summary>
   /// Indonesian language (Indonesia).
   /// </summary>
   id_ID,

   /// <summary>
   /// Italian language (Italy).
   /// </summary>
   it,

   /// <summary>
   /// Japanese language (Japan).
   /// </summary>
   ja,

   /// <summary>
   /// Korean language (South Korea).
   /// </summary>
   ko,

   /// <summary>
   /// Latvian language (Latvia).
   /// </summary>
   lv,

   /// <summary>
   /// Norwegian language (Bokmål dialect, Norway).
   /// </summary>
   nb_NO,

   /// <summary>
   /// Nepali language (Nepal).
   /// </summary>
   ne,

   /// <summary>
   /// Dutch language (Netherlands).
   /// </summary>
   nl,

   /// <summary>
   /// Dutch language (Belgium).
   /// </summary>
   nl_BE,

   /// <summary>
   /// Polish language (Poland).
   /// </summary>
   pl,

   /// <summary>
   /// Portuguese language (Brazil).
   /// </summary>
   pt_BR,

   /// <summary>
   /// Portuguese language (Portugal).
   /// </summary>
   pt_PT,

   /// <summary>
   /// Romanian language (Romania).
   /// </summary>
   ro,

   /// <summary>
   /// Russian language (Russia).
   /// </summary>
   ru,

   /// <summary>
   /// Slovak language (Slovakia).
   /// </summary>
   sk,

   /// <summary>
   /// Swedish language (Sweden).
   /// </summary>
   sv,

   /// <summary>
   /// Turkish language (Turkey).
   /// </summary>
   tr,

   /// <summary>
   /// Ukrainian language (Ukraine).
   /// </summary>
   uk,

   /// <summary>
   /// Vietnamese language (Vietnam).
   /// </summary>
   vi,

   /// <summary>
   /// Chinese language (Simplified, China).
   /// </summary>
   zh_CN,

   /// <summary>
   /// Chinese language (Traditional, Taiwan).
   /// </summary>
   zh_TW,

   /// <summary>
   /// Zulu language (South Africa).
   /// </summary>
   zu_ZA
}

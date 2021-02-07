## v33.0.1
Release Date: 2021-02-07

* Locale data parity with faker-js@5478d65.
* `nep` locale renamed to `ne`. 
* `de` locale; `Date` dataset changed.
* `es` locale; gender names added.
* `fr` locale; `Date` dataset changed.
* `nb_NO` locale; gender names added.
* `nl` locale; `Address`, `Commerce`, `Date`, `Hacker`, `Internet` datasets changed; gender names added. 
* `pt_BR` locale; gender names added.
* `tr` locale; gender names added.
* `vi` locale; gender names added. `Date`, `Lorem`, datasets changed.
* Added `Internet.Port()`; generate port numbers from 1 to 65535.

## v32.1.1
Release Date: 2021-01-31

* Minor improvements to XML docs.
* Better exception messages around empty collections and empty arrays.
* PR 339: Fix null reference exception when calling StrictMode(true) on Faker<T> with no rules.
* PR 352: Fix first name generation for locales that don't support gender names.

## v32.0.2
Release Date: 2020-12-12

* Issue 342: Use realistic `pt_BR` city names.

## v32.0.1
Release Date: 2020-11-28

* Issue 336: Fixed `Internet.Avatar()` 403 Forbidden URLs. AWS S3 bucket hosting uifaces.com avatars was disabled. Avatars are now decentrally hosted on IPFS globally and retrieved from Cloudflare's IPFS gateway.
* To help host, pin root CID: Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye
* Browse: https://cloudflare-ipfs.com/ipfs/Qmd3W5DuhgHirLHGVixi6V76LhCkZUz6pnFt5AJBiyvHye/
* Details: https://github.com/bchavez/Bogus/issues/336

## v31.0.3
Release Date: 2020-10-03

* Issue 328, 327: Fixed `pt_BR` locale where `Address.City()` threw `ArgumentOutOfRangeException`.

## v31.0.2
Release Date: 2020-09-13, UNPUBLISHED FROM NUGET

* Ensures Bogus Premium datasets load with new `DataSet.Get(category, path)` overload.

## v31.0.1
Release Date: 2020-09-13, UNPUBLISHED FROM NUGET

* Locale data parity with faker-js@91dc8a3.
* Added `Music` dataset with `.Genre()` method.
* Added `Date.TimeZoneString()`.
* Added new Finnish `fi` locale.
* Added new Hrvatski `hr` locale.
* `cz` locale updated.
* `en` locale updated.
* `en_IE` locale updated.
* `en_IND` locale updated.
* `fa` locale updated.
* `fr` locale updated.
* `ja` locale updated.
* `pl` locale updated.
* `pt_BR` locale updated.
* `ru` locale updated.
* `sv` locale updated.
* `vi` locale updated.
* `zh_CN` locale updated.

## v30.0.4
Release Date: 2020-08-15

* Issue 319: The `Random.Decimal()` implementation reverted to previous v29 implementation. Avoids arithmetic `OverflowException` when calling `Random.Decimal(0, decimal.MaxValue)`. The v30 implementation moved to `Bogus.Extensions` namespace as `Random.Decimal2()` which can generate more decimal precision.

## v30.0.3
Release Date: 2020-08-13, UNPUBLISHED FROM NUGET

* Added `f.Address.CountryOfUnitedKingdom()` extension method in `Bogus.Extensions.UnitedKingdom`.

## v30.0.2
Release Date: 2020-08-05, UNPUBLISHED FROM NUGET

* Deterministic sequences may have changed.
* Promoted v30.0.1-beta-4 to v30.0.2 release.

## v30.0.1-beta-4
Release Date: 2020-07-23

* Change credit card `CheckDigitExtension` methods to public

## v30.0.1-beta-3
Release Date: 2020-06-29

* Issue 307: Fixed `Internet.UserAgent()` sometimes generating invalid user agent strings that could not be parsed by `System.Net.Http.HttpRequestMessage.Headers`.

## v30.0.1-beta-2
Release Date: 2020-06-20

* Added `Finance.Litecoin()`.
* Added `Commerce.ProductDescription()`.
* Add PlaceIMG image service. `Image.PlaceImgUrl()`.
* Data parity with faker.js. Deterministic sequences may have changed.
* New `en_NG` Nigerian locale.
* `en` updated.
* `nl_BE` updated.
* `de` updated.
* `ru` updated.
* `zh_CN` updated.
* `zh_TW` updated.
* `ar` updated. 
* `cz` updated.
* `es_MX` updated.
* `sk` updated.
* `it` updated.

## v30.0.1-beta-1
Release Date: 2020-06-14

* PR 300: `Random.Number()` now inclusive of `max: int.MaxValue`.
* PR 300: `Random.Even()` better random distribution and range checking.
* PR 300: `Random.Odd()` better random distribution and range checking.  
* PR 300: `Random.Int()` bug fixed where `.Int()` may not return `int.MaxValue`.
* PR 300: `Random.Decimal()` with greater decimal precision.
* Deterministic values may have changed.
* Big thank you to @logiclrd for PR 300!

## v29.0.2
Release Date: 2020-04-11

* Minor update to `ru` locale data. Two `ru` city names could appear as one. 

## v29.0.1
Release Date: 2020-02-10

* Data parity with faker.js. Deterministic sequences using `Internet.Avatar()` may have changed.
* Add support for .snupkg NuGet Symbol Server Packages via SourceLink.
* Added Randomizer.EnumValues() that makes selecting a subset of enum values easier.
* Modified `.OrNull(f)` extension method signatures for type-safe with nullable primitive types, structs, and reference types via `in` parameter to avoid ambiguous calls.

## v28.4.4
Release Date: 2019-12-10

* PR 272: Add extension method for generating Norwegian national identity numbers. `Person.Fødselsnummer()`. Thanks @mika-s!

## v28.4.3
Release Date: 2019-12-03

* Issue 271: Minor bug fix in Brazil `Person.Cpf()` extension method. Previously, only the first call to `Person.Cpf(includeFormatSymbols)` respected the `includeFormatSymbols` parameter due to the final result being saved in `Person` context. `Person.Cpf()` now respects the `includeFormatSymbols` parameter after subsequent repeat calls to `Cpf()` with the same `Person`. Thanks for testing @ArthNRick!

## v28.4.2
Release Date: 2019-11-30

* PR 269: Adds `includeFormatSymbols` parameter to include or exclude formatting characters for Brazil `Person.Cpf()` and `Company.Cnpj()` extension methods.

## v28.4.1
Release Date: 2019-10-14

* Issue 260: Improved social security number (SSN) generation that should pass basic validation by avoiding invalid SSN ranges. Note: Deterministic SSNs generated with `Person.Ssn()` will change.
* Issue 252: `Internet.Ip()` now avoids generating IP addresses with a leading zero. For example, Bogus will not generate an IP address with a leading zero like 0.1.2.3. Note: Deterministic IPs generated with `Internet.Ip()` will change.
* PR 261: Added `Internet.IpAddress()`, `Internet.Ipv6Address()`, `Internet.IpEndPoint()`, and `Internet.Ipv6EndPoint()`.
* Issue 258: Add `Internet.UrlRootedPath()` to generate random `/foo/bar` paths.
* Added `Internet.UrlWithPath(fileExt:".txt")` fileExt extension parameter to generate URLs with a specific file extension.

## v28.3.2
Release Date: 2019-10-04

* PR 259: Fixes `.GenerateForever(ruleset)` to use ruleset parameter when supplied as argument. Thanks @StanleyGoldman!

## v28.3.1
Release Date: 2019-09-20

* Issue 255 / PR 256: Allows interfaces with `Faker<T>` using `Faker<IFoo>.CustomInstantiator(f => new Foo())`. Thanks Rowland!

## v28.2.1
Release Date: 2019-09-10

* Added `Faker<T>.RuleFor(string, (f, t) => )` overload.
* Internal re-factoring `.RuleFor` overload logic. Simplified overload call chain.
* Internal `Faker<T>.RuleForInternal()` renamed to `Faker<T>.AddRule()`

## v28.1.1
Release Date: 2019-09-09

* Issue 253, PR 254: New rule overload for `Faker<T>.RuleFor('string',...)`. Helps cases that require rules for protected or hidden members of `T`.

## v28.0.3
Release Date: 2019-08-28

* Issue 249: Fixed `Internet.Url()` including spaces in domain names for `pt_BR` locale or any locale with compound first names that may contain spaces. Thanks RodrigoRodriguesX10!
* PR 241: General code quality improvements in `DataSets.System`. Better XML docs and lower array allocations. Thanks bartdebever!
* PR 245: XML doc improvements to `DataSets.Lorem`. Thanks bartdebever! 

## v28.0.2
Release Date: 2019-07-07

* PR 235: Added `Bogus.DataSets.Vehicle.GbRegistrationPlate()` in `Bogus.Extensions.UnitedKingdom` extension namespace to generate GB registration plates. Thanks @colinangusmackay.

## v28.0.1
Release Date: 2019-07-02

* BREAKING: Deterministic sequence values may have changed for fake email addresses derived from `Internet.Email()` or `Internet.UserName()` in locales other than `en`.
* Issue 229: Adds `Finance.Iban(countryCode)` ISO3166 country code parameter. Allows generating IBAN codes for specific countries. The country code must be a supported otherwise an exception is thrown.  
* Issue 225: Better support for transliteration of international Unicode characters to US-Latin/Roman ASCII character sets. `Internet.Email()` and `Internet.UserName()` are more respectful of specified locale using character transliteration.
* Added `.Transliterate()` string extension method in `Bogus.Extensions` namespace.
* Added `Internet.UserNameUnicode()` that preserves Unicode characters in user names.
* Minor performance improvement to `Utils.Slugify` using compiled Regex.
* Issue 232: Adds `.OrNull[T]() where T : struct` overload which makes it easier to work with nullable types without type casting.
* Added `defaultValue` parameter to `.OrDefault(f, weight, defaultValue)` that can default to a different value than the `default` keyword.


## v27.0.1
Release Date: 2019-05-02

* Issue 218: Fixed bug that prevented global static `Faker.DefaultStrictMode` from working.
* Issue 210: Added `Randomizer.Utf16String` that generates technically valid Unicode with paired high/low surrogates.
* Added `placeholder.com` image service.

## v26.0.2
Release Date: 2019-03-22

* New `Person(seed)` constructor for seeding person objects by integer. Thanks @sgoguen!
* Fixed `Person.DateOfBirth` not using `Date.SystemClock` as 'now' reference.

## v26.0.1
Release Date: 2019-02-26

* Data and feature parity with faker.js @ d3ce6f1
* New `Vehicle` data set added.
* `en_IND` state abbreviations and state names updated.
* `en_CA`, `fr_CA` improved Canadian postal codes.
* `pt_PT` locale updated with new and changed data.
* `en` company names updated.
* `pt_PT` locale updated.
* Minor bug in `Date.Weekday` and `Date.Month` that could cause locale weekday/month values to default to `en`.
* Note: deterministic sequences may have changed.

## v25.0.4
Release Date: 2019-01-17

* PR 194: Update Dutch `nl` locale with extra `name.first_name`, `address.street_suffix`, and `company.suffix`.

## v25.0.3
Release Date: 2019-01-06

* Added `f.Images.LoremFlickrUrl()` (https://loremflickr.com) image service.
* Issue 193: Turkish :tr: state/providence names added to 'tr' locale.

## v25.0.2
Release Date: 2018-12-11

* Issue 192: Fixed IndexOutOfRangeException when `Company.CompanyName()` is used with `az` locale.

## v25.0.1
Release Date: 2018-11-27

* Add SourceLink compatibility with Visual Studio 2017.
* Obsoleted **LoremPixel.com** `Image` categories. The image service is usually down or very slow. Consider using `Images.PicsumUrl()` as a replacement. This version is an obsolete-warn, next release will have Image category APIs removed.

## v24.3.1
Release Date: 2018-11-03

* `tr` - Turkish locale first/last names updated. Lorem data set updated. Thanks ahmetcanaydemir!
* Added `f.Image.PicsumUrl` (https://picsum.photos) service as faster alternative to Lorem Pixel. 

## v24.3.0
Release Date: 2018-10-02

* Data / feature parity with faker.js @ 9dd5a52
* `af_ZA` - New South Africa (Afrikaans) locale added.
* `zu_ZA` - New South Africa (Zulu) locale added.
* `en_ZA` - South Africa (English) locale updated
* `ru` - Russian locale updated.
* `id_ID` - Indonesia locale updated.
* `es` - Spanish locale updated.
* `f.Images.DatUri` now accepts an HTML color parameter.
* PR 180: Resolved Turkish Culture `.ToLower` causing invalid JSON dataset path `.ToLowerInvariant` now used. 

## v24.2.0
Release Date: 2018-09-27

* Issue 179: Fixed regression introduced in 23.0.3 that forbid setting of internal/non-public members of `T` in when `Faker<T>` is used.

## v24.1.0
Release Date: 2018-09-26, UNPUBLISHED FROM NUGET

* Added `nullWeight` parameter to `.OrNull()` extension method for weighted generation of null values.
* Added new `.OrDefault()` extension method. Thanks @anorborg!

## v24.0.0
Release Date: 2018-09-26, UNPUBLISHED FROM NUGET

* BREAKING: Deterministic values may have changed. Parity with **faker.js** @ 07f39bd3.
* `en_ZA` - South Africa (English) locale added.
* `fr_CH` - French (Switzerland) locale added.
* `pl` locale "phone numbers" updated.
* `sv` locale "names" section updated.
* `en`, `de`, `de_AT` "names" section updated.
* Added `f.Commerce.Ean8` EAN-8 product barcode number generator.
* Added `f.Commerce.Ean13` EAN-13 product barcode number generator.

## v23.0.3
Release Date: 2018-08-29, UNPUBLISHED FROM NUGET 

* PR 170: Faster `Faker[T].Generate()` with setter cache. Approx 1.7x speedup. Thanks Mpdreamz!

## v23.0.2
Release Date: 2018-08-13

* BREAKING CHANGE: Deterministic sequence values may have changed. Unit tests expecting specific values may be different if `Bogus.Person` is used.
* Issue 168: Added `Bogus.Person.Address.State` field.
* Issue 139: Added `Date.SystemClock` static property for setting global time Bogus uses for date calculations.
* Issue 169: `Date.Weekday()` should return a weekday not a month.  

## v22.3.2
Release Date: 2018-07-18

* Added `f.Random.Guid()` for better GUID discoverability.
* PR 164: Added new Tax ID extensions `Person.Nif()` and `Company.Nipc()` for Portugal. Thanks JoseJr!  

## v22.3.1
Release Date: 2018-07-05

* PR 159: Add UK National Insurance Number `f.Finance.Nino()` in `Bogus.Extensions.UnitedKingdom`. Thanks mortware!
* PR 160: `DateTimeOffset` support added to `f.Date` dataset. Methods are suffixed by "Offset". IE: `f.Date.SoonOffset`. Thanks Simon!
* Added `refDate` parameter to `f.Date.Soon` and `f.Date.Recent`.
* `f.Date.Between(start, end)` now respects `DateTimeKind.Utc`.

## v22.2.1
Release Date: 2018-06-29

* PR 153: Possible breaking changes: Minor typo & spelling corrections made to some parameter names. Thanks for the corrections Simon!

## v22.1.4
Release Date: 2018-06-26

* PR 151: Added `Bogus.Distributions.Gaussian` namespace for numerical Normal Distribution generated values! Thanks codersg! 
* Added `uniqueSuffix` parameter to `Internet.Email()` to help with unique email constraints.
 
## v22.1.3
Release Date: 2018-06-14

* PR 149: Added new Arabic 'ar' locale. Thanks Saied!
* PR 148: Fixed `.FullName()` for locales where both first/last name have genders. Thanks binarycode!

## v22.1.2
Release Date: 2018-05-29

* Improved XML documentation comments on `Faker[T]` API.

## v22.1.1
Release Date: 2018-05-20

* PR 144: Argument support for mustache handlebars. Example: `{{name.firstname(Male)}}`
* Using **C# 7.3** generic `Enum` constraints for methods that only accept enums. Example: `f.PickRandom<Enum>()`.

## v22.0.9
Release Date: 2018-05-17

* Issue 143: Fixed rare case when `f.IndexGlobal` could be zero twice at start of generation.
* Fixed typo in XML docs.

## v22.0.8
Release Date: 2018-04-09

* New `Company.Ein()` to generate employer identification numbers.
* Preparing release of extended data sets for Bogus.

## v22.0.7
Release Date: 2018-04-01

* New `Internet.Color()` format options: CSS `rgb(...)` and delimited RGB.
* New `System.AndroidId()` to generate GCM registration ID.
* New `System.ApplePushToken()` to generate a random Apple Push Token.
* New `System.BlackBerryPin()` to generate a random Black Berry PIN.
* New `Randomizer.Hash()` to generate random hashes of specified length.
* New `Randomizer.String2()` to generate random strings with specified character sets.

## v22.0.6
Release Date: 2018-03-29

* Added `Randomizer.String` method to generate strings. Uses `Chars()` method.
* PR 136: Improve speed of `DataSet.ParseTokens()`. Thanks @danij!

## v22.0.5
Release Date: 2018-03-02

* Bogus now throws exceptions for locales it doesn't recognize. Improves developer experience.
* New extension method `.ToBogusLocale()` on `System.Globalization.CultureInfo` to help translate from **.NET** locale codes to **Bogus** locale codes.

## v22.0.3
Release Date: 2018-02-27
 
* Generate more realistic Bitcoin addresses.
* New extension method `Faker<T>.GenerateBetween(min, max)` that generates N objects between `min` and `max`. Located in `Bogus.Extensions`. N should be considered non-deterministic but technically depends on the parameters each time this extension method was called. 
* Added `Lorem.Paragraphs(min, max)` overload.
* Added improved XML doc comments and parameter names on `Lorem.Paragraph`.
* UK extension method `.ShortCode()` renamed to `.SortCode()` as originally intended.
* Marked `DataSet.Get/.GetObject/BObject` methods as `protected internal`. Reducing API surface noise.
* Added new `.OrNull` in `Bogus.Extensions` to help create randomly null values. Example: `.RuleFor(x=>x.Prop, f=>f.Random.Word().OrNull(f))`.
* New groundwork for extending Bogus with premium (paid) data sets and tooling.

## v22.0.2
Release Date: 2018-01-05

* Issue 121: Fixes the inability to `.Ignore(...)` a property or field after a rule had already been set.

## v22.0.1
Release Date: 2017-12-23

* Issue 120: `.Generate(n)` now returns `List<T>` instead of `IList<T>`.
* Added `f.Address.CountryCode()` ISO 3166-1 alpha-3 country code generator.
* New `Bogus.Extensions.Extras` namespace for generally useful helper methods.
* Added `Finance.CreditCardNumberObfuscated()` extension to `Bogus.Extensions.Extras` namespace.
* Moved credit card `CheckDigit()` extensions to `Bogus.Extensions.Extras` namespace. 

## v21.0.5
Release Date: 2017-12-16

* Better error support.
* Added `Person.FullName` field.
* Allowed `Faker<T>.FinishWith` to be called multiple times. Last call wins.

## v21.0.4
Release Date: 2017-12-13

* Fixed `f.Image` URL generation.

## v21.0.2 
* Re-enabled **.NET Standard 1.3** targeting.
* Added `Gender` field to `Person`. Deterministic sequences may have changed.
* Added `Randomizer.Bool(weight)` to generate weighted boolean values of true.
* Italian `CodiceFiscale()` extension method added. Extends `Person` and `Finance`.

## v20.0.2
Release Date: 2017-11-06

* Fixed Issue 102: `f.Random.Uuid()` is now deterministic based on global or local seed.

## v20.0.1
Release Date: 2017-11-04

* Added `Faker<T>.Clone()`: Clones internal state of a `Faker<T>` and allows for complex faking scenarios and rule combinations.
* Added `Faker<T>.UseSeed(n)`: Allows you to specify a localized seed value on a `Faker<T>` instead of a global static `Randomizer.Seed`.
* Stronger `Seed` determinism for multi-threaded scenarios.

## v19.0.2
Release Date: 2017-11-01

* Fixed #99: Possible threading issue that can cause `System.ArgumentException`.

## v19.0.1
Release Date: 2017-10-26, UNPUBLISHED FROM NUGET

* Using new BSON binary data format for locales.
* Removed dependency on Newtonsoft.Json!
* Locale Updates - 
* `fr`: new street address prefixes.
* `fa`: new street addresses.
* `pl`: removed 2008 value from city.
* `en`: new gender first names
* New Dutch (Belgium) `nl_BE` locale.
* New Romanian `ro` locale.
* Added `f.Finance.RoutingNumber` - Generates an ABA routing number with valid check digit.
* Added `Faker.GenerateForever` that returns `IEnumerable<T>` with unlimited generated items when iterated over.
* Added United Kingdom extension method to generate bank ShortCodes on `f.Finance.ShortCode()`.
* Re-ordered adjective and buzz in the `f.Company.Bs` for a correct gramatics.
* Added `f.Address.Direction`. Generates cardinal or ordinal directions.
* Added `f.Address.CardinalDirection`. Generates "North", "South", etc.
* Added `f.Address.OrdinalDirection`. Generates "Northeast", "Southwest", etc.

## v18.0.2
Release Date: 2017-09-14

* Issue 86: Removed diacritic mark/accents (á, í, ó, ú, etc) from generated email addresses and user names.
* Added `string.RemoveDiacritics` helper method.

## v18.0.1
Release Date: 2017-09-13

* Fixed bug in Finland's `f.Person.Henkilötunnus` personal identity code generator that sometimes produced 11 characters.
* Added `f.Finance.Ethereum`. Generate an Ethereum address.
* Added `f.Finance.CreditCardCvv`. Generate a random credit card CVV number.
* Improved `f.Finance.CreditCardNumber`. Generate a random credit card number.
* Added `f.Random.Hexadecimal`. Generates a random hexadecimal string.
* Added `f.System.DirectoryPath`. Generates a random directory path.
* Added `f.System.FilePath`. Generates a random file path.
* Added `f.Date.Soon`. Generates a date and time that will happen soon.
* Added `f.Random.ArrayElements`. Gets a random subset of an array.
* Added `f.Random.ListItems`. Gets a random subset of a list.
* Added `f.Company.Cnpj` extension method for Brazil. Generates Brazilian company document.
* Improved `f.PhoneNumbers`. More realistic US phone numbers.
* Improved `f.Address.Latitude/Longitude` with min and max parameters.
* Minimum for `f.Commerce.Price` is now $1.00 (not zero).
* Reduced assembly size by removing redundant locale data.
* Locale updates:
* `en_AU` - Update Australian postcode ranges.
* `en_IND` - Indian postcodes are always numeric.
* `ru` - Word corrections.

## v17.0.1
Release Date: 2017-08-24

* Migration to **.NET Standard 2.0**.

## v16.0.3
Release Date: 2017-08-24

* With additional overloads for `.PickRandom(IList)` and `.PickRandom(ICollection)` we can now add `.PickRandom("cat", "dog", "fish")` back to the API. 

## v16.0.2
Release Date: 2017-08-23

* BREAKING CHANGE: `Faker.Generate(n)` now calls `.ToList()` under the hood to escape LINQ deferred execution. Remembering to call `.ToList()` after `.Generate(n)` was a sticking point for new users writing test assertions on generated values. Please do not call `Faker.Generate(n).ToList()` as it would execute `.ToList()` twice. Simply, `Faker.Generate(n)` is enough.
* `f.Generate` and `f => f.Make` now return `IList<T>` to signify the breaking change above. 
* Issue #92: Added `.GenerateLazy` to keep old behavior and returns `IEnumerable<T>`.
* Issue #93: Renamed a `PickRandom` overload to avoid the compiler from picking wrong `PickRandom` method.
* Added `f.PickRandomParam("cat","dog","fish")`. 
* Removed `[Obsolete]` methods. 

## v15.0.7
Release Date: 2017-08-20

* Issue #88 - API aesthetics: Added `Name.FullName()` convenience method to generate a full name.

## v15.0.6
Release Date: 2017-08-02

* PR #87: Added `.Rules()` method on `RuleSet`. Thanks @digitalcyote.

## v15.0.5
Release Date: 2017-07-28

* Add parameter for including `Currency` fund codes (BOV, CLF, COU, MXV, UYI).
* Fixed minor issue in `Person.Email` having duplicate names.
* Helper method: `f.PickRandomWithout(ExcludeItem1, ExcludeItem2)` added.
* Helper method: `f.PickRandom("cat", "dog", "fish")` added.
* Performance: 40% reduction in Bogus' DLL size and memory footprint by removing whitespace in **Json** data files.
* Newtonsoft dependency update 10.0.3.  

## v15.0.3
Release Date: 2017-05-06

* Added `f => f.Rant` to generate random user content like product reviews.
* Added `new Faker[T].Rules( (f, t) => ...)` as a shortcut for building rules quickly.
* Added `Address.FullAddress`
* Added `Internet.UrlWithPath` allowing to create URLs with random paths.
* Added `ru` (Russian locale) hacker adjective, ing-verb, noun, and verb.
* Added `Internet.Mac` address separator parameter.
* Feature parity with **faker.js** @ 6cdb93ef...
* Using new C# 7 features. =)

## v15.0.1
Release Date: 2017-04-11

* Building with Visual Studio 2017.
* Issue 70: Fixed `ArgumentException` that occurs with derived hidden `new` properties.

## v12.0.2
* `f => f.Generate(n, i => ...)` overload allows use of index when using `f.Generate`.

## v12.0.1
Release Date: 2017-03-27

* PR 64: Improved `.PickRandom(IEnumerable)` performance. Thanks @chuuddo.
* Added `"string".ClampLength(max,min)` extension method to clamp length of strings between min and max.
* Issue 67: Fixed `Randomizer.Int(int.MaxValue, int.MinValue)` range overflow not returning random `int32` values.

## v11.0.5
Release Date: 2017-03-20

* Compatibility with `Newtonsoft.Json` v10.0.1

## v11.0.4
Release Date: 2017-03-14

* Added `.RuleFor(x.Item, "foo")`. Eliminates ceremony of `f =>` for simple values.

## v11.0.3
Release Date: 2017-03-13

* Added range option to `Sentence`.

## v11.0.2
Release Date: 2017-02-23

* New Feature: Allow implicit and explicit type casts: `Order o = orderFaker` and `var o = (Order)orderFaker` without having to call `orderFaker.Generate()`.

## v11.0.1
Release Date: 2017-02-21

* Added `IndexGlobal` alias for `UniqueIndex`.
* Added `IndexFaker` for uniqueness in Faker[T] lifetime.
* Added `IndexVariable` a developer controlled index convenience variable. 
* Added `Database` dataset to `f => f.Database` facade.
* Fixed Issue 57 - Avoid unexpected behavior with Parent-Child generators using `UniqueIndex`.
* Removed some `Internet.Avatars` that returned 404.
* Reached feature/data parity with `faker.js` v4.1.0.

## v10.0.1
Release Date: 2017-02-18

* `Internet.UserAgent` - Generates browser user agent strings.
* `Internet.Password` - Generates user passwords using regex.
* Added `az`/Azerbaijani locale.
* Disallow `/` character in `System.FileName`.
* `Helpers.Slugify` properly replaces spaces with `-` dashes.
* `Lorem.Slug` slugs some lorem text.
* `Finance.Iban` - Generates International Bank Account Numbers (IBAN).
* `Finance.Bic` - Generates Bank Identifier Code (BIC) codes.
* `Random.WeightedRandom` - Returns a weighted random distribution of items.
* Allow `https://` in random images.
* `Images.DataUri` - Generates "data:image/svg" URI with width and height.
* `Database` data set for generating column, collation, type stuff.

## v9.0.2
Release Date: 2017-01-19

* Bug: Issue 54: Work around for Visual Studio IntelliSense.

## v9.0.1
Release Date: 2017-01-18

* New Feature: Bogus is now a signed assembly; PublicToken: fa1bb3f3f218129a

## v8.0.4
Release Date: 2017-01-16

* New Feature: Added `PickRandom(IEnumerable)` overload. Thanks joleharkes.

## v8.0.3
Release Date: 2016-12-20

* New Feature: `RuleForType(typeof(string))` allows bulk/default for a particular type on a class. Useful for very large classes with a specific type.

## v8.0.2
Release Date: 2016-12-07

* Issue 46. Fixed threading deadlock situation with static faker initialization. Thanks Mpdreamz.
* Added `f => f.Generate(count, ()=> f.Phone.PhoneNumber())` helper for better fluency when filling properties with `List` of `T`.

## v8.0.1
Release Date: 2016-11-25

* Added `pt_PT` Portuguese (Portugal) locale. 

## v8.0.1-beta-1
Release Date: 2016-10-22

* Allow `Faker<T>.RuleFor` rules to be overridden. Last set rule wins.

## v7.1.7
Release Date: 2016-10-11

* `Faker<T>.AssertConfigurationIsValid` to help in unit testing scenarios.
* Add `Internet.Ipv6` method to generate IPv6 addresses.

## v7.1.6
Release Date: 2016-08-07

* Added `f => f.Commerce` on `Faker`.

## v7.1.5
Release Date: 2016-07-27

* Added `cz`/Czech locale
* Updated `en`, `nl`, `pl`, `sk`, `sv` locales.
* Realistic Dutch city naming
* `Randomizer.AlphaNumeric` added.
* `Randomizer.Double` now accepts `(min,max)` arguments
* Added convenience `Randomizer` for random `Decimal`, `Float`, `Byte`, `Bytes`, `SByte`, `Int`, `UInt`, `ULong`, `Long`, `Short`, `UShort`, `Char` and `Chars`.

## v7.1.4
Release Date: 2016-07-06

* Newtonsoft Json 9.0.1 dependency support.

## v7.1.3
Release Date: 2016-06-27

* :boom: .NET Core 1.0 RTM Support.

## v7.1.3-beta-1
Release Date: 2016-05-20

* Compatibility with .NET Standard 1.3 and .NET Core RC2.

## v7.1.2
Release Date: 2016-05-16

* Roll-up Release for .NET Framework since last non-beta release.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v7.1.2-beta-1
Release Date: 2016-05-16

* Clamp Randomizer maximum value to int.MaxValue.

## v7.1.1
Release Date: 2016-05-15

* Roll-up Release for .NET Framework since last non-beta release.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v7.1.1-beta-1
Release Date: 2016-05-15

* Locale update
* Date.Recent(0) generates random times for current date between midnight and now.
* New `System` data set for generating fake file names and mime-types.
* Added `Date.Timespan` for random timespan values.
* Added `System.Semver` for random semantic versions.
* Added `System.Version` for random System.Version objects.
* Added `Internet.ExampleEmail` for simple @example.com email generation.
* Added `Finance.BitcoinAddress` for random bitcoin addresses.
* BREAKING: Fake "seeded" data generated by Bogus may be different from previous versions.
* WARN: Address.City may have changed in some random seeds

## v6.1.1
Release Date: 2016-03-29

* Roll-up Release for .NET Framework since last non-beta release.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v6.1.1-beta-1
Release Date: 2016-03-29

* Fixed index out of bounds bug in faker.Random.Word().
* Commerce.Department output may have changed as a result of this fix.

## v5.1.1-beta-3
Release Date: 2016-03-23

* Removed RuleFor(x = x.Prop, constantValue), was confusing the API.
* Added 0-arity RuleFor(x = x.Prop, () => someValue)

## v5.1.1-beta-2
Release Date: 2016-03-22

* Make f.UniqueIndex as int for convenience.
* Use generic RuleFor(x = x.Prop, constantValue).

## v5.1.1-beta-1
Release Date: 2016-03-21

* New RuleFor(x = x.Prop, constantValue)
* Support for Hashids.net: RuleFor(x = x.Id, f = f.Hashids.Encode())
* New f.UniqueIndex, useful for composing property values that require uniqueness.

## v5.0.1
Release Date: 2016-02-25

* Roll-up Release for .NET Framework since v4.0.1.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v5.0.1-beta-2
Release Date: 2016-02-25

* JvanderStad PR15: Lazy load Person. Avoids extra Seed.Next calls that may interfere with seeded content.
* JvanderStad PR16: Better address generation. Respects locale address formats.
* Added "dotnet5.4" moniker support.
* BREAKING: Fake "seeded" data generated by Bogus may be different from previous versions.

## v4.0.1
Release Date: 2016-02-15

* Roll-up Release for .NET Framework since v3.0.6.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v4.0.1-beta-1
Release Date: 2016-02-15

* Bogus - Feature parity with faker.js.
* System module added. Generate random file names and extensions.
* Randomizer - Added Uuid().
* Locales Updated: en_GB, sv, sk, de_CH, en.
* Locales Added: id_ID, el, lv.
* Prevent apostrophes in return value of Internet.DomainWords
* Added more parameters for Image data set.
* BREAKING API METHODS:
* Lorem - Better API methods: Seeded tests based on "content" will fail due to upgrade.

## v3.0.6
Release Date: 2016-01-21

* Roll-up Release for .NET Framework since v3.0.5.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.6-beta-1
Release Date: 2016-01-21

* Issue #13: Fixed StrictMode to exclude private fields.
* New Feature: Ignore property or field in StrictMode: Faker[Order].Ignore(o => o.OrderId).
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5
Release Date: 2016-01-20

* Roll-up Release for .NET Framework since v3.0.4.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5-beta-4
Release Date: 2016-01-19

* Issue #13: StrictMode() now ignores read-only properties.
* Newtonsoft.Json v8 compatibility.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5-beta-3
Release Date: 2016-01-18

* Issue #12: Make Bogus thread-safe on Generate() and DataSets. Avoids threading issues in test runners.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5-beta-2
Release Date: 2016-01-11

* CoreCLR support (CoreCLR users please continue using latest beta release until CoreCLR is RTM.).

## v3.0.4
Release Date: 2015-12-10

* Issue 10: Make Bogus work with fields also, not just properties. Fixes LINQPad issues.

## v3.0.3
Release Date: 2015-12-09

* PR 9: quantumplation - Fixed typo in Lorem.Sentance() -> Lorem.Sentence()

## v3.0.2
Release Date: 2015-11-24

* Generate US: SSN - Social Security Numbers.
* Generate Canada: SIN - Social Insurance Numbers.
* Generate Brazil: Cadastro de Pessoas Fisicas - CPF Numbers.
* Generate Finland: Henkilotunnus - Person ID numbers
* Generate Denmark: Det Centrale Personregister - Person ID numbers.
* Allow exclude values on Randomizer.Enum.
* Randomizer.Replace accepts '*' replace with letter or digit.
* Added Lorem.Letter(num).
* Can switch locale on Name: f.Name["en"].LastName()

## v3.0.1
Release Date: 2015-10-22

* Added debug symbols to symbolsource.org.
* PR#6: Fixed lastname and empty list exception -salixzs
* Switch to semantic versioning at par with FakerJS.

## v3.0.0.4
* Adding generators: Date.Month(), Date.Weekday()
* Sentences using lexically correct "A foo bar."
* Added Spanish Mexico (es_MX) locale.

## v3.0.0.3
Release Date: 2015-07-21

* Issue #2: Use latest Newtonsoft.Json 7.0.0.0 -Mpdreamz

## v3.0.0.2
Release Date: 2015-07-11

* Includes Ireland (English) locale.

## v3.0.0.1
Release Date: 2015-07-11

* Migrated to new FakerJS datafile format. Build system uses gulp file to directly import FakerJS locales.
* Faker.Parse() can now tokenize and replace handlebar formats.
* Added Faker.Hacker and Faker.Company properties.
* Added Custom separator on Lorem.Paragraph.
* Added Canada (French) (fr_CA) locale.
* Added Ukrainian (uk) locale.
* Added Ireland (en_IE) locale.
* Added Internet.Mac for mac addresses.
* Support for Canadian post/zip codes.
* Exposed Name.JobTitle, Name.JobDescriptor, Name.JobArea, Name.JobType
* Exposed Address.CountryCode
* Replace symbols in domain words so it generates output for all locales
* Support for gender names, but only for locales that support it. Russian('ru') but not English('en').
* Corrected abbreviation for Yukon to reflect its official abbreviation of "YT".

## v2.1.5.2
Release Date: 2015-06-22

* Fixed instantiating a Person in a non-US locale. -antongeorgiev

## v2.1.5.1
Release Date: 2015-06-11

* Added Georgian, Turkish, and Chinese (Taiwan) locales.
* Added Name.JobTitle()
* Added Internet.Url() and Internet.Protocol().
* Sync'd up with faker.js v2.1.5.

## v2.1.4.2
Release Date: 2015-06-11

* Fixed bug in Faker.Person and Faker[T] that generates new person context after every new object.
* Added support for .FinishWith() for post-processing that runs after all rules but before returning new instance.
* Added Newtonsoft.Json as NuGet dependency.

## v2.1.4.1
Release Date: 2015-06-10

* Minor changes, mostly XML doc update and Person moved from DataSet to Bogus namespace.

## v2.1.4
Release Date: 2015-06-08

* Initial port from faker.js 2.1.4.
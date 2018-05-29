## v22.1.2
* Improved XML documentation comments on `Faker[T]` API.

## v22.1.1
* PR 144: Argument support for mustache handlebars. Example: `{{name.firstname(Male)}}`
* Using **C# 7.3** generic `Enum` constraints for methods that only accept enums. Example: `f.PickRandom<Enum>()`.

## v22.0.9
* Issue 143: Fixed rare case when `f.IndexGlobal` could be zero twice at start of generation.
* Fixed typo in XML docs.

## v22.0.8
* New `Company.Ein()` to generate employer identification numbers.
* Preparing release of extended data sets for Bogus.

## v22.0.7
* New `Internet.Color()` format options: CSS `rgb(...)` and delimited RGB.
* New `System.AndroidId()` to generate GCM registration ID.
* New `System.ApplePushToken()` to generate a random Apple Push Token.
* New `System.BlackBerryPin()` to generate a random Black Berry PIN.
* New `Randomizer.Hash()` to generate random hashes of specified length.
* New `Randomizer.String2()` to generate random strings with specified character sets.

## v22.0.6
* Added `Randomizer.String` method to generate strings. Uses `Chars()` method.
* PR 136: Improve speed of `DataSet.ParseTokens()`. Thanks @danij!

## v22.0.5
* Bogus now throws exceptions for locales it doesn't recognize. Improves developer experience.
* New extension method `.ToBogusLocale()` on `System.Globalization.CultureInfo` to help translate from **.NET** locale codes to **Bogus** locale codes.

## v22.0.3 
* Generate more realistic Bitcoin addresses.
* New extension method `Faker<T>.GenerateBetween(min, max)` that generates N objects between `min` and `max`. Located in `Bogus.Extensions`. N should be considered non-deterministic but technically depends on the parameters each time this extension method was called. 
* Added `Lorem.Paragraphs(min, max)` overload.
* Added improved XML doc comments and parameter names on `Lorem.Paragraph`.
* UK extension method `.ShortCode()` renamed to `.SortCode()` as originally intended.
* Marked `DataSet.Get/.GetObject/BObject` methods as `protected internal`. Reducing API surface noise.
* Added new `.OrNull` in `Bogus.Extensions` to help create randomly null values. Example: `.RuleFor(x=>x.Prop, f=>f.Random.Word().OrNull(f))`.
* New groundwork for extending Bogus with premium (paid) data sets and tooling.

## v22.0.2
* Issue 121: Fixes the inability to `.Ignore(...)` a property or field after a rule had already been set.

## v22.0.1
* Issue 120: `.Generate(n)` now returns `List<T>` instead of `IList<T>`.
* Added `f.Address.CountryCode()` ISO 3166-1 alpha-3 country code generator.
* New `Bogus.Extensions.Extras` namespace for generally useful helper methods.
* Added `Finance.CreditCardNumberObfuscated()` extension to `Bogus.Extensions.Extras` namespace.
* Moved credit card `CheckDigit()` extensions to `Bogus.Extensions.Extras` namespace. 

## v21.0.5
* Better error support.
* Added `Person.FullName` field.
* Allowed `Faker<T>.FinishWith` to be called multiple times. Last call wins.

## v21.0.4
* Fixed `f.Image` URL generation.

## v21.0.2 
* Re-enabled **.NET Standard 1.3** targeting.
* Added `Gender` field to `Person`. Deterministic sequences may have changed.
* Added `Randomizer.Bool(weight)` to generate weighted boolean values of true.
* Italian `CodiceFiscale()` extension method added. Extends `Person` and `Finance`.

## v20.0.2
* Fixed Issue 102: `f.Random.Uuid()` is now deterministic based on global or local seed.

## v20.0.1
* Added `Faker<T>.Clone()`: Clones internal state of a `Faker<T>` and allows for complex faking scenarios and rule combinations.
* Added `Faker<T>.UseSeed(n)`: Allows you to specify a localized seed value on a `Faker<T>` instead of a global static `Randomizer.Seed`.
* Stronger `Seed` determinism for multi-threaded scenarios.

## v19.0.2
* Fixed #99: Possible threading issue that can cause `System.ArgumentException`.

## v19.0.1
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
* Issue 86: Removed diacritic mark/accents (á, í, ó, ú, etc) from generated email addresses and user names.
* Added `string.RemoveDiacritics` helper method.

## v18.0.1
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
* Migration to **.NET Standard 2.0**.

## v16.0.3
* With additional overloads for `.PickRandom(IList)` and `.PickRandom(ICollection)` we can now add `.PickRandom("cat", "dog", "fish")` back to the API. 

## v16.0.2
* BREAKING CHANGE: `Faker.Generate(n)` now calls `.ToList()` under the hood to escape LINQ deferred execution. Remembering to call `.ToList()` after `.Generate(n)` was a sticking point for new users writing test assertions on generated values. Please do not call `Faker.Generate(n).ToList()` as it would execute `.ToList()` twice. Simply, `Faker.Generate(n)` is enough.
* `f.Generate` and `f => f.Make` now return `IList<T>` to signify the breaking change above. 
* Issue #92: Added `.GenerateLazy` to keep old behavior and returns `IEnumerable<T>`.
* Issue #93: Renamed a `PickRandom` overload to avoid the compiler from picking wrong `PickRandom` method.
* Added `f.PickRandomParam("cat","dog","fish")`. 
* Removed `[Obsolete]` methods. 

## v15.0.7
* Issue #88 - API aesthetics: Added `Name.FullName()` convenience method to generate a full name.

## v15.0.6
* PR #87: Added `.Rules()` method on `RuleSet`. Thanks @digitalcyote.

## v15.0.5
* Add parameter for including `Currency` fund codes (BOV, CLF, COU, MXV, UYI).
* Fixed minor issue in `Person.Email` having duplicate names.
* Helper method: `f.PickRandomWithout(ExcludeItem1, ExcludeItem2)` added.
* Helper method: `f.PickRandom("cat", "dog", "fish")` added.
* Performance: 40% reduction in Bogus' DLL size and memory footprint by removing whitespace in **Json** data files.
* Newtonsoft dependency update 10.0.3.  

## v15.0.3
* Added `f => f.Rant` to generate random user content like product reviews.
* Added `new Faker[T].Rules( (f, t) => ...)` as a shortcut for building rules quickly.
* Added `Address.FullAddress`
* Added `Internet.UrlWithPath` allowing to create URLs with random paths.
* Added `ru` (Russian locale) hacker adjective, ing-verb, noun, and verb.
* Added `Internet.Mac` address separator parameter.
* Feature parity with **faker.js** @ 6cdb93ef...
* Using new C# 7 features. =)

## v15.0.1
* Building with Visual Studio 2017.
* Issue 70: Fixed `ArgumentException` that occurs with derived hidden `new` properties.

## v12.0.2
* `f => f.Generate(n, i => ...)` overload allows use of index when using `f.Generate`.

## v12.0.1
* PR 64: Improved `.PickRandom(IEnumerable)` performance. Thanks @chuuddo.
* Added `"string".ClampLength(max,min)` extension method to clamp length of strings between min and max.
* Issue 67: Fixed `Randomizer.Int(int.MaxValue, int.MinValue)` range overflow not returning random `int32` values.

## v11.0.5
* Compatibility with `Newtonsoft.Json` v10.0.1

## v11.0.4
* Added `.RuleFor(x.Item, "foo")`. Eliminates ceremony of `f =>` for simple values.

## v11.0.3
* Added range option to `Sentence`.

## v11.0.2
* New Feature: Allow implicit and explicit type casts: `Order o = orderFaker` and `var o = (Order)orderFaker` without having to call `orderFaker.Generate()`.

## v11.0.1
* Added `IndexGlobal` alias for `UniqueIndex`.
* Added `IndexFaker` for uniqueness in Faker[T] lifetime.
* Added `IndexVariable` a developer controlled index convenience variable. 
* Added `Database` dataset to `f => f.Database` facade.
* Fixed Issue 57 - Avoid unexpected behavior with Parent-Child generators using `UniqueIndex`.
* Removed some `Internet.Avatars` that returned 404.
* Reached feature/data parity with `faker.js` v4.1.0.

## v10.0.1
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
* Bug: Issue 54: Work around for Visual Studio IntelliSense.

## v9.0.1
* New Feature: Bogus is now a signed assembly; PublicToken: fa1bb3f3f218129a

## v8.0.4
* New Feature: Added `PickRandom(IEnumerable)` overload. Thanks joleharkes.

## v8.0.3
* New Feature: `RuleForType(typeof(string))` allows bulk/default for a particular type on a class. Useful for very large classes with a specific type.

## v8.0.2
* Issue 46. Fixed threading deadlock situation with static faker initialization. Thanks Mpdreamz.
* Added `f => f.Generate(count, ()=> f.Phone.PhoneNumber())` helper for better fluency when filling properties with `List` of `T`.

## v8.0.1
* Added `pt_PT` Portuguese (Portugal) locale. 

## v8.0.1-beta-1
* Allow `Faker<T>.RuleFor` rules to be overridden. Last set rule wins.

## v7.1.7
* `Faker<T>.AssertConfigurationIsValid` to help in unit testing scenarios.
* Add `Internet.Ipv6` method to generate IPv6 addresses.

## v7.1.6
* Added `f => f.Commerce` on `Faker`.

## v7.1.5
* Added `cz`/Czech locale
* Updated `en`, `nl`, `pl`, `sk`, `sv` locales.
* Realistic Dutch city naming
* `Randomizer.AlphaNumeric` added.
* `Randomizer.Double` now accepts `(min,max)` arguments
* Added convenience `Randomizer` for random `Decimal`, `Float`, `Byte`, `Bytes`, `SByte`, `Int`, `UInt`, `ULong`, `Long`, `Short`, `UShort`, `Char` and `Chars`.

## v7.1.4
* Newtonsoft Json 9.0.1 dependency support.

## v7.1.3
* :boom: .NET Core 1.0 RTM Support.

## v7.1.3-beta-1
* Compatibility with .NET Standard 1.3 and .NET Core RC2.

## v7.1.2
* Roll-up Release for .NET Framework since last non-beta release.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v7.1.2-beta-1
* Clamp Randomizer maximum value to int.MaxValue.

## v7.1.1
* Roll-up Release for .NET Framework since last non-beta release.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v7.1.1-beta-1
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
* Roll-up Release for .NET Framework since last non-beta release.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v6.1.1-beta-1
* Fixed index out of bounds bug in faker.Random.Word().
* Commerce.Department output may have changed as a result of this fix.

## v5.1.1-beta-3
* Removed RuleFor(x = x.Prop, constantValue), was confusing the API.
* Added 0-arity RuleFor(x = x.Prop, () => someValue)

## v5.1.1-beta-2
* Make f.UniqueIndex as int for convenience.
* Use generic RuleFor(x = x.Prop, constantValue).

## v5.1.1-beta-1
* New RuleFor(x = x.Prop, constantValue)
* Support for Hashids.net: RuleFor(x = x.Id, f = f.Hashids.Encode())
* New f.UniqueIndex, useful for composing property values that require uniqueness.

## v5.0.1
* Roll-up Release for .NET Framework since v4.0.1.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v5.0.1-beta-2
* JvanderStad PR15: Lazy load Person. Avoids extra Seed.Next calls that may interfere with seeded content.
* JvanderStad PR16: Better address generation. Respects locale address formats.
* Added "dotnet5.4" moniker support.
* BREAKING: Fake "seeded" data generated by Bogus may be different from previous versions.

## v4.0.1
* Roll-up Release for .NET Framework since v3.0.6.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v4.0.1-beta-1
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
* Roll-up Release for .NET Framework since v3.0.5.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.6-beta-1
* Issue #13: Fixed StrictMode to exclude private fields.
* New Feature: Ignore property or field in StrictMode: Faker[Order].Ignore(o => o.OrderId).
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5
* Roll-up Release for .NET Framework since v3.0.4.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5-beta-4
* Issue #13: StrictMode() now ignores read-only properties.
* Newtonsoft.Json v8 compatibility.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5-beta-3
* Issue #12: Make Bogus thread-safe on Generate() and DataSets. Avoids threading issues in test runners.
* CoreCLR users please continue using latest beta release until CoreCLR is RTM.

## v3.0.5-beta-2
* CoreCLR support (CoreCLR users please continue using latest beta release until CoreCLR is RTM.).

## v3.0.4
* Issue 10: Make Bogus work with fields also, not just properties. Fixes LINQPad issues.

## v3.0.3
* PR 9: quantumplation - Fixed typo in Lorem.Sentance() -> Lorem.Sentence()

## v3.0.2
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
* Added debug symbols to symbolsource.org.
* PR#6: Fixed lastname and empty list exception -salixzs
* Switch to semantic versioning at par with FakerJS.

## v3.0.0.4
* Adding generators: Date.Month(), Date.Weekday()
* Sentences using lexically correct "A foo bar."
* Added Spanish Mexico (es_MX) locale.

## v3.0.0.3
* Issue #2: Use latest Newtonsoft.Json 7.0.0.0 -Mpdreamz

## v3.0.0.2
* Includes Ireland (English) locale.

## v3.0.0.1
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

## v2.1.5.2:
* Fixed instantiating a Person in a non-US locale. -antongeorgiev

## v2.1.5.1:
* Added Georgian, Turkish, and Chinese (Taiwan) locales.
* Added Name.JobTitle()
* Added Internet.Url() and Internet.Protocol().
* Sync'd up with faker.js v2.1.5.

## v2.1.4.2:
* Fixed bug in Faker.Person and Faker[T] that generates new person context after every new object.
* Added support for .FinishWith() for post-processing that runs after all rules but before returning new instance.
* Added Newtonsoft.Json as NuGet dependency.

## v2.1.4.1:
* Minor changes, mostly XML doc update and Person moved from DataSet to Bogus namespace.

## v2.1.4.0:
* Initial port from faker.js 2.1.4.
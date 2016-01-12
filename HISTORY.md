## v3.0.5-beta-1
* CoreCLR support

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
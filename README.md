[![Build status](https://ci.appveyor.com/api/projects/status/dxa14myphnlbplc6/branch/master?svg=true)](https://ci.appveyor.com/project/bchavez/bogus)  [![Twitter](https://img.shields.io/twitter/url/https/github.com/bchavez/Bogus.svg?style=social)](https://twitter.com/intent/tweet?text=Simple%20and%20Sane%20Fake%20Data%20Generator%20for%20.NET:&amp;amp;url=https%3A%2F%2Fgithub.com%2Fbchavez%2FBogus) [![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/bchavez/Bogus) <a href="http://www.jetbrains.com/resharper"><img src="http://i61.tinypic.com/15qvwj7.jpg" alt="ReSharper" title="ReSharper"></a>
<img src="https://raw.githubusercontent.com/bchavez/Bogus/master/Docs/logo.png" align='right' />

Bogus for .NET: C#, F#, and VB.NET
======================

Project Description
-------------------
Hello. I'm your host **[Brian Chavez](https://github.com/bchavez)** ([twitter](https://twitter.com/bchavez)). **Bogus** is a simple and sane fake data generator for **.NET** languages like **C#**, **F#** and **VB.NET**. **Bogus** is fundamentally a **C#** port of [`faker.js`](https://github.com/marak/Faker.js/)
and inspired by [`FluentValidation`](https://github.com/JeremySkinner/FluentValidation)'s syntax sugar.

**Bogus** will help you load databases, UI and apps with fake data for your testing needs. If you like **Bogus** star :star: the repository and show your friends! :smile: If you find **Bogus** useful consider supporting the project by purchasing a [**Bogus Premium**](#bogus-premium-extensions) license that gives you extra **Bogus** superpowers! :dizzy: :muscle: 


### Download & Install
**Nuget Package [Bogus](https://www.nuget.org/packages/Bogus/)**

```powershell
Install-Package Bogus
```
Minimum Requirements: **.NET Standard 1.3** or **.NET Standard 2.0** or **.NET Framework 4.0**.

##### Projects That Use Bogus

* [**Elasticsearch .NET Client (NEST)**](https://github.com/elastic/elasticsearch-net) [[code]](https://github.com/elastic/elasticsearch-net/tree/82c938893b2ff4ddca03a8e977ad14a16da712ba/src/Tests/Framework/MockData)
* [**GraphQL for .NET**](https://github.com/graphql-dotnet/graphql-dotnet) [[code]](https://github.com/graphql-dotnet/graphql-dotnet/blob/3c7bd27dbd00c3971a7b7bfef4f277946e716e25/src/GraphQL.DataLoader.Tests/Models/Fake.cs)
* [**Microsoft Windows-XAML / Template10**](https://github.com/Windows-XAML/Template10) [[code]](https://github.com/Windows-XAML/Template10/blob/beed5e58a4f8ab381cff6f063d2a91db5b4fc3bc/Basics/PrismSample/Services/DataService.cs#L1)
* [**Microsoft Learning / Developing Microsoft Azure Solutions**](https://github.com/MicrosoftLearning/20532-DevelopingMicrosoftAzureSolutions) [[code]](https://github.com/MicrosoftLearning/20532-DevelopingMicrosoftAzureSolutions/blob/4bb595f6b908798f8b3d49773455699102650806/Allfiles/Mod03/Labfiles/Starter/Contoso.Events.Data/ContextInitializer.cs)
* [**Microsoft OfficeDev / Microsoft Teams Sample Connector**](https://github.com/OfficeDev/microsoft-teams-sample-connector-csharp) [[code]](https://github.com/OfficeDev/microsoft-teams-sample-connector-csharp/blob/8805bb1acb136949905e4644c4e714dd7b70a61a/TeamsToDoAppConnector/Utils/TaskHelper.cs)

##### Featured In
* [**Microsoft Build 2018 - Azure Tips and Tricks - May 8th, 2018**](https://www.youtube.com/watch?v=088e5IUqF6g&t=12m31s)
* **NuGet Must Haves - [Top 10 Unit Testing Libraries in 2017](http://nugetmusthaves.com/article/top-unit-testing-libraries)**
* **[.NET Rocks Podcast - #BetterKnowThatFramework - March 16th 2017](https://twitter.com/bchavez/status/842479138850070528)**
* **[.NET Engineering Blog: NuGet Package of the week #1. - "This week in .NET - December 8th 2015"](https://blogs.msdn.microsoft.com/dotnet/2015/12/08/the-week-in-net-12082015/)**

##### Blog Posts
* [Jack Histon](https://twitter.com/jackhiston) - [How to Create Bogus Data in C#](http://jackhiston.com/2017/10/1/how-to-create-bogus-data-in-c/)
* [Christos Matskas](https://twitter.com/christosmatskas) - [Creating .NET fakes using Bogus](https://cmatskas.com/creating-net-fakes-using-bogus-2/)
* [Jason Roberts](https://twitter.com/robertsjason) - [Lifelike Test Data Generation with Bogus](http://dontcodetired.com/blog/post/Lifelike-Test-Data-Generation-with-Bogus)
* Mark Timmings - [Auto generating test data with Bogus](http://putridparrot.com/blog/auto-generating-test-data-with-bogus/)
* [.NET Core Generating Test Data](https://coderulez.wordpress.com/2017/05/10/net-core-generating-test-data/)
* Steve Leigh - [Seedy Fake Users](http://stevesspace.com/2017/01/seedy-fake-users/)
* Dominik Roszkowski - [Bogus fake data generator in .Net testing](http://dominikroszkowski.pl/2017/07/bogus-in-testing/)

##### The Crypto Tip Jar!
<a href="https://commerce.coinbase.com/checkout/2faa393a-6fc3-4365-993a-6cc110bc4d35"><img src="https://raw.githubusercontent.com/bchavez/Bogus/master/Docs/tipjar.png" /></a>
* :dog2: **Dogecoin**: `D6Y9oaf963cgcjp6AgD6sDWWLTXGGYx9r2`


Usage
-----
### The Great C# Example

```csharp
public enum Gender
{
    Male,
    Female
}

//Set the randomzier seed if you wish to generate repeatable data sets.
Randomizer.Seed = new Random(8675309);

var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

var orderIds = 0;
var testOrders = new Faker<Order>()
    //Ensure all properties have rules. By default, StrictMode is false
    //Set a global policy by using Faker.DefaultStrictMode
    .StrictMode(true)
    //OrderId is deterministic
    .RuleFor(o => o.OrderId, f => orderIds++)
    //Pick some fruit from a basket
    .RuleFor(o => o.Item, f => f.PickRandom(fruit))
    //A random quantity from 1 to 10
    .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));


var userIds = 0;
var testUsers = new Faker<User>()
    //Optional: Call for objects that have complex initialization
    .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))

    //Use an enum outside scope.
    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())

    //Basic rules using built-in generators
    .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
    .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
    .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
    .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
    .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")

    //Use a method outside scope.
    .RuleFor(u => u.CartId, f => Guid.NewGuid())
    //Compound property with context, use the first/last name properties
    .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
    //And composability of a complex collection.
    .RuleFor(u => u.Orders, f => testOrders.Generate(3).ToList())
    //Optional: After all rules are applied finish with the following action
    .FinishWith((f, u) =>
        {
            Console.WriteLine("User Created! Id={0}", u.Id);
        });

var user = testUsers.Generate();
Console.WriteLine(user.DumpAsJson());

/* OUTPUT:
User Created! Id=0
 *
{
  "Id": 0,
  "FirstName": "Audrey",
  "LastName": "Spencer",
  "FullName": "Audrey Spencer",
  "UserName": "Audrey_Spencer72",
  "Email": "Audrey82@gmail.com",
  "Avatar": "https://s3.amazonaws.com/uifaces/faces/twitter/itstotallyamy/128.jpg",
  "CartId": "863f9462-5b88-471f-b833-991d68db8c93",
  "SSN": "923-88-4231",
  "Gender": 0,
  "Orders": [
    {
      "OrderId": 0,
      "Item": "orange",
      "Quantity": 8
    },
    {
      "OrderId": 1,
      "Item": "banana",
      "Quantity": 2
    },
    {
      "OrderId": 2,
      "Item": "kiwi",
      "Quantity": 9
    }
  ]
} */
```

#### [**Click here for F# and VB.NET examples!**](#f-and-vbnet-examples)

### Locales

Since we're a port of **faker.js**, we support a whole bunch of different
locales. Here's an example in Korean:

```csharp
[Test]
public void With_Korean_Locale()
{
    var lorem = new Bogus.DataSets.Lorem(locale: "ko");
    Console.WriteLine(lorem.Sentence(5));
}

/* 국가는 무상으로 행위로 의무를 구성하지 신체의 처벌받지 예술가의 경우와 */
```

**Bogus** supports the following locales:

| Locale Code    | Language                | | Locale Code    | Language                 |
|:--------------:|:-----------------------:|-|:--------------:|:------------------------:|
|`af_ZA         `|South Africa (Afrikaans)  ||`ge            `|Georgian                  |
|`ar            `|Arabic                    ||`id_ID         `|Indonesia                 |
|`az            `|Azerbaijani               ||`it            `|Italian                   |
|`cz            `|Czech                     ||`ja            `|Japanese                  |
|`de            `|German                    ||`ko            `|Korean                    |
|`de_AT         `|German (Austria)          ||`lv            `|Latvian                   |
|`de_CH         `|German (Switzerland)      ||`nb_NO         `|Norwegian                 |
|`el            `|Greek                     ||`nep           `|Nepalese                  |
|`en            `|English                   ||`nl            `|Dutch                     |
|`en_AU         `|Australia (English)       ||`nl_BE         `|Dutch (Belgium)           |
|`en_au_ocker   `|Australia Ocker (English) ||`pl            `|Polish                    |
|`en_BORK       `|Bork (English)            ||`pt_BR         `|Portuguese (Brazil)       |
|`en_CA         `|Canada (English)          ||`pt_PT         `|Portuguese (Portugal)     |
|`en_GB         `|Great Britain (English)   ||`ro            `|Romanian                  |
|`en_IE         `|Ireland (English)         ||`ru            `|Russian                   |
|`en_IND        `|India (English)           ||`sk            `|Slovakian                 |
|`en_US         `|United States (English)   ||`sv            `|Swedish                   |
|`en_ZA         `|South Africa (English)    ||`tr            `|Turkish                   |
|`es            `|Spanish                   ||`uk            `|Ukrainian                 |
|`es_MX         `|Spanish Mexico            ||`vi            `|Vietnamese                |
|`fa            `|Farsi                     ||`zh_CN         `|Chinese                   |
|`fr            `|French                    ||`zh_TW         `|Chinese (Taiwan)          |
|`fr_CA         `|Canada (French)           ||`zu_ZA         `|South Africa (Zulu)       |
|`fr_CH         `|French (Switzerland)      ||||


***Note:*** Some locales may not have a complete data set. For example, [`zh_CN`](https://github.com/Marak/faker.js/tree/master/lib/locales/zh_CN) does not have a `lorem` data set, but [`ko`](https://github.com/Marak/faker.js/tree/master/lib/locales/ko) has a `lorem` data set. **Bogus** will default to `en` if a *locale-specific* data set is not found. To further illustrate the previous example, the missing `zh_CN:lorem` data set will default to the `en:lorem` data set.

If you'd like to help contribute new locales or update existing ones please see our
[Creating Locales](https://github.com/bchavez/Bogus/wiki/Creating-Locales) wiki page
for more info.

### Without Fluent Syntax

You can use **Bogus** without a fluent setup. The examples below highlight three alternative ways to use **Bogus** without a fluent syntax setup.
1. Using the `Faker` facade.
2. Using **DataSets** directly.
3. Using `Faker<T>` **inheritance**.

All three alternative styles of using **Bogus** produce the same `Order` result as demonstrated below:
```csharp
public void Using_The_Faker_Facade()
{
    var faker = new Faker("en");
    var o = new Order()
        {
            OrderId = faker.Random.Number(1, 100),
            Item = faker.Lorem.Sentence(),
            Quantity = faker.Random.Number(1, 10)
        };
    o.Dump()
}
public void Using_DataSets_Directly()
{
    var random = new Bogus.Randomizer();
    var lorem = new Bogus.DataSets.Lorem("en");
    var o = new Order()
        {
            OrderId = random.Number(1, 100),
            Item = lorem.Sentence(),
            Quantity = random.Number(1, 10)
        };
    o.Dump();
}
public void Using_FakerT_Inheritance()
{
   public class OrderFaker : Faker<Order> {
      public OrderFaker() {
         RuleFor(o => o.OrderId, f => f.Random.Number(1, 100));
         RuleFor(o => o.Item, f => f.Lorem.Sentence());
         RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));
      }
   }
   var orderFaker = new OrderFaker();
   var o = orderFaker.Generate();
   o.Dump();
}
/* OUTPUT:
{
  "OrderId": 61,
  "Item": "vel est ipsa",
  "Quantity": 7
} */
```

### Bogus API Support
* **`Address`**
	* `ZipCode` - Get a zipcode.
	* `City` - Get a city name.
	* `StreetAddress` - Get a street address.
	* `CityPrefix` - Get a city prefix.
	* `CitySuffix` - Get a city suffix.
	* `StreetName` - Get a street name.
	* `BuildingNumber` - Get a building number.
	* `StreetSuffix` - Get a street suffix.
	* `SecondaryAddress` - Get a secondary address like 'Apt. 2' or 'Suite 321'.
	* `County` - Get a county.
	* `Country` - Get a country.
	* `FullAddress` - Get a full address like Street, City, Country.
	* `CountryCode` - Get a random ISO 3166-1 country code.
	* `State` - Get a state.
	* `StateAbbr` - Get a state abbreviation.
	* `Latitude` - Get a Latitude
	* `Longitude` - Get a Longitude
	* `Direction` - Generates a cardinal or ordinal direction. IE: Northwest, South, SW, E.
	* `CardinalDirection` - Generates a cardinal direction. IE: North, South, E, W.
	* `OrdinalDirection` - Generates an ordinal direction. IE: Northwest, Southeast, SW, NE.
* **`Commerce`**
	* `Department` - Get a random commerce department.
	* `Price` - Get a random product price.
	* `Categories` - Get random product categories.
	* `ProductName` - Get a random product name.
	* `Color` - Get a random color.
	* `Product` - Get a random product.
	* `ProductAdjective` - Random product adjective.
	* `ProductMaterial` - Random product material.
	* `Ean8` - Get a random EAN-8 barcode number.
	* `Ean13` - Get a random EAN-13 barcode number.
* **`Company`**
	* `CompanySuffix` - Get a company suffix. "Inc" and "LLC" etc.
	* `CompanyName` - Get a company name.
	* `CatchPhrase` - Get a company catch phrase.
	* `Bs` - Get a company BS phrase.
* **`Database`**
	* `Column` - Generates a column name.
	* `Type` - Generates a column type.
	* `Collation` - Generates a collation.
	* `Engine` - Generates a storage engine.
* **`Date`**
	* `Past` - Get a `DateTime` in the past between `refDate` and `yearsToGoBack`.
	* `PastOffset` - Get a `DateTimeOffset` in the past between `refDate` and `yearsToGoBack`.
	* `Soon` - Get a `DateTime` that will happen soon.
	* `SoonOffset` - Get a `DateTimeOffset` that will happen soon.
	* `Future` - Get a `DateTime` in the future between `refDate` and `yearsToGoForward`.
	* `FutureOffset` - Get a `DateTimeOffset` in the future between `refDate` and `yearsToGoForward`.
	* `Between` - Get a random `DateTime` between `start` and `end`.
	* `BetweenOffset` - Get a random `DateTimeOffset` between `start` and `end`.
	* `Recent` - Get a random `DateTime` within the last few days.
	* `RecentOffset` - Get a random `DateTimeOffset` within the last few days.
	* `Timespan` - Get a random `TimeSpan`.
	* `Month` - Get a random month
	* `Weekday` - Get a random weekday
* **`Finance`**
	* `Account` - Get an account number. Default length is 8 digits.
	* `AccountName` - Get an account name. Like "savings", "checking", "Home Loan" etc..
	* `Amount` - Get a random amount. Default 0 - 1000.
	* `TransactionType` - Get a transaction type: "deposit", "withdrawal", "payment", or "invoice".
	* `Currency` - Get a random currency.
	* `CreditCardNumber` - Generate a random credit card number with valid Luhn checksum.
	* `CreditCardCvv` - Generate a credit card CVV
	* `BitcoinAddress` - Generates a random Bitcoin address.
	* `EthereumAddress` - Generate a random Ethereum address.
	* `RoutingNumber` - Generates an ABA routing number with valid check digit.
	* `Bic` - Generates Bank Identifier Code (BIC) code.
	* `Iban` - Generates an International Bank Account Number (IBAN).
* **`Hacker`**
	* `Abbreviation` - Returns an abbreviation.
	* `Adjective` - Returns a adjective.
	* `Noun` - Returns a noun.
	* `Verb` - Returns a verb.
	* `IngVerb` - Returns an -ing verb.
	* `Phrase` - Returns a phrase.
* **`Images`**
	* `Image` - Gets a random image.
	* `Abstract` - Gets an abstract looking image.
	* `Animals` - Gets an image of an animal.
	* `Business` - Gets a business looking image.
	* `Cats` - Gets a picture of a cat.
	* `City` - Gets a city looking image.
	* `Food` - Gets an image of food.
	* `Nightlife` - Gets an image with city looking nightlife.
	* `Fashion` - Gets an image in the fashion category.
	* `People` - Gets an image of humans.
	* `Nature` - Gets an image of nature.
	* `Sports` - Gets an image related to sports.
	* `Technics` - Get a technology related image.
	* `Transport` - Get a transportation related image.
	* `DataUri` - Get a SVG data URI image with a specific width and height.
* **`Internet`**
	* `Avatar` - Generates a legit Internet URL avatar from twitter accounts.
	* `Email` - Generates an email address.
	* `ExampleEmail` - Generates an example email with @example.com.
	* `UserName` - Generates user names.
	* `DomainName` - Generates a random domain name.
	* `DomainWord` - Generates a domain word used for domain names.
	* `DomainSuffix` - Generates a domain name suffix like .com, .net, .org
	* `Ip` - Gets a random IP address.
	* `Ipv6` - Generates a random IPv6 address.
	* `UserAgent` - Generates a random user agent.
	* `Mac` - Gets a random mac address.
	* `Password` - Generates a random password.
	* `Color` - Gets a random aesthetically pleasing color near the base RGB. See [here](http://stackoverflow.com/questions/43044/algorithm-to-randomly-generate-an-aesthetically-pleasing-color-palette).
	* `Protocol` - Returns a random protocol. HTTP or HTTPS.
	* `Url` - Generates a random URL.
	* `UrlWithPath` - Get a random URL with random path.
* **`Lorem`**
	* `Word` - Get a random lorem word.
	* `Words` - Get some lorem words
	* `Letter` - Get a character letter.
	* `Sentence` - Get a random sentence of specific number of words.
	* `Sentences` - Get some sentences.
	* `Paragraph` - Get a paragraph.
	* `Paragraphs` - Get a specified number of paragraphs.
	* `Text` - Get random text on a random lorem methods.
	* `Lines` - Get lines of lorem.
	* `Slug` - Slugify lorem words.
* **`Name`**
	* `FirstName` - Get a first name. Getting a gender specific name is only supported on locales that support it.
	* `LastName` - Get a last name. Getting a gender specific name is only supported on locales that support it.
	* `FullName` - Get a full name, concatenation of calling FirstName and LastName.
	* `Prefix` - Gets a random prefix for a name.
	* `Suffix` - Gets a random suffix for a name.
	* `FindName` - Gets a full name.
	* `JobTitle` - Gets a random job title.
	* `JobDescriptor` - Get a job description.
	* `JobArea` - Get a job area expertise.
	* `JobType` - Get a type of job.
* **`PhoneNumbers`**
	* `PhoneNumber` - Get a phone number.
	* `PhoneNumberFormat` - Gets a phone number via format array index as defined in a locale's phone_number.formats[] array.
* **`Rant`**
	* `Review` - Generates a random user review.
	* `Reviews` - Generate an array of random reviews.
* **`System`**
	* `FileName` - Get a random file name.
	* `DirectoryPath` - Get a random directory path (Unix).
	* `FilePath` - Get a random file path (Unix).
	* `MimeType` - Get a random mime type
	* `CommonFileType` - Returns a commonly used file type.
	* `CommonFileExt` - Returns a commonly used file extension.
	* `FileType` - Returns any file type available as mime-type.
	* `FileExt` - Gets a random extension for the given mime type.
	* `Semver` - Get a random semver version string.
	* `Version` - Get a random `Version`.
	* `Exception` - Get a random `Exception` with a fake stack trace.
	* `AndroidId` - Get a random GCM registration ID.
	* `ApplePushToken` - Get a random Apple Push Token
	* `BlackBerryPin` - Get a random BlackBerry Device PIN

#### API Extension Methods
* **`using Bogus.Extensions.Brazil;`**
	* `Bogus.Person.Cpf()` - Cadastro de Pessoas Físicas
	* `Bogus.DataSets.Company.Cnpj()` - Cadastro Nacional da Pessoa Jurídica
* **`using Bogus.Extensions.Canada;`**
	* `Bogus.Person.Sin()` - Social Insurance Number for Canada
* **`using Bogus.Extensions.Denmark;`**
	* `Bogus.Person.Cpr()` - Danish Personal Identification number
* **`using Bogus.Extensions.Finland;`**
	* `Bogus.Person.Henkilötunnus()` - Finnish Henkilötunnus
* **`using Bogus.Extensions.Italy;`**
	* `Bogus.Person.CodiceFiscale()` - Codice Fiscale
	* `Bogus.DataSets.Finance.CodiceFiscale()` - Codice Fiscale
* **`using Bogus.Extensions.Portugal;`**
	* `Bogus.Person.Nif()` - Número de Identificação Fiscal (NIF)
	* `Bogus.DataSets.Company.Nipc()` - Número de Identificação de Pessoa Colectiva (NIPC)
* **`using Bogus.Extensions.UnitedKingdom;`**
	* `Bogus.DataSets.Finance.SortCode()` - Banking Sort Code
	* `Bogus.DataSets.Finance.Nino()` - National Insurance Number
* **`using Bogus.Extensions.UnitedStates;`**
	* `Bogus.Person.Ssn()` - Social Security Number
	* `Bogus.DataSets.Company.Ein()` - Employer Identification Number
* **`using Bogus.Distributions.Gaussian;`**
    * `Randomizer.GaussianInt()` - Generate an `int` based on a specific normal distribution.
    * `Randomizer.GaussianFloat()` - Generate a `float` based on a specific normal distribution.
    * `Randomizer.GaussianDouble()` - Generate a `double` based on a specific normal distribution.
    * `Randomizer.GaussianDecimal()` - Generate a `decimal` based on a specific normal distribution.
    
#### Amazing Community Extensions
* [**AutoBogus**](https://github.com/nickdodd79/AutoBogus) ([`NuGet Package`](https://www.nuget.org/packages?q=AutoBogus)) by [@nickdodd79](https://github.com/nickdodd79/) - Extends **Bogus** by adding automatic `.RuleFor()` creation and population capabilities.
* [**NaughtyStrings.Bogus**](https://github.com/SimonCropp/NaughtyStrings) ([`NuGet Package`](https://www.nuget.org/packages/NaughtyStrings.Bogus/)) by [@SimonCropp](https://github.com/SimonCropp) - Extends **Bogus** with list of naughty strings which have a high probability of causing issues when used as user-input data. Examples:
    * `.SQLInjection()` - Strings which can cause a SQL injection if inputs are not sanitized.
    * `.ScriptInjection()` - Strings which attempt to invoke a benign script injection; shows vulnerability to XSS.
    * `.iOSVulnerabilities()` - Strings which crashed iMessage in various versions of iOS. 
    * `.KnownCVEsandVulnerabilities()` - Strings that test for known vulnerabilities.
    * `.ServerCodeInjection()` - Strings which can cause user to run code on server as a privileged user.
    * and more!
* [**WaffleGenerator.Bogus**](https://github.com/SimonCropp/WaffleGenerator) ([`NuGet Package`](https://www.nuget.org/packages/WaffleGenerator.Bogus/)) by [@SimonCropp](https://github.com/SimonCropp) - The Waffle Generator produces of text which, on first glance, looks like real, ponderous, prose; replete with clichés.
* [**NodaTime.Bogus**](https://github.com/SimonCropp/NodaTime.Bogus) ([`NuGet Package`](https://www.nuget.org/packages/NodaTime.Bogus/)) by [@SimonCropp](https://github.com/SimonCropp) - Adds support for [NodaTime](https://nodatime.org/) to **Bogus**. 
* [**CountryData.Bogus**](https://github.com/SimonCropp/CountryData) ([`NuGet Package`](https://www.nuget.org/packages/CountryData.Bogus/)) by [@SimonCropp](https://github.com/SimonCropp) - Wrapper around [GeoNames Data](https://www.geonames.org/). Examples:
    * `.Country().Name()` - Random country name.
    * `.Country().CurrencyCode()` - Random currency code.
    * `.Australia().Capital()` - Country capital.
    * `.Country().Iceland().PostCode()` - Random country post code.
* [**AustralianElectorates.Bogus**](https://github.com/SimonCropp/AustralianElectorates) ([`NuGet Package`](https://www.nuget.org/packages/AustralianElectorates.Bogus/)) by [@SimonCropp](https://github.com/SimonCropp) - Wrapper around Australian Electoral Commission (AEC) data (https://www.aec.gov.au/). Examples:
    * `.AustralianElectorates().Electorate()` - Random electorate.
    * `.AustralianElectorates().Name()` - Random electorate name.
    * `.AustralianElectorates().CurrentMember()` - Random current electorate member for parliament.
    * `.AustralianElectorates().CurrentMemberName()` - Random name of current a electorate member for parliament.
    * `.AustralianElectorates().Member()` - Random electorate member for parliament.
    * `.AustralianElectorates().MemberName()` - Random name of a electorate member for parliament.

## Bogus Premium Extensions!
<img src="https://raw.githubusercontent.com/bchavez/Bogus/master/Docs/logo_green.png" align='left' height="42px" width="42px" style="padding-right: 15px" /> **Bogus Premium** [[**Purchase Now!**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium)] by [@bchavez](https://github.com/bchavez)<br/>You can help support the **Bogus** open source project by purchasing a [**Bogus Premium**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium) license! With an active premium license you'll be supporting this cool open-source project. You'll also gain new superpowers that extended **Bogus** with new features and exclusive data sets! Check 'em out below!

* **Premium:** [**Bogus.Tools.Analyzer**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium) ([NuGet Package](https://www.nuget.org/packages/Bogus.Tools.Analyzer/)) - Save time using this handy Roslyn analyzer to generate and detect missing `.RuleFor()` rules at development & compile time! This tool is included with the [**Bogus Premium**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium) license!
  <img src="https://raw.githubusercontent.com/bchavez/Bogus/master/Docs/bogus_premium_tools_analyzer_demo.gif" />
* **Premium:** [**Bogus.Locations**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium) ([NuGet Package](https://www.nuget.org/packages/Bogus.Locations/)) - A dataset that contains real geographical information for places and things. Create fake GPS points and paths. Helpful for creating geodesic data for location-based aware apps.
  * **`Location`**
    * `Altitude` - Generate a random altitude, in meters. Default max height is 8848m (Mount Everest). Heights are always positive.
    * `AreaCircle` - Get a latitude and longitude within a specific radius in meters.
    * `Depth` - Generate a random depth, in meters. Default max depth is -10994m (Mariana Trench). Depths are always negative.
    * `Geohash` - Generates a random Geohash. [See](https://en.wikipedia.org/wiki/Geohash).
* **Premium:** [**Bogus.Healthcare**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium) ([NuGet Package](https://www.nuget.org/packages/Bogus.Healthcare/)) - A data set for the Healthcare industry. Generate fake medical records, injuries, diagnosis, drugs, dosages, human anatomy, and ICD-9 medical codes. Especially helpful in HIPAA regulated environments!
  * **`Drugs`**
    * `Administration` - Get how a drug should be administered. IE: oral, nasal, injection.
    * `Dosage` - Get a drug dosage with MG units
    * `DosageAmount` - Get a drug dosage.
    * `DosageForm` - Get the form of a drug. IE: tablet, capsule, gel.
    * `Ingredient` - Get a drug ingredient. IE: folic acid, magnesium hydroxide, ibuprofen.
    * `Vitamin` - Get a random vitamin.
  * **`Human`**
    * `BloodType` - Get a random blood type. Ex: A+, OB
    * `BodyPartExternal` - Get an external body part name. IE: Head, Arm, Leg.
    * `BodyPartInternal` - Get an internal body part name. IE: Bladder, Lung, Heart.
    * `BodyRegion` - Get a human body region. IE: Head and Neck, Thorax.
    * `BodySystem` - Get a human body system. IE: Digestive, Nervous, Circulatory.
    * `Diagnosis` - Shortcut to Icd9.DiagnosisLongDescription".
    * `InfectiousDisease` - Get an infectious disease. IE: Chickenpox, Polio, Zika Fever.
    * `Pain` - Get a human pain. Ex: Chest pain, Headache, Toothache.
    * `Plasma` - Get a random plasma type. Ex: O, A, B, AB
    * `Procedure` - Shortcut to Icd9.ProcedureLongDescription.
  * **`Icd9`**
    * `DiagnosisCode` - Get a ICD9 diagnosis code.
    * `DiagnosisEntry` - Get a medical diagnosis.
    * `DiagnosisLongDescription` - Get a medical diagnosis description. IE: Meningitis due to coxsackie virus.
    * `DiagnosisShortDescription` - Get a short description of a medical diagnosis.
    * `ProcedureCode` - Get a ICD9 procedure code.
    * `ProcedureEntry` - Get a medical procedure.
    * `ProcedureLongDescription` - Get a medical procedure description.
    * `ProcedureShortDescription` - Get a short description of a medical procedure.
  * **`Medical`**
    * `Hospital` - Get a random hospital.
    * `HospitalCity` - Get a hospital city.
    * `HospitalName` - Get a random hospital name. IE: UCLA Medical Center
    * `HospitalState` - Get a hospital state.
    * `HospitalStreetAddress` - Get a hospital street address.
    * `HospitalZipCode` - Get a hospital ZipCode.
    * `Phrase` - Get a random medical phrase.
    * `Word` - Get a medical word.
* **Premium:** [**Bogus.Hollywood**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium) ([NuGet Package](https://www.nuget.org/packages/Bogus.Hollywood/))- A dataset for the Entertainment industry.
  * **`Movies`**
    * `ActorName` - Get a famous actor. IE: Keanu Reeves, Liam Neeson, and Natalie Portman.
    * `MovieCollection` - Get a random movie collection. IE: Star Wars Collection, Indiana Jones Collection.
    * `MovieOverview` - Get a random movie overview
    * `MovieReleaseDate` - Get a random movie release date.
    * `MovieTagline` - Get a random movie tagline.
    * `MovieTitle` - Get a random movie title
    * `Production` - Get a production company.
  * **`Tv`**
    * `ActorName` - Get a famous actor. IE: Keanu Reeves, Liam Neeson, and Natalie Portman.
    * `Network` - Get a random TV network. IE: BBC, ABC, NBC, FOX.
    * `Production` - Get a production company
    * `Series` - Get a name of a TV series. IE: Rick and Morty, Silicon Valley, The Walking Dead
* **Premium:** [**Bogus.Text**](https://github.com/bchavez/Bogus/wiki/Bogus-Premium) ([NuGet Package](https://www.nuget.org/packages/Bogus.Text/)) - A dataset that contains historical texts in the public domain. Create fake sentences from famous speeches, classic books, and law
  * **`Literature`**
    * `CommonSense` - Text from "Common Sense, by Thomas Paine (1776)"
    * `JfkSpeech` - Text from "JFK's Inaugural Address"
    * `Knowledge` - Text from "A Treatise Concerning the Principles of Human knowledge, by George Berkeley (1710)"

---

### Helper Methods
The features shown below come standard with the [**Bogus**](https://www.nuget.org/packages/Bogus/) NuGet package.

#### Person
If you want to generate a `Person` with context relevant properties like
an email that looks like it belongs to someone with the same first/last name,
create a person!

```csharp
[Test]
public void Create_Context_Related_Person()
{
    var person = new Bogus.Person();

    person.Dump();
}

/* OUTPUT:
{
  "FirstName": "Lee",
  "LastName": "Brown",
  "UserName": "Lee_Brown3",
  "Avatar": "https://s3.amazonaws.com/uifaces/faces/twitter/ccinojasso1/128.jpg",
  "Email": "Lee_Brown369@yahoo.com",
  "DateOfBirth": "1984-01-16T21:31:27.87666",
  "Address": {
    "Street": "2552 Bernard Rapid",
    "Suite": "Suite 199",
    "City": "New Haskell side",
    "ZipCode": "78425-0411",
    "Geo": {
      "Lat": -35.8154,
      "Lng": -140.2044
    }
  },
  "Phone": "1-500-790-8836 x5069",
  "Website": "javier.biz",
  "Company": {
    "Name": "Kuphal and Sons",
    "CatchPhrase": "Organic even-keeled monitoring",
    "Bs": "open-source brand e-business"
  }
} */
```

#### Replace

Replace a formatted string with random numbers `#`, letters `?`, or `*` random number or letter:
```csharp
[Test]
public void Create_an_SSN()
{
    var ssn = new Bogus.Randomizer().Replace("###-##-####");
    ssn.Dump();

    var code = new Randomizer().Replace("##? ??? ####");
    code.Dump();

    var serial = new Randomizer().Replace("**-****");
    code.Dump();
}
/* OUTPUT:
"618-19-3064"
"39E SPC 0790"
"L3-J9N5"
*/
```

#### Parse Handlebars
You can also parse strings in the following format:
```csharp
[Test]
public void Handlebar()
{
    var faker = new Faker();
    var randomName = faker.Parse("{{name.lastName}}, {{name.firstName}} {{name.suffix}}");
    randomName.Dump();
}

/* OUTPUT:
"Roob, Michale PhD"
*/
```
The name of a dataset is determined using `DataCategory` attribute or class name otherwise. (i.e `PhoneNumber` dataset in handlebars expression should be named as `phone_number`)

You can pass parameters to methods using braces:

```csharp
[Test]
public void HandlebarWithParameters()
{
    var faker = new Faker();
    var randomName = faker.Parse("{{name.firstname(Female)}}, {{name.firstname(Male)}}");
    randomName.Dump();
}

/* OUTPUT:
"Lindsay, Jonathan"
*/
```

#### Implicit and Explicit Type Conversion
You can also use implicit type conversion to make your code look cleaner without having to explicitly call `Faker<T>.Generate()`.

```csharp
var orderFaker = new Faker<Order>()
                     .RuleFor(o => o.OrderId, f => f.IndexVariable++)
                     .RuleFor(o => o.Item, f => f.Commerce.Product())
                     .RuleFor(o => o.Quantity, f => f.Random.Number(1,3));

Order testOrder1 = orderFaker;
Order testOrder2 = orderFaker;
testOrder1.Dump();
testOrder2.Dump();

/* OUTPUT:
{
  "OrderId": 0,
  "Item": "Computer",
  "Quantity": 2
}
{
  "OrderId": 1,
  "Item": "Tuna",
  "Quantity": 3
}
*/

//Explicit works too!
var anotherOrder = (Order)orderFaker;
```

#### Bulk Rules
Sometimes writing `.RuleFor(x => x.Prop, ...)` can get repetitive, use the `.Rules((f, t) => {...})` shortcut to specify rules in bulk as shown below:
```
public void create_rules_for_an_object_the_easy_way()
{
    var faker = new Faker<Order>()
        .StrictMode(false)
        .Rules((f, o) =>
            {
                o.Quantity = f.Random.Number(1, 4);
                o.Item = f.Commerce.Product();
                o.OrderId = 25;
            });
    Order o = faker.Generate();
}
```
***Note***: When using the bulk `.Rules(...)` action, `StrictMode` cannot be set to `true` since individual properties of type `T` cannot be indpendently checked to ensure each property has a rule.

F# and VB.NET Examples
----------------------
#### The Fabulous F# Examples
* Using the `Faker` facade with immutable **F#** record types:

```fsharp
type Customer = { FirstName : string
                  LastName : string
                  Age : int
                  Title : string }

//The faker facade
let f = Faker();

let generator() = 
   { FirstName = f.Name.FirstName()
     LastName  = f.Name.LastName()
     Age       = f.Random.Number(18,60)
     Title     = f.Name.JobTitle() }
     
generator() |> Dump |> ignore

(* OUTPUT:
  FirstName = "Russell"
  LastName = "Nader"
  Age = 34
  Title = "Senior Web Officer"
*)
```

* Using the `Faker<T>` class with immutable **F#** record types:

```fsharp
type Customer = { FirstName : string
                  LastName : string
                  Age : int
                  Title : string }

let customerFaker =
    Bogus
        .Faker<Customer>()
        .CustomInstantiator(fun f ->
             { FirstName = f.Name.FirstName()
               LastName  = f.Name.LastName()
               Age       = f.Random.Number(18,60)
               Title     = f.Name.JobTitle() })

customerFaker.Generate() |> Dump |> ignore

(* OUTPUT:
  FirstName = "Sasha"
  LastName = "Roberts"
  Age = 20;
  Title = "Internal Security Specialist"
*)
```

* Using the `Faker<T>` class with mutable classes in **F#**:

```fsharp
open Bogus
type Customer() =
  member val FirstName = "" with get, set
  member val LastName = "" with get, set
  member val Age = 0 with get,set
  member val Title = "" with get,set

let faker = 
        Faker<Customer>()
          //Make a rule for each property
          .RuleFor( (fun c -> c.FirstName), fun (f:Faker) -> f.Name.FirstName() )
          .RuleFor( (fun c -> c.LastName), fun (f:Faker) -> f.Name.LastName() )

          //Or, alternatively, in bulk with .Rules()
          .Rules( fun f c -> 
                    c.Age <- f.Random.Int(18,35) 
                    c.Title <- f.Name.JobTitle() )
  
faker.Generate() |> Dump |> ignore

(* OUTPUT:
  FirstName: Jarrell
  LastName: Tremblay
  Age: 32
  Title: Senior Web Designer
*)
```

#### The Very Basic VB.NET Example
```visualbasic
Imports Bogus

Public Class Customer
    Public Property FirstName() As String
    Public Property LastName() As String
    Public Property Age() As Integer
    Public Property Title() As String
End Class

Sub Main
    Dim faker As New Faker(Of Customer)
    
    '-- Make a rule for each property
    faker.RuleFor( Function(c) c.FirstName, Function(f) f.Name.FirstName) _
         .RuleFor( Function(c) c.LastName, Function(f) f.Name.LastName) _
         _
         .Rules( Sub(f, c)   '-- Or, alternatively, in bulk with .Rules() 
                   c.Age = f.Random.Int(18,35) 
                   c.Title = f.Name.JobTitle()
                 End Sub )
            
    faker.Generate.Dump
End Sub

' OUTPUT:
' FirstName: Jeremie 
' LastName: Mills 
' Age: 32 
' Title: Quality Supervisor 
```



Building
--------
* Download the source code.
* Run `build.cmd`.

Upon successful build, the results will be in the `\__compile` directory.
The `build.cmd` compiles the C# code and embeds the locales in `Source\Bogus\data`.
If you want to rebuild the NuGet packages run `build.cmd pack` and the NuGet
packages will be in `__package`.

#### Rebundling Locales
If you wish to re-bundle the latest **faker.js** locales, you'll need to first:

1. `git submodule init`
2. `git submodule update`
3. Ensure, [NodeJS](https://nodejs.org/) and `gulp` are properly installed.
4. `cd Source\Builder`
5. `npm install` to install required dev dependencies.
6. `gulp import.locales` to regenerate locales in `Source\Bogus\data`.
7. In solution explorer add any new locales not already included as an
`EmbeddedResource`.
8. Finally, run `build.cmd`.

### License
* [MIT License](https://github.com/bchavez/Bogus/blob/master/LICENSE)




Contributors
---------
Created by [Brian Chavez](http://bchavez.bitarmory.com).

[faker.js](https://github.com/marak/Faker.js/) made possible by Matthew Bergman & Marak Squires.


A big thanks to GitHub and all contributors:

* [Anton Georgiev](https://github.com/antongeorgiev)
* [Martijn Laarman](https://github.com/Mpdreamz)
* [Anrijs Vitolins](https://github.com/salixzs)
* [Pi Lanningham](https://github.com/quantumplation)
* [JvanderStad](https://github.com/JvanderStad)
* [Giuseppe Dimauro](https://github.com/gdimauro)



Bogus for .NET/C#
======================
![Bogus](https://raw.githubusercontent.com/bchavez/Bogus/master/Docs/logo.png)


Project Description
-------------------
A simple and sane fake data generator for C# and .NET. Bogus is a C# port of [faker.js](https://github.com/marak/Faker.js/)
and inspired by FluentValidation's syntax sugar.

**Bogus** will help you load databases, UI and apps with fake data for your testing needs.


### Download & Install
**Nuget Package [Bogus](https://www.nuget.org/packages/Bogus/)**

```
Install-Package Bogus
```


Usage
-----
### The Great Example

```csharp
public enum Gender
{
    Male,
    Female
}

//Set the randomzier seed if you wish to generate repeatable data sets.
Randomizer.Seed = new Random(3897234);

var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

var orderIds = 0;
var testOrders = new Faker<Order>()
    //Ensure all properties have rules.
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

    //Basic rules using built-in generators
    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
    .RuleFor(u => u.LastName, f => f.Name.LastName())
    .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
    .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))

    //Use an enum outside scope.
    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
    //Use a method outside scope.
    .RuleFor(u => u.CartId, f => Guid.NewGuid())
    //Compound property with context, use the first/last name properties
    .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
    //And composability of a complex collection.
    .RuleFor(u => u.Orders, f => testOrders.Generate(3).ToList())
    //After all rules are applied finish with the following action
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

### Locales
    
Since we're a port of **faker.js**, we support a whole bunch of different
locales. Here's an example in Korean:

```csharp
[Test]
public void With_Korean_Locale()
{
    var lorem = new Bogus.DataSets.Lorem(locale: "ko");
    Console.WriteLine(lorem.Sentance(5));
}

/* 국가는 무상으로 행위로 의무를 구성하지 신체의 처벌받지 예술가의 경우와 */
```

**Bogus** has support following locales:

| Locale Code  | Language      |
|:------------:|:-------------:|
|`de`          |German
|`de_AT`       |German (Austria)
|`de_CH`       |German (Switzerland)
|`en`          |English
|`en_AU`       |Australia (English)
|`en_au_ocker` |Australia Ocker (English)
|`en_BORK`     |Bork (English)
|`en_CA`       |Canada (English)
|`en_GB`       |Great Britain (English)
|`en_IND`      |India (English)
|`en_US`       |United States (English)
|`es`          |Spanish
|`fa`          |Farsi
|`fr`          |French
|`fr_CA`       |Canada (French)
|`ge`          |Georgian
|`it`          |Italian
|`ja`          |Japanese
|`ko`          |Korean
|`nb_NO`       |Norwegian
|`nep`         |Nepalese
|`nl`          |Dutch
|`pl`          |Polish
|`pt_BR`       |Portuguese (Brazil)
|`ru`          |Russian
|`sk`          |Slovakian
|`sv`          |Swedish
|`tr`          |Turkish
|`uk`          |Ukrainian
|`vi`          |Vietnamese
|`zh_CN`       |Chinese
|`zh_TW`       |Chinese (Taiwan)


If you'd like to help contribute new locales or update existing ones please see our 
[Creating Locales](https://github.com/bchavez/Bogus/wiki/Creating-Locales) wiki page
for more info.

### Without Fluent Syntax

You can use **Bogus** without a complex setup. Just use a dataset directly.

```csharp
[Test]
public void Without_Fluent_Syntax()
{
    var random = new Bogus.Randomizer();
    var lorem = new Bogus.DataSets.Lorem();
    var o = new Order()
        {
            OrderId = random.Number(1, 100),
            Item = lorem.Sentance(),
            Quantity = random.Number(1, 10)
        };
    o.Dump();
}
/* OUTPUT:
{
  "OrderId": 61,
  "Item": "vel est ipsa",
  "Quantity": 7
} */
```

### Helpers
    
##### Person
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

##### Replace

Replace a formatted string with some numbers:
```csharp
[Test]
public void Create_an_SSN()
{
    var ssn = new Bogus.Randomizer().Replace("###-##-####");
    ssn.Dump();
}
/* OUTPUT:
"618-19-3064"
```

Building
--------
* Download the source code.
* Run `build.bat`.

Upon successful build, the results will be in the `\__package` directory.

### License
* [MIT License](https://github.com/bchavez/Bogus/blob/master/LICENSE)




Contributors
---------
Created by [Brian Chavez](http://bchavez.bitarmory.com).

A big thanks to GitHub and all contributors:

* [Anton Georgiev](https://github.com/antongeorgiev)


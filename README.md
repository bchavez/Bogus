Bogus for .NET/C#
======================

Project Description
-------------------
A simple and sane fake data generator for C# and .NET. Inspired by [faker.js](https://github.com/marak/Faker.js/) and FluentValidation.

![Bogus](https://raw.githubusercontent.com/bchavez/Bogus/master/Docs/logofull.png)

### License
* [MIT License](https://github.com/bchavez/Bogus/blob/master/LICENSE)

### Requirements
* .NET 4.0

### Download & Install
Nuget Package [Bogus](https://www.nuget.org/packages/Bogus/)

```

Install-Package Bogus

```

Building
--------
* Download the source code.
* Run `build.bat`.

Upon successful build, the results will be in the `\__package` directory.


Usage
-----
### The Great Example
#### Some Examples

```csharp


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
// Optional: Call for objects that have complex initialization
    .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))

    // Basic rules using built-in generators
    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
    .RuleFor(u => u.LastName, f => f.Name.LastName())
    .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
    .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))

    // Use an enum outside scope.
    .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
// Use a method outside scope.
    .RuleFor(u => u.CartId, f => Guid.NewGuid())
// Compound property with context, use the first name + random last name
    .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
// And finally, composability of a complex collection.
    .RuleFor(u => u.Orders, f => testOrders.Generate(3).ToList());

var user = testUsers.Generate();

```

-------
#### Handling Callbacks on Your Server


Contributors
---------
Created by [Brian Chavez](http://bchavez.bitarmory.com).

A big thanks to GitHub and all contributors:

* 
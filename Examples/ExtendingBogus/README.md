[1]:https://github.com/bchavez/Bogus#the-great-c-example

## Getting Started with Bogus

#### Requirements
* **.NET Core 3.1** or later

#### Description

The `ExtendingBogus` example shows how to extend **Bogus**' APIs in the following ways:

1. Using a custom C# extension method; see `ExtensionsForAddress.cs`.

   Augmenting **Bogus**' APIs via C# extension is useful to make APIs cleaner and more suitable for your specific situation.

1. Using a custom data set; see `FoodDataSet.cs`. 

   Creating a custom data set is useful when categorizing many 'related' APIs together.

To run the example, perform the following commands *inside* this `ExtendingBogus` folder:

  * `dotnet restore`
  * `dotnet build`
  * `dotnet run`
  
After the `dotnet` commands are successfully executed above, you should see some extended *custom* fake data printed to the console!

```
> dotnet run
```
```json
{
  "FaveDrink": "Soda",
  "FaveCandy": "Jelly bean",
  "PostCode": "M4W"
}
```
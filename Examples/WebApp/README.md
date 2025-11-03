# Bogus

<img width="1918" height="910" alt="image" src="https://github.com/user-attachments/assets/f20ea249-3472-48e0-a8fd-b2f3790db84c" />

## One-Click Development Environment

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=Ivy-Interactive%2FIvy-Examples&machine=standardLinux32gb&devcontainer_path=.devcontainer%2Fbogus%2Fdevcontainer.json&location=EuropeWest)

Click the badge above to open Ivy Examples repository in GitHub Codespaces with:
- **.NET 9.0** SDK pre-installed
- **Ready-to-run** development environment
- **No local setup** required

## Created Using Ivy

Web application created using [Ivy-Framework](https://github.com/Ivy-Interactive/Ivy-Framework).

**Ivy** - The ultimate framework for building internal tools with LLM code generation by unifying front-end and back-end into a single C# codebase. With Ivy, you can build robust internal tools and dashboards using C# and AI assistance based on your existing database.

Ivy is a web framework for building interactive web applications using C# and .NET.

## Interactive Example For Fake Data Generation

This example showcases generating realistic fake data for testing and development using the Bogus library. Configure custom fruit lists and generate randomized orders with quantities and lot numbers.

**What This Application Does:**

- **Manage Fruits**: Add and remove custom fruits to use in order generation
- **Generate Orders**: Create realistic fake orders with random items, quantities, and optional lot numbers
- **Live Preview**: View generated orders in a formatted table with totals
- **Data Table**: Interactive table showing Order ID, Fruit, Quantity, and Lot Number

**Technical Implementation:**

- Uses Bogus `Faker<T>` with fluent rule configuration for type-safe fake data generation
- Implements `StrictMode(true)` to ensure all properties are explicitly defined
- Uses `RuleFor()` to define generation rules for each property
- Demonstrates `OrNull()` for optional properties with configurable probability
- Single C# view (`Apps/BogusApp.cs`) built with Ivy UI primitives including Cards, Tables, and Expandables
- State management with `ImmutableArray` for reactive updates

## How to Run

1. **Prerequisites**: .NET 9.0 SDK
2. **Navigate to the example**:
   ```bash
   cd bogus
   ```
3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
4. **Run the application**:
   ```bash
   dotnet watch
   ```
5. **Open your browser** to the URL shown in the terminal (typically `http://localhost:5010`)

## How to Deploy

Deploy this example to Ivy's hosting platform:

1. **Navigate to the example**:
   ```bash
   cd bogus
   ```
2. **Deploy to Ivy hosting**:
   ```bash
   ivy deploy
   ```
This will deploy your fake data generator application with a single command.

## Learn More

- Bogus library on GitHub: [github.com/bchavez/Bogus](https://github.com/bchavez/Bogus)
- Ivy Documentation: [docs.ivy.app](https://docs.ivy.app)

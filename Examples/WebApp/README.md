# Bogus Web Application Example

Interactive web application showcasing the Bogus library for generating realistic fake data. This example demonstrates how to use Bogus in a web application built with the [Ivy Framework](https://github.com/Ivy-Interactive/Ivy-Framework).

## Overview

This web application provides an interactive demonstration of Bogus's fake data generation capabilities. Configure custom fruit lists and generate randomized orders with quantities and lot numbers.

**What This Application Does:**

- **Manage Fruits**: Add and remove custom fruits to use in order generation
- **Generate Orders**: Create realistic fake orders with random items, quantities, and optional lot numbers
- **Live Preview**: View generated orders in a formatted table with totals
- **Data Table**: Interactive table showing Order ID, Fruit, Quantity, and Lot Number

## Technical Implementation

- Uses Bogus `Faker<T>` with fluent rule configuration for type-safe fake data generation
- Implements `StrictMode(true)` to ensure all properties are explicitly defined
- Uses `RuleFor()` to define generation rules for each property
- Demonstrates `OrNull()` for optional properties with configurable probability
- Single C# view (`Apps/BogusApp.cs`) built with Ivy UI primitives including Cards, Tables, and Expandables
- State management with `ImmutableArray` for reactive updates

## Prerequisites

- **.NET 9.0 SDK** or later
- The application uses the Ivy Framework for the web UI

## How to Run

1. **Navigate to the example directory**:

   ```bash
   cd Examples/WebApp
   ```

2. **Restore dependencies**:

   ```bash
   dotnet restore
   ```

3. **Run the application**:

   ```bash
   dotnet watch
   ```

4. **Open your browser** to the URL shown in the terminal (typically `http://localhost:5010`)

## Project Structure

- `Program.cs` - Application entry point and server configuration
- `Apps/BogusApp.cs` - Main application view demonstrating Bogus usage
- `BogusExample.csproj` - Project file with Bogus and Ivy dependencies
- `Dockerfile` - Docker configuration for containerized deployment

## Learn More

- **Bogus Library**: [github.com/bchavez/Bogus](https://github.com/bchavez/Bogus)
- **Ivy Framework**: [github.com/Ivy-Interactive/Ivy-Framework](https://github.com/Ivy-Interactive/Ivy-Framework)
- **Ivy Documentation**: [docs.ivy.app](https://docs.ivy.app)

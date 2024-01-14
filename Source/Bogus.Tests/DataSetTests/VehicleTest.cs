using Bogus.DataSets;
using FluentAssertions;
using Xunit;

namespace Bogus.Tests.DataSetTests;

public class VehicleTest : SeededTest
{
   private Vehicle vehicle;

   public VehicleTest()
   {
      vehicle = new Vehicle();
   }

   [Fact]
   public void can_get_a_vin_number()
   {
      vehicle.Vin().Should().Be("L3TN1M1OHAY675714");
   }

   [Fact]
   public void cannot_return_vin_bigger_than_17_chars()
   {
      vehicle.Random = new Randomizer(43576);
      vehicle.Vin().Should().HaveLength(17).And.Be("XTVJ5JFU2YBV99999");
   }

   [Fact]
   public void can_get_a_manufacture()
   {
      vehicle.Manufacturer().Should().Be("Maserati");
   }

   [Fact]
   public void can_get_a_model()
   {
      vehicle.Model().Should().Be("Prius");
   }

   [Fact]
   public void can_get_a_type()
   {
      vehicle.Type().Should().Be("Minivan");
   }

   [Fact]
   public void can_get_a_fuel()
   {
      vehicle.Fuel().Should().Be("Gasoline");
   }
}
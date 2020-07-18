using System;
using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions.UnitedKingdom;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.ExtensionTests
{
   public class RegistrationPlateExtensionsForGreatBritainTests : SeededTest
   {
      private readonly ITestOutputHelper testOutputHelper;

       public RegistrationPlateExtensionsForGreatBritainTests(ITestOutputHelper testOutputHelper)
       {
           this.testOutputHelper = testOutputHelper;
       }

       [Fact]
       public void reject_registration_date_before_current_style()
       {
           var vehicle = new Vehicle();

           Action a = () => vehicle.GbRegistrationPlate(new DateTime(2001, 8, 31), new DateTime(2019, 7, 5));
           a.Should().Throw<ArgumentOutOfRangeException>()
              .Where(ex => ex.Message.StartsWith("Can only accept registration dates between 2001-09-01 and 2051-02-28."))
              .Where(ex => ex.ParamName == "dateFrom");
       }

       [Fact]
       public void reject_registration_date_after_current_style()
       {
           var vehicle = new Vehicle();
           Action a = () => vehicle.GbRegistrationPlate(new DateTime(2019, 7, 5), new DateTime(2051, 9, 1));
           a.Should().Throw<ArgumentOutOfRangeException>()
              .Where( ex => ex.Message.StartsWith("Can only accept registration dates between 2001-09-01 and 2051-02-28."))
              .Where( ex => ex.ParamName == "dateTo" );
       }

       [Fact]
       public void date_expressed_wrong_way_around_still_works()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.GbRegistrationPlate(new DateTime(2019, 8, 31), new DateTime(2019, 3, 1));
           plate.Substring(2, 2).Should().Be("19");
       }

       [Fact]
       public void early_part_of_year_is_has_age_holdover_from_previous_year()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.GbRegistrationPlate(new DateTime(2019, 1, 1), new DateTime(2019, 2, 28));
           plate.Substring(2, 2).Should().Be("68");
       }

       [Fact]
       public void mid_part_of_year_is_has_age_equivalent_to_two_digit_year()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.GbRegistrationPlate(new DateTime(2009, 3, 1), new DateTime(2009, 8, 31));
           plate.Substring(2, 2).Should().Be("09");
       }

       [Fact]
       public void end_part_of_year_is_has_age_equivalent_to_two_digit_year_offset_by_fifty()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.GbRegistrationPlate(new DateTime(2009, 9, 1), new DateTime(2009, 12, 31));
           plate.Substring(2, 2).Should().Be("59");
       }

       [Fact]
       public void new_licence_plate_on_each_generate()
       {
           var vehicle = new Vehicle();
           var plates = Enumerable
               .Range(1,10)
               .Select(_ => vehicle.GbRegistrationPlate(new DateTime(2001, 9, 1), new DateTime(2019, 7, 5)))
               .ToArray();

           testOutputHelper.WriteLine(string.Join(Environment.NewLine, plates));
           plates.Distinct().Count().Should().Be(plates.Length);
       }

       [Fact]
       public void edinburgh_mid2007_plates_have_exception_applied_to_them()
       {
          Randomizer.Seed = new Random(293);
          var vehicle = new Vehicle();
          var plate = vehicle.GbRegistrationPlate(new DateTime(2007, 3, 1), new DateTime(2007, 8, 31));
          plate.Should().StartWith("TN07");
       }

       [Theory]
       [InlineData(1, "XE68")]
       [InlineData(2, "XF68")]
       [InlineData(3, "XA19")]
       [InlineData(4, "XB19")]
       [InlineData(5, "XC19")]
       [InlineData(6, "XD19")]
       [InlineData(7, "XE19")]
       [InlineData(8, "XF19")]
       [InlineData(9, "XA69")]
       [InlineData(10, "XB69")]
       [InlineData(11, "XC69")]
       [InlineData(12, "XD69")]
       public void export_plates_have_pseudo_location_marker_based_on_registration_date(int month, string partialPlate)
       {
          Randomizer.Seed = new Random(14);
          var vehicle = new Vehicle();
          var plate = vehicle.GbRegistrationPlate(new DateTime(2019, month, 1), new DateTime(2019, month, DateTime.DaysInMonth(2019, month)));
          plate.Should().StartWith(partialPlate);
       }
   }
}

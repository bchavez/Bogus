using System;
using System.Linq;
using Bogus.DataSets;
using Bogus.Extensions.UnitedKingdom;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Bogus.Tests.ExtensionTests
{
   public class RegistrationPlateExtensionsForUnitedKingdomTests : SeededTest
   {
      private readonly ITestOutputHelper testOutputHelper;

       public RegistrationPlateExtensionsForUnitedKingdomTests(ITestOutputHelper testOutputHelper)
       {
           this.testOutputHelper = testOutputHelper;
       }

       [Fact]
       public void reject_registration_date_before_current_style()
       {
           var vehicle = new Vehicle();

           Action a = () => vehicle.UkRegistrationPlate(new DateTime(2001, 8, 31), new DateTime(2019, 7, 5));
           a.ShouldThrow<ArgumentOutOfRangeException>()
              .WithMessage($"Can only accept registration dates between 2001-09-01 and 2051-02-28.{Environment.NewLine}Parameter name: dateFrom");
       }

       [Fact]
       public void reject_registration_date_after_current_style()
       {
           var vehicle = new Vehicle();
           Action a = () => vehicle.UkRegistrationPlate(new DateTime(2019, 7, 5), new DateTime(2051, 9, 1));
           a.ShouldThrow<ArgumentOutOfRangeException>()
              .WithMessage($"Can only accept registration dates between 2001-09-01 and 2051-02-28.{Environment.NewLine}Parameter name: dateTo");
       }

       [Fact]
       public void date_expressed_wrong_way_around_still_works()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.UkRegistrationPlate(new DateTime(2019, 8, 31), new DateTime(2019, 3, 1));
           plate.Substring(2, 2).Should().Be("19");
       }

       [Fact]
       public void early_part_of_year_is_has_age_holdover_from_previous_year()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.UkRegistrationPlate(new DateTime(2019, 1, 1), new DateTime(2019, 2, 28));
           plate.Substring(2, 2).Should().Be("68");
       }

       [Fact]
       public void mid_part_of_year_is_has_age_equivalent_to_two_digit_year()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.UkRegistrationPlate(new DateTime(2009, 3, 1), new DateTime(2009, 8, 31));
           plate.Substring(2, 2).Should().Be("09");
       }

       [Fact]
       public void end_part_of_year_is_has_age_equivalent_to_two_digit_year_offset_by_fifty()
       {
           var vehicle = new Vehicle();
           var plate = vehicle.UkRegistrationPlate(new DateTime(2009, 9, 1), new DateTime(2009, 12, 31));
           plate.Substring(2, 2).Should().Be("59");
       }

       [Fact]
       public void new_licence_plate_on_each_generate()
       {
           var vehicle = new Vehicle();
           vehicle.Random = new Randomizer(1234);

           var plates = Enumerable
               .Range(1,10)
               .Select(_ => vehicle.UkRegistrationPlate(new DateTime(2001, 9, 1), new DateTime(2019, 7, 5)))
               .ToArray();

           testOutputHelper.WriteLine(string.Join(Environment.NewLine, plates));
           plates.Distinct().Count().Should().Be(plates.Length);
       }
   }
}
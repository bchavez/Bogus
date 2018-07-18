using Bogus.DataSets;
using Bogus.Extensions.UnitedKingdom;
using Bogus.Extensions.Italy;
using Bogus.Extensions.Portugal;
using FluentAssertions;
using System;
using Xunit;

namespace Bogus.Tests.ExtensionTests
{
    public class ExtensionTest : SeededTest
    {
        [Fact]
        public void can_create_sortcode()
        {
            var f = new Finance();
            f.SortCode().Should().Be("61-86-06");
            f.SortCode(false).Should().Be("064391");
        }

        [Fact]
        public void can_create_nino()
        {
            var f = new Finance();
            var nino = f.Nino(false);
            var regex = @"^[A-CEGHJ-PR-TW-Z][A-CEGHJ-NPR-TW-Z][0-9]{2}[0-9]{2}[0-9]{2}[ABCD]";
            nino.Should().MatchRegex(regex);
        }

        [Fact]
        public void can_create_separated_nino()
        {
           var f = new Finance();
           var nino = f.Nino();
           var regex = @"^[A-CEGHJ-PR-TW-Z][A-CEGHJ-NPR-TW-Z]\s[0-9]{2}\s[0-9]{2}\s[0-9]{2}\s[ABCD]";
           nino.Should().MatchRegex(regex);
        }

        [Fact]
        public void can_create_codice_fiscale()
        {
            var f = new Faker("it");
            var codiceFiscale = f.Person.CodiceFiscale();
            codiceFiscale.Should().MatchRegex("^[A-Z]{2}[A-ZX][A-Z]{2}[A-ZX][0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$");
        }

        [Fact]
        public void codice_fiscale_is_16_chars_long()
        {
            var f = new Faker("it");
            var person = f.Person;

            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().HaveLength(16);
        }

        [Fact]
        public void codice_fiscale_generated_twice_are_equal()
        {
            var f = new Faker("it");
            var person = f.Person;

            var codiceFiscale1 = person.CodiceFiscale();
            var codiceFiscale2 = person.CodiceFiscale();

            codiceFiscale1.Should().Be(codiceFiscale2);
        }

        [Fact]
        public void codice_fiscale_day_part_is_birthday_plus_40_for_females()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.Gender = Name.Gender.Female; //force female gender
            person.DateOfBirth = new DateTime(2010, 5, 13);
            var codiceFiscale = person.CodiceFiscale();
            var dayPart = codiceFiscale.Substring(9, 2);
            var dayPartAsInt = Convert.ToInt16(dayPart);

            dayPartAsInt.Should().Be(53);
        }

        [Fact]
        public void codice_fiscale_day_part_is_birthday_for_males()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.Gender = Name.Gender.Male; //force male gender
            person.DateOfBirth = new DateTime(2010, 5, 13);
            var codiceFiscale = person.CodiceFiscale();
            var dayPart = codiceFiscale.Substring(9, 2);
            var dayPartAsInt = Convert.ToInt16(dayPart);

            dayPartAsInt.Should().Be(13);
        }

        [Fact]
        public void codice_fiscale_is_case_insensitive()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = person.FirstName.ToUpper();
            person.LastName = person.LastName.ToUpper();
            var codiceFiscaleUpper = person.CodiceFiscale();
            person.FirstName = person.FirstName.ToLower();
            person.LastName = person.LastName.ToLower();
            var codiceFiscaleLower = person.CodiceFiscale();

            codiceFiscaleUpper.Should().Be(codiceFiscaleLower);
        }

        [Fact]
        public void double_names_are_correctly_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Bice Mia"; //force first name
            person.LastName = "Alex Bea"; //force last name
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("LXBBCM");
        }

        [Fact]
        public void more_than_three_consonants_in_first_name_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Annamaria";
            person.LastName = "Masi";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().MatchRegex("^.{3}NMR");
        }

        [Fact]
        public void more_than_three_consonants_in_last_name_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Michela";
            person.LastName = "Astratto";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("STR");
        }

        [Fact]
        public void names_starting_with_vowel_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "EULA";
            person.LastName = "AIRONE";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("RNALEU");
        }

        [Fact]
        public void names_with_apostrophe_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Morena";
            person.LastName = "D'Urzo";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("DRZMRN");
        }

        [Fact]
        public void names_with_symbols_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Pasquale";
            person.LastName = "D'Ama-Deidda";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("DMDPQL");
        }

        [Fact]
        public void names_with_one_consonant_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Maia";
            person.LastName = "Aria";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("RAIMAI");
        }

        [Fact]
        public void names_with_three_consonants_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Michi";
            person.LastName = "Macchi";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("MCCMCH");
        }

        [Fact]
        public void names_with_two_consonants_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Nico";
            person.LastName = "Masi";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("MSANCI");
        }

        [Fact]
        public void names_with_two_letters_are_squeezed()
        {
            var f = new Faker("it");
            var person = f.Person;

            person.FirstName = "Ka";
            person.LastName = "Ro";
            var codiceFiscale = person.CodiceFiscale();

            codiceFiscale.Should().StartWith("ROXKAX");
        }

        [Fact]
        public void codice_fiscale_can_be_computed_from_finance_class()
        {
            var f = new Faker("it");

            var codiceFiscale = f.Finance.CodiceFiscale("Rossi", "Mario", new DateTime(1990, 4, 23), true);

            codiceFiscale.Should().StartWith("RSSMRA90D23");
        }
       [Fact]
       public void nif_generator_for_person()
       {
          //Arrange
          var regex = @"^[0-9]{9}$";
          var f = new Faker("pt_PT");
          var person = f.Person;

          //Act
          var nifNumber = person.Nif();

          //Assert
          nifNumber.Should().MatchRegex(regex).And.HaveLength(9).And.Match(p => p.Substring(0, 1) == "1" || p.Substring(0, 1) == "2");
       }

       [Fact]
       public void nif_generator_for_company()
       {
          //Arrange
          var regex = @"^[0-9]{9}$";
          var f = new Faker("pt_PT");
          var company = f.Company;

          //Act
          var nipcNumber = company.Nipc();

          //Assert
          nipcNumber.Should().MatchRegex(regex).And.HaveLength(9).And.Match(p => p.Substring(0, 1) == "5" || p.Substring(0, 1) == "6" || p.Substring(0, 1) == "8" || p.Substring(0, 1) == "9");
       }
   }
}

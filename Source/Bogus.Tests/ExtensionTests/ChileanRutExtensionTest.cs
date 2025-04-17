using Bogus.Extensions.Chile;
using FluentAssertions;
using System;
using System.Text.RegularExpressions;
using Xunit;

namespace Bogus.Tests.ExtensionTests
{
   public class ChileanRutExtensionTest
   {

      [Fact]
      public void ChileanRutExtension_Should_BeValid_When_UsingDotFormat()
      {
         // Arrange
         var faker = new Faker<Person>()
            .RuleFor(p => p.Rut, f => f.Person.Rut(true));

         // Act
         var person = faker.Generate();

         // Assert
         InternalValidationRutHelper.IsRutValid(person.Rut).Should().Be(true);
      }

      [Fact]
      public void ChileanRutExtension_Should_BeValid_When_NotUsingDotFormat()
      {
         // Arrange
         var faker = new Faker<Person>()
            .RuleFor(p => p.Rut, f => f.Person.Rut(false));

         // Act
         var person = faker.Generate();

         // Assert
         InternalValidationRutHelper.IsRutValid(person.Rut).Should().Be(true);
      }

      private class Person
      {
         internal string Rut { get; set; } = "";
      }
   }

   internal static class InternalValidationRutHelper
   {
      internal static bool IsRutValid(string rut)
      {
         if (string.IsNullOrEmpty(rut))
            return false;

         rut = rut.Replace(".", "");

         var expresion = new Regex("^([0-9]+-[0-9K])$");
         string dv = rut.Substring(rut.Length - 1, 1);
         if (!expresion.IsMatch(rut))
         {
            return false;
         }
         char[] charCorte = { '-' };
         string[] rutTemp = rut.Split(charCorte);
         if (dv != Digito(int.Parse(rutTemp[0])))
         {
            return false;
         }
         return true;
      }

      private static string Digito(int rut)
      {
         if (rut < 1000000 || rut > 99999999)
            throw new ArgumentOutOfRangeException($"The provided integer {nameof(rut)} is outside of the range between 1000000 and 99999999");

         int suma = 0;
         int multiplicador = 1;

         while (rut != 0)
         {
            multiplicador++;
            if (multiplicador == 8)
               multiplicador = 2;
            suma += (rut % 10) * multiplicador;
            rut /= 10;
         }

         suma = 11 - (suma % 11);

         if (suma == 11)
         {
            return "0";
         }
         else if (suma == 10)
         {
            return "K";
         }
         else
         {
            return suma.ToString();
         }
      }
   }
}
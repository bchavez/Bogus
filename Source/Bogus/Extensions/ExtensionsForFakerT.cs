using System.Collections.Generic;

namespace Bogus.Extensions
{
   /// <summary>
   /// Extensions for <see cref="Faker{T}"/>.
   /// </summary>
   public static class ExtensionsForFakerT
   {
      /// <summary>
      /// Generate multiple fake objects of T. The number of generated items is randomly chosen between <see cref="min"/> and <see cref="max"/> values.
      /// The random number between <see cref="min"/> and <see cref="max"/> should be considered non-deterministic but technically depends on the parameters each time this method was called.
      /// </summary>
      /// <param name="faker">The <see cref="Faker{T}"/> to extend with this extension method.</param>
      /// <param name="min">Minimum number of T objects to create. Inclusive.</param>
      /// <param name="max">Maximum number of T objects to create. Inclusive.</param>
      public static List<T> GenerateBetween<T>(this Faker<T> faker, int min, int max, string ruleSets = null) where T : class
      {
         var itnernals = faker as IFakerTInternal;
         var r = itnernals.FakerHub.Random;
         var n = r.Number(min, max);
         return faker.Generate(n, ruleSets);
      }

      /// <summary>
      /// Helpful extension for creating randomly null values for <seealso cref="Faker{T}"/>.RuleFor() rules.
      /// Example: .RuleFor(x=>x.Prop, f=>f.Random.Word().OrNull(f))
      /// </summary>
      public static object OrNull(this object value, Faker f)
      {
         return f.Random.Bool() ? value : null;
      }
   }
}
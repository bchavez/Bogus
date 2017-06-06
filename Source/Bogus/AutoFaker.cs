using System.Collections.Generic;

namespace Bogus
{
  /// <summary>
  /// A class used to conveniently invoke generate requests for a given type.
  /// </summary>
  public static class AutoFaker
  {
    /// <summary>
    /// Generates a populated instance of type <typeparamref name="TType"/>.
    /// </summary>
    /// <typeparam name="TType">The type of instance to generate.</typeparam>
    /// <param name="binder">The <see cref="IAutoBinder"/> instance to use for the generation request.</param>
    /// <returns>The generated instance of type <typeparamref name="TType"/>.</returns>
    public static TType Generate<TType>(IAutoBinder binder = null)
      where TType : class
    {
      var auto = new AutoFaker<TType>(binder);
      return auto.Generate();
    }

    /// <summary>
    /// Generates a collection of populated instances of type <typeparamref name="TType"/>.
    /// </summary>
    /// <typeparam name="TType">The type of instance to generate.</typeparam>
    /// <param name="count">The number of instances to generate.</param>
    /// <param name="binder">The <see cref="IAutoBinder"/> instance to use for the generation request.</param>
    /// <returns>The collection of generated instances of type <typeparamref name="TType"/>.</returns>
    public static IEnumerable<TType> Generate<TType>(int count, IAutoBinder binder = null)
      where TType : class
    {
      var auto = new AutoFaker<TType>(binder);
      return auto.Generate(count);
    }
  }
}

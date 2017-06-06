using System.Collections.Generic;

namespace Bogus
{
  /// <summary>
  /// A class that provides context for a generate request.
  /// </summary>
  public sealed class AutoGenerateContext
  {
    private const int DefaultCount = 3;

    internal AutoGenerateContext(Faker faker, IEnumerable<string> ruleSets, IAutoBinder binder)
    {
      this.Faker = faker;
      this.RuleSets = ruleSets;
      this.Binder = binder;
    }
    
    /// <summary>
    /// The underlying <see cref="Bogus.Faker"/> instance used to generate random values.
    /// </summary>
    public Faker Faker { get; }

    /// <summary>
    /// The requested rule sets provided for the generate request.
    /// </summary>
    public IEnumerable<string> RuleSets { get; }

    internal IAutoBinder Binder { get; }

    /// <summary>
    /// Creates a populated instance of type <typeparamref name="TType"/>.
    /// </summary>
    /// <typeparam name="TType">The instance type to generate.</typeparam>
    /// <param name="context">The <see cref="AutoGenerateContext"/> instance for the generate request.</param>
    /// <returns>A populated instance of <typeparamref name="TType"/>.</returns>
    public TType Generate<TType>(AutoGenerateContext context)
    {
      var generator = AutoGeneratorFactory.GetGenerator<TType>(context);
      return (TType)generator.Generate(context);
    }

    /// <summary>
    /// Creates a collection of populated instances of type <typeparamref name="TType"/>.
    /// </summary>
    /// <typeparam name="TType">The instance type to generate.</typeparam>
    /// <param name="context">The <see cref="AutoGenerateContext"/> instance for the generate request.</param>
    /// <returns>A collection of populated instances of <typeparamref name="TType"/>.</returns>
    public IEnumerable<TType> GenerateMany<TType>(AutoGenerateContext context)
    {
      var items = new List<TType>();

      for (var index = 0; index < DefaultCount; index++)
      {
        var item = Generate<TType>(context);

        // Ensure the generated value is not null (which means the type couldn't be generated)
        if (item != null)
        {
          items.Add(item);
        }
      }

      return items;
    }
  }
}

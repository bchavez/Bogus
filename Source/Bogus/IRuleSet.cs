using System;
using System.Linq.Expressions;

namespace Bogus
{
   /// <summary>
   /// An interface for defining a set of rules.
   /// </summary>
   public interface IRuleSet<T> where T : class
   {
      /// <summary>
      /// Uses the factory method to generate new instances.
      /// </summary>
      Faker<T> CustomInstantiator(Func<Faker, T> factoryMethod);

      /// <summary>
      /// Creates a rule for a compound property and providing access to the instance being generated.
      /// </summary>
      Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, TProperty> setter);

      /// <summary>
      /// Creates a rule for a property.
      /// </summary>
      Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, TProperty> setter);

      /// <summary>
      /// Creates a rule for a property.
      /// </summary>
      Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<TProperty> valueFunction);

      /// <summary>
      /// Ignore a property or field when using StrictMode.
      /// </summary>
      Faker<T> Ignore<TPropertyOrField>(Expression<Func<T, TPropertyOrField>> propertyOrField);

      /// <summary>
      /// Ensures all properties of T have rules.
      /// </summary>
      /// <param name="ensureRulesForAllProperties">Overrides any global setting in Faker.DefaultStrictMode</param>
      Faker<T> StrictMode(bool ensureRulesForAllProperties);

      /// <summary>
      /// Action is invoked after all the rules are applied.
      /// </summary>
      Faker<T> FinishWith(Action<Faker, T> action);

      /// <summary>
      /// Creates a rule for a property.
      /// </summary>
      Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, TProperty value);

      /// <summary>
      /// Gives you a way to specify multiple rules inside an action
      /// without having to call RuleFor multiple times. Note: StrictMode
      /// must be false since property rules cannot be individually checked.
      /// </summary>
      Faker<T> Rules(Action<Faker, T> setActions);
   }
}
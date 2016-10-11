using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bogus
{
  /// <summary>
  /// Generates fake objects of T.
  /// </summary>
  public class Faker<T> : ILocaleAware, IRuleSet<T> where T : class
  {
    private const string Default = "default";
    private static readonly string[] DefaultRuleSet = { Default };
#pragma warning disable 1591
    protected internal Faker FakerHub;
    protected internal IBinder binder;
    protected internal readonly MultiDictionary<string, string, PopulateAction<T>> Actions = new MultiDictionary<string, string, PopulateAction<T>>(StringComparer.OrdinalIgnoreCase);
    protected internal readonly Dictionary<string, FinalizeAction<T>> FinalizeActions = new Dictionary<string, FinalizeAction<T>>(StringComparer.OrdinalIgnoreCase);
    protected internal Dictionary<string, Func<Faker, T>> CreateActions = new Dictionary<string, Func<Faker, T>>(StringComparer.OrdinalIgnoreCase);
    protected internal readonly MultiSetDictionary<string, string> Ignores = new MultiSetDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    protected internal readonly Dictionary<string, MemberInfo> TypeProperties;
    protected internal Dictionary<string, bool> StrictModes = new Dictionary<string, bool>();
    protected internal bool? IsValid;
    protected internal string currentRuleSet = Default;
#pragma warning restore 1591

    /// <summary>
    /// Creates a Faker with a locale.
    /// </summary>
    /// <param name="locale">language</param>
    /// <param name="binder">A binder that discovers properties or fields on T that are candidates for faking. Null uses the default Binder.</param>
    public Faker(string locale = "en", IBinder binder = null)
    {
      this.binder = binder ?? new Binder();
      this.Locale = locale;
      FakerHub = new Faker(locale);
      TypeProperties = this.binder.GetMembers(typeof(T));
      this.CreateActions[Default] = faker => Activator.CreateInstance<T>();
    }

    /// <summary>
    /// Uses the factory method to generate new instances.
    /// </summary>
    public Faker<T> CustomInstantiator(Func<Faker, T> factoryMethod)
    {
      this.CreateActions[currentRuleSet] = factoryMethod;
      return this;
    }

    /// <summary>
    /// Creates a rule for a compound property and providing access to the instance being generated.
    /// </summary>
    public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, TProperty> setter)
    {
      var propName = PropertyName.For(property);

      Func<Faker, T, object> invoker = (f, t) => setter(f, t);

      var rule = new PopulateAction<T>
      {
        Action = invoker,
        RuleSet = currentRuleSet,
        PropertyName = propName
      };

      this.Actions.Add(currentRuleSet, propName, rule);

      return this;
    }

    /// <summary>
    /// Creates a rule for a property.
    /// </summary>
    public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, TProperty> setter)
    {
      var propName = PropertyName.For(property);

      Func<Faker, T, object> invoker = (f, t) => setter(f);

      var rule = new PopulateAction<T>
      {
        Action = invoker,
        RuleSet = currentRuleSet,
        PropertyName = propName,
      };

      this.Actions.Add(currentRuleSet, propName, rule);

      return this;
    }

    /// <summary>
    /// Creates a rule for a property.
    /// </summary>
    public Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<TProperty> valueFunction)
    {
      return RuleFor(property, (f) => valueFunction());
    }

    /// <summary>
    /// Create a rule set that can be executed in specialized cases.
    /// </summary>
    /// <param name="ruleSetName">The rule set name</param>
    /// <param name="action">The set of rules to apply when this rules set is specified.</param>
    public Faker<T> RuleSet(string ruleSetName, Action<IRuleSet<T>> action)
    {
      if (currentRuleSet != Default) throw new ArgumentException("Cannot create a rule set within a rule set.");
      currentRuleSet = ruleSetName;
      action(this);
      currentRuleSet = Default;
      return this;
    }

    /// <summary>
    /// Ignore a property or field when using StrictMode.
    /// </summary>
    /// <typeparam name="TPropertyOrField"></typeparam>
    /// <param name="propertyOrField"></param>
    /// <returns></returns>
    public Faker<T> Ignore<TPropertyOrField>(Expression<Func<T, TPropertyOrField>> propertyOrField)
    {
      var propNameOrField = PropertyName.For(propertyOrField);

      MemberInfo mi;
      if (!this.TypeProperties.TryGetValue(propNameOrField, out mi))
      {
        throw new ArgumentException($"The property or field {propNameOrField} was not found on {typeof(T)} during the binding discovery of T. Can't ignore something that doesn't exist.");
      }
      this.Ignores.Add(currentRuleSet, propNameOrField);

      return this;
    }

    /// <summary>
    /// Ensures all properties of T have rules.
    /// </summary>
    /// <param name="ensureRulesForAllProperties">Overrides any global setting in Faker.DefaultStrictMode</param>
    /// <returns></returns>
    public Faker<T> StrictMode(bool ensureRulesForAllProperties)
    {
      this.StrictModes[currentRuleSet] = ensureRulesForAllProperties;
      return this;
    }

    /// <summary>
    /// Action is invoked after all the rules are applied.
    /// </summary>
    public Faker<T> FinishWith(Action<Faker, T> action)
    {
      var rule = new FinalizeAction<T>
      {
        Action = action,
        RuleSet = currentRuleSet
      };
      this.FinalizeActions.Add(currentRuleSet, rule);
      return this;
    }

    private string[] ParseDirtyRulesSets(string dirtyRules)
    {
      dirtyRules = dirtyRules?.Trim(',').Trim();
      if (string.IsNullOrWhiteSpace(dirtyRules)) return DefaultRuleSet;
      return dirtyRules.Split(',')
          .Where(s => !string.IsNullOrWhiteSpace(s))
          .Select(s => s.Trim()).ToArray();
    }

    /// <summary>
    /// Generates a fake object of T.
    /// </summary>
    /// <returns></returns>
    public virtual T Generate(string ruleSets = null)
    {
      Func<Faker, T> createRule = null;
      var cleanRules = ParseDirtyRulesSets(ruleSets);

      if (string.IsNullOrWhiteSpace(ruleSets))
      {
        createRule = CreateActions[Default];
      }
      else
      {
        var firstRule = cleanRules[0];
        createRule = CreateActions.TryGetValue(firstRule, out createRule) ? createRule : CreateActions[Default];
      }
      var instance = createRule(this.FakerHub);

      PopulateInternal(instance, cleanRules);

      return instance;
    }

    /// <summary>
    /// Generates multiple fake objects of T.
    /// </summary>
    public virtual IEnumerable<T> Generate(int count, string ruleSets = null)
    {
      return Enumerable.Range(1, count)
          .Select(i => Generate(ruleSets));
    }

    /// <summary>
    /// Only populates an instance of T.
    /// </summary>
    public virtual void Populate(T instance, string ruleSets = null)
    {
      var cleanRules = ParseDirtyRulesSets(ruleSets);
      PopulateInternal(instance, cleanRules);
    }

    private void PopulateInternal(T instance, string[] ruleSets)
    {
      if (!IsValid.HasValue)
      {
        //run validation
        this.IsValid = ValidateInternal(ruleSets).IsValid;
      }

      if (!IsValid.GetValueOrDefault())
      {
        throw new InvalidOperationException($"StrictMode validation failure on {typeof(T)}. The Binder found {TypeProperties.Count} properties/fields but have different number of actions rules. Also, check RuleSets.");
      }

      var typeProps = TypeProperties;

      lock (Randomizer.Locker.Value)
      {
        foreach (var ruleSet in ruleSets)
        {
          Dictionary<string, PopulateAction<T>> populateActions;
          if (this.Actions.TryGetValue(ruleSet, out populateActions))
          {
            foreach (var action in populateActions.Values)
            {
              MemberInfo member;
              typeProps.TryGetValue(action.PropertyName, out member);
              var valueFactory = action.Action;

              var prop = member as PropertyInfo;
              prop?.SetValue(instance, valueFactory(FakerHub, instance), null);

              var field = member as FieldInfo;
              field?.SetValue(instance, valueFactory(FakerHub, instance));
            }
          }
        }

        foreach (var ruleSet in ruleSets)
        {
          FinalizeAction<T> finalizer;
          if (this.FinalizeActions.TryGetValue(ruleSet, out finalizer))
          {
            finalizer.Action(this.FakerHub, instance);
          }
        }

        FakerHub.NewContext();
      }
    }

    /// <summary>
    /// Checks if all properties have rules.
    /// </summary>
    /// <param name="ruleSets"></param>
    /// <returns>True if validation passes, false otherwise.</returns>
    public virtual bool Validate(string ruleSets = null)
    {
      var rules = ruleSets == null
          ? this.Actions.Keys.ToArray()
          : ParseDirtyRulesSets(ruleSets);
      var result = ValidateInternal(rules);
      return result.IsValid;
    }

    /// <summary>
    /// Checks if all properties have rules. In case of lack of rules an exception will be raised
    /// with complete list of missing rules. Very useful in tests for fast forward fixing of 
    /// missing rules
    /// </summary>
    /// <exception cref="MissingFieldException"/>
    /// <param name="ruleSets"></param>
    public virtual void AssertConfigurationIsValid(string ruleSets = null)
    {
      var rules = ruleSets == null
          ? this.Actions.Keys.ToArray()
          : ParseDirtyRulesSets(ruleSets);
      var result = ValidateInternal(rules);
      if (!result.IsValid)
      {
        throw new MissingFieldException($"missing rules for: \n==================\n\n{string.Join("\n", result.MissingRules)}\n");
      }
    }

    private ValidationResult ValidateInternal(string[] ruleSets)
    {
      var result = new ValidationResult { IsValid = true };

      var propsOrFieldsOfT = this.TypeProperties.Keys;
      foreach (var rule in ruleSets)
      {
        var strictMode = Faker.DefaultStrictMode;
        this.StrictModes.TryGetValue(rule, out strictMode);

        HashSet<string> ignores;
        this.Ignores.TryGetValue(rule, out ignores);

        Dictionary<string, PopulateAction<T>> populateActions;
        this.Actions.TryGetValue(rule, out populateActions);

        var finalSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (ignores != null)
          finalSet.UnionWith(ignores);
        if (populateActions != null)
          finalSet.UnionWith(populateActions.Keys);

        if (!finalSet.SetEquals(propsOrFieldsOfT))
        {
          var delta = new List<string>();
          foreach (var propOrField in propsOrFieldsOfT)
            if (!finalSet.Contains(propOrField))
              delta.Add(propOrField);
          result.MissingRules.AddRange(delta);
          result.IsValid = !strictMode;
        }
      }
      return result;
    }

    /// <summary>
    /// The current locale.
    /// </summary>
    public string Locale { get; set; }
  }
}
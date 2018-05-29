using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Bogus
{
   /// <summary>
   /// Hidden API implemented explicitly on <see cref="Faker{T}"/>. When <see cref="Faker{T}"/> is casted explicitly to <see cref="IFakerTInternal"/>, 
   /// the cast reveals some protected internal objects of <see cref="Faker{T}"/> without needing to derive
   /// from <see cref="Faker{T}"/>. This is useful for extensions methods that need access internal variables of <see cref="Faker{T}"/> like <see cref="Faker"/>, <see cref="IBinder"/>, <see cref="LocalSeed"/>, and type of T.
   /// </summary>
   public interface IFakerTInternal
   {
      /// <summary>
      /// The internal FakerHub object that is used in f => f rules. Usually used to gain access to a source of randomness by extension methods.
      /// </summary>
      Faker FakerHub { get; }

      /// <summary>
      /// The field/property binder used by <see cref="Faker{T}"/>.
      /// </summary>
      IBinder Binder { get; }

      /// <summary>
      /// The local seed of <see cref="Faker{T}"/> if available. Null local seed means the Global <see cref="Randomizer.Seed"/> property is being used.
      /// </summary>
      int? LocalSeed { get; }

      /// <summary>
      /// The type of T in <see cref="Faker{T}"/>.
      /// </summary>
      Type TypeOfT { get; }
   }

   /// <summary>
   /// Generates fake objects of <typeparamref name="T"/>.
   /// </summary>
   /// <typeparam name="T">The object to fake.</typeparam>
   public class Faker<T> : IFakerTInternal, ILocaleAware, IRuleSet<T> where T : class
   {
#pragma warning disable 1591
      protected const string Default = "default";
      private static readonly string[] DefaultRuleSet = {Default};
      protected internal Faker FakerHub;
      protected internal IBinder binder;

      protected internal readonly MultiDictionary<string, string, PopulateAction<T>> Actions =
         new MultiDictionary<string, string, PopulateAction<T>>(StringComparer.OrdinalIgnoreCase);

      protected internal readonly Dictionary<string, FinalizeAction<T>> FinalizeActions = new Dictionary<string, FinalizeAction<T>>(StringComparer.OrdinalIgnoreCase);
      protected internal Dictionary<string, Func<Faker, T>> CreateActions = new Dictionary<string, Func<Faker, T>>(StringComparer.OrdinalIgnoreCase);
      protected internal readonly Dictionary<string, MemberInfo> TypeProperties;
      protected internal Dictionary<string, bool> StrictModes = new Dictionary<string, bool>();
      protected internal bool? IsValid;
      protected internal string currentRuleSet = Default;
      protected internal int? localSeed; // if null, the global Randomizer.Seed is used.
#pragma warning restore 1591

      Faker IFakerTInternal.FakerHub => this.FakerHub;

      IBinder IFakerTInternal.Binder => this.binder;

      int? IFakerTInternal.LocalSeed => this.localSeed;

      Type IFakerTInternal.TypeOfT => typeof(T);

      /// <summary>
      /// Clones the internal state of a <seealso cref="Faker{T}"/> into a new <seealso cref="Faker{T}"/> so that
      /// both are isolated from each other. The clone will have internal state
      /// reset as if <seealso cref="Generate(string)"/> was never called.
      /// </summary>
      public Faker<T> Clone()
      {
         var clone = new Faker<T>(this.Locale, this.binder);

         //copy internal state.
         //strict modes.
         foreach( var root in this.StrictModes )
         {
            clone.StrictModes.Add(root.Key, root.Value);
         }

         //create actions
         foreach( var root in this.CreateActions )
         {
            clone.CreateActions[root.Key] = root.Value;
         }
         //finalize actions
         foreach( var root in this.FinalizeActions )
         {
            clone.FinalizeActions.Add(root.Key, root.Value);
         }

         //actions
         foreach( var root in this.Actions )
         {
            foreach( var kv in root.Value )
            {
               clone.Actions.Add(root.Key, kv.Key, kv.Value);
            }
         }

         if( localSeed.HasValue )
         {
            clone.UseSeed(localSeed.Value);
         }

         return clone;
      }

      /// <summary>
      /// The current locale.
      /// </summary>
      public string Locale { get; set; }

      /// <summary>
      /// Creates a Faker with default 'en' locale.
      /// </summary>
      public Faker() : this("en", null)
      {
      }

      /// <summary>
      /// Creates a Faker with a locale
      /// </summary>
      public Faker(string locale) : this(locale, null)
      {
      }

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
      /// Creates a seed locally scoped within this <seealso cref="Faker{T}"/> ignoring the globally scoped <seealso cref="Randomizer.Seed"/>.
      /// If this method is never called the global <seealso cref="Randomizer.Seed"/> is used.
      /// </summary>
      /// <param name="seed">The seed value to use within this <seealso cref="Faker{T}"/> instance.</param>
      public virtual Faker<T> UseSeed(int seed)
      {
         this.localSeed = seed;
         this.FakerHub.Random = new Randomizer(seed);
         return this;
      }

      /// <summary>
      /// Instructs <seealso cref="Faker{T}"/> to use the factory method as a source
      /// for new instances of <typeparamref name="T"/>.
      /// </summary>
      public virtual Faker<T> CustomInstantiator(Func<Faker, T> factoryMethod)
      {
         this.CreateActions[currentRuleSet] = factoryMethod;
         return this;
      }

      /// <summary>
      /// Creates a rule for a compound property and providing access to the instance being generated.
      /// </summary>
      public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, T, TProperty> setter)
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
      public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, TProperty value)
      {
         return RuleFor(property, f => value);
      }

      /// <summary>
      /// Creates a rule for a property.
      /// </summary>
      public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<Faker, TProperty> setter)
      {
         var propName = PropertyName.For(property);

         return RuleFor(propName, setter);
      }

      protected virtual Faker<T> RuleFor<TProperty>(string propertyOrField, Func<Faker, TProperty> setter)
      {
         Func<Faker, T, object> invoker = (f, t) => setter(f);

         var rule = new PopulateAction<T>
            {
               Action = invoker,
               RuleSet = currentRuleSet,
               PropertyName = propertyOrField,
            };

         this.Actions.Add(currentRuleSet, propertyOrField, rule);

         return this;
      }

      /// <summary>
      /// Specify multiple rules inside an action without having to call
      /// RuleFor multiple times. Note: <seealso cref="StrictMode"/> must be false
      /// since rules for properties and fields cannot be individually checked when
      /// using this method.
      /// </summary>
      public virtual Faker<T> Rules(Action<Faker, T> setActions)
      {
         Func<Faker, T, object> invoker = (f, t) =>
            {
               setActions(f, t);
               return null;
            };
         var guid = Guid.NewGuid().ToString();
         var rule = new PopulateAction<T>()
            {
               Action = invoker,
               RuleSet = currentRuleSet,
               PropertyName = guid,
               ProhibitInStrictMode = true
            };
         this.Actions.Add(currentRuleSet, guid, rule);
         return this;
      }

      /// <summary>
      /// Creates a rule for a property.
      /// </summary>
      public virtual Faker<T> RuleFor<TProperty>(Expression<Func<T, TProperty>> property, Func<TProperty> valueFunction)
      {
         return RuleFor(property, (f) => valueFunction());
      }

      /// <summary>
      /// Not Implemented: This method only exists as a work around for Visual Studio IntelliSense. See: https://github.com/bchavez/Bogus/issues/54
      /// </summary>
      [Obsolete("This exists here only as a Visual Studio IntelliSense work around. See: https://github.com/bchavez/Bogus/issues/54", true)]
      public void RuleFor<TProperty>(Expression<Func<T, TProperty>> property)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Creates one rule for all types of <typeparamref name="TType"/> on type <typeparamref name="T"/>.
      /// In other words, if you have <typeparamref name="T"/> with many fields or properties of
      /// type <seealso cref="Int32"/> this method allows you to specify a rule for all fields or
      /// properties of type <seealso cref="Int32"/>.
      /// </summary>
      public virtual Faker<T> RuleForType<TType>(Type type, Func<Faker, TType> setterForType)
      {
         if( typeof(TType) != type )
         {
            throw new ArgumentException($"{nameof(TType)} must be the same type as parameter named '{nameof(type)}'");
         }

         foreach( var kvp in this.TypeProperties )
         {
            var propOrFieldType = GetFieldOrPropertyType(kvp.Value);
            var propOrFieldName = kvp.Key;

            if( propOrFieldType == type )
            {
               RuleFor(propOrFieldName, setterForType);
            }
         }

         return this;
      }

      /// <summary>
      /// Utility method to get the Type of a Property or Field
      /// </summary>
      protected virtual Type GetFieldOrPropertyType(MemberInfo mi)
      {
         if( mi is PropertyInfo pi )
         {
            return pi.PropertyType;
         }
         if( mi is FieldInfo fi )
         {
            return fi.FieldType;
         }
         return null;
      }

      /// <summary>
      /// Defines a set of rules under a specific name. Useful for defining
      /// rules for special cases. Note: The name `default` is the name of all rules that are
      /// defined without an explicit rule set.
      /// </summary>
      /// <param name="ruleSetName">The rule set name.</param>
      /// <param name="action">The set of rules to apply when this rules set is specified.</param>
      public virtual Faker<T> RuleSet(string ruleSetName, Action<IRuleSet<T>> action)
      {
         if( currentRuleSet != Default ) throw new ArgumentException("Cannot create a rule set within a rule set.");
         currentRuleSet = ruleSetName;
         action(this);
         currentRuleSet = Default;
         return this;
      }

      /// <summary>
      /// Ignores a property or field when <seealso cref="StrictMode"/> is enabled.
      /// </summary>
      /// <typeparam name="TPropertyOrField"></typeparam>
      /// <param name="propertyOrField"></param>
      public virtual Faker<T> Ignore<TPropertyOrField>(Expression<Func<T, TPropertyOrField>> propertyOrField)
      {
         var propNameOrField = PropertyName.For(propertyOrField);

         if( !this.TypeProperties.TryGetValue(propNameOrField, out MemberInfo mi) )
         {
            throw new ArgumentException(
               $"The property or field {propNameOrField} was not found on {typeof(T)} during the binding discovery of T. Can't ignore something that doesn't exist.");
         }

         var rule = new PopulateAction<T>
            {
               Action = null,
               RuleSet = currentRuleSet,
               PropertyName = propNameOrField
            };

         this.Actions.Add(currentRuleSet, propNameOrField, rule);

         return this;
      }
      
      /// <summary>
      /// When set to true, ensures all properties and public fields of <typeparamref name="T"/> have rules
      /// before an object of <typeparamref name="T"/> is populated or generated. Manual assertion
      /// can be invoked using <seealso cref="Validate"/> and <seealso cref="AssertConfigurationIsValid"/>.
      /// </summary>
      /// <param name="ensureRulesForAllProperties">Overrides any global setting in <seealso cref="Faker.DefaultStrictMode"/>.</param>
      public virtual Faker<T> StrictMode(bool ensureRulesForAllProperties)
      {
         this.StrictModes[currentRuleSet] = ensureRulesForAllProperties;
         return this;
      }

      /// <summary>
      /// A finalizing action rule applied to <typeparamref name="T"/> after all the rules
      /// are executed.
      /// </summary>
      public virtual Faker<T> FinishWith(Action<Faker, T> action)
      {
         var rule = new FinalizeAction<T>
            {
               Action = action,
               RuleSet = currentRuleSet
            };
         this.FinalizeActions[currentRuleSet] = rule;
         return this;
      }

      /// <summary>
      /// Utility method to parse out rule sets form user input.
      /// </summary>
      protected virtual string[] ParseDirtyRulesSets(string dirtyRules)
      {
         dirtyRules = dirtyRules?.Trim(',').Trim();
         if( string.IsNullOrWhiteSpace(dirtyRules) ) return DefaultRuleSet;
         return dirtyRules.Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim()).ToArray();
      }

      /// <summary>
      /// Generates a fake object of <typeparamref name="T"/> using the specified rules in this
      /// <seealso cref="Faker{T}"/>.
      /// </summary>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "rulesetA, rulesetB, default")
      /// </param>
      public virtual T Generate(string ruleSets = null)
      {
         Func<Faker, T> createRule = null;
         var cleanRules = ParseDirtyRulesSets(ruleSets);

         if( string.IsNullOrWhiteSpace(ruleSets) )
         {
            createRule = CreateActions[Default];
         }
         else
         {
            var firstRule = cleanRules[0];
            createRule = CreateActions.TryGetValue(firstRule, out createRule) ? createRule : CreateActions[Default];
         }

         //Issue 143 - We need a new FakerHub context before calling the
         //            constructor. Associated Issue 57: Again, before any
         //            rules execute, we need a context to capture IndexGlobal
         //            and IndexFaker variables.
         FakerHub.NewContext();
         var instance = createRule(this.FakerHub);

         PopulateInternal(instance, cleanRules);

         return instance;
      }

      /// <summary>
      /// Generates a <seealso cref="List{T}"/> fake objects of type <typeparamref name="T"/> using the specified rules in
      /// this <seealso cref="Faker{T}"/>.
      /// </summary>
      /// <param name="count">The number of items to create in the <seealso cref="List{T}"/>.</param>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "rulesetA, rulesetB, default")
      /// </param>
      public virtual List<T> Generate(int count, string ruleSets = null)
      {
         return Enumerable.Range(1, count)
            .Select(i => Generate(ruleSets))
            .ToList();
      }

      /// <summary>
      /// Returns an <seealso cref="IEnumerable{T}"/> with LINQ deferred execution. Generated values
      /// are not guaranteed to be repeatable until <seealso cref="Enumerable.ToList{T}"/> is called.
      /// </summary>
      /// <param name="count">The number of items to create in the <seealso cref="IEnumerable{T}"/>.</param>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "rulesetA, rulesetB, default")
      /// </param>
      public virtual IEnumerable<T> GenerateLazy(int count, string ruleSets = null)
      {
         return Enumerable.Range(1, count)
            .Select(i => Generate(ruleSets));
      }

      /// <summary>
      /// Returns an <see cref="IEnumerable{T}"/> that can be used as an unlimited source 
      /// of <typeparamref name="T"/> when iterated over. Useful for generating unlimited
      /// amounts of data in a memory efficient way. Generated values *should* be repeatable
      /// for a given seed when starting with the first item in the sequence.
      /// </summary>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "rulesetA, rulesetB, default")
      /// </param>
      public virtual IEnumerable<T> GenerateForever(string ruleSets = null)
      {
         while( true )
         {
            yield return this.Generate();
         }
      }

      /// <summary>
      /// Populates an instance of <typeparamref name="T"/> according to the rules
      /// defined in this <seealso cref="Faker{T}"/>.
      /// </summary>
      /// <param name="instance">The instance of <typeparamref name="T"/> to populate.</param>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "rulesetA, rulesetB, default")
      /// </param>
      public virtual void Populate(T instance, string ruleSets = null)
      {
         var cleanRules = ParseDirtyRulesSets(ruleSets);
         PopulateInternal(instance, cleanRules);
      }

      /// <summary>
      /// Populates an instance of <typeparamref name="T"/> according to the rules
      /// defined in this <seealso cref="Faker{T}"/>.
      /// </summary>
      /// <param name="instance">The instance of <typeparamref name="T"/> to populate.</param>
      /// <param name="ruleSets">A comma separated list of rule sets to execute.
      /// Note: The name `default` is the name of all rules defined without an explicit rule set.
      /// When a custom rule set name is provided in <paramref name="ruleSets"/> as parameter,
      /// the `default` rules will not run. If you want rules without an explicit rule set to run
      /// you'll need to include the `default` rule set name in the comma separated
      /// list of rules to run. (ex: "rulesetA, rulesetB, default")
      /// </param>
      protected virtual void PopulateInternal(T instance, string[] ruleSets)
      {
         ValidationResult vr = null;
         if( !IsValid.HasValue )
         {
            //run validation
            vr = ValidateInternal(ruleSets);
            this.IsValid = vr.IsValid;
         }
         if( !IsValid.GetValueOrDefault() )
         {
            throw MakeValidationException(vr ?? ValidateInternal(ruleSets));
         }

         var typeProps = TypeProperties;

         lock( Randomizer.Locker.Value )
         {
            //Issue 57 - Make sure you generate a new context
            //           before executing any rules.
            //Issue 143 - If the FakerHub doesn't have any context
            //            (eg NewContext() has never been called), then call it
            //            so we can increment IndexGlobal and IndexFaker.
            if( !this.FakerHub.HasContext ) FakerHub.NewContext();

            foreach( var ruleSet in ruleSets )
            {
               if( this.Actions.TryGetValue(ruleSet, out var populateActions) )
               {
                  foreach( var action in populateActions.Values )
                  {
                     typeProps.TryGetValue(action.PropertyName, out MemberInfo member);
                     var valueFactory = action.Action;
                     if( valueFactory is null ) continue; // An .Ignore() rule.

                     if( member != null )
                     {
                        var prop = member as PropertyInfo;
                        prop?.SetValue(instance, valueFactory(FakerHub, instance), null);

                        var field = member as FieldInfo;
                        field?.SetValue(instance, valueFactory(FakerHub, instance));
                     }
                     else // member would be null if this was an RuleForObject.
                     {
                        //Invoke this if this is a basic rule which does not select a property or a field.
                        var outputValue = valueFactory(FakerHub, instance);
                     }
                  }
               }
            }

            foreach( var ruleSet in ruleSets )
            {
               if( this.FinalizeActions.TryGetValue(ruleSet, out FinalizeAction<T> finalizer) )
               {
                  finalizer.Action(this.FakerHub, instance);
               }
            }
         }
      }

      /// <summary>
      /// When <seealso cref="StrictMode"/> is enabled, checks if all properties or fields of <typeparamref name="T"/> have
      /// rules defined. Returns true if all rules are defined, false otherwise.
      /// The difference between <seealso cref="Validate"/> and <seealso cref="AssertConfigurationIsValid"/>
      /// is that <seealso cref="Validate"/> will *not* throw <seealso cref="ValidationException"/>
      /// if some rules are missing when <seealso cref="StrictMode"/> is enabled.
      /// </summary>
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
      /// Asserts that all properties have rules. When <seealso cref="StrictMode"/> is enabled, an exception will be raised
      /// with complete list of missing rules. Useful in unit tests to catch missing rules at development
      /// time. The difference between <seealso cref="Validate"/> and <seealso cref="AssertConfigurationIsValid"/>
      /// is that <seealso cref="AssertConfigurationIsValid"/> will throw <seealso cref="ValidationException"/>
      /// if some rules are missing when <seealso cref="StrictMode"/> is enabled. <seealso cref="Validate"/>
      /// will not throw an exception and will return <seealso cref="bool"/> true or false accordingly if
      /// rules are missing when <seealso cref="StrictMode"/> is enabled.
      /// </summary>
      /// <exception cref="ValidationException"/>
      public virtual void AssertConfigurationIsValid(string ruleSets = null)
      {
         string[] rules;
         if( ruleSets is null )
         {
            rules = this.Actions.Keys.ToArray();
         }
         else
         {
            rules = ParseDirtyRulesSets(ruleSets);
         }

         var result = ValidateInternal(rules);
         if( !result.IsValid )
         {
            throw MakeValidationException(result);
         }
      }

      /// <summary>
      /// Composes a <see cref="ValidationException"/> based on the failed validation
      /// results that can be readily used to raise the exception.
      /// </summary>
      protected virtual ValidationException MakeValidationException(ValidationResult result)
      {
         var builder = new StringBuilder();

         result.ExtraMessages.ForEach(m =>
            {
               builder.AppendLine(m);
               builder.AppendLine();
            });

         builder.AppendLine("Validation was called to ensure all properties / fields have rules.")
            .AppendLine($"There are missing rules for Faker<T> '{typeof(T).Name}'.")
            .AppendLine("=========== Missing Rules ===========");

         foreach( var fieldOrProp in result.MissingRules )
         {
            builder.AppendLine(fieldOrProp);
         }

         return new ValidationException(builder.ToString().Trim());
      }

      private ValidationResult ValidateInternal(string[] ruleSets)
      {
         var result = new ValidationResult {IsValid = true};

         var binderPropsOrFieldsOfT = this.TypeProperties.Keys;
         foreach( var rule in ruleSets )
         {
            var strictMode = Faker.DefaultStrictMode;
            this.StrictModes.TryGetValue(rule, out strictMode);

            //If strictMode is not enabled, skip and move on to the next ruleSet.
            if( !strictMode ) continue;

            this.Actions.TryGetValue(rule, out var populateActions);

            var userSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if( populateActions != null )
            {
               userSet.UnionWith(populateActions.Keys);
            }

            //Get the set properties or fields that are only
            //known to the binder, while removing
            //items in userSet that are known to both the user and binder.

            userSet.SymmetricExceptWith(binderPropsOrFieldsOfT);

            //What's left in userSet is the set of properties or fields
            //that the user does not know about + .Rule() methods.

            if( userSet.Count > 0 )
            {
               foreach( var propOrFieldOfT in userSet )
               {
                  if( populateActions.TryGetValue(propOrFieldOfT, out var populateAction) )
                  {
                     // Very much a .Rules() action
                     if( populateAction.ProhibitInStrictMode )
                     {
                        result.ExtraMessages.Add(
                           $"When StrictMode is set to True the Faker<{typeof(T).Name}>.Rules(...) method cannot verify that all properties have rules. You need to use Faker<{typeof(T).Name}>.RuleFor( x => x.Prop, ...) for each property to ensure each property has an associated rule when StrictMode is true; otherwise, set StrictMode to False in order to use Faker<{typeof(T).Name}>.Rules() method.");
                        result.IsValid = false;
                     }
                  }
                  else //The user doesn't know about this property or field. Log it as a validation error.
                  {
                     result.MissingRules.Add(propOrFieldOfT);
                     result.IsValid = false;
                  }
               }
            }
         }
         return result;
      }

      /// <summary>
      /// Provides implicit type conversion from <seealso cref="Faker{T}"/> to <typeparamref name="T"/>. IE: Order testOrder = faker;
      /// </summary>
      public static implicit operator T(Faker<T> faker)
      {
         return faker.Generate();
      }
   }
}
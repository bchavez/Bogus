using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections;

namespace Bogus.Tests.Auto
{
  public static class AssertionExtensions
  {
    public static void ShouldThrow(this Action action, Type expected)
    {
      Type actual = null;

      try
      {
        action.Invoke();
      }
      catch (Exception ex)
      {
        actual = ex.GetType();
      }

      Execute.Assertion
        .ForCondition(actual == expected)
        .FailWith("Expected an exception of type '{0}', but found '{1}'.", expected.FullName, actual?.FullName ?? "null");

    }

    public static void NotBeDefaultFor(this ReferenceTypeAssertions<object, ObjectAssertions> assertions, Type type)
    {
      var defaultValue = GetDefaultValue(type);
      var valid = IsValidValue(type, assertions.Subject, defaultValue);

      Execute.Assertion
        .ForCondition(valid)
        .FailWith("Expected a non-default, but found '{0}'.", defaultValue ?? "null");
    }

    public static void NotContainDefaultFor(this ReferenceTypeAssertions<object, ObjectAssertions> assertions, Type type)
    {
      var subjectType = assertions.Subject?.GetType();
      var items = assertions.Subject as IEnumerable;
      var continuation = Execute.Assertion
        .ForCondition(items != null)
        .FailWith("Expected an enumerable type, but found '{0}'.", subjectType?.FullName ?? "null");

      NotContainDefaultFor(items, type, "item", continuation);
    }

    public static void NotContainDefaultFor(this ReferenceTypeAssertions<object, ObjectAssertions> assertions, Type keyType, Type valueType)
    {
      var subjectType = assertions.Subject?.GetType();
      var items = assertions.Subject as IDictionary;
      var continuation = Execute.Assertion
        .ForCondition(items != null)
        .FailWith("Expected a dictionary type, but found '{0}'.", subjectType?.FullName ?? "null");

      NotContainDefaultFor(items.Keys, keyType, "key", continuation);
      NotContainDefaultFor(items.Values, valueType, "value", continuation);
    }

    public static void NotContainDefaultFor(IEnumerable items, Type itemType, string itemDescription, Continuation continuation)
    { 
      var index = 0;
      
      foreach (var item in items)
      {
        var defaultValue = GetDefaultValue(itemType);
        var valid = IsValidValue(itemType, item, defaultValue);

        continuation
          .Then
          .ForCondition(valid)
          .FailWith("Expected a non-default for {0} at index '{1}', but found '{2}'.", itemDescription, index, defaultValue ?? "null");

        index++;
      }

      continuation
        .Then
        .ForCondition(index != 0)
        .FailWith("Expected one or more items, but found 0.");
    }

    private static object GetDefaultValue(Type type)
    {
      return type.IsValueType
        ? Activator.CreateInstance(type)
        : null;
    }

    private static bool IsValidValue(Type type, object subject, object defaultValue)
    {
      var valid = subject != null && subject.GetType() == type;

      if (defaultValue == null)
      {
        return valid;
      }

      // The following are special cases where the default value is actually valid
      // Just check that the subject isn't null
      if (type.IsEnum || defaultValue is bool)
      {
        return valid;
      }

      return valid && !defaultValue.Equals(subject);
    }
  }
}

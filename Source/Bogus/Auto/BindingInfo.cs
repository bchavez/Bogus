using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bogus.Auto
{
    public sealed class BindingInfo
    {
        internal static readonly Type DictionaryDefinition = typeof(IDictionary<,>);
        internal static readonly Type EnumerableDefinition = typeof(IEnumerable<>);

        internal BindingInfo(Type type, string name, BindingInfo parent = null)
        {
            Type = type;
            Name = name;
            Parent = parent;
            Binder = (i, v) => { };
        }

        internal BindingInfo(MemberInfo member, BindingInfo parent)
        {
            Parent = parent;

            // Resolve the member as either a field or property
            if (member is FieldInfo field)
            {
                Type = field.FieldType;
                Name = field.Name;
                Value = field.GetValue(parent.Value);
                Binder = field.SetValue;
            }
            else if (member is PropertyInfo property)
            {
                Type = property.PropertyType;
                Name = property.Name;
                Value = property.GetValue(parent.Value, null);
                Binder = (i, v) => property.SetValue(i, v, null);
            }
            else
            {
                throw new ArgumentException("Only property and field types are supported.");
            }
        }

        public Type Type { get; }
        public string Name { get; }
        public BindingInfo Parent { get; }
        public object Value { get; set; }

        private Action<object, object> Binder { get; }

        internal void Bind(object instance, object value)
        {
            Binder.Invoke(instance, value);
        }

        public override string ToString()
        {
            return string.Concat(Type.FullName, " > ", Name);
        }
    }
}

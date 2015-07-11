using System;

namespace Bogus
{
    ///// <summary>
    ///// Marker attribute for identifying methods that can be mustached.
    ///// </summary>
    //[AttributeUsage(AttributeTargets.Method)]
    //public class MustasheAttribute : Attribute
    //{
        
    //}

    /// <summary>
    /// DataCategory is used when resolving the final category name inside the locale.
    /// For example, a 'phone_numbers' is the data set name in a locale, but the 
    /// C# style DataSet is PhoneNumbers; When a dataset is marked with DataCategory,
    /// you can specify that category name manually. If no data category is specified,
    /// then the C# class name is used.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataCategoryAttribute : Attribute
    {
        public string Name { get;set; }

        public DataCategoryAttribute(string name)
        {
            this.Name = name;
        }
    }
}
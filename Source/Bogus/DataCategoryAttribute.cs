using System;

namespace Bogus
{
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
      /// <summary>
      /// The category name.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Creates a data category attribute with a specified category name.
      /// </summary>
      public DataCategoryAttribute(string name)
      {
         this.Name = name;
      }
   }
}
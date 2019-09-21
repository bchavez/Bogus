using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Bogus.Platform;

namespace Bogus
{
   /// <summary>
   /// A binder is used in Faker[T] for extracting MemberInfo from T
   /// that are candidates for property/field faking.
   /// </summary>
   public interface IBinder
   {
      /// <summary>
      /// Given T, the method must return a Dictionary[string,MemberInfo] where
      /// string is the field/property name and MemberInfo is the reflected
      /// member info of the field/property that will be used for invoking
      /// and setting values. The returned Dictionary must encompass the full
      /// set of viable properties/fields that can be faked on T.
      /// </summary>
      /// <returns>The full set of MemberInfos for injection.</returns>
      Dictionary<string, MemberInfo> GetMembers(Type t);
   }

   /// <summary>
   /// The default binder used in Faker[T] for extracting MemberInfo from T
   /// that are candidates for property/field faking.
   /// </summary>
   public class Binder : IBinder
   {
      /// <summary>
      /// The binding flags to use when reflecting over T.
      /// </summary>
      protected internal BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

      /// <summary>
      /// Construct a binder with default binding flags. Public/internal properties and public/internal fields.
      /// </summary>
      public Binder()
      {
      }

      /// <summary>
      /// Construct a binder with custom binding flags.
      /// </summary>
      public Binder(BindingFlags bindingFlags)
      {
         BindingFlags = bindingFlags;
      }

      /// <summary>
      /// Given T, the method will return a Dictionary[string,MemberInfo] where
      /// string is the field/property name and MemberInfo is the reflected
      /// member info of the field/property that will be used for invocation 
      /// and setting values. The returned Dictionary must encompass the full
      /// set of viable properties/fields that can be faked on T.
      /// </summary>
      /// <returns>The full set of MemberInfos for injection.</returns>
      public virtual Dictionary<string, MemberInfo> GetMembers(Type t)
      {
         var group = t.GetAllMembers(BindingFlags)
            .Where(m =>
               {
                  if( m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any() )
                  {
                     //no compiler generated stuff
                     return false;
                  }
                  if( m is PropertyInfo pi )
                  {
                     return pi.CanWrite;
                  }
                  if( m is FieldInfo fi )
                  {
                     //No private fields.
                     //GitHub Issue #13
                     return !fi.IsPrivate;
                  }
                  return false;
               })
            .GroupBy(mi => mi.Name);

         //Issue #70 we could get back multiple keys
         //when reflecting over a type. Consider:
         //
         //   ClassA { public int Value {get;set} }
         //   DerivedA : ClassA { public new int Value {get;set;} }
         //
         //So, when reflecting over DerivedA, grab the first
         //reflected MemberInfo that was returned from
         //reflection; the second one was the inherited
         //ClassA.Value.
         return group.ToDictionary(k => k.Key, g => g.First());
      }
   }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

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
        public Dictionary<string, MemberInfo> GetMembers(Type t)
        {
            return t.GetMembers(BindingFlags)
                .Where(m =>
                    {
                        if (m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
                        {
                            //no compiler generated stuff
                            return false;
                        }
                        var pi = m as PropertyInfo;
                        if (pi != null)
                        {
                            return pi.CanWrite;
                        }
                        return m is PropertyInfo || m is FieldInfo;
                    })
                .ToDictionary(pi => pi.Name);
        }
    }
}
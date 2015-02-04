using System;
using System.Linq;
using System.Reflection;

namespace BddSharp.Engine.Extensions
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Assumes that there is only one attribute of the given type. It will throw an except if there is more than one.
        /// </summary>
        public static T GetCustomAttribute<T>(this MemberInfo method) where T : Attribute
        {
            var attrType = typeof(T);

            if (!typeof(Attribute).IsAssignableFrom(attrType))
                throw new ArgumentException(string.Format("The type {0} does not inherit from Attribute", attrType.Name));

            var attrs = method.GetCustomAttributes(attrType, false);

            if (attrs.IsNullOrEmpty())
                return null;

            if (attrs.Count() > 1)
                throw new Exception(string.Format("There is more than one attribute of the type {0} on the given method.", attrType.Name));

            return attrs.OfType<T>().First();
        }
    }
}

using System;
using System.Linq;
using System.Reflection;

namespace WpfMagic.Extensions
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

        public static Type GetUnderlyingType(this Type t, int index = 0)
        {
            if (t == null)
                return null;

            return !t.IsGenericType ? t : t.GetGenericArguments()[index];
        }

        public static Type GetNestedPropertyType(this Type t, string propertyAccessorString)
        {
            if (t == null)
                return null;

            var accessors = propertyAccessorString.Split(new char[] { '.' });

            var currentType = t;

            foreach (var accessor in accessors)
            {
                var property = currentType.GetProperty(accessor);
                if (property == null)
                {
                    currentType = null;
                    break;
                }

                currentType = property.PropertyType;
            }

            return currentType != null ? currentType.GetUnderlyingType() : null;
        }

        public static bool Implements(this Type concreteType, Type interfaceType)
        {
            if (!interfaceType.IsGenericTypeDefinition)
                return interfaceType.IsAssignableFrom(concreteType);

            var baseType = concreteType;

            while (baseType != null && baseType != typeof(object))
            {
                if (baseType == interfaceType || (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == interfaceType))
                    return true;

                if (baseType.GetInterfaces().Any(i => (i.IsGenericType ? i.GetGenericTypeDefinition() : i) == interfaceType))
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }

        internal static void SetProperty(this object obj, string propertyName, object value)
        {
            if (obj == null)
                return;

            var type = obj.GetType();

            var pi = type.GetProperty(propertyName);
            if (pi == null)
                return;

            pi.SetValue(obj, value, null);
        }

        internal static T SafeCreate<T>(this Type t) where T : class
        {
            try
            {
                return (T)Activator.CreateInstance(t);
            }
            catch
            {
            }

            return default(T);
        }

		internal static bool IsActiveViewModel(this Type t)
		{
			return t.FullName.StartsWith("Active") && t.Module.ScopeName == WpfMagic.Mvvm.ActiveViewModelFactory.DYNAMIC_ASSEMBLY_NAME;
		}
    }
}

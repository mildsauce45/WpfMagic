using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using WpfMagic.Attributes;

namespace WpfMagic.Mvvm
{
	public static class ActiveViewModelFactory
	{
		internal const string DYNAMIC_ASSEMBLY_NAME = "ActiveViewModelAssembly";

		public static T CreateInstance<T>() where T : NotifyableObject
		{
			var vmType = typeof(T);

			if (vmType.IsSealed)
				throw new InvalidOperationException("The given view model type cannot be sealed.");

			var assemblyName = new AssemblyName(DYNAMIC_ASSEMBLY_NAME);
			var domain = AppDomain.CurrentDomain;
			var assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

			var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

			var dynamicTypeName = Assembly.CreateQualifiedName(vmType.AssemblyQualifiedName, "Active" + vmType.Name);

			var typeBuilder = moduleBuilder.DefineType(dynamicTypeName, TypeAttributes.Public | TypeAttributes.Class, vmType);

			var srMethod = typeof(NotifyableObject).GetMethod("NotifyPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string) }, null);

			foreach (var pi in GetActiveNotifyCandidates<T>())
				UpdateProperty(pi, typeBuilder, srMethod);

			var dynamicType = typeBuilder.CreateType();

			return (T)Activator.CreateInstance(dynamicType);
		}

		private static IEnumerable<PropertyInfo> GetActiveNotifyCandidates<T>()
		{
			return typeof(T).GetProperties().Where(p => p.GetSetMethod() != null && p.GetSetMethod().IsVirtual && p.GetCustomAttributes(typeof(ActiveAttribute), false).Length > 0);
		}

		private static void UpdateProperty(PropertyInfo pi, TypeBuilder typeBuilder, MethodInfo safeRaiseMethod)
		{
			var propertyBuilder = typeBuilder.DefineProperty(pi.Name, PropertyAttributes.None, pi.PropertyType, null);

			var methodBuilder = typeBuilder.DefineMethod("set_" + pi.Name, MethodAttributes.Public | MethodAttributes.Virtual, null, new Type[] { pi.PropertyType });
			methodBuilder.DefineParameter(1, ParameterAttributes.None, "value");
			var generator = methodBuilder.GetILGenerator();

			// Calls the base setter
			generator.Emit(OpCodes.Nop);
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Call, pi.GetSetMethod());

			generator.Emit(OpCodes.Nop);
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldstr, pi.Name);
			generator.Emit(OpCodes.Callvirt, safeRaiseMethod);
			generator.Emit(OpCodes.Nop);
			generator.Emit(OpCodes.Ret);

			propertyBuilder.SetSetMethod(methodBuilder);

			var getMethodBuilder = typeBuilder.DefineMethod("get_" + pi.Name, MethodAttributes.Public | MethodAttributes.Virtual, pi.PropertyType, Type.EmptyTypes);
			var getGenerator = getMethodBuilder.GetILGenerator();

			getGenerator.Emit(OpCodes.Nop);
			getGenerator.Emit(OpCodes.Ldarg_0);
			getGenerator.Emit(OpCodes.Call, pi.GetGetMethod());
			getGenerator.Emit(OpCodes.Ret);

			propertyBuilder.SetGetMethod(getMethodBuilder);
		}
	}
}

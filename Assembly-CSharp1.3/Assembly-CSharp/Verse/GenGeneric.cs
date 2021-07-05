using System;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000028 RID: 40
	public static class GenGeneric
	{
		// Token: 0x06000232 RID: 562 RVA: 0x0000B7EE File Offset: 0x000099EE
		private static MethodInfo MethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			return genericBase.MakeGenericType(new Type[]
			{
				genericParam
			}).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000B808 File Offset: 0x00009A08
		public static void InvokeGenericMethod(object objectToInvoke, Type genericParam, string methodName, params object[] args)
		{
			objectToInvoke.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(objectToInvoke, args);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B82F File Offset: 0x00009A2F
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName, params object[] args)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, args);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000B840 File Offset: 0x00009A40
		public static object InvokeStaticMethodOnGenericType(Type genericBase, Type genericParam, string methodName)
		{
			return GenGeneric.MethodOnGenericType(genericBase, genericParam, methodName).Invoke(null, null);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000B851 File Offset: 0x00009A51
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName)
		{
			return baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(null, null);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000B872 File Offset: 0x00009A72
		public static object InvokeStaticGenericMethod(Type baseClass, Type genericParam, string methodName, params object[] args)
		{
			return baseClass.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				genericParam
			}).Invoke(null, args);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000B893 File Offset: 0x00009A93
		private static PropertyInfo PropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			return genericBase.MakeGenericType(new Type[]
			{
				genericParam
			}).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B8AD File Offset: 0x00009AAD
		public static object GetStaticPropertyOnGenericType(Type genericBase, Type genericParam, string propertyName)
		{
			return GenGeneric.PropertyOnGenericType(genericBase, genericParam, propertyName).GetGetMethod().Invoke(null, null);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B8C3 File Offset: 0x00009AC3
		public static bool HasGenericDefinition(this Type type, Type Def)
		{
			return type.GetTypeWithGenericDefinition(Def) != null;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		public static Type GetTypeWithGenericDefinition(this Type type, Type Def)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (Def == null)
			{
				throw new ArgumentNullException("Def");
			}
			if (!Def.IsGenericTypeDefinition)
			{
				throw new ArgumentException("The Def needs to be a GenericTypeDefinition", "Def");
			}
			if (Def.IsInterface)
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == Def)
					{
						return type2;
					}
				}
			}
			Type type3 = type;
			while (type3 != null)
			{
				if (type3.IsGenericType && type3.GetGenericTypeDefinition() == Def)
				{
					return type3;
				}
				type3 = type3.BaseType;
			}
			return null;
		}

		// Token: 0x04000063 RID: 99
		public const BindingFlags BindingFlagsAll = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}

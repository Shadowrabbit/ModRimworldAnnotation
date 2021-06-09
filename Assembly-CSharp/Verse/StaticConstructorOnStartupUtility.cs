using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000095 RID: 149
	public static class StaticConstructorOnStartupUtility
	{
		// Token: 0x0600051E RID: 1310 RVA: 0x0008ACB0 File Offset: 0x00088EB0
		public static void CallAll()
		{
			foreach (Type type in GenTypes.AllTypesWithAttribute<StaticConstructorOnStartup>())
			{
				try
				{
					RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Error in static constructor of ",
						type,
						": ",
						ex
					}), false);
				}
			}
			StaticConstructorOnStartupUtility.coreStaticAssetsLoaded = true;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0008AD40 File Offset: 0x00088F40
		public static void ReportProbablyMissingAttributes()
		{
			BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			foreach (Type type in GenTypes.AllTypes)
			{
				if (!type.HasAttribute<StaticConstructorOnStartup>())
				{
					FieldInfo fieldInfo = type.GetFields(bindingAttr).FirstOrDefault(delegate(FieldInfo x)
					{
						Type type2 = x.FieldType;
						if (type2.IsArray)
						{
							type2 = type2.GetElementType();
						}
						return typeof(Texture).IsAssignableFrom(type2) || typeof(Material).IsAssignableFrom(type2) || typeof(Shader).IsAssignableFrom(type2) || typeof(Graphic).IsAssignableFrom(type2) || typeof(GameObject).IsAssignableFrom(type2) || typeof(MaterialPropertyBlock).IsAssignableFrom(type2);
					});
					if (fieldInfo != null)
					{
						Log.Warning(string.Concat(new string[]
						{
							"Type ",
							type.Name,
							" probably needs a StaticConstructorOnStartup attribute, because it has a field ",
							fieldInfo.Name,
							" of type ",
							fieldInfo.FieldType.Name,
							". All assets must be loaded in the main thread."
						}), false);
					}
				}
			}
		}

		// Token: 0x04000283 RID: 643
		public static bool coreStaticAssetsLoaded;
	}
}

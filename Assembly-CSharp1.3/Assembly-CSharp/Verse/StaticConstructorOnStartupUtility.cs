using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200004F RID: 79
	public static class StaticConstructorOnStartupUtility
	{
		// Token: 0x060003CD RID: 973 RVA: 0x00014CD4 File Offset: 0x00012ED4
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
					}));
				}
			}
			StaticConstructorOnStartupUtility.coreStaticAssetsLoaded = true;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00014D64 File Offset: 0x00012F64
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
						}));
					}
				}
			}
		}

		// Token: 0x0400011B RID: 283
		public static bool coreStaticAssetsLoaded;
	}
}

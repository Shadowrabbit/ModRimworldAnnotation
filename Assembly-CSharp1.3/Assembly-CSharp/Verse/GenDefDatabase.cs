using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200007B RID: 123
	public static class GenDefDatabase
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x00018A4E File Offset: 0x00016C4E
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", new object[]
			{
				defName,
				errorOnFail
			});
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00018A80 File Offset: 0x00016C80
		public static Def GetDefSilentFail(Type type, string targetDefName, bool specialCaseForSoundDefs = true)
		{
			if (specialCaseForSoundDefs && type == typeof(SoundDef))
			{
				return SoundDef.Named(targetDefName);
			}
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), type, "GetNamedSilentFail", new object[]
			{
				targetDefName
			});
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00018ACD File Offset: 0x00016CCD
		public static IEnumerable<Def> GetAllDefsInDatabaseForDef(Type defType)
		{
			return ((IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), defType, "AllDefs")).Cast<Def>();
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00018AEE File Offset: 0x00016CEE
		public static IEnumerable<Type> AllDefTypesWithDatabases()
		{
			foreach (Type type in typeof(Def).AllSubclasses())
			{
				if (!type.IsAbstract && !(type == typeof(Def)))
				{
					bool flag = false;
					Type baseType = type.BaseType;
					while (baseType != null && baseType != typeof(Def))
					{
						if (!baseType.IsAbstract)
						{
							flag = true;
							break;
						}
						baseType = baseType.BaseType;
					}
					if (!flag)
					{
						yield return type;
					}
				}
			}
			List<Type>.Enumerator enumerator = default(List<Type>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00018AF7 File Offset: 0x00016CF7
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}
	}
}

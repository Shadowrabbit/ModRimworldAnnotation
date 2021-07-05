using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000CA RID: 202
	public static class GenDefDatabase
	{
		// Token: 0x06000617 RID: 1559 RVA: 0x0000B293 File Offset: 0x00009493
		public static Def GetDef(Type defType, string defName, bool errorOnFail = true)
		{
			return (Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), defType, "GetNamed", new object[]
			{
				defName,
				errorOnFail
			});
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0008E1DC File Offset: 0x0008C3DC
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

		// Token: 0x06000619 RID: 1561 RVA: 0x0000B2C2 File Offset: 0x000094C2
		public static IEnumerable<Def> GetAllDefsInDatabaseForDef(Type defType)
		{
			return ((IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), defType, "AllDefs")).Cast<Def>();
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x0000B2E3 File Offset: 0x000094E3
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
			IEnumerator<Type> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x0000B2EC File Offset: 0x000094EC
		public static IEnumerable<T> DefsToGoInDatabase<T>(ModContentPack mod)
		{
			return mod.AllDefs.OfType<T>();
		}
	}
}

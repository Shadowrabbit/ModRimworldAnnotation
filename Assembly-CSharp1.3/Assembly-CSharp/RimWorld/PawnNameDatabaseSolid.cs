using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E28 RID: 3624
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x060053C5 RID: 21445 RVA: 0x001C5C7C File Offset: 0x001C3E7C
		static PawnNameDatabaseSolid()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
			}
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x001C5CF4 File Offset: 0x001C3EF4
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x001C5D07 File Offset: 0x001C3F07
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x001C5D14 File Offset: 0x001C3F14
		public static IEnumerable<NameTriple> AllNames()
		{
			foreach (KeyValuePair<GenderPossibility, List<NameTriple>> keyValuePair in PawnNameDatabaseSolid.solidNames)
			{
				foreach (NameTriple nameTriple in keyValuePair.Value)
				{
					yield return nameTriple;
				}
				List<NameTriple>.Enumerator enumerator2 = default(List<NameTriple>.Enumerator);
			}
			Dictionary<GenderPossibility, List<NameTriple>>.Enumerator enumerator = default(Dictionary<GenderPossibility, List<NameTriple>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0400314E RID: 12622
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x0400314F RID: 12623
		private const float PreferredNameChance = 0.5f;
	}
}

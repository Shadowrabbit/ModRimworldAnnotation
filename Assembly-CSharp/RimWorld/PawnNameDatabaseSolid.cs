using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014C2 RID: 5314
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x0600726E RID: 29294 RVA: 0x0022FAD8 File Offset: 0x0022DCD8
		static PawnNameDatabaseSolid()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
			}
		}

		// Token: 0x0600726F RID: 29295 RVA: 0x0004CF17 File Offset: 0x0004B117
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x06007270 RID: 29296 RVA: 0x0004CF2A File Offset: 0x0004B12A
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x06007271 RID: 29297 RVA: 0x0004CF37 File Offset: 0x0004B137
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

		// Token: 0x04004B61 RID: 19297
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x04004B62 RID: 19298
		private const float PreferredNameChance = 0.5f;
	}
}

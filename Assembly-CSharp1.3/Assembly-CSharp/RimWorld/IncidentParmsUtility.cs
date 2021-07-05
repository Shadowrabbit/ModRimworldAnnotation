using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BF0 RID: 3056
	public static class IncidentParmsUtility
	{
		// Token: 0x060047EA RID: 18410 RVA: 0x0017BFA4 File Offset: 0x0017A1A4
		public static PawnGroupMakerParms GetDefaultPawnGroupMakerParms(PawnGroupKindDef groupKind, IncidentParms parms, bool ensureCanGenerateAtLeastOnePawn = false)
		{
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = groupKind;
			pawnGroupMakerParms.tile = parms.target.Tile;
			pawnGroupMakerParms.points = parms.points;
			pawnGroupMakerParms.faction = parms.faction;
			pawnGroupMakerParms.traderKind = parms.traderKind;
			pawnGroupMakerParms.generateFightersOnly = parms.generateFightersOnly;
			pawnGroupMakerParms.raidStrategy = parms.raidStrategy;
			pawnGroupMakerParms.forceOneIncap = parms.raidForceOneIncap;
			pawnGroupMakerParms.seed = parms.pawnGroupMakerSeed;
			pawnGroupMakerParms.ideo = parms.pawnIdeo;
			if (ensureCanGenerateAtLeastOnePawn && parms.faction != null)
			{
				pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, parms.faction.def.MinPointsToGeneratePawnGroup(groupKind, null));
			}
			return pawnGroupMakerParms;
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x0017C060 File Offset: 0x0017A260
		public static List<List<Pawn>> SplitIntoGroups(List<Pawn> pawns, Dictionary<Pawn, int> groups)
		{
			List<List<Pawn>> list = new List<List<Pawn>>();
			List<Pawn> list2 = pawns.ToList<Pawn>();
			while (list2.Any<Pawn>())
			{
				List<Pawn> list3 = new List<Pawn>();
				Pawn pawn = list2.Last<Pawn>();
				list2.RemoveLast<Pawn>();
				list3.Add(pawn);
				for (int i = list2.Count - 1; i >= 0; i--)
				{
					if (IncidentParmsUtility.GetGroup(pawn, groups) == IncidentParmsUtility.GetGroup(list2[i], groups))
					{
						list3.Add(list2[i]);
						list2.RemoveAt(i);
					}
				}
				list.Add(list3);
			}
			return list;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x0017C0EC File Offset: 0x0017A2EC
		private static int GetGroup(Pawn pawn, Dictionary<Pawn, int> groups)
		{
			int result;
			if (groups == null || !groups.TryGetValue(pawn, out result))
			{
				return -1;
			}
			return result;
		}
	}
}

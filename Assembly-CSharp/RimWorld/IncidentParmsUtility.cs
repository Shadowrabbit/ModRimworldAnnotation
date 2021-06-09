using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001189 RID: 4489
	public static class IncidentParmsUtility
	{
		// Token: 0x060062F3 RID: 25331 RVA: 0x001ED980 File Offset: 0x001EBB80
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
			if (ensureCanGenerateAtLeastOnePawn && parms.faction != null)
			{
				pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, parms.faction.def.MinPointsToGeneratePawnGroup(groupKind));
			}
			return pawnGroupMakerParms;
		}

		// Token: 0x060062F4 RID: 25332 RVA: 0x001EDA30 File Offset: 0x001EBC30
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

		// Token: 0x060062F5 RID: 25333 RVA: 0x001EDABC File Offset: 0x001EBCBC
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

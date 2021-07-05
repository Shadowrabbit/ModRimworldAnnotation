using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DCC RID: 3532
	public static class PawnsArrivalModeWorkerUtility
	{
		// Token: 0x060051FB RID: 20987 RVA: 0x001BA844 File Offset: 0x001B8A44
		public static void DropInDropPodsNearSpawnCenter(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			bool flag = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			DropPodUtility.DropThingsNear(parms.spawnCenter, map, pawns.Cast<Thing>(), parms.podOpenDelay, false, true, flag || parms.raidArrivalModeForQuickMilitaryAid, true);
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x001BA8A0 File Offset: 0x001B8AA0
		public static List<Pair<List<Pawn>, IntVec3>> SplitIntoRandomGroupsNearMapEdge(List<Pawn> pawns, Map map, bool arriveInPods)
		{
			List<Pair<List<Pawn>, IntVec3>> list = new List<Pair<List<Pawn>, IntVec3>>();
			if (!pawns.Any<Pawn>())
			{
				return list;
			}
			int maxGroupsCount = PawnsArrivalModeWorkerUtility.GetMaxGroupsCount(pawns.Count);
			int num = (maxGroupsCount == 1) ? 1 : Rand.RangeInclusive(2, maxGroupsCount);
			for (int i = 0; i < num; i++)
			{
				IntVec3 second = PawnsArrivalModeWorkerUtility.FindNewMapEdgeGroupCenter(map, list, arriveInPods);
				list.Add(new Pair<List<Pawn>, IntVec3>(new List<Pawn>(), second)
				{
					First = 
					{
						pawns[i]
					}
				});
			}
			for (int j = num; j < pawns.Count; j++)
			{
				list.RandomElement<Pair<List<Pawn>, IntVec3>>().First.Add(pawns[j]);
			}
			return list;
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x001BA94C File Offset: 0x001B8B4C
		private static IntVec3 FindNewMapEdgeGroupCenter(Map map, List<Pair<List<Pawn>, IntVec3>> groups, bool arriveInPods)
		{
			IntVec3 result = IntVec3.Invalid;
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec;
				if (arriveInPods)
				{
					intVec = DropCellFinder.FindRaidDropCenterDistant(map, false);
				}
				else if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Hostile, false, null))
				{
					intVec = DropCellFinder.FindRaidDropCenterDistant(map, false);
				}
				if (!groups.Any<Pair<List<Pawn>, IntVec3>>())
				{
					result = intVec;
					break;
				}
				float num2 = float.MaxValue;
				for (int j = 0; j < groups.Count; j++)
				{
					float num3 = (float)intVec.DistanceToSquared(groups[j].Second);
					if (num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!result.IsValid || num2 > num)
				{
					num = num2;
					result = intVec;
				}
			}
			return result;
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x001BA9FD File Offset: 0x001B8BFD
		private static int GetMaxGroupsCount(int pawnsCount)
		{
			if (pawnsCount <= 1)
			{
				return 1;
			}
			return Mathf.Clamp(pawnsCount / 2, 2, 3);
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x001BAA10 File Offset: 0x001B8C10
		public static void SetPawnGroupsInfo(IncidentParms parms, List<Pair<List<Pawn>, IntVec3>> groups)
		{
			parms.pawnGroups = new Dictionary<Pawn, int>();
			for (int i = 0; i < groups.Count; i++)
			{
				for (int j = 0; j < groups[i].First.Count; j++)
				{
					parms.pawnGroups.Add(groups[i].First[j], i);
				}
			}
		}

		// Token: 0x04003075 RID: 12405
		private const int MaxGroupsCount = 3;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AAC RID: 2732
	public static class WanderUtility
	{
		// Token: 0x0600409F RID: 16543 RVA: 0x00183038 File Offset: 0x00181238
		public static IntVec3 BestCloseWanderRoot(IntVec3 trueWanderRoot, Pawn pawn)
		{
			for (int i = 0; i < 50; i++)
			{
				IntVec3 intVec;
				if (i < 8)
				{
					intVec = trueWanderRoot + GenRadial.RadialPattern[i];
				}
				else
				{
					intVec = trueWanderRoot + GenRadial.RadialPattern[i - 8 + 1] * 7;
				}
				if (intVec.InBounds(pawn.Map) && intVec.Walkable(pawn.Map) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x060040A0 RID: 16544 RVA: 0x001830BC File Offset: 0x001812BC
		public static bool InSameRoom(IntVec3 locA, IntVec3 locB, Map map)
		{
			Room room = locA.GetRoom(map, RegionType.Set_All);
			return room == null || room == locB.GetRoom(map, RegionType.Set_All);
		}

		// Token: 0x060040A1 RID: 16545 RVA: 0x001830E4 File Offset: 0x001812E4
		public static IntVec3 GetColonyWanderRoot(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				WanderUtility.gatherSpots.Clear();
				for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
				{
					IntVec3 position = pawn.Map.gatherSpotLister.activeSpots[i].parent.Position;
					if (!position.IsForbidden(pawn) && pawn.CanReach(position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn))
					{
						WanderUtility.gatherSpots.Add(position);
					}
				}
				if (WanderUtility.gatherSpots.Count > 0)
				{
					return WanderUtility.gatherSpots.RandomElement<IntVec3>();
				}
			}
			WanderUtility.candidateCells.Clear();
			WanderUtility.candidateBuildingsInRandomOrder.Clear();
			WanderUtility.candidateBuildingsInRandomOrder.AddRange(pawn.Map.listerBuildings.allBuildingsColonist);
			WanderUtility.candidateBuildingsInRandomOrder.Shuffle<Building>();
			int num = 0;
			int j = 0;
			while (j < WanderUtility.candidateBuildingsInRandomOrder.Count)
			{
				if (num > 80 && WanderUtility.candidateCells.Count > 0)
				{
					return WanderUtility.candidateCells.RandomElement<IntVec3>();
				}
				Building building = WanderUtility.candidateBuildingsInRandomOrder[j];
				if ((building.def == ThingDefOf.Wall || building.def.building.ai_chillDestination) && !building.Position.IsForbidden(pawn) && pawn.Map.areaManager.Home[building.Position])
				{
					IntVec3 intVec = GenAdjFast.AdjacentCells8Way(building).RandomElement<IntVec3>();
					if (intVec.Standable(building.Map) && !intVec.IsForbidden(pawn) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !intVec.IsInPrisonCell(pawn.Map))
					{
						WanderUtility.candidateCells.Add(intVec);
						if ((pawn.Position - building.Position).LengthHorizontalSquared <= 1225)
						{
							return intVec;
						}
					}
				}
				j++;
				num++;
			}
			Pawn pawn2;
			if ((from c in pawn.Map.mapPawns.FreeColonistsSpawned
			where !c.Position.IsForbidden(pawn) && pawn.CanReach(c.Position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn)
			select c).TryRandomElement(out pawn2))
			{
				return pawn2.Position;
			}
			return pawn.Position;
		}

		// Token: 0x04002C6F RID: 11375
		private static List<IntVec3> gatherSpots = new List<IntVec3>();

		// Token: 0x04002C70 RID: 11376
		private static List<IntVec3> candidateCells = new List<IntVec3>();

		// Token: 0x04002C71 RID: 11377
		private static List<Building> candidateBuildingsInRandomOrder = new List<Building>();
	}
}

using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000644 RID: 1604
	public class JobGiver_WanderInRoofedCellsInPen : JobGiver_Wander
	{
		// Token: 0x06002D99 RID: 11673 RVA: 0x001106FD File Offset: 0x0010E8FD
		public JobGiver_WanderInRoofedCellsInPen()
		{
			this.wanderRadius = 10f;
			this.wanderDestValidator = ((Pawn pawn, IntVec3 cell, IntVec3 root) => cell.Roofed(pawn.Map));
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x00110738 File Offset: 0x0010E938
		protected override IntVec3 GetExactWanderDest(Pawn pawn)
		{
			Map map = pawn.Map;
			if (!JobGiver_WanderInRoofedCellsInPen.ShouldSeekRoofedCells(pawn))
			{
				return IntVec3.Invalid;
			}
			CompAnimalPenMarker currentPenOf = AnimalPenUtility.GetCurrentPenOf(pawn, false);
			if (currentPenOf == null)
			{
				return IntVec3.Invalid;
			}
			IntVec3 intVec = pawn.Position;
			if (!intVec.Roofed(map))
			{
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Add(pawn.GetRegion(RegionType.Set_Passable));
				intVec = JobGiver_WanderInRoofedCellsInPen.FindNearbyRoofedCellIn(pawn.Position, JobGiver_WanderInRoofedCellsInPen.wanderRootRegions, map);
			}
			if (!intVec.IsValid || !intVec.Roofed(map))
			{
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.AddRange(currentPenOf.PenState.ConnectedRegions);
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Shuffle<Region>();
				JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.TruncateToLength(4);
				intVec = JobGiver_WanderInRoofedCellsInPen.FindNearbyRoofedCellIn(pawn.Position, JobGiver_WanderInRoofedCellsInPen.wanderRootRegions, map);
			}
			JobGiver_WanderInRoofedCellsInPen.wanderRootRegions.Clear();
			if (!intVec.IsValid)
			{
				return IntVec3.Invalid;
			}
			return RCellFinder.RandomWanderDestFor(pawn, intVec, this.wanderRadius, this.wanderDestValidator, PawnUtility.ResolveMaxDanger(pawn, this.maxDanger));
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x0002974C File Offset: 0x0002794C
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x00110839 File Offset: 0x0010EA39
		private static bool ShouldSeekRoofedCells(Pawn pawn)
		{
			return pawn.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout);
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x00110850 File Offset: 0x0010EA50
		private static IntVec3 FindNearbyRoofedCellIn(IntVec3 root, List<Region> regions, Map map)
		{
			JobGiver_WanderInRoofedCellsInPen.<>c__DisplayClass6_0 CS$<>8__locals1 = new JobGiver_WanderInRoofedCellsInPen.<>c__DisplayClass6_0();
			CS$<>8__locals1.map = map;
			IntVec3 result = IntVec3.Invalid;
			int num = int.MaxValue;
			using (List<Region>.Enumerator enumerator = regions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec;
					if (enumerator.Current.TryFindRandomCellInRegion(new Predicate<IntVec3>(CS$<>8__locals1.<FindNearbyRoofedCellIn>g__SafeCell|0), out intVec))
					{
						int num2 = root.DistanceToSquared(intVec);
						if (num2 < num)
						{
							result = intVec;
							num = num2;
						}
						if (num <= 100)
						{
							return result;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001BE8 RID: 7144
		private const int PenRegionsSampleSize = 4;

		// Token: 0x04001BE9 RID: 7145
		private static readonly List<Region> wanderRootRegions = new List<Region>();
	}
}

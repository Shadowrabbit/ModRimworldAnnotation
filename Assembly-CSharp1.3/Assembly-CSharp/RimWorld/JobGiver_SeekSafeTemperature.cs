using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007D1 RID: 2001
	public class JobGiver_SeekSafeTemperature : ThinkNode_JobGiver
	{
		// Token: 0x060035D9 RID: 13785 RVA: 0x0013108C File Offset: 0x0012F28C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Serious))
			{
				return null;
			}
			FloatRange tempRange = pawn.ComfortableTemperatureRange();
			if (tempRange.Includes(pawn.AmbientTemperature))
			{
				return JobMaker.MakeJob(JobDefOf.Wait_SafeTemperature, 500, true);
			}
			Region region = JobGiver_SeekSafeTemperature.ClosestRegionWithinTemperatureRange(pawn.Position, pawn.Map, tempRange, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), RegionType.Set_Passable);
			if (region != null)
			{
				return JobMaker.MakeJob(JobDefOf.GotoSafeTemperature, region.RandomCell);
			}
			return null;
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x00131110 File Offset: 0x0012F310
		private static Region ClosestRegionWithinTemperatureRange(IntVec3 root, Map map, FloatRange tempRange, TraverseParms traverseParms, RegionType traversableRegionTypes = RegionType.Set_Passable)
		{
			Region region = root.GetRegion(map, traversableRegionTypes);
			if (region == null)
			{
				return null;
			}
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
			Region foundReg = null;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				if (r.IsDoorway)
				{
					return false;
				}
				if (tempRange.Includes(r.Room.Temperature))
				{
					foundReg = r;
					return true;
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, traversableRegionTypes);
			return foundReg;
		}
	}
}

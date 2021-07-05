using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000764 RID: 1892
	public class JobGiver_LayEgg : ThinkNode_JobGiver
	{
		// Token: 0x06003452 RID: 13394 RVA: 0x0012897C File Offset: 0x00126B7C
		protected override Job TryGiveJob(Pawn pawn)
		{
			CompEggLayer compEggLayer = pawn.TryGetComp<CompEggLayer>();
			if (compEggLayer == null || !compEggLayer.CanLayNow)
			{
				return null;
			}
			ThingDef singleDef = compEggLayer.NextEggType();
			PathEndMode peMode = PathEndMode.OnCell;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false, false, false);
			if (pawn.Faction == Faction.OfPlayer)
			{
				Thing bestEggBox = this.GetBestEggBox(pawn, peMode, traverseParms);
				if (bestEggBox != null)
				{
					return JobMaker.MakeJob(JobDefOf.LayEgg, bestEggBox);
				}
			}
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(singleDef), peMode, traverseParms, 30f, (Thing x) => pawn.GetRoom(RegionType.Set_All) == null || x.GetRoom(RegionType.Set_All) == pawn.GetRoom(RegionType.Set_All), null, 0, -1, false, RegionType.Set_Passable, false);
			return JobMaker.MakeJob(JobDefOf.LayEgg, (thing != null) ? thing.Position : RCellFinder.RandomWanderDestFor(pawn, pawn.Position, 5f, null, Danger.Some));
		}

		// Token: 0x06003453 RID: 13395 RVA: 0x00128A7C File Offset: 0x00126C7C
		private Thing GetBestEggBox(Pawn pawn, PathEndMode peMode, TraverseParms tp)
		{
			JobGiver_LayEgg.<>c__DisplayClass4_0 CS$<>8__locals1 = new JobGiver_LayEgg.<>c__DisplayClass4_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.eggDef = CS$<>8__locals1.pawn.TryGetComp<CompEggLayer>().NextEggType();
			return GenClosest.ClosestThing_Regionwise_ReachablePrioritized(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForDef(ThingDefOf.EggBox), peMode, tp, 30f, new Predicate<Thing>(CS$<>8__locals1.<GetBestEggBox>g__IsUsableBox|0), new Func<Thing, float>(JobGiver_LayEgg.<>c.<>9.<GetBestEggBox>g__GetScore|4_1), 10, 30);
		}

		// Token: 0x04001E41 RID: 7745
		private const float LayRadius = 5f;

		// Token: 0x04001E42 RID: 7746
		private const float MaxSearchRadius = 30f;

		// Token: 0x04001E43 RID: 7747
		private const int MinSearchRegions = 10;
	}
}

using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CD3 RID: 3283
	public class JobGiver_Haul : ThinkNode_JobGiver
	{
		// Token: 0x06004BD1 RID: 19409 RVA: 0x001A7314 File Offset: 0x001A5514
		protected override Job TryGiveJob(Pawn pawn)
		{
			Predicate<Thing> validator = delegate(Thing t)
			{
				IntVec3 intVec;
				return !t.IsForbidden(pawn) && HaulAIUtility.PawnCanAutomaticallyHaulFast(pawn, t, false) && pawn.carryTracker.MaxStackSpaceEver(t.def) > 0 && StoreUtility.TryFindBestBetterStoreCellFor(t, pawn, pawn.Map, StoreUtility.CurrentStoragePriorityOf(t), pawn.Faction, out intVec, true);
			};
			Thing thing = GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerHaulables.ThingsPotentiallyNeedingHauling(), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null);
			if (thing != null)
			{
				return HaulAIUtility.HaulToStorageJob(pawn, thing);
			}
			return null;
		}
	}
}

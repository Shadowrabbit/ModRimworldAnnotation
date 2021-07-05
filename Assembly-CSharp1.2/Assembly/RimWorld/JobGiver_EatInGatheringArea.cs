using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CB0 RID: 3248
	public class JobGiver_EatInGatheringArea : ThinkNode_JobGiver
	{
		// Token: 0x06004B6F RID: 19311 RVA: 0x001A5908 File Offset: 0x001A3B08
		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			if ((double)pawn.needs.food.CurLevelPercentage > 0.9)
			{
				return null;
			}
			IntVec3 cell = duty.focus.Cell;
			Thing thing = this.FindFood(pawn, cell);
			if (thing == null)
			{
				return null;
			}
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, thing);
			job.count = FoodUtility.WillIngestStackCountOf(pawn, thing.def, thing.def.GetStatValueAbstract(StatDefOf.Nutrition, null));
			return job;
		}

		// Token: 0x06004B70 RID: 19312 RVA: 0x001A5994 File Offset: 0x001A3B94
		private Thing FindFood(Pawn pawn, IntVec3 gatheringSpot)
		{
			Predicate<Thing> validator = (Thing x) => x.IngestibleNow && x.def.IsNutritionGivingIngestible && GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && !x.def.IsDrug && x.def.ingestible.preferability > FoodPreferability.RawBad && pawn.WillEat(x, null, true) && !x.IsForbidden(pawn) && x.IsSociallyProper(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), 14f, validator, null, 0, 12, false, RegionType.Set_Passable, false);
		}
	}
}

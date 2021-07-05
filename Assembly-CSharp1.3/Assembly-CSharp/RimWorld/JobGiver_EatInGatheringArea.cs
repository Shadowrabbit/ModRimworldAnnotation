using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200079B RID: 1947
	public class JobGiver_EatInGatheringArea : ThinkNode_JobGiver
	{
		// Token: 0x06003534 RID: 13620 RVA: 0x0012D150 File Offset: 0x0012B350
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

		// Token: 0x06003535 RID: 13621 RVA: 0x0012D1DC File Offset: 0x0012B3DC
		private Thing FindFood(Pawn pawn, IntVec3 gatheringSpot)
		{
			Predicate<Thing> validator = (Thing x) => x.IngestibleNow && x.def.IsNutritionGivingIngestible && GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && !x.def.IsDrug && x.def.ingestible.preferability > FoodPreferability.RawBad && pawn.WillEat(x, null, true) && !x.IsForbidden(pawn) && x.IsSociallyProper(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), 14f, validator, null, 0, 12, false, RegionType.Set_Passable, false);
		}
	}
}

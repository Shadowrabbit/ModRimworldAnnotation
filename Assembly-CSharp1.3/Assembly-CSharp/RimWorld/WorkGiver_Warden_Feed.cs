using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200082F RID: 2095
	public class WorkGiver_Warden_Feed : WorkGiver_Warden
	{
		// Token: 0x06003790 RID: 14224 RVA: 0x0013947C File Offset: 0x0013767C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner(pawn, t, false))
			{
				return null;
			}
			Pawn pawn2 = (Pawn)t;
			if (!WardenFeedUtility.ShouldBeFed(pawn2))
			{
				return null;
			}
			if (pawn2.needs.food.CurLevelPercentage >= pawn2.needs.food.PercentageThreshHungry + 0.02f)
			{
				return null;
			}
			Thing thing;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, false, false, false, false, false, false, FoodPreferability.Undefined))
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return null;
			}
			float nutrition = FoodUtility.GetNutrition(thing, thingDef);
			Job job = JobMaker.MakeJob(JobDefOf.FeedPatient, thing, pawn2);
			job.count = FoodUtility.WillIngestStackCountOf(pawn2, thingDef, nutrition);
			return job;
		}
	}
}

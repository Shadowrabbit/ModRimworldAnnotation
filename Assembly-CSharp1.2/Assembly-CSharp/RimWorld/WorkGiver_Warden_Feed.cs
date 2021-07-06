using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D62 RID: 3426
	public class WorkGiver_Warden_Feed : WorkGiver_Warden
	{
		// Token: 0x06004E44 RID: 20036 RVA: 0x001B0C58 File Offset: 0x001AEE58
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!base.ShouldTakeCareOfPrisoner_NewTemp(pawn, t, forced))
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
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, false, false, false, false, false, FoodPreferability.Undefined))
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

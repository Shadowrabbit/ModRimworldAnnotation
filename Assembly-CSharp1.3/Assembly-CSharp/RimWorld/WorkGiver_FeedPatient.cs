using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200083B RID: 2107
	public class WorkGiver_FeedPatient : WorkGiver_Scanner
	{
		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060037D6 RID: 14294 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x060037D7 RID: 14295 RVA: 0x00034716 File Offset: 0x00032916
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x0013AE97 File Offset: 0x00139097
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x0013AEAC File Offset: 0x001390AC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn)
			{
				return false;
			}
			if (this.def.feedHumanlikesOnly && !pawn2.RaceProps.Humanlike)
			{
				return false;
			}
			if (this.def.feedAnimalsOnly && !pawn2.RaceProps.Animal)
			{
				return false;
			}
			if (!FeedPatientUtility.IsHungry(pawn2))
			{
				return false;
			}
			if (!FeedPatientUtility.ShouldBeFed(pawn2))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			Thing thing;
			ThingDef thingDef;
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, true, false, false, false, false, false, FoodPreferability.Undefined))
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x0013AF6C File Offset: 0x0013916C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing thing;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, true, false, false, false, false, false, FoodPreferability.Undefined))
			{
				float nutrition = FoodUtility.GetNutrition(thing, thingDef);
				Job job = JobMaker.MakeJob(JobDefOf.FeedPatient);
				job.targetA = thing;
				job.targetB = pawn2;
				job.count = FoodUtility.WillIngestStackCountOf(pawn2, thingDef, nutrition);
				return job;
			}
			return null;
		}
	}
}

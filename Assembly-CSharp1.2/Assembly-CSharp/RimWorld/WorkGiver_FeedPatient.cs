using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D75 RID: 3445
	public class WorkGiver_FeedPatient : WorkGiver_Scanner
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06004E9D RID: 20125 RVA: 0x0003738E File Offset: 0x0003558E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06004E9E RID: 20126 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x00037712 File Offset: 0x00035912
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedHungryPawns;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x001B261C File Offset: 0x001B081C
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
			if (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, true, false, false, false, false, FoodPreferability.Undefined))
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x001B26DC File Offset: 0x001B08DC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			Thing thing;
			ThingDef thingDef;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn2, pawn2.needs.food.CurCategory == HungerCategory.Starving, out thing, out thingDef, false, true, false, true, false, false, false, false, FoodPreferability.Undefined))
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

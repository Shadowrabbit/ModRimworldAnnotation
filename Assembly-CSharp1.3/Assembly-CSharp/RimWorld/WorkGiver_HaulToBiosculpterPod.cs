using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000847 RID: 2119
	public class WorkGiver_HaulToBiosculpterPod : WorkGiver_Scanner
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06003815 RID: 14357 RVA: 0x0013BDDD File Offset: 0x00139FDD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.BiosculpterPod);
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06003816 RID: 14358 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x0013BDEC File Offset: 0x00139FEC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Biosculpting"))
			{
				return false;
			}
			if (t.IsForbidden(pawn) || !pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			CompBiosculpterPod compBiosculpterPod = t.TryGetComp<CompBiosculpterPod>();
			if (compBiosculpterPod == null || !compBiosculpterPod.PowerOn || compBiosculpterPod.State != BiosculpterPodState.LoadingNutrition)
			{
				return false;
			}
			if (t.IsBurning())
			{
				return false;
			}
			if (this.FindNutrition(pawn, compBiosculpterPod).Thing == null)
			{
				JobFailReason.Is("NoFood".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x0013BE90 File Offset: 0x0013A090
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompBiosculpterPod compBiosculpterPod = t.TryGetComp<CompBiosculpterPod>();
			if (compBiosculpterPod == null)
			{
				return null;
			}
			ThingCount thingCount = this.FindNutrition(pawn, compBiosculpterPod);
			if (thingCount.Thing == null)
			{
				return null;
			}
			Job job = HaulAIUtility.HaulToContainerJob(pawn, thingCount.Thing, t);
			job.count = Mathf.Min(job.count, thingCount.Count);
			return job;
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x0013BEE4 File Offset: 0x0013A0E4
		private ThingCount FindNutrition(Pawn pawn, CompBiosculpterPod pod)
		{
			WorkGiver_HaulToBiosculpterPod.<>c__DisplayClass6_0 CS$<>8__locals1 = new WorkGiver_HaulToBiosculpterPod.<>c__DisplayClass6_0();
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.pod = pod;
			Thing thing = GenClosest.ClosestThingReachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, new Predicate<Thing>(CS$<>8__locals1.<FindNutrition>g__Validator|0), null, 0, -1, false, RegionType.Set_Passable, false);
			if (thing == null)
			{
				return default(ThingCount);
			}
			int b = Mathf.CeilToInt(CS$<>8__locals1.pod.RequiredNutritionRemaining / thing.GetStatValue(StatDefOf.Nutrition, true));
			return new ThingCount(thing, Mathf.Min(thing.stackCount, b));
		}
	}
}

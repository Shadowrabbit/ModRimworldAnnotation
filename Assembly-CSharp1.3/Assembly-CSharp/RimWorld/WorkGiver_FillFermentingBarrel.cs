using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200083E RID: 2110
	public class WorkGiver_FillFermentingBarrel : WorkGiver_Scanner
	{
		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x0013B271 File Offset: 0x00139471
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.FermentingBarrel);
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x060037E7 RID: 14311 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x0013B280 File Offset: 0x00139480
		public static void ResetStaticData()
		{
			WorkGiver_FillFermentingBarrel.TemperatureTrans = "BadTemperature".Translate().ToLower();
			WorkGiver_FillFermentingBarrel.NoWortTrans = "NoWort".Translate();
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x0013B2C0 File Offset: 0x001394C0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel building_FermentingBarrel = t as Building_FermentingBarrel;
			if (building_FermentingBarrel == null || building_FermentingBarrel.Fermented || building_FermentingBarrel.SpaceLeftForWort <= 0)
			{
				return false;
			}
			float ambientTemperature = building_FermentingBarrel.AmbientTemperature;
			CompProperties_TemperatureRuinable compProperties = building_FermentingBarrel.def.GetCompProperties<CompProperties_TemperatureRuinable>();
			if (ambientTemperature < compProperties.minSafeTemperature + 2f || ambientTemperature > compProperties.maxSafeTemperature - 2f)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.TemperatureTrans, null);
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
			if (this.FindWort(pawn, building_FermentingBarrel) == null)
			{
				JobFailReason.Is(WorkGiver_FillFermentingBarrel.NoWortTrans, null);
				return false;
			}
			return !t.IsBurning();
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x0013B384 File Offset: 0x00139584
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building_FermentingBarrel barrel = (Building_FermentingBarrel)t;
			Thing t2 = this.FindWort(pawn, barrel);
			return JobMaker.MakeJob(JobDefOf.FillFermentingBarrel, t, t2);
		}

		// Token: 0x060037EB RID: 14315 RVA: 0x0013B3B8 File Offset: 0x001395B8
		private Thing FindWort(Pawn pawn, Building_FermentingBarrel barrel)
		{
			Predicate<Thing> validator = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.Wort), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x04001F26 RID: 7974
		private static string TemperatureTrans;

		// Token: 0x04001F27 RID: 7975
		private static string NoWortTrans;
	}
}

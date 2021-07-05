using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200083F RID: 2111
	public class WorkGiver_FixBrokenDownBuilding : WorkGiver_Scanner
	{
		// Token: 0x060037ED RID: 14317 RVA: 0x0013B422 File Offset: 0x00139622
		public static void ResetStaticData()
		{
			WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans = "NotInHomeArea".Translate();
			WorkGiver_FixBrokenDownBuilding.NoComponentsToRepairTrans = "NoComponentsToRepair".Translate();
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x060037EE RID: 14318 RVA: 0x0013B44C File Offset: 0x0013964C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x0013B455 File Offset: 0x00139655
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings;
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x0013B467 File Offset: 0x00139667
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.GetComponent<BreakdownManager>().brokenDownThings.Count == 0;
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x060037F1 RID: 14321 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060037F2 RID: 14322 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060037F3 RID: 14323 RVA: 0x0013B484 File Offset: 0x00139684
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (!building.def.building.repairable)
			{
				return false;
			}
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			if (!t.IsBrokenDown())
			{
				return false;
			}
			if (t.IsForbidden(pawn))
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				return false;
			}
			if (!pawn.CanReserve(building, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) != null)
			{
				return false;
			}
			if (building.IsBurning())
			{
				return false;
			}
			if (this.FindClosestComponent(pawn) == null)
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NoComponentsToRepairTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x060037F4 RID: 14324 RVA: 0x0013B55C File Offset: 0x0013975C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Thing t2 = this.FindClosestComponent(pawn);
			Job job = JobMaker.MakeJob(JobDefOf.FixBrokenDownBuilding, t, t2);
			job.count = 1;
			return job;
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x0013B590 File Offset: 0x00139790
		private Thing FindClosestComponent(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.ComponentIndustrial), PathEndMode.InteractionCell, TraverseParms.For(pawn, pawn.NormalMaxDanger(), TraverseMode.ByPawn, false, false, false), 9999f, (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x04001F28 RID: 7976
		public static string NotInHomeAreaTrans;

		// Token: 0x04001F29 RID: 7977
		private static string NoComponentsToRepairTrans;
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D9C RID: 3484
	public class WorkGiver_Repair : WorkGiver_Scanner
	{
		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x06004F67 RID: 20327 RVA: 0x000377A2 File Offset: 0x000359A2
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x00037D96 File Offset: 0x00035F96
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x00037DAE File Offset: 0x00035FAE
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction).Count == 0;
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x001B4AE4 File Offset: 0x001B2CE4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Building building = t as Building;
			if (building == null)
			{
				return false;
			}
			if (!pawn.Map.listerBuildingsRepairable.Contains(pawn.Faction, building))
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
			if (!t.def.useHitPoints || t.HitPoints == t.MaxHitPoints)
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				return false;
			}
			return pawn.CanReserve(building, 1, -1, null, forced) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) == null && (!building.def.mineable || building.Map.designationManager.DesignationAt(building.Position, DesignationDefOf.Mine) == null) && !building.IsBurning();
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x00037DCE File Offset: 0x00035FCE
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Repair, t);
		}
	}
}

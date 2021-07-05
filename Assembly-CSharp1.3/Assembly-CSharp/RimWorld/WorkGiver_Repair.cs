using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200085A RID: 2138
	public class WorkGiver_Repair : WorkGiver_Scanner
	{
		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06003878 RID: 14456 RVA: 0x0013B44C File Offset: 0x0013964C
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial);
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06003879 RID: 14457 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x0013D4B8 File Offset: 0x0013B6B8
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x0013D4D0 File Offset: 0x0013B6D0
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction).Count == 0;
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x0013D4F0 File Offset: 0x0013B6F0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!RepairUtility.PawnCanRepairNow(pawn, t))
			{
				return false;
			}
			Building building = t as Building;
			if (pawn.Faction == Faction.OfPlayer && !pawn.Map.areaManager.Home[t.Position])
			{
				JobFailReason.Is(WorkGiver_FixBrokenDownBuilding.NotInHomeAreaTrans, null);
				return false;
			}
			return pawn.CanReserve(building, 1, -1, null, forced) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Deconstruct) == null && (!building.def.mineable || building.Map.designationManager.DesignationAt(building.Position, DesignationDefOf.Mine) == null) && !building.IsBurning();
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x0013D5AB File Offset: 0x0013B7AB
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Repair, t);
		}
	}
}

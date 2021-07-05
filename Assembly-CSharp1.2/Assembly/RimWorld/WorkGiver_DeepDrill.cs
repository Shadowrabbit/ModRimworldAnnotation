using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D6A RID: 3434
	public class WorkGiver_DeepDrill : WorkGiver_Scanner
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06004E62 RID: 20066 RVA: 0x00037544 File Offset: 0x00035744
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.DeepDrill);
			}
		}

		// Token: 0x17000BFF RID: 3071
		// (get) Token: 0x06004E63 RID: 20067 RVA: 0x00037420 File Offset: 0x00035620
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x001B123C File Offset: 0x001AF43C
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def == ThingDefOf.DeepDrill)
				{
					CompPowerTrader comp = building.GetComp<CompPowerTrader>();
					if ((comp == null || comp.PowerOn) && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Uninstall) == null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x001B12B0 File Offset: 0x001AF4B0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && !building.IsForbidden(pawn) && pawn.CanReserve(building, 1, -1, null, forced) && building.TryGetComp<CompDeepDrill>().CanDrillNow() && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Uninstall) == null && !building.IsBurning();
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x00037550 File Offset: 0x00035750
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateDeepDrill, t, 1500, true);
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000838 RID: 2104
	public class WorkGiver_DeepDrill : WorkGiver_Scanner
	{
		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x060037B2 RID: 14258 RVA: 0x00139D42 File Offset: 0x00137F42
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.DeepDrill);
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x060037B3 RID: 14259 RVA: 0x001398A1 File Offset: 0x00137AA1
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x00139D50 File Offset: 0x00137F50
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

		// Token: 0x060037B6 RID: 14262 RVA: 0x00139DC4 File Offset: 0x00137FC4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.Faction != pawn.Faction)
			{
				return false;
			}
			Building building = t as Building;
			return building != null && !building.IsForbidden(pawn) && pawn.CanReserve(building, 1, -1, null, forced) && building.TryGetComp<CompDeepDrill>().CanDrillNow() && building.Map.designationManager.DesignationOn(building, DesignationDefOf.Uninstall) == null && !building.IsBurning();
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x00139E40 File Offset: 0x00138040
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.OperateDeepDrill, t, 1500, true);
		}
	}
}

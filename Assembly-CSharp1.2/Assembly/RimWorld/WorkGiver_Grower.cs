using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D7C RID: 3452
	public abstract class WorkGiver_Grower : WorkGiver_Scanner
	{
		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06004EC2 RID: 20162 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool AllowUnreachable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004EC3 RID: 20163 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool ExtraRequirements(IPlantToGrowSettable settable, Pawn pawn)
		{
			return true;
		}

		// Token: 0x06004EC4 RID: 20164 RVA: 0x000377FE File Offset: 0x000359FE
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			Danger maxDanger = pawn.NormalMaxDanger();
			List<Building> bList = pawn.Map.listerBuildings.allBuildingsColonist;
			int num;
			for (int i = 0; i < bList.Count; i = num + 1)
			{
				Building_PlantGrower building_PlantGrower = bList[i] as Building_PlantGrower;
				if (building_PlantGrower != null && this.ExtraRequirements(building_PlantGrower, pawn) && !building_PlantGrower.IsForbidden(pawn) && pawn.CanReach(building_PlantGrower, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn) && !building_PlantGrower.IsBurning())
				{
					foreach (IntVec3 intVec in building_PlantGrower.OccupiedRect())
					{
						yield return intVec;
					}
					WorkGiver_Grower.wantedPlantDef = null;
				}
				num = i;
			}
			WorkGiver_Grower.wantedPlantDef = null;
			List<Zone> zonesList = pawn.Map.zoneManager.AllZones;
			for (int i = 0; i < zonesList.Count; i = num + 1)
			{
				Zone_Growing growZone = zonesList[i] as Zone_Growing;
				if (growZone != null)
				{
					if (growZone.cells.Count == 0)
					{
						Log.ErrorOnce("Grow zone has 0 cells: " + growZone, -563487, false);
					}
					else if (this.ExtraRequirements(growZone, pawn) && !growZone.ContainsStaticFire && pawn.CanReach(growZone.Cells[0], PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
					{
						for (int j = 0; j < growZone.cells.Count; j = num + 1)
						{
							yield return growZone.cells[j];
							num = j;
						}
						WorkGiver_Grower.wantedPlantDef = null;
						growZone = null;
					}
				}
				num = i;
			}
			WorkGiver_Grower.wantedPlantDef = null;
			yield break;
			yield break;
		}

		// Token: 0x06004EC5 RID: 20165 RVA: 0x001B2CF4 File Offset: 0x001B0EF4
		public static ThingDef CalculateWantedPlantDef(IntVec3 c, Map map)
		{
			IPlantToGrowSettable plantToGrowSettable = c.GetPlantToGrowSettable(map);
			if (plantToGrowSettable == null)
			{
				return null;
			}
			return plantToGrowSettable.GetPlantDefToGrow();
		}

		// Token: 0x0400334B RID: 13131
		protected static ThingDef wantedPlantDef;
	}
}

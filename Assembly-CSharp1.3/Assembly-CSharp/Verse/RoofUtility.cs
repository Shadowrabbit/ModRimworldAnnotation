using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000216 RID: 534
	public static class RoofUtility
	{
		// Token: 0x06000F46 RID: 3910 RVA: 0x00056AD4 File Offset: 0x00054CD4
		public static Thing FirstBlockingThing(IntVec3 pos, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(pos);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.plant != null && list[i].def.plant.interferesWithRoof)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x00056B34 File Offset: 0x00054D34
		public static bool IsAnyCellUnderRoof(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			bool result = false;
			RoofGrid roofGrid = thing.Map.roofGrid;
			foreach (IntVec3 c in cellRect)
			{
				if (roofGrid.Roofed(c))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x00056BA4 File Offset: 0x00054DA4
		public static bool CanHandleBlockingThing(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return true;
			}
			if (blocker.def.category == ThingCategory.Plant)
			{
				if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
				{
					return false;
				}
				if (worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00056BE0 File Offset: 0x00054DE0
		public static Job HandleBlockingThingJob(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return null;
			}
			if (blocker.def.category != ThingCategory.Plant || !worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
			{
				return null;
			}
			if (!PlantUtility.PawnWillingToCutPlant_Job(blocker, worker))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.CutPlant, blocker);
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000302 RID: 770
	public static class RoofUtility
	{
		// Token: 0x060013C0 RID: 5056 RVA: 0x000CBB28 File Offset: 0x000C9D28
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

		// Token: 0x060013C1 RID: 5057 RVA: 0x000CBB88 File Offset: 0x000C9D88
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

		// Token: 0x060013C2 RID: 5058 RVA: 0x000142D1 File Offset: 0x000124D1
		public static bool CanHandleBlockingThing(Thing blocker, Pawn worker, bool forced = false)
		{
			return blocker == null || (blocker.def.category == ThingCategory.Plant && worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced));
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00014302 File Offset: 0x00012502
		public static Job HandleBlockingThingJob(Thing blocker, Pawn worker, bool forced = false)
		{
			if (blocker == null)
			{
				return null;
			}
			if (blocker.def.category == ThingCategory.Plant && worker.CanReserveAndReach(blocker, PathEndMode.ClosestTouch, worker.NormalMaxDanger(), 1, -1, null, forced))
			{
				return JobMaker.MakeJob(JobDefOf.CutPlant, blocker);
			}
			return null;
		}
	}
}

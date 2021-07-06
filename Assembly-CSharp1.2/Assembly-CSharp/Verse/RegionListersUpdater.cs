using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002DE RID: 734
	public static class RegionListersUpdater
	{
		// Token: 0x060012B0 RID: 4784 RVA: 0x000C7B18 File Offset: 0x000C5D18
		public static void DeregisterInRegions(Thing thing, Map map)
		{
			if (!ListerThings.EverListable(thing.def, ListerThingsUse.Region))
			{
				return;
			}
			RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, true);
			for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
			{
				ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
				if (listerThings.Contains(thing))
				{
					listerThings.Remove(thing);
				}
			}
			RegionListersUpdater.tmpRegions.Clear();
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000C7B84 File Offset: 0x000C5D84
		public static void RegisterInRegions(Thing thing, Map map)
		{
			if (!ListerThings.EverListable(thing.def, ListerThingsUse.Region))
			{
				return;
			}
			RegionListersUpdater.GetTouchableRegions(thing, map, RegionListersUpdater.tmpRegions, false);
			for (int i = 0; i < RegionListersUpdater.tmpRegions.Count; i++)
			{
				ListerThings listerThings = RegionListersUpdater.tmpRegions[i].ListerThings;
				if (!listerThings.Contains(thing))
				{
					listerThings.Add(thing);
				}
			}
			RegionListersUpdater.tmpRegions.Clear();
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000C7BF0 File Offset: 0x000C5DF0
		public static void RegisterAllAt(IntVec3 c, Map map, HashSet<Thing> processedThings = null)
		{
			List<Thing> thingList = c.GetThingList(map);
			int count = thingList.Count;
			for (int i = 0; i < count; i++)
			{
				Thing thing = thingList[i];
				if (processedThings == null || processedThings.Add(thing))
				{
					RegionListersUpdater.RegisterInRegions(thing, map);
				}
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000C7C34 File Offset: 0x000C5E34
		public static void GetTouchableRegions(Thing thing, Map map, List<Region> outRegions, bool allowAdjacentEvenIfCantTouch = false)
		{
			outRegions.Clear();
			CellRect cellRect = thing.OccupiedRect();
			CellRect cellRect2 = cellRect;
			if (RegionListersUpdater.CanRegisterInAdjacentRegions(thing))
			{
				cellRect2 = cellRect2.ExpandedBy(1);
			}
			foreach (IntVec3 intVec in cellRect2)
			{
				if (intVec.InBounds(map))
				{
					Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(intVec);
					if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.type.Passable() && !outRegions.Contains(validRegionAt_NoRebuild))
					{
						if (cellRect.Contains(intVec))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
						else if (allowAdjacentEvenIfCantTouch || ReachabilityImmediate.CanReachImmediate(intVec, thing, map, PathEndMode.Touch, null))
						{
							outRegions.Add(validRegionAt_NoRebuild);
						}
					}
				}
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0000A2A7 File Offset: 0x000084A7
		private static bool CanRegisterInAdjacentRegions(Thing thing)
		{
			return true;
		}

		// Token: 0x04000EF8 RID: 3832
		private static List<Region> tmpRegions = new List<Region>();
	}
}

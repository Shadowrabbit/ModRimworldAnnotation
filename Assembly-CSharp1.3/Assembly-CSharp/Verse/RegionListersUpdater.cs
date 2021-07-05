using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000206 RID: 518
	public static class RegionListersUpdater
	{
		// Token: 0x06000EA9 RID: 3753 RVA: 0x000532B0 File Offset: 0x000514B0
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

		// Token: 0x06000EAA RID: 3754 RVA: 0x0005331C File Offset: 0x0005151C
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

		// Token: 0x06000EAB RID: 3755 RVA: 0x00053388 File Offset: 0x00051588
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

		// Token: 0x06000EAC RID: 3756 RVA: 0x000533CC File Offset: 0x000515CC
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

		// Token: 0x06000EAD RID: 3757 RVA: 0x000126F5 File Offset: 0x000108F5
		private static bool CanRegisterInAdjacentRegions(Thing thing)
		{
			return true;
		}

		// Token: 0x04000BD0 RID: 3024
		private static List<Region> tmpRegions = new List<Region>();
	}
}

using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000A1 RID: 161
	public static class Autotests_RegionListers
	{
		// Token: 0x06000556 RID: 1366 RVA: 0x0000A847 File Offset: 0x00008A47
		public static void CheckBugs(Map map)
		{
			Autotests_RegionListers.CalculateExpectedListers(map);
			Autotests_RegionListers.CheckThingRegisteredTwice(map);
			Autotests_RegionListers.CheckThingNotRegisteredButShould();
			Autotests_RegionListers.CheckThingRegisteredButShouldnt(map);
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0008B504 File Offset: 0x00089704
		private static void CheckThingRegisteredTwice(Map map)
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				Autotests_RegionListers.CheckDuplicates(keyValuePair.Value, keyValuePair.Key, true);
			}
			foreach (Region region in map.regionGrid.AllRegions)
			{
				Autotests_RegionListers.CheckDuplicates(region.ListerThings.AllThings, region, false);
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0008B5B0 File Offset: 0x000897B0
		private static void CheckDuplicates(List<Thing> lister, Region region, bool expected)
		{
			for (int i = 1; i < lister.Count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					if (lister[i] == lister[j])
					{
						if (expected)
						{
							Log.Error(string.Concat(new object[]
							{
								"Region error: thing ",
								lister[i],
								" is expected to be registered twice in ",
								region,
								"? This should never happen."
							}), false);
						}
						else
						{
							Log.Error(string.Concat(new object[]
							{
								"Region error: thing ",
								lister[i],
								" is registered twice in ",
								region
							}), false);
						}
					}
				}
			}
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0008B660 File Offset: 0x00089860
		private static void CheckThingNotRegisteredButShould()
		{
			foreach (KeyValuePair<Region, List<Thing>> keyValuePair in Autotests_RegionListers.expectedListers)
			{
				List<Thing> value = keyValuePair.Value;
				List<Thing> allThings = keyValuePair.Key.ListerThings.AllThings;
				for (int i = 0; i < value.Count; i++)
				{
					if (!allThings.Contains(value[i]))
					{
						Log.Error(string.Concat(new object[]
						{
							"Region error: thing ",
							value[i],
							" at ",
							value[i].Position,
							" should be registered in ",
							keyValuePair.Key,
							" but it's not."
						}), false);
					}
				}
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0008B74C File Offset: 0x0008994C
		private static void CheckThingRegisteredButShouldnt(Map map)
		{
			foreach (Region region in map.regionGrid.AllRegions)
			{
				List<Thing> list;
				if (!Autotests_RegionListers.expectedListers.TryGetValue(region, out list))
				{
					list = null;
				}
				List<Thing> allThings = region.ListerThings.AllThings;
				for (int i = 0; i < allThings.Count; i++)
				{
					if (list == null || !list.Contains(allThings[i]))
					{
						Log.Error(string.Concat(new object[]
						{
							"Region error: thing ",
							allThings[i],
							" at ",
							allThings[i].Position,
							" is registered in ",
							region,
							" but it shouldn't be."
						}), false);
					}
				}
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0008B838 File Offset: 0x00089A38
		private static void CalculateExpectedListers(Map map)
		{
			Autotests_RegionListers.expectedListers.Clear();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				if (ListerThings.EverListable(thing.def, ListerThingsUse.Region))
				{
					RegionListersUpdater.GetTouchableRegions(thing, map, Autotests_RegionListers.tmpTouchableRegions, false);
					for (int j = 0; j < Autotests_RegionListers.tmpTouchableRegions.Count; j++)
					{
						Region key = Autotests_RegionListers.tmpTouchableRegions[j];
						List<Thing> list;
						if (!Autotests_RegionListers.expectedListers.TryGetValue(key, out list))
						{
							list = new List<Thing>();
							Autotests_RegionListers.expectedListers.Add(key, list);
						}
						list.Add(allThings[i]);
					}
				}
			}
		}

		// Token: 0x0400029E RID: 670
		private static Dictionary<Region, List<Thing>> expectedListers = new Dictionary<Region, List<Thing>>();

		// Token: 0x0400029F RID: 671
		private static List<Region> tmpTouchableRegions = new List<Region>();
	}
}

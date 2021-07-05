using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002EA RID: 746
	public static class RegionTypeUtility
	{
		// Token: 0x060012E4 RID: 4836 RVA: 0x00013801 File Offset: 0x00011A01
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00013807 File Offset: 0x00011A07
		public static bool AllowsMultipleRegionsPerRoom(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x000C87C0 File Offset: 0x000C69C0
		public static RegionType GetExpectedRegionType(this IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return RegionType.None;
			}
			if (c.GetDoor(map) != null)
			{
				return RegionType.Portal;
			}
			if (c.Walkable(map))
			{
				return RegionType.Normal;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.Fillage == FillCategory.Full)
				{
					return RegionType.None;
				}
			}
			return RegionType.ImpassableFreeAirExchange;
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x000C8820 File Offset: 0x000C6A20
		public static RegionType GetRegionType(this IntVec3 c, Map map)
		{
			Region region = c.GetRegion(map, RegionType.Set_All);
			if (region == null)
			{
				return RegionType.None;
			}
			return region.type;
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x00013810 File Offset: 0x00011A10
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) > RegionType.None;
		}
	}
}

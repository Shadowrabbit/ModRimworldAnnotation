using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200020C RID: 524
	public static class RegionTypeUtility
	{
		// Token: 0x06000ECA RID: 3786 RVA: 0x00053E64 File Offset: 0x00052064
		public static bool IsOneCellRegion(this RegionType regionType)
		{
			return regionType == RegionType.Portal;
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00053E6A File Offset: 0x0005206A
		public static bool AllowsMultipleRegionsPerDistrict(this RegionType regionType)
		{
			return regionType != RegionType.Portal;
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00053E74 File Offset: 0x00052074
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
			if (c.GetFence(map) != null)
			{
				return RegionType.Fence;
			}
			if (c.WalkableByNormal(map))
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

		// Token: 0x06000ECD RID: 3789 RVA: 0x00053EDD File Offset: 0x000520DD
		public static bool Passable(this RegionType regionType)
		{
			return (regionType & RegionType.Set_Passable) > RegionType.None;
		}
	}
}

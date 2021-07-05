using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001090 RID: 4240
	public static class BlightUtility
	{
		// Token: 0x06006500 RID: 25856 RVA: 0x00221614 File Offset: 0x0021F814
		public static Plant GetFirstBlightableNowPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.BlightableNow)
			{
				return plant;
			}
			return null;
		}

		// Token: 0x06006501 RID: 25857 RVA: 0x00221638 File Offset: 0x0021F838
		public static Plant GetFirstBlightableEverPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.Blightable)
			{
				return plant;
			}
			return null;
		}
	}
}

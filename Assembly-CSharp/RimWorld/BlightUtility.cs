using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E7 RID: 5863
	public static class BlightUtility
	{
		// Token: 0x060080CD RID: 32973 RVA: 0x002627FC File Offset: 0x002609FC
		public static Plant GetFirstBlightableNowPlant(IntVec3 c, Map map)
		{
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.BlightableNow)
			{
				return plant;
			}
			return null;
		}

		// Token: 0x060080CE RID: 32974 RVA: 0x00262820 File Offset: 0x00260A20
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

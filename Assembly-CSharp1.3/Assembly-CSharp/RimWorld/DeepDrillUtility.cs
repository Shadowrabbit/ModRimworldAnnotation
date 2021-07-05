﻿using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111F RID: 4383
	public static class DeepDrillUtility
	{
		// Token: 0x0600694B RID: 26955 RVA: 0x00237DE4 File Offset: 0x00235FE4
		public static ThingDef GetNextResource(IntVec3 p, Map map)
		{
			ThingDef result;
			int num;
			IntVec3 intVec;
			DeepDrillUtility.GetNextResource(p, map, out result, out num, out intVec);
			return result;
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x00237E00 File Offset: 0x00236000
		public static bool GetNextResource(IntVec3 p, Map map, out ThingDef resDef, out int countPresent, out IntVec3 cell)
		{
			for (int i = 0; i < 21; i++)
			{
				IntVec3 intVec = p + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					ThingDef thingDef = map.deepResourceGrid.ThingDefAt(intVec);
					if (thingDef != null)
					{
						resDef = thingDef;
						countPresent = map.deepResourceGrid.CountAt(intVec);
						cell = intVec;
						return true;
					}
				}
			}
			resDef = DeepDrillUtility.GetBaseResource(map, p);
			countPresent = int.MaxValue;
			cell = p;
			return false;
		}

		// Token: 0x0600694D RID: 26957 RVA: 0x00237E7C File Offset: 0x0023607C
		public static ThingDef GetBaseResource(Map map, IntVec3 cell)
		{
			if (!map.Biome.hasBedrock)
			{
				return null;
			}
			Rand.PushState();
			Rand.Seed = cell.GetHashCode();
			ThingDef result = (from rock in Find.World.NaturalRockTypesIn(map.Tile)
			select rock.building.mineableThing).RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04003AEA RID: 15082
		public const int NumCellsToScan = 21;
	}
}
